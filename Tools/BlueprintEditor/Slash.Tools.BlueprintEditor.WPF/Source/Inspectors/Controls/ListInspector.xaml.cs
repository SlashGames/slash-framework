// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ListInspector.xaml.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace BlueprintEditor.Inspectors.Controls
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
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
                this.inspectorFactory.CreateInspectorControlFor(this.itemInspectorProperty, item, false);

            // Create item wrapper.
            ListInspectorItem itemWrapperControl = new ListInspectorItem { Control = (InspectorControl)propertyControl };
            itemWrapperControl.DeleteClicked += this.OnItemDeleteClicked;
            itemWrapperControl.ValueChanged += this.OnItemValueChanged;

            this.Items.Children.Add(itemWrapperControl);
        }

        private void BtAdd_OnClick(object sender, RoutedEventArgs e)
        {
            InspectorPropertyData dataContext = (InspectorPropertyData)this.DataContext;
            if (this.value == null || dataContext.ValueInherited)
            {
                this.value = this.CopyInheritedValue(this.value);
                dataContext.ValueInherited = false;
                dataContext.Value = this.value;
            }

            object item = this.itemType == typeof(string) ? string.Empty : Activator.CreateInstance(this.itemType);
            this.value.Add(item);

            // Create item control.
            this.AddItemControl(item);

            // Value changed.
            this.OnValueChanged();
        }

        private void ClearItemControls()
        {
            this.Items.Children.Clear();
        }

        private IList CopyInheritedValue(IEnumerable inheritedList)
        {
            IList newList = (IList)Activator.CreateInstance(typeof(List<>).MakeGenericType(this.itemType));
            if (inheritedList != null)
            {
                // Copy inherited items.
                foreach (var inheritedItem in inheritedList)
                {
                    newList.Add(inheritedItem.Clone());
                }
            }
            return newList;
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
            InspectorPropertyAttribute inspectorProperty = dataContext.InspectorProperty;
            this.itemType = inspectorProperty.AttributeType ?? inspectorProperty.PropertyType.GetGenericArguments()[0];
            this.itemInspectorProperty = inspectorProperty.Clone();
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

            // Copy inherited value if necessary.
            InspectorPropertyData dataContext = (InspectorPropertyData)this.DataContext;
            if (dataContext.ValueInherited)
            {
                this.value = this.CopyInheritedValue(this.value);
                dataContext.ValueInherited = false;
                dataContext.Value = this.value;
            }

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