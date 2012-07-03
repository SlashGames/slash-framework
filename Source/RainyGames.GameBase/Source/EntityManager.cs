// -----------------------------------------------------------------------
// <copyright file="EntityManager.cs" company="Rainy Games">
// Copyright (c) Rainy Games. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace RainyGames.GameBase
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Creates and removes game entities. Holds references to all component
    /// managers, delegating all calls for adding or removing components.
    /// </summary>
    public class EntityManager
    {
        #region Constants and Fields

        /// <summary>
        /// Game this manager controls the entities of.
        /// </summary>
        private Game game;

        /// <summary>
        /// Id that will be assigned to the next entitiy created.
        /// </summary>
        private long nextEntityId;

        /// <summary>
        /// All active entity ids.
        /// </summary>
        private HashSet<long> entities;

        /// <summary>
        /// Ids of all entities that have been removed in this tick.
        /// </summary>
        private HashSet<long> removedEntities;

        /// <summary>
        /// Managers that are mapping entity ids to specific components.
        /// </summary>
        private Dictionary<Type, ComponentManager> componentManagers;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Constructs a new entity manager without any initial entities.
        /// </summary>
        /// <param name="game">
        /// Game to manage the entities for.
        /// </param>
        public EntityManager(Game game)
        {
            this.game = game;
            this.nextEntityId = 0;
            this.entities = new HashSet<long>();
            this.removedEntities = new HashSet<long>();
            this.componentManagers = new Dictionary<Type, ComponentManager>();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Total number of entities managed by this EntityManager instance.
        /// </summary>
        public long EntityCount
        {
            get { return this.entities.Count; }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Creates a new entity.
        /// </summary>
        /// <returns>
        /// Unique id of the new entity.
        /// </returns>
        public long CreateEntity()
        {
            long id = this.nextEntityId++;
            this.entities.Add(id);
            this.game.EventManager.QueueEvent(FrameworkEventType.EntityCreated, id);
            return id;
        }

        /// <summary>
        /// Issues the entity with the specified id for removal at the end of
        /// the current tick.
        /// </summary>
        /// <param name="id">
        /// Id of the entity to remove.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Entity id is negative.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Entity id has not yet been assigned.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// Entity with the specified id has already been removed.
        /// </exception>
        public void RemoveEntity(long id)
        {
            this.CheckEntityId(id);

            this.game.EventManager.QueueEvent(FrameworkEventType.EntityRemoved, id);

            this.removedEntities.Add(id);
        }

        /// <summary>
        /// Removes all entities that have been issued for removal during the
        /// current tick, detaching all components.
        /// </summary>
        public void CleanUpEntities()
        {
            foreach (long id in this.removedEntities)
            {
                foreach (ComponentManager manager in this.componentManagers.Values)
                {
                    manager.RemoveComponent(id);
                }

                this.entities.Remove(id);
            }

            this.removedEntities.Clear();
        }

        /// <summary>
        /// Attaches the passed component to the entity with the specified id.
        /// </summary>
        /// <param name="entityId">
        /// Id of the entity to attach the component to.
        /// </param>
        /// <param name="component">
        /// Component to attach.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Entity id is negative.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Entity id has not yet been assigned.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// Entity with the specified id has already been removed.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// Passed component is null.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// There is already a component of the same type attached.
        /// </exception>
        public void AddComponent(long entityId, IComponent component)
        {
            this.CheckEntityId(entityId);

            Type componentType = component.GetType();

            if (!this.componentManagers.ContainsKey(componentType))
            {
                this.componentManagers.Add(componentType, new ComponentManager(this.game));
            }

            this.componentManagers[component.GetType()].AddComponent(entityId, component);
        }

        /// <summary>
        /// Removes a component of the passed type from the entity with the specified id.
        /// </summary>
        /// <param name="entityId">
        /// Id of the entity to remove the component from.
        /// </param>
        /// <param name="componentType">
        /// Type of the component to remove.
        /// </param>
        /// <returns>
        /// Whether a component has been removed, or not.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Entity id is negative.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Entity id has not yet been assigned.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// Entity with the specified id has already been removed.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// Passed component type is null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// A component of the passed type has never been added before.
        /// </exception>
        public bool RemoveComponent(long entityId, Type componentType)
        {
            this.CheckEntityId(entityId);

            if (componentType == null)
            {
                throw new ArgumentNullException("componentType");
            }

            if (!this.componentManagers.ContainsKey(componentType))
            {
                throw new ArgumentException(
                    "A component of type " + componentType + " has never been added before.",
                    "componentType");
            }

            return this.componentManagers[componentType].RemoveComponent(entityId);
        }

        /// <summary>
        /// Gets a component of the passed type attached to the entity with the specified id.
        /// </summary>
        /// <param name="entityId">
        /// Id of the entity to get the component of.
        /// </param>
        /// <param name="componentType">
        /// Type of the component to get.
        /// </param>
        /// <returns>
        /// The component, if there is one of the specified type attached to the entity, and
        /// null otherwise.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Entity id is negative.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Entity id has not yet been assigned.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// Entity with the specified id has already been removed.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// Passed component type is null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// A component of the passed type has never been added before.
        /// </exception>
        public IComponent GetComponent(long entityId, Type componentType)
        {
            this.CheckEntityId(entityId);

            if (componentType == null)
            {
                throw new ArgumentNullException("componentType");
            }

            if (!this.componentManagers.ContainsKey(componentType))
            {
                throw new ArgumentException(
                    "A component of type " + componentType + " has never been added before.",
                    "componentType");
            }

            return this.componentManagers[componentType].GetComponent(entityId);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Checks whether the passed entity is valid, throwing an exception if not.
        /// </summary>
        /// <param name="id">
        /// Entity id to check.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Entity id is negative.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Entity id has not yet been assigned.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// Entity with the specified id has already been removed.
        /// </exception>
        private void CheckEntityId(long id)
        {
            if (id < 0)
            {
                throw new ArgumentOutOfRangeException("id", "Entity ids are always non-negative.");
            }

            if (id >= this.nextEntityId)
            {
                throw new ArgumentOutOfRangeException("id", "Entity id " + id + " has not yet been assigned.");
            }

            if (!this.entities.Contains(id))
            {
                throw new ArgumentException("id", "The entity with id " + id + " has already been removed.");
            }
        }

        #endregion
    }
}
