// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ListInspector.xaml.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace BlueprintEditor.Inspectors
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Windows;

    using BlueprintEditor.Windows;

    using Slash.GameBase.Inspector.Attributes;
    using Slash.SystemExt.Utils;

    /// <summary>
    ///   Inspector to show and edit a list of values.
    /// </summary>
    public partial class ListInspector
    {
        #region Fields

        private readonly InspectorFactory inspectorFactory = new InspectorFactory();

        private readonly List<ListInspectorItem> itemControls = new List<ListInspectorItem>();

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

        private void AddItemControl(int index, object item)
        {
            IInspectorControl propertyControl =
                this.inspectorFactory.CreateInspectorControlFor(this.itemInspectorProperty, item);

            // Create item wrapper.
            ListInspectorItem itemWrapperControl = new ListInspectorItem
                {
                    Control = (InspectorControl)propertyControl,
                    ItemIndex = index
                };
            itemWrapperControl.DeleteClicked += this.OnItemDeleteClicked;
            itemWrapperControl.ValueChanged += this.OnItemValueChanged;

            this.itemControls.Add(itemWrapperControl);

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
            int itemIndex = this.value.Count;
            this.value.Add(item);

            dataContext.Value = this.value;

            // Create item control.
            this.AddItemControl(itemIndex, item);
        }

        private void BtEdit_OnClick(object sender, RoutedEventArgs e)
        {
            // Show window for editing the list.
            ListInspectorWindow dlg = new ListInspectorWindow();
            var propertyData = (InspectorPropertyData)this.DataContext;
            dlg.Init(propertyData);
            dlg.ShowDialog();

            // Store edited list.
            propertyData.Value = dlg.GetPropertyValue();
        }

        private void DeleteItemControl(int itemIndex)
        {
            // Check if item exists.
            if (itemIndex >= this.itemControls.Count)
            {
                return;
            }

            // Get control.
            ListInspectorItem itemControl = this.itemControls[itemIndex];

            // Unregister from events.
            itemControl.DeleteClicked -= this.OnItemDeleteClicked;
            itemControl.ValueChanged -= this.OnItemValueChanged;

            // Remove.
            this.itemControls.RemoveAt(itemIndex);
            this.Items.Children.Remove(itemControl);

            // Adjust index of following controls.
            for (int index = itemIndex; index < this.itemControls.Count; index++)
            {
                this.itemControls[index].ItemIndex = index;
            }
        }

        private void OnDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            InspectorPropertyData dataContext = (InspectorPropertyData)this.DataContext;

            this.value = (IList)dataContext.Value;
            this.itemType = dataContext.InspectorProperty.PropertyType.GetGenericArguments()[0];
            this.itemInspectorProperty = dataContext.InspectorProperty.Clone();
            this.itemInspectorProperty.List = false;

            // Set items.
            if (this.value != null)
            {
                for (int index = 0; index < this.value.Count; index++)
                {
                    var item = this.value[index];
                    this.AddItemControl(index, item);
                }
            }
        }

        private void OnItemDeleteClicked(object sender, RoutedEventArgs e)
        {
            ListInspectorItem listInspectorItem = (ListInspectorItem)sender;

            // Delete control at index.
            this.DeleteItemControl(listInspectorItem.ItemIndex);

            // Delete item from list.
            this.value.RemoveAt(listInspectorItem.ItemIndex);

            // List changed.
            this.OnValueChanged();
        }

        private void OnItemValueChanged(object sender, RoutedEventArgs routedEventArgs)
        {
            ListInspectorItem.ValueChangedEventArgs eventArgs = (ListInspectorItem.ValueChangedEventArgs)routedEventArgs;

            ListInspectorItem itemControl = (ListInspectorItem)sender;
            this.value[itemControl.ItemIndex] = eventArgs.NewValue;

            // List changed.
            this.OnValueChanged();
        }

        #endregion
    }
}