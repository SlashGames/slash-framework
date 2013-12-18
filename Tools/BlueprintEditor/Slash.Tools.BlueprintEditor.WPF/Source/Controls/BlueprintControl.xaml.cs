// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BlueprintControl.xaml.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace BlueprintEditor.Controls
{
    using System;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;

    using BlueprintEditor.Commands;
    using BlueprintEditor.Inspectors;
    using BlueprintEditor.ViewModels;

    using Slash.GameBase.Inspector.Attributes;
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

        private void AddComponentInspectors(Type componentType, Panel panel)
        {
            // Get attributes.
            InspectorComponent componentInfo = InspectorComponentTable.GetComponent(componentType);
            if (componentInfo == null)
            {
                return;
            }

            // Add label for component name.
            Label componentLabel = new Label
                {
                    Content = componentInfo.Type.Name,
                    ToolTip = componentInfo.Component.Description,
                    FontWeight = FontWeights.Bold
                };
            panel.Children.Add(componentLabel);

            BlueprintViewModel viewModel = (BlueprintViewModel)this.DataContext;

            // Add inspectors for component properties.
            foreach (var inspectorProperty in componentInfo.Properties)
            {
                // Get current value.
                object propertyValue;
                if (!viewModel.Blueprint.AttributeTable.TryGetValue(inspectorProperty.Name, out propertyValue))
                {
                    propertyValue = inspectorProperty.Default;
                }

                // Create control for inspector property.
                var propertyControl = this.inspectorFactory.CreateInspectorControlFor(inspectorProperty, propertyValue);
                if (propertyControl == null)
                {
                    continue;
                }

                // Setup control.
                propertyControl.ToolTip = inspectorProperty.Description;

                // Subscribe for change of value.
                propertyControl.ValueChanged += this.OnPropertyControlValueChanged;

                // Add to panel.
                panel.Children.Add((UIElement)propertyControl);
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

            this.OnBlueprintComponentsChanged();
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

            this.OnBlueprintComponentsChanged();
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

        private void OnBlueprintComponentsChanged()
        {
            this.UpdateInspectors();
        }

        private void OnDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            this.UpdateInspectors();
        }

        private void OnPropertyControlValueChanged(
            InspectorPropertyAttribute inspectorProperty, object newValue, object oldValue)
        {
            BlueprintViewModel viewModel = (BlueprintViewModel)this.DataContext;
            Console.WriteLine("Property value {0} changed from {1} to {2}", inspectorProperty, oldValue, newValue);

            // Update blueprint configuration.

            // Remove if default value.
            if (Equals(newValue, inspectorProperty.Default))
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
            foreach (var componentType in viewModel.AddedComponents)
            {
                this.AddComponentInspectors(componentType, this.AttributesPanel);
            }
        }

        #endregion
    }
}