// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BlueprintControl.xaml.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace BlueprintEditor.Controls
{
    using System;
    using System.Collections.Generic;
    using System.Windows;

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

        #endregion

        #region Constructors and Destructors

        public BlueprintControl()
        {
            this.InitializeComponent();
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
        }

        private void BtAdd_OnClick(object sender, RoutedEventArgs e)
        {
            if (this.BlueprintControlContext.Blueprint == null)
            {
                // TODO(co): Gray out button.
                MessageBox.Show("No blueprint selected.");
                return;
            }

            // Get selected component type of available component types.
            Type componentType = (Type)this.LbComponentsAvailable.SelectedItem;
            if (componentType == null)
            {
                // TODO(co): Gray out button.
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
        }

        private void BtRemove_OnClick(object sender, RoutedEventArgs e)
        {
            if (this.BlueprintControlContext.Blueprint == null)
            {
                // TODO(co): Gray out button.
                MessageBox.Show("No blueprint selected.");
                return;
            }

            // Get selected component type of added component types.
            Type componentType = (Type)this.LbComponentsAdded.SelectedItem;
            if (componentType == null)
            {
                // TODO(co): Gray out button.
                MessageBox.Show("No component type selected.");
                return;
            }

            // Remove component type from blueprint.
            this.BlueprintControlContext.Blueprint.ComponentTypes.Remove(componentType);

            // Remove from available, add to blueprint component types.
            this.LbComponentsAdded.Items.Remove(componentType);
            this.LbComponentsAvailable.Items.Add(componentType);

            // Select component type.
            this.LbComponentsAvailable.SelectedItem = componentType;
        }

        private void UpdateAvailableComponents()
        {
            // Clear items.
            this.LbComponentsAvailable.Items.Clear();

            if (this.AvailableComponentTypes == null || this.BlueprintControlContext == null)
            {
                return;
            }

            // Add available component types.
            foreach (var componentType in this.AvailableComponentTypes)
            {
                if (this.BlueprintControlContext.Blueprint != null
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

        #endregion
    }
}