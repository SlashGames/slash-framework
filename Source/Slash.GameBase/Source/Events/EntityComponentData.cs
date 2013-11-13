// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EntityComponentData.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.GameBase.Events
{
    using System;
    
    using Slash.GameBase.Components;

    /// <summary>
    ///   Data container holding information on a component event, such as the
    ///   entity and the component the event occurred for.
    /// </summary>
    public class EntityComponentData
    {
        #region Constructors and Destructors

        /// <summary>
        ///   Constructs a new data object holding information on a component
        ///   event.
        /// </summary>
        /// <param name="entityId">
        ///   Id of the entity the component event has been fired for.
        /// </param>
        /// <param name="component">
        ///   Component that has been interacted with.
        /// </param>
        /// <exception cref="ArgumentNullException">
        ///   Passed component is null.
        /// </exception>
        public EntityComponentData(int entityId, IEntityComponent component)
        {
            if (component == null)
            {
                throw new ArgumentNullException("component");
            }

            this.EntityId = entityId;
            this.Component = component;
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///   Component that has been interacted with.
        /// </summary>
        public IEntityComponent Component { get; private set; }

        /// <summary>
        ///   Id of the entity the component event has been fired for.
        /// </summary>
        public int EntityId { get; private set; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///   Returns the id of the entity and the component for which this event has occurred.
        /// </summary>
        /// <returns>Entity id and component.</returns>
        public override string ToString()
        {
            return string.Format("Entity id: {0}, component: {1}", this.EntityId, this.Component);
        }

        #endregion
    }
}