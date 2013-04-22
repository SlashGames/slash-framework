// -----------------------------------------------------------------------
// <copyright file="Event.cs" company="Slash Games">
// Copyright (c) Slash Games. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace SlashGames.GameBase
{
    using System;

    /// <summary>
    /// Event that has occurred within the Slash Games Framework. May contain
    /// additional data listeners might be interested in.
    /// </summary>
    public class Event
    {
        #region Constructors and Destructors

        /// <summary>
        /// Constructs a new event of the specified type.
        /// </summary>
        /// <param name="eventType">
        /// Type of the new event.
        /// </param>
        public Event(object eventType)
            : this(eventType, null)
        {
        }

        /// <summary>
        /// Constructs a new event of the specified type and holding the
        /// passed event data.
        /// </summary>
        /// <param name="eventType">
        /// Type of the new event.
        /// </param>
        /// <param name="eventData">
        /// Data any listener might be interested in.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// Specified event type is null.
        /// </exception>
        public Event(object eventType, object eventData)
        {
            if (eventType == null)
            {
                throw new ArgumentNullException("eventType");
            }

            this.EventType = eventType;
            this.EventData = eventData;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Type of this event.
        /// </summary>
        public object EventType { get; private set; }

        /// <summary>
        /// Data any listener might be interested in.
        /// </summary>
        public object EventData { get; private set; }

        #endregion

        #region Public Methods

        /// <summary>
        /// Returns the type and data of this event, as string.
        /// </summary>
        /// <returns>Type and data of this event, as string.</returns>
        public override string ToString()
        {
            return string.Format("Event: {0} - Event data: {1}", this.EventType, this.EventData);
        }

        #endregion
    }
}
