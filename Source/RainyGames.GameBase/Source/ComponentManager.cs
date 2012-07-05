// -----------------------------------------------------------------------
// <copyright file="ComponentManager.cs" company="Rainy Games">
// Copyright (c) Rainy Games. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace RainyGames.GameBase
{
    using System;
    using System.Collections.Generic;
    using RainyGames.GameBase.EventArgs;

    /// <summary>
    /// Maps entity ids to specific game components. By contract this manager
    /// should be responsible for mapping components of a single type, only.
    /// This way, entity ids can be mapped to different components, one of each
    /// type.
    /// </summary>
    public class ComponentManager
    {
        #region Constants and Fields

        /// <summary>
        /// Game this manager maps the entity ids of.
        /// </summary>
        private Game game;

        /// <summary>
        /// Components attached to game entities.
        /// </summary>
        private Dictionary<long, IComponent> components;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Constructs a new component manager without any initial components.
        /// </summary>
        /// <param name="game">
        /// Game to map the entity ids of.
        /// </param>
        public ComponentManager(Game game)
        {
            this.game = game;
            this.components = new Dictionary<long, IComponent>();
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Attaches the passed component to the entity with the specified id.
        /// Note that this manager does not check whether the specified id is valid.
        /// </summary>
        /// <param name="entityId">
        /// Id of the entity to attach the component to.
        /// </param>
        /// <param name="component">
        /// Component to attach.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// Passed component is null.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// There is already a component of the same type attached.
        /// </exception>
        public void AddComponent(long entityId, IComponent component)
        {
            if (component == null)
            {
                throw new ArgumentNullException("component");
            }

            if (!this.components.ContainsKey(entityId))
            {
                this.components.Add(entityId, component);
                this.game.EventManager.QueueEvent(FrameworkEventType.ComponentAdded, new ComponentEventArgs(entityId, component));
            }
            else
            {
                throw new InvalidOperationException("There is already a component of type "
                    + component.GetType() + " attached to entity with id " + entityId + ".");
            }
        }

        /// <summary>
        /// Removes the component mapped to the entity with the specified id.
        /// Note that this manager does not check whether the specified id is valid.
        /// </summary>
        /// <param name="entityId">
        /// Id of the entity to remove the component from.
        /// </param>
        /// <returns>
        /// Whether a component has been removed, or not.
        /// </returns>
        public bool RemoveComponent(long entityId)
        {
            IComponent component;
            if (this.components.TryGetValue(entityId, out component))
            {
                this.components.Remove(entityId);
                this.game.EventManager.QueueEvent(FrameworkEventType.ComponentRemoved, new ComponentEventArgs(entityId, component));
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Gets the component mapped to the entity with the specified id.
        /// Note that this manager does not check whether the specified id is valid.
        /// </summary>
        /// <param name="entityId">
        /// Id of the entity to get the component of.
        /// </param>
        /// <returns>
        /// The component, if there is one attached to the entity, and null otherwise.
        /// </returns>
        public IComponent GetComponent(long entityId)
        {
            IComponent component;
            this.components.TryGetValue(entityId, out component);
            return component;
        }

        #endregion
    }
}
