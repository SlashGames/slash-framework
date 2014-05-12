// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BlueprintViewModel.cs" company="Slash Games">
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
    using System.Runtime.CompilerServices;

    using BlueprintEditor.Annotations;

    using MonitoredUndo;

    using Slash.GameBase.Blueprints;

    public class BlueprintViewModel : INotifyPropertyChanged,
                                      ISupportsUndo,
                                      IDataErrorInfo,
                                      IEquatable<BlueprintViewModel>
    {
        #region Fields

        private IEnumerable<Type> assemblyComponents;

        /// <summary>
        ///   Id of the blueprint.
        /// </summary>
        private string blueprintId;

        /// <summary>
        ///   New blueprint id.
        /// </summary>
        private string newBlueprintId;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///   Constructor.
        /// </summary>
        /// <param name="blueprintId">Blueprint id.</param>
        /// <param name="blueprint">Blueprint.</param>
        public BlueprintViewModel(string blueprintId, Blueprint blueprint)
        {
            this.BlueprintId = this.newBlueprintId = blueprintId;
            this.Blueprint = blueprint;

            this.DerivedBlueprints = new ObservableCollection<BlueprintViewModel>();

            // Set added components.
            this.AddedComponents = new ObservableCollection<Type>(this.Blueprint.ComponentTypes);
            this.AddedComponents.CollectionChanged += this.OnAddedComponentsChanged;

            // Set available components.
            this.AvailableComponents = new ObservableCollection<Type>();
            this.AvailableComponents.CollectionChanged += this.OnAvailableComponentsChanged;
        }

        #endregion

        #region Public Events

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Public Properties

        /// <summary>
        ///   Components which are already added to the blueprint.
        /// </summary>
        public ObservableCollection<Type> AddedComponents { get; private set; }

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

                this.UpdateAvailableComponents();
            }
        }

        /// <summary>
        ///   Components which can be added to the blueprint.
        /// </summary>
        public ObservableCollection<Type> AvailableComponents { get; private set; }

        /// <summary>
        ///   Blueprint this item represents.
        /// </summary>
        public Blueprint Blueprint { get; private set; }

        /// <summary>
        ///   Id of the blueprint.
        /// </summary>
        public string BlueprintId
        {
            get
            {
                return this.blueprintId;
            }
            set
            {
                if (value == this.blueprintId)
                {
                    return;
                }

                this.blueprintId = value;

                this.OnPropertyChanged();
            }
        }

        /// <summary>
        ///   Blueprints derived from the blueprint.
        /// </summary>
        public ObservableCollection<BlueprintViewModel> DerivedBlueprints { get; set; }

        public string Error
        {
            get
            {
                return null;
            }
        }

        /// <summary>
        ///   Id of the blueprint.
        /// </summary>
        public string NewBlueprintId
        {
            get
            {
                return this.newBlueprintId;
            }
            set
            {
                if (value == this.newBlueprintId)
                {
                    return;
                }

                this.newBlueprintId = value;

                this.OnPropertyChanged();
            }
        }

        /// <summary>
        ///   Parent of this blueprint.
        /// </summary>
        public BlueprintViewModel Parent { get; set; }

        /// <summary>
        ///   Root view model.
        /// </summary>
        public BlueprintManagerViewModel Root { get; set; }

        #endregion

        #region Public Indexers

        public string this[string columnName]
        {
            get
            {
                string result = null;
                if (columnName == "NewBlueprintId")
                {
                    if (this.Root != null)
                    {
                        BlueprintViewModel blueprintViewModel =
                            this.Root.Blueprints.FirstOrDefault(
                                viewModel => viewModel.BlueprintId == this.NewBlueprintId);
                        if (blueprintViewModel != null && blueprintViewModel != this)
                        {
                            result = "Blueprint id already exists.";
                        }
                        else if (this.newBlueprintId != this.blueprintId)
                        {
                            // Move in blueprint manager.
                            this.Root.ChangeBlueprintId(this, this.newBlueprintId);
                        }
                    }
                }
                return result;
            }
        }

        #endregion

        #region Public Methods and Operators

        public void AddComponent(Type componentType)
        {
            if (this.Blueprint.ComponentTypes.Contains(componentType))
            {
                throw new ArgumentException(
                    string.Format("Component type '{0}' already added to blueprint.", componentType.Name),
                    "componentType");
            }

            // Add to blueprint component types.
            this.AddedComponents.Add(componentType);

            // Make children inherit component.
            foreach (var child in this.DerivedBlueprints)
            {
                child.InheritComponent(componentType);
            }
        }

        public bool Equals(BlueprintViewModel other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }
            if (ReferenceEquals(this, other))
            {
                return true;
            }
            return string.Equals(this.blueprintId, other.blueprintId);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }
            if (ReferenceEquals(this, obj))
            {
                return true;
            }
            if (obj.GetType() != this.GetType())
            {
                return false;
            }
            return this.Equals((BlueprintViewModel)obj);
        }

        /// <summary>
        ///   Gets all components of this blueprint and its ancestors.
        /// </summary>
        /// <returns>Components of this blueprint and its parents.</returns>
        public IEnumerable<Type> GetComponents()
        {
            return this.AddedComponents.Union(this.GetParentComponents());
        }

        public override int GetHashCode()
        {
            return (this.blueprintId != null ? this.blueprintId.GetHashCode() : 0);
        }

        /// <summary>
        ///   Gets all components of the ancestors of this blueprint.
        /// </summary>
        /// <returns>Components of the parent of this blueprint and its parents.</returns>
        public IEnumerable<Type> GetParentComponents()
        {
            if (this.Parent == null)
            {
                yield break;
            }

            foreach (var type in this.Parent.Blueprint.ComponentTypes.Union(this.Parent.GetParentComponents()))
            {
                yield return type;
            }
        }

        public object GetUndoRoot()
        {
            return this.Root;
        }

        /// <summary>
        ///   Removes the specified component from the current and avaliable components,
        ///   proceeding recursively with all deriving blueprints.
        /// </summary>
        /// <param name="componentType">Type of the component to inherit.</param>
        public void InheritComponent(Type componentType)
        {
            this.AddedComponents.Remove(componentType);
            this.AvailableComponents.Remove(componentType);

            foreach (var child in this.DerivedBlueprints)
            {
                child.InheritComponent(componentType);
            }
        }

        public bool RemoveComponent(Type componentType)
        {
            // Make children no longer inherit component.
            foreach (var child in this.DerivedBlueprints)
            {
                child.UninheritComponent(componentType);
            }

            return this.AddedComponents.Remove(componentType);
        }

        public override string ToString()
        {
            return string.Format("{0}", this.BlueprintId);
        }

        /// <summary>
        ///   Adds the specified component to the avaliable components,
        ///   proceeding recursively with all deriving blueprints.
        /// </summary>
        /// <param name="componentType">Type of the component to uninherit.</param>
        public void UninheritComponent(Type componentType)
        {
            this.AvailableComponents.Add(componentType);

            foreach (var child in this.DerivedBlueprints)
            {
                child.UninheritComponent(componentType);
            }
        }

        public void UpdateAvailableComponents()
        {
            if (this.assemblyComponents == null)
            {
                this.AvailableComponents.Clear();
                return;
            }

            // Update available components.
            IEnumerable<Type> newAvailableComponents =
                this.assemblyComponents.Except(this.Blueprint.ComponentTypes)
                    .Except(this.GetParentComponents())
                    .ToList();

            this.AvailableComponents.Clear();

            foreach (var newAvailableComponent in newAvailableComponents)
            {
                this.AvailableComponents.Add(newAvailableComponent);
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

        private void OnAddedComponentsChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            string undoMessage = null;
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                // Add component types to blueprint.
                foreach (Type item in e.NewItems)
                {
                    // Remove from available component types.
                    this.AvailableComponents.Remove(item);

                    this.Blueprint.ComponentTypes.Add(item);
                }

                // Create undo message.
                if (e.NewItems.Count == 1)
                {
                    Type item = (Type)e.NewItems[0];
                    undoMessage = string.Format("Add component '{0}'", item.Name);
                }
                else
                {
                    undoMessage = "Add components";
                }
            }
            else if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                // Remove component types from blueprint.
                foreach (Type item in e.OldItems)
                {
                    this.Blueprint.ComponentTypes.Remove(item);

                    // Remove from available, add to blueprint component types.
                    if (this.AssemblyComponents.Contains(item))
                    {
                        this.AvailableComponents.Add(item);
                    }
                }

                // Create undo message.
                if (e.OldItems.Count == 1)
                {
                    Type item = (Type)e.OldItems[0];
                    undoMessage = string.Format("Remove component '{0}'", item.Name);
                }
                else
                {
                    undoMessage = "Remove components";
                }
            }

            // Log the collection change with the undo framework.
            DefaultChangeFactory.Current.OnCollectionChanged(
                this, "AddedComponents", this.AddedComponents, e, undoMessage ?? "Blueprint components changed");
        }

        private void OnAvailableComponentsChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            this.OnPropertyChanged("AvailableComponents");
        }

        #endregion
    }
}