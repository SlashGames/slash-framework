// -----------------------------------------------------------------------
// <copyright file="ComponentEventArgs.cs" company="Rainy Games">
// Copyright (c) Rainy Games. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace RainyGames.GameBase.EventArgs
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
        /// <param name="component">
        /// Component that has been interacted with.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// Passed component is null.
        /// </exception>
        public ComponentEventArgs(long entityId, IComponent component)
        {
            if (component == null)
            {
                throw new ArgumentNullException("component");
            }

            this.EntityId = entityId;
            this.Component = component;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Id of the entity the component event has been fired for.
        /// </summary>
        public long EntityId { get; private set; }

        /// <summary>
        /// Component that has been interacted with.
        /// </summary>
        public IComponent Component { get; private set; }

        #endregion
    }
}
