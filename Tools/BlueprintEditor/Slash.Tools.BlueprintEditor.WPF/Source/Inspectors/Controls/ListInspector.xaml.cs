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
            itemWrapperControl.DownClicked += this.OnItemDownClicked;
            itemWrapperControl.UpClicked += this.OnItemUpClicked;
            itemWrapperControl.ValueChanged += this.OnItemValueChanged;

            this.Items.Children.Add(itemWrapperControl);
        }

        private void BtAdd_OnClick(object sender, RoutedEventArgs e)
        {
            IList items = this.GetListToModify();

            object item = this.itemType == typeof(string) ? string.Empty : Activator.CreateInstance(this.itemType);
            items.Add(item);

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
            itemControl.DownClicked -= this.OnItemDownClicked;
            itemControl.UpClicked -= this.OnItemUpClicked;
            itemControl.ValueChanged -= this.OnItemValueChanged;

            // Remove.
            this.Items.Children.Remove(itemControl);
        }

        private void MoveItem(ListInspectorItem itemControl, int deltaMove)
        {
            if (deltaMove == 0)
            {
                return;
            }

            // Get control index.
            int itemIndex = this.Items.Children.IndexOf(itemControl);

            int newItemIndex = itemIndex + deltaMove;

            // Check range.
            if (newItemIndex < 0 || newItemIndex >= this.Items.Children.Count)
            {
                return;
            }

            var items = this.GetListToModify();
            var item = items[itemIndex];

            // Remove from old index.
            this.Items.Children.RemoveAt(itemIndex);
            items.RemoveAt(itemIndex);
            
            // Add to new index.
            this.Items.Children.Insert(newItemIndex, itemControl);
            items.Insert(newItemIndex, item);
        }

        private IList GetListToModify()
        {
            // Copy inherited value if necessary.
            IList list;
            InspectorPropertyData dataContext = (InspectorPropertyData)this.DataContext;
            if (dataContext.Value == null || dataContext.ValueInherited)
            {
                list = this.CopyInheritedValue((IList)dataContext.Value);
                dataContext.ValueInherited = false;
                dataContext.Value = list;
            }
            else
            {
                list = (IList)dataContext.Value;
            }
            return list;
        }

        private void OnDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            InspectorPropertyData dataContext = (InspectorPropertyData)this.DataContext;

            InspectorPropertyAttribute inspectorProperty = dataContext.InspectorProperty;
            this.itemType = inspectorProperty.AttributeType ?? inspectorProperty.PropertyType.GetGenericArguments()[0];
            this.itemInspectorProperty = inspectorProperty.Clone();
            this.itemInspectorProperty.List = false;

            // Set items.
            this.ClearItemControls();
            IList items = (IList)dataContext.Value;
            if (items != null)
            {
                foreach (var item in items)
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

            IList items = this.GetListToModify();

            // Delete item from list.
            items.RemoveAt(itemIndex);

            // List changed.
            this.OnValueChanged();
        }

        private void OnItemDownClicked(object sender, RoutedEventArgs e)
        {
            ListInspectorItem itemControl = (ListInspectorItem)sender;
            this.MoveItem(itemControl, +1);
        }

        private void OnItemUpClicked(object sender, RoutedEventArgs e)
        {
            ListInspectorItem itemControl = (ListInspectorItem)sender;
            this.MoveItem(itemControl, -1);
        }

        private void OnItemValueChanged(object sender, RoutedEventArgs routedEventArgs)
        {
            ListInspectorItem.ValueChangedEventArgs eventArgs = (ListInspectorItem.ValueChangedEventArgs)routedEventArgs;

            ListInspectorItem itemControl = (ListInspectorItem)sender;

            // Get control index.
            int itemIndex = this.Items.Children.IndexOf(itemControl);

            IList items = this.GetListToModify();
            items[itemIndex] = eventArgs.NewValue;

            // List changed.
            this.OnValueChanged();
        }

        #endregion
    }
}