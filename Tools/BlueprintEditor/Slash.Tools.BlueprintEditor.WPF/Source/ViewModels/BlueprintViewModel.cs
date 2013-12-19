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

    public class BlueprintViewModel : INotifyPropertyChanged, ISupportsUndo
    {
        #region Fields

        private IEnumerable<Type> assemblyComponents;

        /// <summary>
        ///   Id of the blueprint.
        /// </summary>
        private string blueprintId;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///   Constructor.
        /// </summary>
        /// <param name="blueprintId">Blueprint id.</param>
        /// <param name="blueprint">Blueprint.</param>
        public BlueprintViewModel(string blueprintId, Blueprint blueprint)
        {
            this.blueprintId = blueprintId;
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

                string oldBlueprintId = this.blueprintId;
                this.blueprintId = value;

                // Move in blueprint manager.
                if (this.BlueprintManager != null)
                {
                    if (oldBlueprintId != null && value != null)
                    {
                        this.BlueprintManager.ChangeBlueprintId(oldBlueprintId, value);
                    }
                }

                this.OnPropertyChanged();
            }
        }

        /// <summary>
        ///   Blueprint manager the blueprint belongs to.
        /// </summary>
        public BlueprintManager BlueprintManager { get; set; }

        /// <summary>
        ///   Blueprints derived from the blueprint.
        /// </summary>
        public ObservableCollection<BlueprintViewModel> DerivedBlueprints { get; set; }

        /// <summary>
        ///   Parent of this blueprint.
        /// </summary>
        public BlueprintViewModel Parent { get; set; }

        public object Root { get; set; }

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

            // Remove from available, add to blueprint component types.
            this.AvailableComponents.Remove(componentType);
            this.AddedComponents.Add(componentType);

            // Make children inherit component.
            foreach (var child in this.DerivedBlueprints)
            {
                child.InheritComponent(componentType);
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
            // Remove from available, add to blueprint component types.
            if (this.AssemblyComponents.Contains(componentType))
            {
                this.AvailableComponents.Add(componentType);
            }

            // Make children no longer inherit component.
            foreach (var child in this.DerivedBlueprints)
            {
                child.UninheritComponent(componentType);
            }

            return this.AddedComponents.Remove(componentType);
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
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                // Add component types to blueprint.
                foreach (Type item in e.NewItems)
                {
                    this.Blueprint.ComponentTypes.Add(item);
                }
            }
            else if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                // Remove component types from blueprint.
                foreach (Type item in e.OldItems)
                {
                    this.Blueprint.ComponentTypes.Remove(item);
                }
            }
        }

        private void OnAvailableComponentsChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            this.OnPropertyChanged("AvailableComponents");
        }

        private void UpdateAvailableComponents()
        {
            if (this.assemblyComponents == null)
            {
                this.AvailableComponents.Clear();
                return;
            }

            // Update available components.
            IEnumerable<Type> newAvailableComponents =
                this.assemblyComponents.Except(this.Blueprint.ComponentTypes).ToList();

            foreach (var availableComponent in this.AvailableComponents)
            {
                if (!newAvailableComponents.Contains(availableComponent))
                {
                    this.AvailableComponents.Remove(availableComponent);
                }
            }

            foreach (var newAvailableComponent in newAvailableComponents)
            {
                if (!this.AvailableComponents.Contains(newAvailableComponent))
                {
                    this.AvailableComponents.Add(newAvailableComponent);
                }
            }
        }

        #endregion
    }
}