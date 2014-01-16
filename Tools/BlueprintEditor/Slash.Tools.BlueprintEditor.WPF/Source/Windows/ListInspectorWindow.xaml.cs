// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ListInspectorWindow.xaml.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace BlueprintEditor.Windows
{
    using System.Collections;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;
    using System.Windows;

    using BlueprintEditor.Annotations;
    using BlueprintEditor.Inspectors;

    using Slash.GameBase.Inspector.Attributes;

    /// <summary>
    ///   Allows editing the value of a list property by providing buttons for
    ///   adding new items, editing and removing existing ones.
    /// </summary>
    public partial class ListInspectorWindow : IDataErrorInfo
    {
        #region Fields

        /// <summary>
        ///   Data about the list property being edited.
        /// </summary>
        private InspectorPropertyData propertyData;

        #endregion

        #region Constructors and Destructors

        public ListInspectorWindow()
        {
            this.InitializeComponent();
        }

        #endregion

        #region Public Properties

        public string Error { get; private set; }

        /// <summary>
        ///   Items shown in the list view.
        /// </summary>
        public ObservableCollection<ItemWrapper> Items { get; set; }

        /// <summary>
        ///   Item to add to the list.
        /// </summary>
        public string NewItemText { get; set; }

        #endregion

        #region Public Indexers

        public string this[string columnName]
        {
            get
            {
                // Implements IDataErrorInfo indexer for returning validation error messages.
                if (columnName == "NewItemText")
                {
                    object convertedValue;

                    if (
                        !this.propertyData.InspectorProperty.TryConvertStringToValue(
                            this.TbAdd.Text, out convertedValue))
                    {
                        return InspectorPropertyData.ValidationMessageConversionFailed;
                    }

                    var error = this.propertyData.InspectorProperty.Validate(convertedValue);

                    if (error != null)
                    {
                        return error.Message;
                    }
                }

                return null;
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///   Gets the current list property value as edited by the user.
        /// </summary>
        /// <returns>Current list property value.</returns>
        public IList GetPropertyValue()
        {
            // Convert item wrapper to actual items.
            var list = this.propertyData.InspectorProperty.GetEmptyList();

            foreach (var item in this.Items)
            {
                list.Add(item.Item);
            }

            return list;
        }

        public void Init(InspectorPropertyData propertyData)
        {
            this.propertyData = propertyData;
            this.DataContext = this;

            // Set title.
            this.Title = string.Format("List Inspector - {0}", propertyData.InspectorProperty.Name);

            // Setup list view.
            this.Items = new ObservableCollection<ItemWrapper>();

            var list = (IEnumerable)propertyData.Value;
            if (list != null)
            {
                foreach (var item in list)
                {
                    var wrapper = ItemWrapper.WrapListItem(this.propertyData.InspectorProperty, item);

                    if (wrapper != null)
                    {
                        this.Items.Add(wrapper);
                    }
                }
            }
        }

        #endregion

        #region Methods

        private void OnAdd(object sender, RoutedEventArgs e)
        {
            var wrapper = ItemWrapper.WrapListItem(this.propertyData.InspectorProperty, this.TbAdd.Text);

            if (wrapper != null)
            {
                this.Items.Add(wrapper);
            }
        }

        private void OnClose(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void OnRemove(object sender, RoutedEventArgs e)
        {
            this.Items.RemoveAt(this.ListView.SelectedIndex);
        }

        #endregion

        /// <summary>
        ///   Wraps a list item, enabling the list view to update when the user
        ///   edits the list item.
        /// </summary>
        public class ItemWrapper : INotifyPropertyChanged
        {
            #region Fields

            /// <summary>
            ///   Wrapped list item.
            /// </summary>
            private object item;

            #endregion

            #region Constructors and Destructors

            private ItemWrapper()
            {
            }

            #endregion

            #region Public Events

            public event PropertyChangedEventHandler PropertyChanged;

            #endregion

            #region Public Properties

            /// <summary>
            ///   Data about the list property being edited. Used for validating of list items.
            /// </summary>
            public InspectorPropertyAttribute InspectorProperty { get; set; }

            /// <summary>
            ///   Wrapped list item.
            /// </summary>
            public object Item
            {
                get
                {
                    return this.item;
                }

                set
                {
                    object convertedValue;

                    if (value is string)
                    {
                        if (!this.InspectorProperty.TryConvertStringToValue((string)value, out convertedValue))
                        {
                            return;
                        }
                    }
                    else
                    {
                        convertedValue = value;
                    }

                    var error = this.InspectorProperty.Validate(convertedValue);

                    if (error == null)
                    {
                        this.item = convertedValue;
                        this.OnPropertyChanged();
                    }
                }
            }

            #endregion

            #region Public Methods and Operators

            /// <summary>
            ///   Checks whether the specified item is valid for the passed list property, and returns a list view wrapper for that item, if it is valid, and null otherwise.
            /// </summary>
            /// <param name="listProperty">List property used for validation.</param>
            /// <param name="itemToWrap">Item to wrap.</param>
            /// <returns>List view wrapper for that item, if it is valid, and null otherwise.</returns>
            public static ItemWrapper WrapListItem(InspectorPropertyAttribute listProperty, object itemToWrap)
            {
                var wrapper = new ItemWrapper { InspectorProperty = listProperty, Item = itemToWrap };
                return wrapper.Item != null ? wrapper : null;
            }

            #endregion

            #region Methods

            [NotifyPropertyChangedInvocator]
            protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
            {
                var handler = this.PropertyChanged;
                if (handler != null)
                {
                    handler(this, new PropertyChangedEventArgs(propertyName));
                }
            }

            #endregion
        }
    }
}