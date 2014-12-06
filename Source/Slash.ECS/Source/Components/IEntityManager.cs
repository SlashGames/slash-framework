// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IEntityManager.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.ECS.Components
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    using Slash.Collections.AttributeTables;
    using Slash.ECS.Blueprints;
    using Slash.ECS.Events;

    public delegate void ComponentAddedDelegate<in T>(int entityId, T component);

    public delegate void ComponentRemovedDelegate<in T>(int entityId, T component);

    public interface IEntityManager
    {
        #region Public Properties

        IEnumerable<int> Entities { get; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///   Re-activates the entity with the specified id, if it is inactive.
        /// </summary>
        /// <param name="entityId">Id of the entity to activate.</param>
        void ActivateEntity(int entityId);

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
        void AddComponent(int entityId, IEntityComponent component);

        /// <summary>
        ///   Attaches a new component of the passed type to the entity with the specified id.
        /// </summary>
        /// <typeparam name="T">Type of the component to add.</typeparam>
        /// <param name="entityId">Id of the entity to attach the component to.</param>
        /// <returns>Attached component.</returns>
        T AddComponent<T>(int entityId) where T : IEntityComponent, new();

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
        void AddComponent(int entityId, IEntityComponent component, bool sendEvent);

        /// <summary>
        ///   Removes all entities that have been issued for removal during the
        ///   current tick, detaching all components.
        /// </summary>
        void CleanUpEntities();

        /// <summary>
        ///   Returns an iterator over all components of the specified type.
        /// </summary>
        /// <param name="type"> Type of the components to get. </param>
        /// <returns> Components of the specified type. </returns>
        /// <exception cref="ArgumentNullException">Specified type is null.</exception>
        IEnumerable ComponentsOfType(Type type);

        /// <summary>
        ///   Creates a new entity.
        /// </summary>
        /// <returns> Unique id of the new entity. </returns>
        int CreateEntity();

        /// <summary>
        ///   Creates a new entity with the specified id.
        /// </summary>
        /// <param name="id">Id of the entity to create.</param>
        /// <returns>Unique id of the new entity.</returns>
        int CreateEntity(int id);

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
        int CreateEntity(Blueprint blueprint, IAttributeTable configuration, List<Type> additionalComponents);

        /// <summary>
        ///   Creates a new entity, adding components matching the passed
        ///   blueprint.
        /// </summary>
        /// <param name="blueprint">Blueprint describing the entity to create.</param>
        /// <returns>Unique id of the new entity.</returns>
        int CreateEntity(Blueprint blueprint);

        /// <summary>
        ///   Creates a new entity, adding components of the blueprint with the specified id.
        /// </summary>
        /// <param name="blueprintId">Id of blueprint describing the entity to create.</param>
        /// <returns>Unique id of the new entity.</returns>
        int CreateEntity(string blueprintId);

        /// <summary>
        ///   Creates a new entity, adding components matching the passed
        ///   blueprint and initializing these with the data stored in the
        ///   blueprint and the specified configuration. Configuration data
        ///   is preferred over blueprint data.
        /// </summary>
        /// <param name="blueprintId"> Id of blueprint describing the entity to create. </param>
        /// <param name="configuration"> Data for initializing the entity. </param>
        /// <returns> Unique id of the new entity. </returns>
        int CreateEntity(string blueprintId, IAttributeTable configuration);

        /// <summary>
        ///   Creates a new entity, adding components matching the passed
        ///   blueprint and initializing these with the data stored in the
        ///   blueprint and the specified configuration. Configuration data
        ///   is preferred over blueprint data.
        /// </summary>
        /// <param name="blueprint"> Blueprint describing the entity to create. </param>
        /// <param name="configuration"> Data for initializing the entity. </param>
        /// <returns> Unique id of the new entity. </returns>
        int CreateEntity(Blueprint blueprint, IAttributeTable configuration);

        /// <summary>
        ///   De-activates the entity with the specified id. Inactive entities
        ///   are considered as removed, until they are re-activated again.
        /// </summary>
        /// <param name="entityId">Id of the entity to de-activate.</param>
        void DeactivateEntity(int entityId);

        /// <summary>
        ///   Returns an iterator over all entities having components of the specified type attached.
        /// </summary>
        /// <param name="type"> Type of the components to get the entities of. </param>
        /// <returns> Entities having components of the specified type attached. </returns>
        /// <exception cref="ArgumentNullException">Specified type is null.</exception>
        IEnumerable<int> EntitiesWithComponent(Type type);

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
        bool EntityIsAlive(int entityId);

        /// <summary>
        ///   Checks if the entity with the specified id will be removed this
        ///   frame.
        /// </summary>
        /// <param name="entityId">Id of the entity to check.</param>
        /// <returns>
        ///   <c>true</c>, if the entity with the specified id is about to be removed, and
        ///   <c>false</c>, otherwise.
        /// </returns>
        bool EntityIsBeingRemoved(int entityId);

        /// <summary>
        ///   Checks whether the entity with the specified id is inactive.
        /// </summary>
        /// <param name="id">Id of the entity to check.</param>
        /// <returns>
        ///   <c>true</c>, if the entity is inactive, and
        ///   <c>false</c> otherwise.
        /// </returns>
        bool EntityIsInactive(int id);

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
        IEntityComponent GetComponent(int entityId, Type componentType);

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
        T GetComponent<T>(int entityId) where T : IEntityComponent;

        /// <summary>
        ///   Retrieves an array containing the ids of all living entities in
        ///   O(n).
        /// </summary>
        /// <returns> Array containing the ids of all entities that haven't been removed yet. </returns>
        int[] GetEntities();

        /// <summary>
        ///   Returns the entity ids of all entities which fulfill the specified predicate.
        /// </summary>
        /// <param name="predicate"> Predicate to fulfill. </param>
        /// <returns> Collection of ids of all entities which fulfill the specified predicate. </returns>
        IEnumerable<int> GetEntities(Func<int, bool> predicate);

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
        bool GetEntityComponent<TComponent>(Entity2Data data, out int entityId, out TComponent component)
            where TComponent : class, IEntityComponent;

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
        bool GetEntityComponents<TComponentTypeA, TComponentTypeB>(
            Entity2Data data,
            out int entityIdA,
            out int entityIdB,
            out TComponentTypeA componentA,
            out TComponentTypeB componentB) where TComponentTypeA : class, IEntityComponent
            where TComponentTypeB : class, IEntityComponent;

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
        void InitEntity(
            int entityId, Blueprint blueprint, IAttributeTable configuration, IEnumerable<Type> additionalComponents);

        /// <summary>
        ///   Initializes the specified entity, adding the specified components.
        /// </summary>
        /// <param name="entityId">Id of the entity to initialize.</param>
        /// <param name="components">Initialized components to add to the entity.</param>
        void InitEntity(int entityId, IEnumerable<IEntityComponent> components);

        /// <summary>
        ///   Registers listeners to track adding/removing of components of type T.
        /// </summary>
        /// <typeparam name="T">Type of component to track.</typeparam>
        /// <param name="onComponentAdded">Callback when a new component of the type was added.</param>
        /// <param name="onComponentRemoved">Callback when a component of the type was removed.</param>
        void RegisterComponentListeners<T>(
            ComponentAddedDelegate<T> onComponentAdded, ComponentRemovedDelegate<T> onComponentRemoved);

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
        bool RemoveComponent(int entityId, Type componentType);

        /// <summary>
        ///   Removes all entities.
        /// </summary>
        void RemoveEntities();

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
        void RemoveEntity(int entityId);

        Blueprint Save(int entityId);

        /// <summary>
        ///   Tries to get a component of the passed type attached to the entity with the specified id.
        /// </summary>
        /// <param name="entityId">Id of the entity to get the component of.</param>
        /// <param name="componentType">Type of the component to get.</param>
        /// <param name="entityComponent">Retrieved entity component, or null, if no component could be found.</param>
        /// <returns>
        ///   <c>true</c>, if a component could be found, and <c>false</c> otherwise.
        /// </returns>
        bool TryGetComponent(int entityId, Type componentType, out IEntityComponent entityComponent);

        /// <summary>
        ///   Tries to get a component of the passed type attached to the entity with the specified id.
        /// </summary>
        /// <typeparam name="T">Type of the component to get.</typeparam>
        /// <param name="entityId">Id of the entity to get the component of.</param>
        /// <param name="entityComponent">Retrieved entity component, or null, if no component could be found.</param>
        /// <returns>
        ///   <c>true</c>, if a component could be found, and <c>false</c> otherwise.
        /// </returns>
        bool TryGetComponent<T>(int entityId, out T entityComponent) where T : IEntityComponent;

        #endregion
    }
}