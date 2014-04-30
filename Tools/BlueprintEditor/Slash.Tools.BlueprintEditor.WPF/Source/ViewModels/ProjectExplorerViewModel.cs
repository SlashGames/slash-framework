// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ProjectExplorerViewModel.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace BlueprintEditor.ViewModels
{
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Windows.Data;

    public class ProjectExplorerViewModel
    {
        #region Fields

        private ListCollectionView projectFilesView;

        #endregion

        #region Constructors and Destructors

        public ProjectExplorerViewModel()
        {
            this.ProjectFiles = new ObservableCollection<ProjectFileViewModel>();
        }

        #endregion

        #region Public Properties

        public ObservableCollection<ProjectFileViewModel> ProjectFiles { get; private set; }

        public ListCollectionView ProjectFilesView
        {
            get
            {
                if (this.projectFilesView == null)
                {
                    this.projectFilesView = new ListCollectionView(this.ProjectFiles);
                    this.projectFilesView.SortDescriptions.Add(
                        new SortDescription("ProjectFileName", ListSortDirection.Ascending));
                }
                return this.projectFilesView;
            }
        }

        #endregion
    }
}