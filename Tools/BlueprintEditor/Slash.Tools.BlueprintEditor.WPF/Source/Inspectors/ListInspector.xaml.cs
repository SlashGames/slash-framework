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

        private readonly List<ItemControl> itemControls = new List<ItemControl>();

        private Type itemType;

        private IList value;

        private InspectorPropertyAttribute itemInspectorProperty;

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
            IInspectorControl propertyControl = this.inspectorFactory.CreateInspectorControlFor(this.itemInspectorProperty, item);

            ItemControl itemControl = ItemControl.Create(propertyControl, index);
            itemControl.ValueChanged += this.OnItemValueChanged;
            this.itemControls.Add(itemControl);

            this.Items.Children.Add((UIElement)propertyControl);
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

        private void OnItemValueChanged(ItemControl itemControl, object newValue, object oldValue)
        {
            this.value[itemControl.Index] = newValue;

            InspectorPropertyData dataContext = (InspectorPropertyData)this.DataContext;
            dataContext.Value = this.value;
        }

        #endregion

        private class ItemControl
        {
            #region Public Events

            public event Action<ItemControl, object, object> ValueChanged;

            #endregion

            #region Public Properties

            public IInspectorControl Control { get; set; }

            public int Index { get; set; }

            #endregion

            #region Public Methods and Operators

            public static ItemControl Create(IInspectorControl propertyControl, int index)
            {
                ItemControl itemControl = new ItemControl();
                itemControl.Index = index;
                itemControl.Control = propertyControl;
                itemControl.Control.ValueChanged += itemControl.OnValueChanged;

                return itemControl;
            }

            public static void Destroy(ItemControl itemControl)
            {
                itemControl.Control.ValueChanged -= itemControl.OnValueChanged;
            }

            #endregion

            #region Methods

            private void OnValueChanged(InspectorPropertyAttribute inspectorproperty, object newValue, object oldValue)
            {
                var handler = this.ValueChanged;
                if (handler != null)
                {
                    handler(this, newValue, oldValue);
                }
            }

            #endregion
        }
    }
}