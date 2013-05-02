// -----------------------------------------------------------------------
// <copyright file="ComponentEventArgs.cs" company="Slash Games">
// Copyright (c) Slash Games. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace SlashGames.GameBase.EventArgs
{
    using System;

    /// <summary>
    /// Data container holding information on a component event, such as the
    /// entity and the component the event occurred for.
    /// </summary>
    public class ComponentEventArgs
    {
        #region Constructors and Destructors

        /// <summary>
        /// Constructs a new data object holding information on a component
        /// event.
        /// </summary>
        /// <param name="entityId">
        /// Id of the entity the component event has been fired for.
        /// </param>
        /// <param name="entityComponent">
        /// Component that has been interacted with.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// Passed component is null.
        /// </exception>
        public ComponentEventArgs(int entityId, IEntityComponent entityComponent)
        {
            if (entityComponent == null)
            {
                throw new ArgumentNullException("entityComponent");
            }

            this.EntityId = entityId;
            this.EntityComponent = entityComponent;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Id of the entity the component event has been fired for.
        /// </summary>
        public int EntityId { get; private set; }

        /// <summary>
        /// Component that has been interacted with.
        /// </summary>
        public IEntityComponent EntityComponent { get; private set; }

        #endregion

        #region Public Methods

        /// <summary>
        /// Returns the id of the entity and the component for which this event has occurred.
        /// </summary>
        /// <returns>Entity id and component.</returns>
        public override string ToString()
        {
            return string.Format("Entity id: {0}, component: {1}", this.EntityId, this.EntityComponent);
        }

        #endregion
    }
}
