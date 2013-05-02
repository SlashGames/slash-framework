// -----------------------------------------------------------------------
// <copyright file="ComponentManager.cs" company="Slash Games">
// Copyright (c) Slash Games. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace SlashGames.GameBase
{
    using System;
    using System.Collections.Generic;

    using SlashGames.GameBase.EventArgs;

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
        private readonly Game game;

        /// <summary>
        /// Components attached to game entities.
        /// </summary>
        private readonly Dictionary<int, IEntityComponent> components;

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
            this.components = new Dictionary<int, IEntityComponent>();
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
        /// <param name="entityComponent">
        /// Component to attach.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// Passed component is null.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// There is already a component of the same type attached.
        /// </exception>
        public void AddComponent(int entityId, IEntityComponent entityComponent)
        {
            if (entityComponent == null)
            {
                throw new ArgumentNullException("entityComponent");
            }

            if (this.components.ContainsKey(entityId))
            {
                throw new InvalidOperationException(
                    "There is already a component of type " + entityComponent.GetType() + " attached to entity with id "
                    + entityId + ".");
            }

            this.components.Add(entityId, entityComponent);
            this.game.EventManager.QueueEvent(
                FrameworkEventType.ComponentAdded, new ComponentEventArgs(entityId, entityComponent));
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
        public bool RemoveComponent(int entityId)
        {
            IEntityComponent entityComponent;

            if (!this.components.TryGetValue(entityId, out entityComponent))
            {
                return false;
            }

            this.components.Remove(entityId);
            this.game.EventManager.QueueEvent(
                FrameworkEventType.ComponentRemoved, new ComponentEventArgs(entityId, entityComponent));
            return true;
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
        public IEntityComponent GetComponent(int entityId)
        {
            IEntityComponent entityComponent;
            this.components.TryGetValue(entityId, out entityComponent);
            return entityComponent;
        }

        /// <summary>
        /// Returns an iterator over all components of this manager.
        /// </summary>
        /// <returns>Components of this manager.</returns>
        public System.Collections.IEnumerable Components()
        {
            foreach (KeyValuePair<int, IEntityComponent> component in this.components)
            {
                yield return component;
            }
        }
        #endregion
    }
}
