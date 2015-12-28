// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BlueprintComboBox.xaml.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace BlueprintEditor.Controls
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;

    using BlueprintEditor.Annotations;
    using BlueprintEditor.ViewModels;

    public partial class BlueprintComboBox : INotifyPropertyChanged
    {
        #region Static Fields

        public static readonly DependencyProperty SelectedBlueprintProperty =
            DependencyProperty.Register(
                "SelectedBlueprint",
                typeof(BlueprintViewModel),
                typeof(BlueprintComboBox),
                new PropertyMetadata(null) { PropertyChangedCallback = OnSelectedBlueprintChanged });

        #endregion

        #region Fields

        private ListCollectionView blueprintsView;

        private string selectedBlueprintId;

        #endregion

        #region Constructors and Destructors

        public BlueprintComboBox()
        {
            this.InitializeComponent();

            this.DataContextChanged += this.OnDataContextChanged;

            this.ComboBox.DataContext = this;
            this.ComboBox.SelectionChanged += this.OnSelectionChanged;
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

        /// <summary>
        ///   Method used to determine whether a blueprint is shown in this combo box.
        /// </summary>
        public Predicate<BlueprintViewModel> Filter { get; set; }

        /// <summary>
        ///   Current selected blueprint.
        /// </summary>
        public BlueprintViewModel SelectedBlueprint
        {
            get
            {
                return (BlueprintViewModel)this.GetValue(SelectedBlueprintProperty);
            }
            set
            {
                this.SetValue(SelectedBlueprintProperty, value);
            }
        }

        /// <summary>
        ///   Current selected blueprint id.
        /// </summary>
        public string SelectedBlueprintId
        {
            get
            {
                return this.selectedBlueprintId;
            }
            set
            {
                if (value == this.selectedBlueprintId)
                {
                    return;
                }

                this.selectedBlueprintId = value;

                this.UpdateSelectedBlueprint();
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///   Updates the item collection of this combo box from the context.
        /// </summary>
        public void Refresh()
        {
            // TODO(np): Why is this not automatically updated by the binding upon setting the context?
            this.ComboBox.ItemsSource = this.Blueprints;
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

        private static void OnSelectedBlueprintChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            BlueprintComboBox comboBox = (BlueprintComboBox)d;
            comboBox.OnPropertyChanged("SelectedBlueprint");
        }

        private ListCollectionView CreateBlueprintsView()
        {
            BlueprintManagerViewModel dataContext = (BlueprintManagerViewModel)this.DataContext;
            if (dataContext == null)
            {
                return null;
            }

            ListCollectionView newBlueprintsView = new ListCollectionView(dataContext.Blueprints);
            newBlueprintsView.SortDescriptions.Add(new SortDescription("BlueprintId", ListSortDirection.Ascending));

            if (this.Filter != null)
            {
                newBlueprintsView.Filter = blueprint => this.Filter((BlueprintViewModel)blueprint);
            }

            return newBlueprintsView;
        }

        private bool ignoreSelectionChange;

        private void OnDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            ListCollectionView newBlueprints = this.CreateBlueprintsView();

            // The selected item of the combo box may change if the available items change.
            // This change is ignored and the combo box's selected item is updated afterwards,
            // so the data and the view are consistent.
            this.ignoreSelectionChange = true;
            this.Blueprints = newBlueprints;
            this.ignoreSelectionChange = false;
            this.UpdateSelectedBlueprint();
        }

        private void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.ignoreSelectionChange)
            {
                return;
            }

            BlueprintViewModel newSelectedBlueprint = (BlueprintViewModel)this.ComboBox.SelectedItem;
            this.SelectedBlueprint = newSelectedBlueprint;
            this.SelectedBlueprintId = newSelectedBlueprint != null ? newSelectedBlueprint.BlueprintId : null;
        }

        private void UpdateSelectedBlueprint()
        {
            // Search for view model in current blueprints.
            BlueprintManagerViewModel dataContext = (BlueprintManagerViewModel)this.DataContext;
            var selectedBlueprint = dataContext != null
                                        ? dataContext.Blueprints.FirstOrDefault(
                                            blueprint => blueprint.BlueprintId == this.selectedBlueprintId)
                                        : null;

            this.ComboBox.SelectedItem = selectedBlueprint;
        }

        #endregion
    }
}