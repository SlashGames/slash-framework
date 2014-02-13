// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ListInspector.xaml.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace BlueprintEditor.Inspectors
{
    using System;
    using System.Collections;
    using System.Windows;

    using Slash.GameBase.Inspector.Attributes;
    using Slash.SystemExt.Utils;

    /// <summary>
    ///   Inspector to show and edit a list of values.
    /// </summary>
    public partial class ListInspector
    {
        #region Fields

        private readonly InspectorFactory inspectorFactory = new InspectorFactory();

        private InspectorPropertyAttribute itemInspectorProperty;

        private Type itemType;

        private IList value;

        #endregion

        #region Constructors and Destructors

        public ListInspector()
        {
            this.InitializeComponent();
            this.DataContextChanged += this.OnDataContextChanged;
        }

        #endregion

        #region Methods

        private void AddItemControl(object item)
        {
            IInspectorControl propertyControl =
                this.inspectorFactory.CreateInspectorControlFor(this.itemInspectorProperty, item);

            // Create item wrapper.
            ListInspectorItem itemWrapperControl = new ListInspectorItem { Control = (InspectorControl)propertyControl };
            itemWrapperControl.DeleteClicked += this.OnItemDeleteClicked;
            itemWrapperControl.ValueChanged += this.OnItemValueChanged;

            this.Items.Children.Add(itemWrapperControl);
        }

        private void BtAdd_OnClick(object sender, RoutedEventArgs e)
        {
            InspectorPropertyData dataContext = (InspectorPropertyData)this.DataContext;
            if (this.value == null)
            {
                this.value = (IList)Activator.CreateInstance(dataContext.InspectorProperty.PropertyType);
            }

            object item = Activator.CreateInstance(this.itemType);
            this.value.Add(item);

            dataContext.Value = this.value;

            // Create item control.
            this.AddItemControl(item);
        }

        private void ClearItemControls()
        {
            this.Items.Children.Clear();
        }

        private void DeleteItemControl(ListInspectorItem itemControl)
        {
            // Unregister from events.
            itemControl.DeleteClicked -= this.OnItemDeleteClicked;
            itemControl.ValueChanged -= this.OnItemValueChanged;

            // Remove.
            this.Items.Children.Remove(itemControl);
        }

        private void OnDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            InspectorPropertyData dataContext = (InspectorPropertyData)this.DataContext;

            this.value = (IList)dataContext.Value;
            this.itemType = dataContext.InspectorProperty.PropertyType.GetGenericArguments()[0];
            this.itemInspectorProperty = dataContext.InspectorProperty.Clone();
            this.itemInspectorProperty.List = false;

            // Set items.
            this.ClearItemControls();
            if (this.value != null)
            {
                foreach (var item in this.value)
                {
                    this.AddItemControl(item);
                }
            }
        }

        private void OnItemDeleteClicked(object sender, RoutedEventArgs e)
        {
            ListInspectorItem itemControl = (ListInspectorItem)sender;

            // Get control index.
            int itemIndex = this.Items.Children.IndexOf(itemControl);

            // Delete control.
            this.DeleteItemControl(itemControl);

            // Delete item from list.
            this.value.RemoveAt(itemIndex);

            // List changed.
            this.OnValueChanged();
        }

        private void OnItemValueChanged(object sender, RoutedEventArgs routedEventArgs)
        {
            ListInspectorItem.ValueChangedEventArgs eventArgs = (ListInspectorItem.ValueChangedEventArgs)routedEventArgs;

            ListInspectorItem itemControl = (ListInspectorItem)sender;

            // Get control index.
            int itemIndex = this.Items.Children.IndexOf(itemControl);

            this.value[itemIndex] = eventArgs.NewValue;

            // List changed.
            this.OnValueChanged();
        }

        #endregion
    }
}