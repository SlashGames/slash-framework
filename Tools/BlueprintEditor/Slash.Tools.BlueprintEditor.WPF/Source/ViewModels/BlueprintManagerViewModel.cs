// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BlueprintManagerViewModel.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace BlueprintEditor.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Collections.Specialized;
    using System.ComponentModel;
    using System.Linq;
    using System.Windows.Data;

    using MonitoredUndo;

    using Slash.GameBase.Blueprints;
    using Slash.Tools.BlueprintEditor.Logic.Annotations;

    public class BlueprintManagerViewModel : INotifyPropertyChanged, IDataErrorInfo, ISupportsUndo
    {
        #region Fields

        private readonly BlueprintManager blueprintManager;

        private ListCollectionView blueprintsView;

        private string newBlueprintId;

        #endregion

        #region Constructors and Destructors

        public BlueprintManagerViewModel(BlueprintManager blueprintManager)
        {
            this.blueprintManager = blueprintManager;

            this.Blueprints = new ObservableCollection<BlueprintViewModel>();

            IEnumerable<KeyValuePair<string, Blueprint>> blueprints = this.blueprintManager.Blueprints;
            foreach (var blueprintPair in blueprints)
            {
                this.Blueprints.Add(
                    new BlueprintViewModel { BlueprintId = blueprintPair.Key, Blueprint = blueprintPair.Value });
            }

            this.Blueprints.CollectionChanged += this.OnBlueprintsChanged;
        }

        #endregion

        #region Public Events

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Public Properties

        public IEnumerable<KeyValuePair<string, Blueprint>> BlueprintPairs
        {
            get
            {
                return this.blueprintManager.Blueprints;
            }
        }

        public ObservableCollection<BlueprintViewModel> Blueprints { get; private set; }

        public ListCollectionView BlueprintsView
        {
            get
            {
                if (this.blueprintsView == null)
                {
                    this.blueprintsView = new ListCollectionView(this.Blueprints);
                    this.blueprintsView.SortDescriptions.Add(
                        new SortDescription("BlueprintId", ListSortDirection.Ascending));
                }
                return this.blueprintsView;
            }
        }

        public string Error
        {
            get
            {
                return null;
            }
        }

        public string NewBlueprintId
        {
            get
            {
                return this.newBlueprintId;
            }
            set
            {
                if (Equals(value, this.newBlueprintId))
                {
                    return;
                }

                this.newBlueprintId = value;

                this.OnPropertyChanged("NewBlueprintId");
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
                    if (this.blueprintManager != null && this.NewBlueprintId != null)
                    {
                        if (this.blueprintManager.ContainsBlueprint(this.NewBlueprintId))
                        {
                            result = "Blueprint id already exists.";
                        }
                    }
                }

                return result;
            }
        }

        #endregion

        #region Public Methods and Operators

        public bool CanExecuteCreateNewBlueprint()
        {
            return this.blueprintManager != null && !String.IsNullOrEmpty(this.newBlueprintId)
                   && !this.blueprintManager.ContainsBlueprint(this.newBlueprintId);
        }

        public void CreateNewBlueprint()
        {
            // Update blueprint view models.
            this.Blueprints.Add(
                new BlueprintViewModel { BlueprintId = this.newBlueprintId, Blueprint = new Blueprint() });

            // Clear blueprint id.
            this.NewBlueprintId = String.Empty;
        }

        public object GetUndoRoot()
        {
            return this;
        }

        public void RemoveBlueprint(string blueprintId)
        {
            this.blueprintManager.RemoveBlueprint(blueprintId);
            this.Blueprints.Remove(this.Blueprints.First(blueprint => blueprint.BlueprintId == blueprintId));
        }

        #endregion

        #region Methods

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = this.PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private void OnBlueprintsChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            string undoMessage = null;
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                // Update blueprints in blueprint manager.
                foreach (BlueprintViewModel item in e.NewItems)
                {
                    this.blueprintManager.AddBlueprint(item.BlueprintId, item.Blueprint);
                }

                undoMessage = "Blueprint added";
                foreach (BlueprintViewModel item in e.NewItems)
                {
                    item.Root = this;
                }
            }
            else if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                // Update blueprints in blueprint manager.
                foreach (BlueprintViewModel item in e.OldItems)
                {
                    this.blueprintManager.RemoveBlueprint(item.BlueprintId);
                }

                undoMessage = "Blueprint removed";
                foreach (BlueprintViewModel item in e.OldItems)
                {
                    item.Root = null;
                }
            }

            // This line will log the collection change with the undo framework.
            DefaultChangeFactory.Current.OnCollectionChanged(
                this, "Blueprints", this.Blueprints, e, undoMessage ?? "Collection of Blueprints changed");
        }

        #endregion
    }
}