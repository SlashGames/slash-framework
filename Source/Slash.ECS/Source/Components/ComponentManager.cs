// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ComponentManager.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.ECS.Components
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    /// <summary>
    ///   Maps entity ids to specific game components. By contract this manager
    ///   should be responsible for mapping components of a single type, only.
    ///   This way, entity ids can be mapped to different components, one of each
    ///   type.
    /// </summary>
    public class ComponentManager
    {
        #region Fields

        /// <summary>
        ///   Components attached to game entities.
        /// </summary>
        private readonly Dictionary<int, IEntityComponent> components;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///   Constructs a new component manager without any initial components.
        /// </summary>
        public ComponentManager()
        {
            this.components = new Dictionary<int, IEntityComponent>();
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///   Attaches the passed component to the entity with the specified id.
        ///   Note that this manager does not check whether the specified id is valid.
        /// </summary>
        /// <param name="entityId">
        ///   Id of the entity to attach the component to.
        /// </param>
        /// <param name="component">
        ///   Component to attach.
        /// </param>
        /// <exception cref="ArgumentNullException">
        ///   Passed component is null.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        ///   There is already a component of the same type attached.
        /// </exception>
        public void AddComponent(int entityId, IEntityComponent component)
        {
            if (component == null)
            {
                throw new ArgumentNullException("component");
            }

            if (this.components.ContainsKey(entityId))
            {
                throw new InvalidOperationException(
                    "There is already a component of type " + component.GetType() + " attached to entity with id "
                    + entityId + ".");
            }

            this.components.Add(entityId, component);
        }

        /// <summary>
        ///   Returns an iterator over all components of this manager.
        /// </summary>
        /// <returns>Components of this manager.</returns>
        public IEnumerable Components()
        {
            return this.components.Values;
        }

        /// <summary>
        ///   Returns an iterator over all entities having components of this manager attached.
        /// </summary>
        /// <returns>Entities having components of this manager attached.</returns>
        public IEnumerable<int> Entities()
        {
            return this.components.Keys;
        }

        /// <summary>
        ///   Gets the component mapped to the entity with the specified id.
        ///   Note that this manager does not check whether the specified id is valid.
        /// </summary>
        /// <param name="entityId">
        ///   Id of the entity to get the component of.
        /// </param>
        /// <returns>
        ///   The component, if there is one attached to the entity, and null otherwise.
        /// </returns>
        public IEntityComponent GetComponent(int entityId)
        {
            IEntityComponent component;
            this.components.TryGetValue(entityId, out component);
            return component;
        }

        /// <summary>
        ///   Removes the component mapped to the entity with the specified id.
        ///   Note that this manager does not check whether the specified id is valid.
        /// </summary>
        /// <param name="entityId">
        ///   Id of the entity to remove the component from.
        /// </param>
        /// <returns>
        ///   Whether a component has been removed, or not.
        /// </returns>
        public bool RemoveComponent(int entityId)
        {
            IEntityComponent component;
            return this.RemoveComponent(entityId, out component);
        }

        /// <summary>
        ///   Removes the component mapped to the entity with the specified id.
        ///   Note that this manager does not check whether the specified id is valid.
        /// </summary>
        /// <param name="entityId">
        ///   Id of the entity to remove the component from.
        /// </param>
        /// <param name="component">Removed component.</param>
        /// <returns>
        ///   Whether a component has been removed, or not.
        /// </returns>
        public bool RemoveComponent(int entityId, out IEntityComponent component)
        {
            if (this.components.TryGetValue(entityId, out component))
            {
                this.components.Remove(entityId);
                return true;
            }

            return false;
        }

        #endregion
    }
}