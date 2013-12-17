// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BlueprintTreeView.xaml.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace BlueprintEditor.Controls
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Media;
    using System.Windows;
    using System.Windows.Input;

    using Slash.GameBase.Blueprints;

    public class BlueprintSelectionChangedEventArgs : RoutedEventArgs
    {
        #region Constructors and Destructors

        public BlueprintSelectionChangedEventArgs(RoutedEvent routedEvent, object source)
            : base(routedEvent, source)
        {
        }

        #endregion

        #region Public Properties

        public Blueprint Blueprint { get; set; }

        public string BlueprintId { get; set; }

        #endregion
    }

    /// <summary>
    ///   Interaction logic for BlueprintTreeView.xaml
    /// </summary>
    public partial class BlueprintTreeView : IDataErrorInfo
    {
        #region Static Fields

        public static readonly RoutedEvent BlueprintSelectionChangedEvent =
            EventManager.RegisterRoutedEvent(
                "BlueprintSelectionChanged",
                RoutingStrategy.Bubble,
                typeof(RoutedEventHandler),
                typeof(BlueprintTreeView));

        public static RoutedCommand NewBlueprintCommand = new RoutedCommand();

        #endregion

        #region Fields

        /// <summary>
        ///   Indicates that the tree view is currently updated, so no selection event should be raised.
        /// </summary>
        private bool isUpdating;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///   Constructor.
        /// </summary>
        public BlueprintTreeView()
        {
            this.InitializeComponent();

            this.TvTree.SelectedItemChanged += this.OnSelectedItemChanged;
            this.DataContextChanged += this.OnDataContextChanged;
        }

        #endregion

        #region Public Events

        public event RoutedEventHandler BlueprintSelectionChanged
        {
            add
            {
                this.AddHandler(BlueprintSelectionChangedEvent, value);
            }
            remove
            {
                this.RemoveHandler(BlueprintSelectionChangedEvent, value);
            }
        }

        #endregion

        #region Public Properties

        public string Error { get; private set; }

        public string NewBlueprintId { get; set; }

        /// <summary>
        ///   Returns the selected item.
        /// </summary>
        public BlueprintTreeViewItem SelectedItem
        {
            get
            {
                if (this.TvTree == null || this.TvTree.SelectedItem == null)
                {
                    return null;
                }

                return (BlueprintTreeViewItem)this.TvTree.SelectedItem;
            }
        }

        #endregion

        #region Public Indexers

        public string this[string columnName]
        {
            get
            {
                string result = null;
                if (columnName == "NewBlueprintId")
                {
                    // Check if already existent.
                    BlueprintManager blueprintManager = (BlueprintManager)this.DataContext;
                    if (blueprintManager != null)
                    {
                        if (blueprintManager.ContainsBlueprint(this.NewBlueprintId))
                        {
                            result = "Blueprint id already exists.";
                        }
                    }
                }

                return result;
            }
        }

        #endregion

        #region Methods

        private void BtDeleteBlueprint_OnClick(object sender, RoutedEventArgs e)
        {
            // Check if an item is selected.
            BlueprintTreeViewItem selectedItem = this.SelectedItem;
            if (selectedItem == null)
            {
                return;
            }

            // Delete current selected blueprint.
            ((BlueprintManager)this.DataContext).RemoveBlueprint(selectedItem.BlueprintId);
        }

        private void CanExecuteCreateNewBlueprint(object sender, CanExecuteRoutedEventArgs e)
        {
            string newBlueprintId = this.TbNewBlueprintId.Text;
            BlueprintManager blueprintManager = this.DataContext as BlueprintManager;
            e.CanExecute = blueprintManager != null && !string.IsNullOrEmpty(newBlueprintId)
                           && !blueprintManager.ContainsBlueprint(newBlueprintId);
        }

        private void ExecutedCreateNewBlueprint(object sender, ExecutedRoutedEventArgs e)
        {
            string newBlueprintId = this.TbNewBlueprintId.Text;
            try
            {
                ((BlueprintManager)this.DataContext).AddBlueprint(newBlueprintId, new Blueprint());
            }
            catch (ArgumentException ex)
            {
                EditorDialog.Error("Unable to add blueprint", ex.Message);
            }

            // Clear textbox.
            this.TbNewBlueprintId.Text = string.Empty;
        }

        private void OnBlueprintsChanged()
        {
            this.UpdateTreeView((BlueprintManager)this.DataContext);
        }

        private void OnDataContextChanged(
            object sender, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            BlueprintManager oldBlueprintManager = dependencyPropertyChangedEventArgs.OldValue as BlueprintManager;
            if (oldBlueprintManager != null)
            {
                oldBlueprintManager.BlueprintsChanged -= this.OnBlueprintsChanged;
            }

            BlueprintManager newBlueprintManager = dependencyPropertyChangedEventArgs.NewValue as BlueprintManager;
            if (newBlueprintManager != null)
            {
                newBlueprintManager.BlueprintsChanged += this.OnBlueprintsChanged;
            }

            this.UpdateTreeView(newBlueprintManager);
        }

        private void OnSelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (this.isUpdating)
            {
                return;
            }

            BlueprintTreeViewItem selectedItem = (BlueprintTreeViewItem)this.TvTree.SelectedItem;
            this.OnSelectedItemChanged(selectedItem);
        }

        private void OnSelectedItemChanged(BlueprintTreeViewItem selectedItem)
        {
            this.RaiseEvent(
                new BlueprintSelectionChangedEventArgs(BlueprintSelectionChangedEvent, this)
                    {
                        Blueprint = selectedItem != null ? selectedItem.Blueprint : null,
                        BlueprintId = selectedItem != null ? selectedItem.BlueprintId : null
                    });
        }

        private void UpdateTreeView(BlueprintManager blueprintManager)
        {
            if (this.TvTree == null)
            {
                return;
            }

            this.isUpdating = true;

            // Store old selected item.
            BlueprintTreeViewItem selectedItem = this.SelectedItem;

            this.TvTree.Items.Clear();
            if (blueprintManager == null)
            {
                this.isUpdating = false;
                return;
            }

            bool oldItemStillExists = false;
            IEnumerable<KeyValuePair<string, Blueprint>> blueprints =
                blueprintManager.Blueprints.OrderBy(blueprintPair => blueprintPair.Key);
            foreach (var blueprintPair in blueprints)
            {
                BlueprintTreeViewItem blueprintTreeViewItem = new BlueprintTreeViewItem(
                    blueprintPair.Key, blueprintPair.Value);

                // Restore selected item.
                if (selectedItem != null && ReferenceEquals(selectedItem.Blueprint, blueprintPair.Value))
                {
                    blueprintTreeViewItem.IsSelected = true;
                    oldItemStillExists = true;
                }

                this.TvTree.Items.Add(blueprintTreeViewItem);
            }

            this.isUpdating = false;

            // If old item doesn't exist anymore, the selection changed.
            if (selectedItem != null && !oldItemStillExists)
            {
                this.OnSelectedItemChanged(null);
            }
        }

        #endregion
    }
}