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

        #region Delegates

        public delegate void LocalizedStringPropertyValueChangedDelegate(
            BlueprintViewModel blueprint, InspectorPropertyAttribute inspectorProperty, object newValue);

        #endregion

        #region Public Events

        public event LocalizedStringPropertyValueChangedDelegate LocalizedStringPropertyValueChanged;

        #endregion

        #region Methods

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
        private object GetCurrentAttributeValue(InspectorPropertyAttribute property, out bool inherited)
        {
            BlueprintViewModel viewModel = (BlueprintViewModel)this.DataContext;
            return this.GetCurrentAttributeValue(viewModel, property, out inherited);
        }

        /// <summary>
        ///   Gets the current value of the specified property for the passed blueprint,
        ///   taking into account, in order: Blueprint attribute table, parents, default value.
        /// </summary>
        /// <param name="viewModel">Blueprint to get the attribute value for.</param>
        /// <param name="property">Property to get the current value of.</param>
        /// <param name="inherited"></param>
        /// <returns>Current value of the specified property for the passed blueprint.</returns>
        private object GetCurrentAttributeValue(
            BlueprintViewModel viewModel, InspectorPropertyAttribute property, out bool inherited)
        {
            object propertyValue;

            // Check own attribute table.
            if (viewModel.Blueprint.AttributeTable.TryGetValue(property.Name, out propertyValue))
            {
                inherited = false;
                return propertyValue;
            }

            inherited = true;

            // Search in parents.
            if (viewModel.Parent != null)
            {
                bool tmp;
                return this.GetCurrentAttributeValue(viewModel.Parent, property, out tmp);
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

        private void OnLocalizedStringPropertyValueChanged(
            BlueprintViewModel blueprint, InspectorPropertyAttribute inspectorProperty, object newValue)
        {
            var handler = this.LocalizedStringPropertyValueChanged;
            if (handler != null)
            {
                handler(blueprint, inspectorProperty, newValue);
            }
        }

        private void OnPropertyControlValueChanged(InspectorPropertyAttribute inspectorProperty, object newValue)
        {
            BlueprintViewModel viewModel = (BlueprintViewModel)this.DataContext;

            // Check for localized attribute changes.
            var stringProperty = inspectorProperty as InspectorStringAttribute;

            if (stringProperty != null && stringProperty.Localized)
            {
                this.OnLocalizedStringPropertyValueChanged(viewModel, inspectorProperty, newValue);
                return;
            }

            // Update blueprint configuration.
            object defaultValue = inspectorProperty.Default;
            if (viewModel.Parent != null)
            {
                bool inherited;
                defaultValue = this.GetCurrentAttributeValue(viewModel.Parent, inspectorProperty, out inherited);
            }

            // Check if property is set to default/inherited value.
            if (Equals(newValue, defaultValue))
            {
                viewModel.Blueprint.AttributeTable.RemoveValue(inspectorProperty.Name);
            }
            else
            {
                viewModel.Blueprint.AttributeTable.SetValue(inspectorProperty.Name, newValue);
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
            this.inspectorFactory.AddComponentInspectorsRecursively(
                viewModel, this.AttributesPanel, this.GetCurrentAttributeValue, this.OnPropertyControlValueChanged);
        }

        #endregion
    }
}