// -----------------------------------------------------------------------
// <copyright file="EventManager.cs" company="Rainy Games">
// Copyright (c) Rainy Games. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace RainyGames.GameBase
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Allows listeners to register for game-related events and notifies them
    /// whenever one of these events is fired.
    /// </summary>
    public class EventManager
    {
        #region Constants and Fields

        /// <summary>
        /// Queue of events to be processed.
        /// </summary>
        private List<Event> events;

        /// <summary>
        /// Listeners that are interested in events of specific types.
        /// </summary>
        private Dictionary<object, List<IEventListener>> listeners;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Constructs a new event manager with an empty event queue and
        /// without any listeners.
        /// </summary>
        public EventManager()
        {
            this.events = new List<Event>();
            this.listeners = new Dictionary<object, List<IEventListener>>();
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Makes the passed listener register interest for events of the
        /// specified type.
        /// </summary>
        /// <param name="listener">
        /// Listener that is interested in events of the specified type.
        /// </param>
        /// <param name="eventType">
        /// Type of the event the passed listener is interested in.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// Passed listener is null.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// Specified event type is null.
        /// </exception>
        public void RegisterListener(IEventListener listener, object eventType)
        {
            if (listener == null)
            {
                throw new ArgumentNullException("listener");
            }

            if (eventType == null)
            {
                throw new ArgumentNullException("eventType");
            }

            if (!this.listeners.ContainsKey(eventType))
            {
                this.listeners.Add(eventType, new List<IEventListener>());
            }

            this.listeners[eventType].Add(listener);
        }

        /// <summary>
        /// Queues a new event of the specified type without any additional
        /// data.
        /// </summary>
        /// <param name="eventType">
        /// Type of the event to queue.
        /// </param>
        public void QueueEvent(object eventType)
        {
            this.QueueEvent(eventType, null);
        }

        /// <summary>
        /// Queues a new event of the specified type along with the passed
        /// event data.
        /// </summary>
        /// <param name="eventType">
        /// Type of the event to queue.
        /// </param>
        /// <param name="eventData">
        /// Data any listeners might be interested in.
        /// </param>
        public void QueueEvent(object eventType, object eventData)
        {
            this.QueueEvent(new Event(eventType, eventData));
        }

        /// <summary>
        /// Queues the passed event to be processed later.
        /// </summary>
        /// <param name="e">
        /// Event to queue.
        /// </param>
        public void QueueEvent(Event e)
        {
            this.events.Add(e);
        }

        /// <summary>
        /// Passes all queued events on to interested listeners and clears the
        /// event queue.
        /// </summary>
        public void ProcessEvents()
        {
            foreach (Event e in this.events)
            {
                if (this.listeners.ContainsKey(e.EventType))
                {
                    List<IEventListener> eventListeners = this.listeners[e.EventType];

                    foreach (IEventListener listener in eventListeners)
                    {
                        listener.Notify(e);
                    }
                }
            }

            this.events.Clear();
        }

        #endregion
    }
}
