// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EntityManager.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.GameBase.Components
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    using Slash.Collections.AttributeTables;
    using Slash.Collections.ObjectModel;
    using Slash.GameBase.Blueprints;
    using Slash.GameBase.Events;
    using Slash.GameBase.Inspector.Data;
    using Slash.GameBase.Inspector.Utils;

    /// <summary>
    ///   Creates and removes game entities. Holds references to all component
    ///   managers, delegating all calls for adding or removing components.
    /// </summary>
    public class EntityManager
    {
        #region Fields

        /// <summary>
        ///   Managers that are mapping entity ids to specific components.
        /// </summary>
        private readonly Dictionary<Type, ComponentManager> componentManagers;

        /// <summary>
        ///   All active entity ids.
        /// </summary>
        private readonly HashSet<int> entities;

        /// <summary>
        ///   Game this manager controls the entities of.
        /// </summary>
        private readonly Game game;

        /// <summary>
        ///   Inactive entities and their components.
        /// </summary>
        private readonly Dictionary<int, List<IEntityComponent>> inactiveEntities;

        /// <summary>
        ///   Inspector types of entity components.
        /// </summary>
        private readonly InspectorTypeTable inspectorTypes;

        /// <summary>
        ///   Ids of all entities that have been removed in this tick.
        /// </summary>
        private readonly HashSet<int> removedEntities;

        /// <summary>
        ///   Id that will be assigned to the next entity created.
        /// </summary>
        private int nextEntityId;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///   Constructs a new entity manager without any initial entities.
        /// </summary>
        /// <param name="game"> Game to manage the entities for. </param>
        public EntityManager(Game game)
        {
            this.game = game;
            this.nextEntityId = 1;
            this.entities = new HashSet<int>();
            this.removedEntities = new HashSet<int>();
            this.inactiveEntities = new Dictionary<int, List<IEntityComponent>>();
            this.componentManagers = new Dictionary<Type, ComponentManager>();
            this.inspectorTypes = InspectorTypeTable.FindInspectorTypes(typeof(IEntityComponent));
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///   Read-only collection of all entities.
        /// </summary>
        public IEnumerable<int> Entities
        {
            get
            {
                return new ReadOnlyCollection<int>(this.entities);
            }
        }

        /// <summary>
        ///   Total number of entities managed by this EntityManager instance.
        /// </summary>
        public int EntityCount
        {
            get
            {
                return this.entities.Count;
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///   Re-activates the entity with the specified id, if it is inactive.
        /// </summary>
        /// <param name="entityId">Id of the entity to activate.</param>
        public void ActivateEntity(int entityId)
        {
            // Check if entity is inactive.
            List<IEntityComponent> components;

            if (!this.inactiveEntities.TryGetValue(entityId, out components))
            {
                return;
            }

            // Activate entity.
            this.CreateEntity(entityId);

            // Add components.
            foreach (IEntityComponent component in components)
            {
                this.AddComponent(entityId, component);
            }

            // Raise event.
            this.game.EventManager.QueueEvent(FrameworkEventType.EntityInitialized, entityId);

            // Remove from list of inactive entities.
            this.inactiveEntities.Remove(entityId);
        }

        /// <summary>
        ///   Attaches the passed component to the entity with the specified id.
        /// </summary>
        /// <param name="entityId"> Id of the entity to attach the component to. </param>
        /// <param name="component"> Component to attach. </param>
        /// <exception cref="ArgumentOutOfRangeException">Entity id is negative.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Entity id has not yet been assigned.</exception>
        /// <exception cref="ArgumentException">Entity with the specified id has already been removed.</exception>
        /// <exception cref="ArgumentNullException">Passed component is null.</exception>
        /// <exception cref="InvalidOperationException">There is already a component of the same type attached.</exception>
        public void AddComponent(int entityId, IEntityComponent component)
        {
            this.AddComponent(entityId, component, true);
        }

        /// <summary>
        ///   Attaches the passed component to the entity with the specified id.
        /// </summary>
        /// <param name="entityId"> Id of the entity to attach the component to. </param>
        /// <param name="component"> Component to attach. </param>
        /// <param name="sendEvent">Indicates if an event should be send about the component adding.</param>
        /// <exception cref="ArgumentOutOfRangeException">Entity id is negative.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Entity id has not yet been assigned.</exception>
        /// <exception cref="ArgumentException">Entity with the specified id has already been removed.</exception>
        /// <exception cref="ArgumentNullException">Passed component is null.</exception>
        /// <exception cref="InvalidOperationException">There is already a component of the same type attached.</exception>
        public void AddComponent(int entityId, IEntityComponent component, bool sendEvent)
        {
            this.CheckEntityId(entityId);

            Type componentType = component.GetType();

            if (!this.componentManagers.ContainsKey(componentType))
            {
                this.componentManagers.Add(componentType, new ComponentManager());
            }

            this.componentManagers[component.GetType()].AddComponent(entityId, component);

            if (sendEvent)
            {
                this.game.EventManager.QueueEvent(
                    FrameworkEventType.ComponentAdded, new EntityComponentData(entityId, component));
            }
        }

        /// <summary>
        ///   Removes all entities that have been issued for removal during the
        ///   current tick, detaching all components.
        /// </summary>
        public void CleanUpEntities()
        {
            foreach (int id in this.removedEntities)
            {
                // Remove components.
                foreach (ComponentManager manager in this.componentManagers.Values)
                {
                    manager.RemoveComponent(id);
                }

                this.entities.Remove(id);
            }

            this.removedEntities.Clear();
        }

        /// <summary>
        ///   Returns an iterator over all components of the specified type.
        /// </summary>
        /// <param name="type"> Type of the components to get. </param>
        /// <returns> Components of the specified type. </returns>
        /// <exception cref="ArgumentNullException">Specified type is null.</exception>
        public IEnumerable ComponentsOfType(Type type)
        {
            ComponentManager componentManager;

            if (type == null)
            {
                throw new ArgumentNullException("type");
            }

            if (!this.componentManagers.TryGetValue(type, out componentManager))
            {
                yield break;
            }

            foreach (IEntityComponent component in componentManager.Components())
            {
                yield return component;
            }
        }

        /// <summary>
        ///   Creates a new entity.
        /// </summary>
        /// <returns> Unique id of the new entity. </returns>
        public int CreateEntity()
        {
            int id = this.nextEntityId++;
            return this.CreateEntity(id);
        }

        /// <summary>
        ///   Creates a new entity with the specified id.
        /// </summary>
        /// <param name="id">Id of the entity to create.</param>
        /// <returns>Unique id of the new entity.</returns>
        public int CreateEntity(int id)
        {
            this.entities.Add(id);
            this.game.EventManager.QueueEvent(FrameworkEventType.EntityCreated, id);
            return id;
        }

        /// <summary>
        ///   Creates a new entity, adding components matching the passed
        ///   blueprint and initializing these with the data stored in the
        ///   blueprint and the specified configuration. Configuration data
        ///   is preferred over blueprint data.
        /// </summary>
        /// <param name="blueprint"> Blueprint describing the entity to create. </param>
        /// <param name="configuration"> Data for initializing the entity. </param>
        /// <param name="additionalComponents">Components to add to the entity, in addition to the ones specified by the blueprint.</param>
        /// <returns> Unique id of the new entity. </returns>
        public int CreateEntity(Blueprint blueprint, IAttributeTable configuration, List<Type> additionalComponents)
        {
            int id = this.CreateEntity();
            this.InitEntity(id, blueprint, configuration, additionalComponents);
            return id;
        }

        /// <summary>
        ///   Creates a new entity, adding components matching the passed
        ///   blueprint.
        /// </summary>
        /// <param name="blueprint">Blueprint describing the entity to create.</param>
        /// <returns>Unique id of the new entity.</returns>
        public int CreateEntity(Blueprint blueprint)
        {
            return this.CreateEntity(blueprint, null);
        }

        /// <summary>
        ///   Creates a new entity, adding components matching the passed
        ///   blueprint and initializing these with the data stored in the
        ///   blueprint and the specified configuration. Configuration data
        ///   is preferred over blueprint data.
        /// </summary>
        /// <param name="blueprint"> Blueprint describing the entity to create. </param>
        /// <param name="configuration"> Data for initializing the entity. </param>
        /// <returns> Unique id of the new entity. </returns>
        public int CreateEntity(Blueprint blueprint, IAttributeTable configuration)
        {
            return this.CreateEntity(blueprint, configuration, null);
        }

        /// <summary>
        ///   De-activates the entity with the specified id. Inactive entities
        ///   are considered as removed, until they are re-activated again.
        /// </summary>
        /// <param name="entityId">Id of the entity to de-activate.</param>
        public void DeactivateEntity(int entityId)
        {
            // Check if entity is active.
            if (this.inactiveEntities.ContainsKey(entityId))
            {
                return;
            }

            // Store entity components and their values.
            List<IEntityComponent> components = new List<IEntityComponent>();

            foreach (ComponentManager manager in this.componentManagers.Values)
            {
                IEntityComponent component;
                if (!manager.RemoveComponent(entityId, out component))
                {
                    continue;
                }

                components.Add(component);

                this.game.EventManager.QueueEvent(
                    FrameworkEventType.ComponentRemoved, new EntityComponentData(entityId, component));
            }

            // Remove entity.
            this.RemoveEntity(entityId);

            // Add to list of inactive entities.
            this.inactiveEntities.Add(entityId, components);
        }

        /// <summary>
        ///   Returns an iterator over all entities having components of the specified type attached.
        /// </summary>
        /// <param name="type"> Type of the components to get the entities of. </param>
        /// <returns> Entities having components of the specified type attached. </returns>
        /// <exception cref="ArgumentNullException">Specified type is null.</exception>
        public IEnumerable<int> EntitiesWithComponent(Type type)
        {
            ComponentManager componentManager;

            if (type == null)
            {
                throw new ArgumentNullException("type");
            }

            if (!this.componentManagers.TryGetValue(type, out componentManager))
            {
                yield break;
            }

            foreach (int entityId in componentManager.Entities())
            {
                yield return entityId;
            }
        }

        /// <summary>
        ///   Checks whether the entity with the passed id has been removed or
        ///   not.
        /// </summary>
        /// <param name="entityId"> Id of the entity to check. </param>
        /// <returns>
        ///   <c>false</c> , if the entity has been removed, and <c>true</c> otherwise.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">Entity id is negative.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Entity id has not yet been assigned.</exception>
        public bool EntityIsAlive(int entityId)
        {
            if (entityId < 0)
            {
                throw new ArgumentOutOfRangeException("entityId", "Entity ids are always non-negative.");
            }

            if (entityId >= this.nextEntityId)
            {
                throw new ArgumentOutOfRangeException(
                    "entityId", "Entity id " + entityId + " has not yet been assigned.");
            }

            return this.entities.Contains(entityId);
        }

        /// <summary>
        ///   Checks if the entity with the specified id will be removed this
        ///   frame.
        /// </summary>
        /// <param name="entityId">Id of the entity to check.</param>
        /// <returns>
        ///   <c>true</c>, if the entity with the specified id is about to be removed, and
        ///   <c>false</c>, otherwise.
        /// </returns>
        public bool EntityIsBeingRemoved(int entityId)
        {
            return this.removedEntities.Contains(entityId);
        }

        /// <summary>
        ///   Checks whether the entity with the specified id is inactive.
        /// </summary>
        /// <param name="id">Id of the entity to check.</param>
        /// <returns>
        ///   <c>true</c>, if the entity is inactive, and
        ///   <c>false</c> otherwise.
        /// </returns>
        public bool EntityIsInactive(int id)
        {
            return this.inactiveEntities.ContainsKey(id);
        }

        /// <summary>
        ///   Gets a component of the passed type attached to the entity with the specified id.
        /// </summary>
        /// <param name="entityId"> Id of the entity to get the component of. </param>
        /// <param name="componentType"> Type of the component to get. </param>
        /// <returns> The component, if there is one of the specified type attached to the entity, and null otherwise. </returns>
        /// <exception cref="ArgumentOutOfRangeException">Entity id is negative.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Entity id has not yet been assigned.</exception>
        /// <exception cref="ArgumentException">Entity with the specified id has already been removed.</exception>
        /// <exception cref="ArgumentNullException">Passed component type is null.</exception>
        public IEntityComponent GetComponent(int entityId, Type componentType)
        {
            this.CheckEntityId(entityId);

            if (componentType == null)
            {
                throw new ArgumentNullException("componentType");
            }

            // Get component manager.
            if (componentType.IsInterface)
            {
                foreach (KeyValuePair<Type, ComponentManager> componentManagerPair in this.componentManagers)
                {
                    if (componentType.IsAssignableFrom(componentManagerPair.Key))
                    {
                        IEntityComponent component = componentManagerPair.Value.GetComponent(entityId);
                        if (component != null)
                        {
                            return component;
                        }
                    }
                }
                return null;
            }

            ComponentManager componentManager;
            return this.componentManagers.TryGetValue(componentType, out componentManager)
                       ? componentManager.GetComponent(entityId)
                       : null;
        }

        /// <summary>
        ///   Gets a component of the passed type attached to the entity with the specified id.
        /// </summary>
        /// <param name="entityId"> Id of the entity to get the component of. </param>
        /// <typeparam name="T"> Type of the component to get. </typeparam>
        /// <returns> The component, if there is one of the specified type attached to the entity, and null otherwise. </returns>
        /// <exception cref="ArgumentOutOfRangeException">Entity id is negative.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Entity id has not yet been assigned.</exception>
        /// <exception cref="ArgumentException">Entity with the specified id has already been removed.</exception>
        /// <exception cref="ArgumentNullException">Passed component type is null.</exception>
        /// <exception cref="ArgumentException">A component of the passed type has never been added before.</exception>
        public T GetComponent<T>(int entityId) where T : IEntityComponent
        {
            return (T)this.GetComponent(entityId, typeof(T));
        }

        /// <summary>
        ///   Retrieves an array containing the ids of all living entities in
        ///   O(n).
        /// </summary>
        /// <returns> Array containing the ids of all entities that haven't been removed yet. </returns>
        public int[] GetEntities()
        {
            if (this.entities.Count == 0)
            {
                return null;
            }

            int[] entityArray = new int[this.entities.Count];
            this.entities.CopyTo(entityArray);
            return entityArray;
        }

        /// <summary>
        ///   Returns the entity ids of all entities which fulfill the specified predicate.
        /// </summary>
        /// <param name="predicate"> Predicate to fulfill. </param>
        /// <returns> Collection of ids of all entities which fulfill the specified predicate. </returns>
        public IEnumerable<int> GetEntities(Func<int, bool> predicate)
        {
            return this.entities.Count == 0 ? null : this.entities.Where(predicate);
        }

        /// <summary>
        ///   Convenience method for retrieving a component from two possible entities.
        /// </summary>
        /// <typeparam name="TComponent">Type of the component to get.</typeparam>
        /// <param name="data">Data for the event that affected two entities.</param>
        /// <param name="entityId">Id of the entity having the component attached.</param>
        /// <param name="component">Component.</param>
        /// <returns>
        ///   True if one of the entities has a <typeparamref name="TComponent" />
        ///   attached; otherwise, false.
        /// </returns>
        public bool GetEntityComponent<TComponent>(Entity2Data data, out int entityId, out TComponent component)
            where TComponent : class, IEntityComponent
        {
            int entityIdA = data.First;
            int entityIdB = data.Second;

            TComponent componentA = this.GetComponent<TComponent>(entityIdA);
            if (componentA != null)
            {
                entityId = entityIdA;
                component = componentA;
                return true;
            }

            TComponent componentB = this.GetComponent<TComponent>(entityIdB);
            if (componentB != null)
            {
                entityId = entityIdB;
                component = componentB;
                return true;
            }

            entityId = 0;
            component = null;
            return false;
        }

        /// <summary>
        ///   Convenience method for retrieving components from two entities
        ///   in case the order of the entities is unknown.
        /// </summary>
        /// <typeparam name="TComponentTypeA">Type of the first component to get.</typeparam>
        /// <typeparam name="TComponentTypeB">Type of the second component to get.</typeparam>
        /// <param name="data">Data for the event that affected two entities.</param>
        /// <param name="entityIdA">Id of the entity having the first component attached.</param>
        /// <param name="entityIdB">Id of the entity having the second component attached.</param>
        /// <param name="componentA">First component.</param>
        /// <param name="componentB">Second component.</param>
        /// <returns>
        ///   <c>true</c>, if one of the entities has a <typeparamref name="TComponentTypeA" />
        ///   and the other one a <typeparamref name="TComponentTypeB" /> attached,
        ///   and <c>false</c> otherwise.
        /// </returns>
        public bool GetEntityComponents<TComponentTypeA, TComponentTypeB>(
            Entity2Data data,
            out int entityIdA,
            out int entityIdB,
            out TComponentTypeA componentA,
            out TComponentTypeB componentB) where TComponentTypeA : class, IEntityComponent
            where TComponentTypeB : class, IEntityComponent
        {
            entityIdA = data.First;
            entityIdB = data.Second;

            componentA = this.GetComponent<TComponentTypeA>(entityIdA);
            componentB = this.GetComponent<TComponentTypeB>(entityIdB);

            if (componentA == null || componentB == null)
            {
                // Check other way round.
                entityIdA = data.Second;
                entityIdB = data.First;

                componentA = this.GetComponent<TComponentTypeA>(entityIdA);
                componentB = this.GetComponent<TComponentTypeB>(entityIdB);

                return componentA != null && componentB != null;
            }

            return true;
        }

        /// <summary>
        ///   Initializes the specified entity, adding components matching the
        ///   passed blueprint and initializing these with the data stored in
        ///   the blueprint and the specified configuration. Configuration
        ///   data is preferred over blueprint data.
        /// </summary>
        /// <param name="entityId">Id of the entity to initialize.</param>
        /// <param name="blueprint"> Blueprint describing the entity to create. </param>
        /// <param name="configuration"> Data for initializing the entity. </param>
        /// <param name="additionalComponents">Components to add to the entity, in addition to the ones specified by the blueprint.</param>
        public void InitEntity(
            int entityId, Blueprint blueprint, IAttributeTable configuration, IEnumerable<Type> additionalComponents)
        {
            if (blueprint == null)
            {
                throw new ArgumentNullException("blueprint", "Blueprint is null.");
            }

            // Setup attribute table.
            HierarchicalAttributeTable attributeTable = new HierarchicalAttributeTable();
            if (configuration != null)
            {
                attributeTable.AddParent(configuration);
            }

            // Add attribute tables of all ancestors.
            IAttributeTable blueprintAttributeTable = blueprint.GetAttributeTable();
            if (blueprintAttributeTable != null)
            {
                attributeTable.AddParent(blueprintAttributeTable);
            }

            // Build list of components to add.
            IEnumerable<Type> blueprintComponentTypes = blueprint.GetAllComponentTypes();
            IEnumerable<Type> componentTypes = additionalComponents != null
                                                   ? blueprintComponentTypes.Union(additionalComponents)
                                                   : blueprintComponentTypes;

            // Add components.
            foreach (Type type in componentTypes)
            {
                this.AddComponent(type, entityId, attributeTable);
            }

            // Raise event.
            this.game.EventManager.QueueEvent(FrameworkEventType.EntityInitialized, entityId);
        }

        /// <summary>
        ///   Removes a component of the passed type from the entity with the specified id.
        /// </summary>
        /// <param name="entityId"> Id of the entity to remove the component from. </param>
        /// <param name="componentType"> Type of the component to remove. </param>
        /// <returns> Whether a component has been removed, or not. </returns>
        /// <exception cref="ArgumentOutOfRangeException">Entity id is negative.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Entity id has not yet been assigned.</exception>
        /// <exception cref="ArgumentException">Entity with the specified id has already been removed.</exception>
        /// <exception cref="ArgumentNullException">Passed component type is null.</exception>
        /// <exception cref="ArgumentException">A component of the passed type has never been added before.</exception>
        public bool RemoveComponent(int entityId, Type componentType)
        {
            this.CheckEntityId(entityId);

            if (componentType == null)
            {
                throw new ArgumentNullException("componentType");
            }

            ComponentManager componentManager;

            if (!this.componentManagers.TryGetValue(componentType, out componentManager))
            {
                throw new ArgumentException(
                    "A component of type " + componentType + " has never been added before.", "componentType");
            }

            IEntityComponent component;
            bool removed = componentManager.RemoveComponent(entityId, out component);

            if (removed)
            {
                this.game.EventManager.QueueEvent(
                    FrameworkEventType.ComponentRemoved, new EntityComponentData(entityId, component));
            }

            return removed;
        }

        /// <summary>
        ///   Removes all entities.
        /// </summary>
        public void RemoveEntities()
        {
            IEnumerable<int> aliveEntities = this.entities.Except(this.removedEntities);
            foreach (int entityId in aliveEntities)
            {
                this.game.EventManager.QueueEvent(FrameworkEventType.EntityRemoved, entityId);

                // Remove components.
                foreach (ComponentManager manager in this.componentManagers.Values)
                {
                    IEntityComponent component;
                    if (manager.RemoveComponent(entityId, out component))
                    {
                        this.game.EventManager.QueueEvent(
                            FrameworkEventType.ComponentRemoved, new EntityComponentData(entityId, component));
                    }
                }

                this.removedEntities.Add(entityId);
            }
        }

        /// <summary>
        ///   <para>
        ///     Issues the entity with the specified id for removal at the end of
        ///     the current tick.
        ///   </para>
        ///   <para>
        ///     If the entity is inactive, it is removed immediately and no
        ///     further event is raised.
        ///   </para>
        /// </summary>
        /// <param name="entityId"> Id of the entity to remove. </param>
        /// <exception cref="ArgumentOutOfRangeException">Entity id is negative.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Entity id has not yet been assigned.</exception>
        /// <exception cref="ArgumentException">Entity with the specified id has already been removed.</exception>
        public void RemoveEntity(int entityId)
        {
            if (this.EntityIsInactive(entityId))
            {
                this.inactiveEntities.Remove(entityId);
                return;
            }

            this.CheckEntityId(entityId);

            if (this.EntityIsBeingRemoved(entityId))
            {
                return;
            }

            // Remove components.
            foreach (ComponentManager manager in this.componentManagers.Values)
            {
                IEntityComponent component = manager.GetComponent(entityId);
                if (component == null)
                {
                    continue;
                }

                this.game.EventManager.QueueEvent(
                    FrameworkEventType.ComponentRemoved, new EntityComponentData(entityId, component));
            }

            this.game.EventManager.QueueEvent(FrameworkEventType.EntityRemoved, entityId);

            this.removedEntities.Add(entityId);
        }

        #endregion

        #region Methods

        /// <summary>
        ///   Adds a component with the specified type to entity with the
        ///   specified id and initializes it with the values taken from
        ///   the passed attribute table.
        /// </summary>
        /// <param name="componentType">Type of the component to add.</param>
        /// <param name="entityId">Id of the entity to add the component to.</param>
        /// <param name="attributeTable">Attribute table to initialize the component with.</param>
        private void AddComponent(Type componentType, int entityId, IAttributeTable attributeTable)
        {
            // Create component.
            IEntityComponent component = (IEntityComponent)Activator.CreateInstance(componentType);

            // Add component. 
            this.AddComponent(entityId, component);

            // Init component.
            this.InitComponent(component, attributeTable);

            // Initialize component with the attribute table data.
            component.InitComponent(attributeTable);
        }

        /// <summary>
        ///   Checks whether the passed entity is valid, throwing an exception if not.
        /// </summary>
        /// <param name="id"> Entity id to check. </param>
        /// <exception cref="ArgumentOutOfRangeException">Entity id is negative.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Entity id has not yet been assigned.</exception>
        /// <exception cref="ArgumentException">Entity with the specified id has already been removed.</exception>
        private void CheckEntityId(int id)
        {
            if (!this.EntityIsAlive(id))
            {
                throw new ArgumentException("id", "The entity with id " + id + " has already been removed.");
            }
        }

        /// <summary>
        ///   Initializes the specified component with the specified attribute table.
        /// </summary>
        /// <param name="component">Component to initialize.</param>
        /// <param name="attributeTable">Attribute table which contains the data of the component.</param>
        private void InitComponent(IEntityComponent component, IAttributeTable attributeTable)
        {
            InspectorUtils.InitFromAttributeTable(
                this.game, this.inspectorTypes.GetInspectorType(component.GetType()), component, attributeTable);
        }

        #endregion
    }
}