// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Event.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.GameBase.Events
{
    using System;
    
    /// <summary>
    ///   Event that has occurred within the entity system. May contain
    ///   additional data listeners might be interested in.
    /// </summary>
    public class GameEvent
    {
        #region Constructors and Destructors

        /// <summary>
        ///   Constructs a new event of the specified type.
        /// </summary>
        /// <param name="eventType">
        ///   Type of the new event.
        /// </param>
        public GameEvent(object eventType)
            : this(eventType, null)
        {
        }

        /// <summary>
        ///   Constructs a new event of the specified type and holding the
        ///   passed event data.
        /// </summary>
        /// <param name="eventType">
        ///   Type of the new event.
        /// </param>
        /// <param name="eventData">
        ///   Data any listener might be interested in.
        /// </param>
        /// <exception cref="ArgumentNullException">
        ///   Specified event type is null.
        /// </exception>
        public GameEvent(object eventType, object eventData)
        {
            if (eventType == null)
            {
                throw new ArgumentNullException("eventType");
            }

            this.EventType = eventType;
            this.EventData = eventData;
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///   Data any listener might be interested in.
        /// </summary>
        public object EventData { get; private set; }

        /// <summary>
        ///   Type of this event.
        /// </summary>
        public object EventType { get; private set; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///   Returns the type and data of this event, as string.
        /// </summary>
        /// <returns>Type and data of this event, as string.</returns>
        public override string ToString()
        {
            return string.Format("Event: {0} - Event data: {1}", this.EventType, this.EventData);
        }

        #endregion
    }
}