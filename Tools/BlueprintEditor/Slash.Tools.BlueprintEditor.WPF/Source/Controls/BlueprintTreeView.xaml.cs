// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BlueprintTreeView.xaml.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace BlueprintEditor.Controls
{
    using System;
    using System.Windows;
    using System.Windows.Input;

    using BlueprintEditor.ViewModels;

    using Slash.GameBase.Blueprints;
    using Slash.Tools.BlueprintEditor.Logic.Context;

    public class BlueprintSelectionChangedEventArgs : RoutedEventArgs
    {
        #region Constructors and Destructors

        public BlueprintSelectionChangedEventArgs(RoutedEvent routedEvent, object source)
            : base(routedEvent, source)
        {
        }

        #endregion

        #region Public Properties

        public BlueprintViewModel Blueprint { get; set; }

        #endregion
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

        #region Properties

        /// <summary>
        ///   Returns the selected item.
        /// </summary>
        private BlueprintViewModel SelectedItem
        {
            get
            {
                if (this.TvTree == null || this.TvTree.SelectedItem == null)
                {
                    return null;
                }

                return (BlueprintViewModel)this.TvTree.SelectedItem;
            }
        }

        #endregion

        #region Methods

        private void BtDeleteBlueprint_OnClick(object sender, RoutedEventArgs e)
        {
            // Check if an item is selected.
            BlueprintViewModel selectedItem = this.SelectedItem;
            if (selectedItem == null)
            {
                return;
            }

            // Delete current selected blueprint.
            ((BlueprintManagerViewModel)this.DataContext).RemoveBlueprint(selectedItem.BlueprintId);
        }

        private void CanExecuteCreateNewBlueprint(object sender, CanExecuteRoutedEventArgs e)
        {
            BlueprintManagerViewModel viewModel = this.DataContext as BlueprintManagerViewModel;
            e.CanExecute = viewModel == null || viewModel.CanExecuteCreateNewBlueprint();
        }

        private void ExecutedCreateNewBlueprint(object sender, ExecutedRoutedEventArgs e)
        {
            try
            {
                BlueprintManagerViewModel viewModel = this.DataContext as BlueprintManagerViewModel;
                if (viewModel != null)
                {
                    viewModel.CreateNewBlueprint();
                }
            }
            catch (ArgumentException ex)
            {
                EditorDialog.Error("Unable to add blueprint", ex.Message);
            }
        }

        private void OnSelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (this.isUpdating)
            {
                return;
            }

            BlueprintViewModel selectedItem = (BlueprintViewModel)this.TvTree.SelectedItem;
            this.OnSelectedItemChanged(selectedItem);
        }

        private void OnSelectedItemChanged(BlueprintViewModel selectedItem)
        {
            this.RaiseEvent(
                new BlueprintSelectionChangedEventArgs(BlueprintSelectionChangedEvent, this)
                    {
                        Blueprint = selectedItem
                    });
        }

        #endregion
    }
}