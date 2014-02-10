// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BlueprintControl.xaml.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace BlueprintEditor.Controls
{
    using System;
    using System.Collections.Specialized;
    using System.Windows;
    using System.Windows.Input;

    using BlueprintEditor.Commands;
    using BlueprintEditor.Inspectors;
    using BlueprintEditor.ViewModels;

    using Slash.GameBase.Inspector.Attributes;
    using Slash.GameBase.Inspector.Data;
    using Slash.Tools.BlueprintEditor.Logic.Data;

    /// <summary>
    ///   Interaction logic for BlueprintControl.xaml
    /// </summary>
    public sealed partial class BlueprintControl
    {
        #region Fields

        private readonly InspectorFactory inspectorFactory = new InspectorFactory();

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///   Constructor.
        /// </summary>
        public BlueprintControl()
        {
            this.InitializeComponent();

            this.DataContextChanged += this.OnDataContextChanged;
        }

        #endregion

        #region Methods

        /// <summary>
        ///   Adds inspectors for the components of the specified blueprint and its parents.
        /// </summary>
        /// <param name="viewModel">Blueprint to add component inspectors for.</param>
        private void AddComponentInspectorsRecursively(BlueprintViewModel viewModel)
        {
            // Add inspectors for parent blueprints.
            if (viewModel.Parent != null)
            {
                this.AddComponentInspectorsRecursively(viewModel.Parent);
            }

            // Add inspectors for specified blueprint.
            foreach (var componentType in viewModel.AddedComponents)
            {
                // Get attributes.
                InspectorType componentInfo = InspectorComponentTable.Instance.GetInspectorType(componentType);
                if (componentInfo == null)
                {
                    continue;
                }

                this.inspectorFactory.AddInspectorControls(
                    componentInfo,
                    this.AttributesPanel,
                    this.GetCurrentAttributeValue,
                    this.OnPropertyControlValueChanged);
            }
        }

        private void CanExecuteAddComponent(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = this.LbComponentsAvailable != null && this.LbComponentsAvailable.SelectedItems != null
                           && this.LbComponentsAvailable.SelectedItems.Count > 0;
        }

        private void CanExecuteRemoveComponent(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = this.LbComponentsAdded != null && this.LbComponentsAdded.SelectedItems != null
                           && this.LbComponentsAdded.SelectedItems.Count > 0;
        }

        private void ExecutedAddComponent(object sender, ExecutedRoutedEventArgs e)
        {
            // Get selected component type of available component types.
            Type componentType = (Type)this.LbComponentsAvailable.SelectedItem;
            if (componentType == null)
            {
                MessageBox.Show("No component type selected.");
                return;
            }

            BlueprintViewModel viewModel = (BlueprintViewModel)this.DataContext;
            viewModel.AddComponent(componentType);

            // Select component type.
            this.LbComponentsAdded.SelectedItem = componentType;
        }

        private void ExecutedRemoveComponent(object sender, ExecutedRoutedEventArgs e)
        {
            // Get selected component type of added component types.
            Type componentType = (Type)this.LbComponentsAdded.SelectedItem;
            if (componentType == null)
            {
                MessageBox.Show("No component type selected.");
                return;
            }

            BlueprintViewModel viewModel = (BlueprintViewModel)this.DataContext;

            if (!viewModel.RemoveComponent(componentType))
            {
                return;
            }

            // Select component type.
            this.LbComponentsAvailable.SelectedItem = componentType;
        }

        /// <summary>
        ///   Gets the current value of the specified property for the passed blueprint,
        ///   taking into account, in order: Blueprint attribute table, parents, default value.
        /// </summary>
        /// <param name="property">Property to get the current value of.</param>
        /// <returns>Current value of the specified property for the passed blueprint.</returns>
        private object GetCurrentAttributeValue(InspectorPropertyAttribute property)
        {
            BlueprintViewModel viewModel = (BlueprintViewModel)this.DataContext;
            return this.GetCurrentAttributeValue(viewModel, property);
        }

        /// <summary>
        ///   Gets the current value of the specified property for the passed blueprint,
        ///   taking into account, in order: Blueprint attribute table, parents, default value.
        /// </summary>
        /// <param name="viewModel">Blueprint to get the attribute value for.</param>
        /// <param name="property">Property to get the current value of.</param>
        /// <returns>Current value of the specified property for the passed blueprint.</returns>
        private object GetCurrentAttributeValue(BlueprintViewModel viewModel, InspectorPropertyAttribute property)
        {
            object propertyValue;

            // Check own attribute table.
            if (viewModel.Blueprint.AttributeTable.TryGetValue(property.Name, out propertyValue))
            {
                return propertyValue;
            }

            // Search in parents.
            if (viewModel.Parent != null)
            {
                return this.GetCurrentAttributeValue(viewModel.Parent, property);
            }

            // Return default value.
            return property.Default;
        }

        private void LbComponentsAdded_OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            // Send command.
            BlueprintCommands.RemoveComponentCommand.Execute(null);
        }

        private void LbComponentsAvailable_OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            // Send command.
            BlueprintCommands.AddComponentCommand.Execute(null);
        }

        private void OnBlueprintComponentsChanged(
            object sender, NotifyCollectionChangedEventArgs notifyCollectionChangedEventArgs)
        {
            this.UpdateInspectors();
        }

        private void OnDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            BlueprintViewModel oldViewModel = (BlueprintViewModel)e.OldValue;
            if (oldViewModel != null)
            {
                oldViewModel.AddedComponents.CollectionChanged -= this.OnBlueprintComponentsChanged;
            }

            BlueprintViewModel newViewModel = (BlueprintViewModel)e.NewValue;
            if (newViewModel != null)
            {
                newViewModel.AddedComponents.CollectionChanged += this.OnBlueprintComponentsChanged;
            }

            this.UpdateInspectors();
        }

        private void OnPropertyControlValueChanged(
            InspectorPropertyAttribute inspectorProperty, object newValue, object oldValue)
        {
            BlueprintViewModel viewModel = (BlueprintViewModel)this.DataContext;
            Console.WriteLine("Property value {0} changed from {1} to {2}", inspectorProperty, oldValue, newValue);

            // Update blueprint configuration.
            if (viewModel.Parent == null)
            {
                // No parent - check if property is reset to default value.
                if (Equals(newValue, inspectorProperty.Default))
                {
                    viewModel.Blueprint.AttributeTable.RemoveValue(inspectorProperty.Name);
                }
                else
                {
                    viewModel.Blueprint.AttributeTable.SetValue(inspectorProperty.Name, newValue);
                }
            }
            else
            {
                // Check if property is set to inherited value.
                if (Equals(newValue, this.GetCurrentAttributeValue(viewModel.Parent, inspectorProperty)))
                {
                    viewModel.Blueprint.AttributeTable.RemoveValue(inspectorProperty.Name);
                }
                else
                {
                    viewModel.Blueprint.AttributeTable.SetValue(inspectorProperty.Name, newValue);
                }
            }
        }

        private void UpdateInspectors()
        {
            // Clear inspectors.
            this.AttributesPanel.Children.Clear();

            BlueprintViewModel viewModel = (BlueprintViewModel)this.DataContext;
            if (viewModel == null || viewModel.AddedComponents == null)
            {
                return;
            }

            // Add inspectors for blueprint components.
            this.AddComponentInspectorsRecursively(viewModel);
        }

        #endregion
    }
}