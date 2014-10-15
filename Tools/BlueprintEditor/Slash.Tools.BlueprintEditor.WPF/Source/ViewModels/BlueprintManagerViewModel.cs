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

    using Slash.ECS.Blueprints;
    using Slash.Tools.BlueprintEditor.Logic.Annotations;
    using Slash.Tools.BlueprintEditor.Logic.Data;

    using AggregateException = Slash.SystemExt.Exceptions.AggregateException;

    public class BlueprintManagerViewModel : INotifyPropertyChanged, IDataErrorInfo, ISupportsUndo
    {
        #region Fields

        private readonly HierarchicalBlueprintManager blueprintManager;

        private IEnumerable<Type> assemblyComponents;

        private ListCollectionView blueprintsView;

        private string newBlueprintId;

        #endregion

        #region Constructors and Destructors

        public BlueprintManagerViewModel(IEnumerable<Type> assemblyComponents, HierarchicalBlueprintManager blueprintManager)
        {
            this.AssemblyComponents = assemblyComponents;
            this.blueprintManager = blueprintManager;

            this.Blueprints = new ObservableCollection<BlueprintViewModel>();

            IEnumerable<KeyValuePair<string, Blueprint>> blueprints = this.blueprintManager.Blueprints;
            foreach (var blueprintPair in blueprints.OrderBy(blueprint => blueprint.Key))
            {
                this.Blueprints.Add(
                    new BlueprintViewModel(blueprintPair.Key, blueprintPair.Value)
                        {
                            AssemblyComponents = this.assemblyComponents,
                            Root = this
                        });
            }

            this.CurrentBlueprintManager = (BlueprintManager)this.blueprintManager.Children.First();

            this.Blueprints.CollectionChanged += this.OnBlueprintsChanged;
        }

        #endregion

        #region Public Events

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Public Properties

        /// <summary>
        ///   All available entity component types which can be added to a blueprint.
        /// </summary>
        public IEnumerable<Type> AssemblyComponents
        {
            get
            {
                return this.assemblyComponents;
            }
            set
            {
                this.assemblyComponents = value;

                // Update component table.
                InspectorComponentTable.LoadComponents();

                if (this.Blueprints != null)
                {
                    foreach (var blueprintViewModel in this.Blueprints)
                    {
                        blueprintViewModel.AssemblyComponents = value;
                    }
                }
            }
        }

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

                    // Don't show blueprints at root level that have parents.
                    this.blueprintsView.Filter = blueprint => ((BlueprintViewModel)blueprint).Parent == null;
                }
                return this.blueprintsView;
            }
        }

        /// <summary>
        ///   Actual blueprint manager in the hiearchy to add new blueprints to.
        /// </summary>
        public BlueprintManager CurrentBlueprintManager { get; set; }

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
                    if (this.Blueprints.Any(blueprintViewModel => blueprintViewModel.BlueprintId == this.NewBlueprintId))
                    {
                        result = "Blueprint id already exists.";
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

        public void ChangeBlueprintId(BlueprintViewModel blueprintViewModel, string newBlueprintId)
        {
            if (this.blueprintManager == null)
            {
                return;
            }

            var oldBlueprintId = blueprintViewModel.BlueprintId;
            this.blueprintManager.ChangeBlueprintId(oldBlueprintId, newBlueprintId);
            blueprintViewModel.BlueprintId = newBlueprintId;

            // Validate new blueprint id again as it may be valid/invalid now.
            this.OnPropertyChanged("NewBlueprintId");

            // Update parents.
            foreach (
                var viewModel in this.Blueprints.Where(viewModel => oldBlueprintId.Equals(viewModel.Blueprint.ParentId))
                )
            {
                viewModel.Blueprint.ParentId = newBlueprintId;
            }
        }

        /// <summary>
        ///   Creates a new blueprint. Uses the NewBlueprintId as the id for the new blueprint.
        /// </summary>
        /// <returns>New blueprint view model.</returns>
        public BlueprintViewModel CreateNewBlueprint()
        {
            if (this.CurrentBlueprintManager == null)
            {
                throw new InvalidOperationException("No blueprint file selected. Please select a blueprint file to add the new blueprint to!");
            }

            // Update blueprint view models.
            var newBlueprint = this.CreateNewBlueprint(this.NewBlueprintId);

            // Clear blueprint id.
            this.NewBlueprintId = string.Empty;

            return newBlueprint;
        }

        /// <summary>
        ///   Creates a new blueprint.
        /// </summary>
        /// <param name="blueprintId">Id for new blueprint.</param>
        /// <param name="original">Original blueprint to copy. Null if to create an empty blueprint.</param>
        /// <returns>New blueprint view model.</returns>
        public BlueprintViewModel CreateNewBlueprint(string blueprintId, BlueprintViewModel original = null)
        {
            if (this.CurrentBlueprintManager == null)
            {
                throw new InvalidOperationException("No blueprint file selected. Please select a blueprint file to add the new blueprint to!");
            }

            // Update blueprint view models.
            var newBlueprint = new BlueprintViewModel(
                blueprintId, original != null ? new Blueprint(original.Blueprint) : new Blueprint())
                {
                    AssemblyComponents = this.assemblyComponents
                };

            // Parent under same blueprint.
            if (original != null)
            {
                newBlueprint.Parent = original.Parent;
            }

            this.Blueprints.Add(newBlueprint);

            return newBlueprint;
        }

        public object GetUndoRoot()
        {
            return this;
        }

        public void RefreshBlueprintView()
        {
            if (this.blueprintsView != null)
            {
                this.blueprintsView.Refresh();
            }
        }

        public void RemoveBlueprint(string blueprintId)
        {
            // Get blueprint to remove.
            var blueprintToRemove = this.Blueprints.First(blueprint => blueprint.BlueprintId == blueprintId);

            // Check for blueprints that derive from the blueprint to remove.
            if (blueprintToRemove.DerivedBlueprints.Count > 0)
            {
                throw new InvalidOperationException("Other blueprints depend on this one!");
            }

            this.blueprintManager.RemoveBlueprint(blueprintId);
            this.Blueprints.Remove(blueprintToRemove);

            // Remove from parent's children list, if necessary.
            if (blueprintToRemove.Parent != null)
            {
                blueprintToRemove.Parent.DerivedBlueprints.Remove(blueprintToRemove);
            }
        }

        /// <summary>
        ///   Changes the parent of the blueprint with the specified id, updating
        ///   children collections.
        /// </summary>
        /// <param name="childId">Id of the blueprint whose parent to change.</param>
        /// <param name="newParentId">Id of the new parent blueprint.</param>
        public void ReparentBlueprint(string childId, string newParentId, bool refreshBlueprintView = true)
        {
            // Find blueprints.
            var childBlueprint = this.Blueprints.First(blueprint => blueprint.BlueprintId.Equals(childId));
            var oldParentBlueprint = childBlueprint.Parent;
            var newParentBlueprint =
                this.Blueprints.FirstOrDefault(blueprint => blueprint.BlueprintId.Equals(newParentId));

            if (newParentBlueprint == null)
            {
                throw new InvalidOperationException(
                    string.Format(
                        "No blueprint with id {1} found. Could not parent blueprint {0} to {1}.", childId, newParentId));
            }

            // Update parent.
            childBlueprint.Blueprint.ParentId = newParentBlueprint.BlueprintId;
            childBlueprint.Parent = newParentBlueprint;

            // Update children of old parent.
            if (oldParentBlueprint != null)
            {
                oldParentBlueprint.DerivedBlueprints.Remove(childBlueprint);
            }

            // Update children of new parent.
            newParentBlueprint.DerivedBlueprints.Add(childBlueprint);

            // Update available components.
            childBlueprint.UpdateAvailableComponents();

            foreach (var descendant in childBlueprint.DerivedBlueprints)
            {
                descendant.UpdateAvailableComponents();
            }

            // Update blueprints view.
            if (refreshBlueprintView)
            {
                this.RefreshBlueprintView();
            }
        }

        /// <summary>
        ///   Checks the parent blueprint id of all blueprints, and reparents these blueprints if necessary.
        /// </summary>
        public void SetupBlueprintHierarchy()
        {
            var errors = new List<Exception>();

            foreach (
                var blueprint in this.Blueprints.Where(blueprint => !string.IsNullOrEmpty(blueprint.Blueprint.ParentId))
                )
            {
                try
                {
                    this.ReparentBlueprint(blueprint.BlueprintId, blueprint.Blueprint.ParentId, false);
                }
                catch (InvalidOperationException e)
                {
                    errors.Add(e);
                }
            }

            if (errors.Count > 0)
            {
                throw new AggregateException(errors);
            }

            this.RefreshBlueprintView();
        }

        public void UpdateLocalizationKeys()
        {
            this.blueprintManager.UpdateLocalizationKeys();
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
                    this.CurrentBlueprintManager.AddBlueprint(item.BlueprintId, item.Blueprint);
                }

                if (e.NewItems.Count == 1)
                {
                    BlueprintViewModel item = (BlueprintViewModel)e.NewItems[0];
                    undoMessage = string.Format("Add blueprint '{0}'", item.BlueprintId);
                }
                else
                {
                    undoMessage = "Add blueprints";
                }

                foreach (BlueprintViewModel item in e.NewItems)
                {
                    item.Root = this;

                    // Setup correct parent hierarchy.
                    if (item.Parent != null)
                    {
                        this.ReparentBlueprint(item.BlueprintId, item.Parent.BlueprintId, false);
                    }
                }
            }
            else if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                // Update blueprints in blueprint manager.
                foreach (BlueprintViewModel item in e.OldItems)
                {
                    this.blueprintManager.RemoveBlueprint(item.BlueprintId);
                }

                if (e.OldItems.Count == 1)
                {
                    BlueprintViewModel item = (BlueprintViewModel)e.OldItems[0];
                    undoMessage = string.Format("Remove blueprint '{0}'", item.BlueprintId);
                }
                else
                {
                    undoMessage = "Remove blueprints";
                }

                foreach (BlueprintViewModel item in e.OldItems)
                {
                    item.Root = null;

                    // Remove from parent's children list, if necessary.
                    if (item.Parent != null)
                    {
                        item.Parent.DerivedBlueprints.Remove(item);
                    }
                }
            }

            // This line will log the collection change with the undo framework.
            DefaultChangeFactory.Current.OnCollectionChanged(
                this, "Blueprints", this.Blueprints, e, undoMessage ?? "Collection of Blueprints changed");

            // Validate new blueprint id again as it may be valid/invalid now.
            this.OnPropertyChanged("NewBlueprintId");

            this.RefreshBlueprintView();
        }

        #endregion
    }
}