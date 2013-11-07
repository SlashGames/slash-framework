// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BlueprintControl.xaml.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace BlueprintEditor.Windows
{
    using System;
    using System.Collections.Generic;
    using System.Windows;

    using Slash.GameBase.Blueprints;

    /// <summary>
    ///   Interaction logic for BlueprintControl.xaml
    /// </summary>
    public partial class BlueprintControl
    {
        #region Static Fields

        public static readonly DependencyProperty AvailableComponentTypesProperty =
            DependencyProperty.Register(
                "AvailableComponentTypes",
                typeof(IEnumerable<Type>),
                typeof(BlueprintControl),
                new FrameworkPropertyMetadata(null, OnAvailableComponentTypesChanged));

        public static readonly DependencyProperty BlueprintIdProperty = DependencyProperty.Register(
            "BlueprintId",
            typeof(string),
            typeof(BlueprintControl),
            new FrameworkPropertyMetadata(null, OnBlueprintIdChanged));

        #endregion

        #region Fields

        private Blueprint blueprint;

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

        public Blueprint Blueprint
        {
            get
            {
                return this.blueprint;
            }
            set
            {
                if (ReferenceEquals(value, this.blueprint))
                {
                    return;
                }

                this.blueprint = value;

                this.UpdateBlueprintComponents();
                this.UpdateAvailableComponents();
            }
        }

        public string BlueprintId
        {
            get
            {
                return (string)this.GetValue(BlueprintIdProperty);
            }
            set
            {
                this.SetValue(BlueprintIdProperty, value);
            }
        }

        #endregion

        #region Methods

        private static void OnAvailableComponentTypesChanged(
            DependencyObject source, DependencyPropertyChangedEventArgs e)
        {
            ((BlueprintControl)source).UpdateAvailableComponents();
        }

        private static void OnBlueprintIdChanged(DependencyObject source, DependencyPropertyChangedEventArgs e)
        {
            // TODO(co): Fire event.
        }

        private void BtAdd_OnClick(object sender, RoutedEventArgs e)
        {
            if (this.blueprint == null)
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
            this.blueprint.ComponentTypes.Add(componentType);

            // Remove from available, add to blueprint component types.
            this.LbComponentsAvailable.Items.Remove(componentType);
            this.LbComponentsAdded.Items.Add(componentType);

            // Select component type.
            this.LbComponentsAdded.SelectedItem = componentType;
        }

        private void BtRemove_OnClick(object sender, RoutedEventArgs e)
        {
            if (this.blueprint == null)
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
            this.blueprint.ComponentTypes.Remove(componentType);

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

            if (this.AvailableComponentTypes == null)
            {
                return;
            }

            // Add available component types.
            foreach (var componentType in this.AvailableComponentTypes)
            {
                if (this.blueprint != null && this.blueprint.ComponentTypes.Contains(componentType))
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

            if (this.blueprint == null)
            {
                return;
            }

            // Add existing components.
            foreach (var componentType in this.blueprint.ComponentTypes)
            {
                this.LbComponentsAdded.Items.Add(componentType);
            }
        }

        #endregion
    }
}