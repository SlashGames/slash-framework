// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BlueprintTreeView.xaml.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace BlueprintEditor.Controls
{
    using System;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;
    using System.Windows;
    using System.Windows.Input;

    using BlueprintEditor.Annotations;
    using BlueprintEditor.ViewModels;

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
    public partial class BlueprintTreeView : INotifyPropertyChanged
    {
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

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Public Properties

        /// <summary>
        ///   Returns the selected item.
        /// </summary>
        public BlueprintViewModel SelectedItem
        {
            get
            {
                return (BlueprintViewModel)this.TvTree.SelectedItem;
            }
        }

        #endregion

        #region Methods

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler = this.PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

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
            this.OnPropertyChanged("SelectedItem");
        }

        #endregion
    }
}