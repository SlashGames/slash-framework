// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ProjectExplorer.xaml.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace BlueprintEditor.Controls
{
    using System.ComponentModel;
    using System.Runtime.CompilerServices;
    using System.Windows.Controls;

    using BlueprintEditor.Annotations;
    using BlueprintEditor.ViewModels;

    /// <summary>
    ///   Interaction logic for ProjectExplorer.xaml
    /// </summary>
    public partial class ProjectExplorer : INotifyPropertyChanged
    {
        #region Constructors and Destructors

        public ProjectExplorer()
        {
            this.InitializeComponent();

            this.ProjectFilesListView.SelectionChanged += this.OnSelectionChanged;
        }

        #endregion

        #region Public Events

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Public Properties

        public ProjectFileViewModel SelectedItem
        {
            get
            {
                return (ProjectFileViewModel)this.ProjectFilesListView.SelectedItem;
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