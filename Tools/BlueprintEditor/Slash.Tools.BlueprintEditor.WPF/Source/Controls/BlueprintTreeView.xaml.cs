// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BlueprintTreeView.xaml.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace BlueprintEditor.Windows
{
    using System;
    using System.Media;
    using System.Windows;

    using BlueprintEditor.Windows.Controls;

    using Slash.GameBase.Blueprints;

    public class BlueprintSelectionChangedEventArgs : RoutedEventArgs
    {
        public BlueprintSelectionChangedEventArgs(RoutedEvent routedEvent, object source)
            : base(routedEvent, source)
        { }

        public Blueprint Blueprint { get; set; }

        public string BlueprintId { get; set; }
    }

    /// <summary>
    ///   Interaction logic for BlueprintTreeView.xaml
    /// </summary>
    public partial class BlueprintTreeView
    {
        #region Static Fields

        public static readonly RoutedEvent BlueprintSelectionChangedEvent =
            EventManager.RegisterRoutedEvent(
                "BlueprintSelectionChanged",
                RoutingStrategy.Bubble,
                typeof(RoutedEventHandler),
                typeof(BlueprintTreeView));

        #endregion

        #region Fields

        /// <summary>
        ///   Blueprint manager to visualize with tree view.
        /// </summary>
        private BlueprintManager blueprintManager;

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
        }

        private void OnSelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (this.isUpdating)
            {
                return;
            }

            BlueprintTreeViewItem selectedItem = (BlueprintTreeViewItem)this.TvTree.SelectedItem;
            RaiseEvent(
                new BlueprintSelectionChangedEventArgs(BlueprintSelectionChangedEvent, sender)
                    {
                        Blueprint = selectedItem != null ? selectedItem.Blueprint : null,
                        BlueprintId = selectedItem != null ? selectedItem.BlueprintId : null
                    });
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

        /// <summary>
        ///   Blueprint manager to visualize with tree view.
        /// </summary>
        public BlueprintManager BlueprintManager
        {
            get
            {
                return this.blueprintManager;
            }
            set
            {
                if (ReferenceEquals(value, this.blueprintManager))
                {
                    return;
                }

                if (this.blueprintManager != null)
                {
                    this.blueprintManager.BlueprintsChanged -= this.OnBlueprintsChanged;
                }

                this.blueprintManager = value;

                if (this.blueprintManager != null)
                {
                    this.blueprintManager.BlueprintsChanged += this.OnBlueprintsChanged;
                }

                this.UpdateTreeView();
            }
        }

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

        #region Methods

        private void BtAddBlueprint_OnClick(object sender, RoutedEventArgs e)
        {
            string newBlueprintId = this.TbNewBlueprintId.Text;
            try
            {
                this.BlueprintManager.AddBlueprint(newBlueprintId, new Blueprint());
            }
            catch (ArgumentException ex)
            {
                SystemSounds.Hand.Play();
                //this.TbMessage.Text = ex.Message;
            }
        }

        private void BtDeleteBlueprint_OnClick(object sender, RoutedEventArgs e)
        {
            // Check if an item is selected.
            BlueprintTreeViewItem selectedItem = this.SelectedItem;
            if (selectedItem == null)
            {
                return;
            }

            // Delete current selected blueprint.
            this.BlueprintManager.RemoveBlueprint(selectedItem.BlueprintId);
        }

        private void OnBlueprintsChanged()
        {
            this.UpdateTreeView();
        }

        private void UpdateTreeView()
        {
            if (this.TvTree == null)
            {
                return;
            }

            this.isUpdating = true;

            // Store old selected item.
            BlueprintTreeViewItem selectedItem = this.SelectedItem;

            this.TvTree.Items.Clear();
            if (this.blueprintManager == null)
            {
                this.isUpdating = false;
                return;
            }

            foreach (var blueprintPair in this.blueprintManager.Blueprints)
            {
                BlueprintTreeViewItem blueprintTreeViewItem = new BlueprintTreeViewItem(blueprintPair.Key, blueprintPair.Value);

                // Restore selected item.
                if (selectedItem != null && ReferenceEquals(selectedItem.Blueprint, blueprintPair.Value))
                {
                    blueprintTreeViewItem.IsSelected = true;
                }

                this.TvTree.Items.Add(blueprintTreeViewItem);
            }

            this.isUpdating = false;
        }

        #endregion
    }
}