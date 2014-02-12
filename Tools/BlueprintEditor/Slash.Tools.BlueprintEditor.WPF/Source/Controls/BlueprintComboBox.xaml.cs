// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BlueprintComboBox.xaml.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace BlueprintEditor.Controls
{
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;
    using System.Windows.Controls;

    using BlueprintEditor.Annotations;
    using BlueprintEditor.ViewModels;


    public partial class BlueprintComboBox : INotifyPropertyChanged
    {
        #region Constructors and Destructors

        public BlueprintComboBox()
        {
            this.InitializeComponent();

            this.CbParentBlueprint.SelectionChanged += this.OnSelectionChanged;
        }

        #endregion

        #region Public Events

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Public Properties

        public ObservableCollection<BlueprintViewModel> Blueprints { get; set; }

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