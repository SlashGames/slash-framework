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
    using System.Windows;
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

            this.DataContextChanged += this.OnDataContextChanged;

            this.CbParentBlueprint.DataContext = this;
            this.CbParentBlueprint.SelectionChanged += this.OnSelectionChanged;
        }

        private void OnDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            this.Blueprints = this.CreateBlueprintsView();
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
                return this.blueprintsView ?? (this.blueprintsView = this.CreateBlueprintsView());
            }
            set
            {
                this.blueprintsView = value;
                this.OnPropertyChanged();
            }
        }

        private ListCollectionView CreateBlueprintsView()
        {
            BlueprintManagerViewModel dataContext = (BlueprintManagerViewModel)this.DataContext;
            if (dataContext == null)
            {
                return null;
            }

            ListCollectionView newBlueprintsView = new ListCollectionView(dataContext.Blueprints);
            newBlueprintsView.SortDescriptions.Add(
                new SortDescription("BlueprintId", ListSortDirection.Ascending));

            if (this.Filter != null)
            {
                newBlueprintsView.Filter = blueprint => this.Filter((BlueprintViewModel)blueprint);
            }

            return newBlueprintsView;
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