// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BlueprintComboBox.xaml.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace BlueprintEditor.Controls
{
    using System;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;
    using System.Windows.Controls;
    using System.Windows.Data;

    using BlueprintEditor.Annotations;
    using BlueprintEditor.ViewModels;

    public partial class BlueprintComboBox : INotifyPropertyChanged
    {
        #region Fields

        private ListCollectionView blueprintsView;

        #endregion

        #region Constructors and Destructors

        public BlueprintComboBox()
        {
            this.InitializeComponent();

            this.CbParentBlueprint.DataContext = this;
            this.CbParentBlueprint.SelectionChanged += this.OnSelectionChanged;
        }

        #endregion

        #region Public Events

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Public Properties

        /// <summary>
        ///   Blueprints shown in the combo box.
        /// </summary>
        public ListCollectionView Blueprints
        {
            get
            {
                if (this.blueprintsView == null)
                {
                    var editorContext = (EditorContext)this.DataContext;
                    this.blueprintsView = new ListCollectionView(editorContext.BlueprintManagerViewModel.Blueprints);
                    this.blueprintsView.SortDescriptions.Add(
                        new SortDescription("BlueprintId", ListSortDirection.Ascending));

                    if (this.Filter != null)
                    {
                        this.blueprintsView.Filter = blueprint => this.Filter((BlueprintViewModel)blueprint);
                    }
                }
                return this.blueprintsView;
            }
        }

        /// <summary>
        ///   Method used to determine whether a blueprint is shown in this combo box.
        /// </summary>
        public Predicate<BlueprintViewModel> Filter { get; set; }

        public BlueprintViewModel SelectedItem
        {
            get
            {
                return (BlueprintViewModel)this.CbParentBlueprint.SelectedItem;
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

        private void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.OnPropertyChanged("SelectedItem");
        }

        #endregion
    }
}