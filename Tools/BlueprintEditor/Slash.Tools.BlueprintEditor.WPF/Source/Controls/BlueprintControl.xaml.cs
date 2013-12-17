// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BlueprintControl.xaml.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace BlueprintEditor.Controls
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;

    using BlueprintEditor.Inspectors;

    using Slash.GameBase.Inspector.Attributes;
    using Slash.Tools.BlueprintEditor.Logic.Data;

    /// <summary>
    ///   Interaction logic for BlueprintControl.xaml
    /// </summary>
    public sealed partial class BlueprintControl
    {
        #region Static Fields

        public static readonly DependencyProperty AvailableComponentTypesProperty =
            DependencyProperty.Register(
                "AvailableComponentTypes",
                typeof(IEnumerable<Type>),
                typeof(BlueprintControl),
                new FrameworkPropertyMetadata(null, OnAvailableComponentTypesChanged));

        public static readonly DependencyProperty ContextProperty =
            DependencyProperty.Register(
                "BlueprintControlContext",
                typeof(BlueprintControlContext),
                typeof(BlueprintControl),
                new FrameworkPropertyMetadata(null, OnContextChanged));

        /// <summary>
        ///   Command to add component to blueprint.
        /// </summary>
        public static ICommand AddComponentCommand = new RoutedCommand();

        /// <summary>
        ///   Command to remove component from blueprint.
        /// </summary>
        public static ICommand RemoveComponentCommand = new RoutedCommand();

        #endregion

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

            // Check if to disable control.
            this.IsEnabled = this.ShouldBeEnabled;
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///   All available entity component types which can be added to a blueprint.
        /// </summary>
        public IEnumerable<Type> AvailableComponentTypes
        {
            get
            {
                return (IEnumerable<Type>)this.GetValue(AvailableComponentTypesProperty);
            }
            set
            {
                this.SetValue(AvailableComponentTypesProperty, value);
            }
        }

        public BlueprintControlContext BlueprintControlContext
        {
            get
            {
                return (BlueprintControlContext)this.GetValue(ContextProperty);
            }
            set
            {
                this.SetValue(ContextProperty, value);
            }
        }

        #endregion

        #region Properties

        /// <summary>
        ///   Checks if the control should be enabled. It's only enabled when a context and a blueprint is set.
        /// </summary>
        private bool ShouldBeEnabled
        {
            get
            {
                return this.BlueprintControlContext != null && this.BlueprintControlContext.Blueprint != null;
            }
        }

        #endregion

        #region Methods

        private static void OnAvailableComponentTypesChanged(
            DependencyObject source, DependencyPropertyChangedEventArgs e)
        {
            BlueprintControl blueprintControl = ((BlueprintControl)source);
            blueprintControl.UpdateAvailableComponents();
        }

        private static void OnContextChanged(DependencyObject source, DependencyPropertyChangedEventArgs e)
        {
            BlueprintControl blueprintControl = (BlueprintControl)source;
            blueprintControl.UpdateBlueprintComponents();
            blueprintControl.UpdateAvailableComponents();
            blueprintControl.UpdateInspectors();

            // Check if to disable control.
            blueprintControl.IsEnabled = blueprintControl.ShouldBeEnabled;
        }

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

            // Add inspectors for component properties.
            foreach (var inspectorProperty in componentInfo.Properties)
            {
                // Get current value.
                object propertyValue;
                if (
                    !this.BlueprintControlContext.Blueprint.AttributeTable.TryGetValue(
                        inspectorProperty.Name, out propertyValue))
                {
                    propertyValue = inspectorProperty.Default;
                }

                // Create control for inspector property.
                var propertyControl = this.inspectorFactory.CreateInspectorControlFor(inspectorProperty, propertyValue);
                if (propertyControl == null)
                {
                    continue;
                }

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
            if (this.BlueprintControlContext.Blueprint == null)
            {
                MessageBox.Show("No blueprint selected.");
                return;
            }

            // Get selected component type of available component types.
            Type componentType = (Type)this.LbComponentsAvailable.SelectedItem;
            if (componentType == null)
            {
                MessageBox.Show("No component type selected.");
                return;
            }

            // Add component type to blueprint.
            this.BlueprintControlContext.Blueprint.ComponentTypes.Add(componentType);

            // Remove from available, add to blueprint component types.
            this.LbComponentsAvailable.Items.Remove(componentType);
            this.LbComponentsAdded.Items.Add(componentType);

            // Select component type.
            this.LbComponentsAdded.SelectedItem = componentType;

            this.OnBlueprintComponentsChanged();
        }

        private void ExecutedRemoveComponent(object sender, ExecutedRoutedEventArgs e)
        {
            if (this.BlueprintControlContext.Blueprint == null)
            {
                MessageBox.Show("No blueprint selected.");
                return;
            }

            // Get selected component type of added component types.
            Type componentType = (Type)this.LbComponentsAdded.SelectedItem;
            if (componentType == null)
            {
                MessageBox.Show("No component type selected.");
                return;
            }

            // Remove component type from blueprint.
            this.BlueprintControlContext.Blueprint.ComponentTypes.Remove(componentType);

            // Remove from available, add to blueprint component types.
            this.LbComponentsAdded.Items.Remove(componentType);
            if (this.AvailableComponentTypes.Contains(componentType))
            {
                this.LbComponentsAvailable.Items.Add(componentType);
            }

            // Select component type.
            this.LbComponentsAvailable.SelectedItem = componentType;

            this.OnBlueprintComponentsChanged();
        }

        private void LbComponentsAdded_OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            // Send command.
            RemoveComponentCommand.Execute(null);
        }

        private void LbComponentsAvailable_OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            // Send command.
            AddComponentCommand.Execute(null);
        }

        private void OnBlueprintComponentsChanged()
        {
            this.UpdateInspectors();
        }

        private void OnPropertyControlValueChanged(
            InspectorPropertyAttribute inspectorProperty, object newValue, object oldValue)
        {
            Console.WriteLine("Property value {0} changed from {1} to {2}", inspectorProperty, oldValue, newValue);

            // Update blueprint configuration.

            // Remove if default value.
            if (Equals(newValue, inspectorProperty.Default))
            {
                this.BlueprintControlContext.Blueprint.AttributeTable.RemoveValue(inspectorProperty.Name);
            }
            else
            {
                this.BlueprintControlContext.Blueprint.AttributeTable.SetValue(inspectorProperty.Name, newValue);
            }
        }

        private void UpdateAvailableComponents()
        {
            // Clear items.
            this.LbComponentsAvailable.Items.Clear();

            if (this.AvailableComponentTypes == null)
            {
                return;
            }

            // Add available component types.
            foreach (var componentType in this.AvailableComponentTypes)
            {
                if (this.BlueprintControlContext != null && this.BlueprintControlContext.Blueprint != null
                    && this.BlueprintControlContext.Blueprint.ComponentTypes.Contains(componentType))
                {
                    continue;
                }

                this.LbComponentsAvailable.Items.Add(componentType);
            }
        }

        private void UpdateBlueprintComponents()
        {
            // Clear items.
            this.LbComponentsAdded.Items.Clear();

            if (this.BlueprintControlContext.Blueprint == null)
            {
                return;
            }

            // Add existing components.
            foreach (var componentType in this.BlueprintControlContext.Blueprint.ComponentTypes)
            {
                this.LbComponentsAdded.Items.Add(componentType);
            }
        }

        private void UpdateInspectors()
        {
            // Clear inspectors.
            this.AttributesPanel.Children.Clear();

            if (this.BlueprintControlContext == null || this.BlueprintControlContext.Blueprint == null)
            {
                return;
            }

            // Add inspectors for blueprint components.
            foreach (var componentType in this.BlueprintControlContext.Blueprint.ComponentTypes)
            {
                this.AddComponentInspectors(componentType, this.AttributesPanel);
            }
        }

        #endregion
    }
}