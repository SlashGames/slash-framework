// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EventManagerLogBehaviour.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Unity.Common.Logging
{
    using System.Collections.Generic;

    using Slash.ECS.Events;

    using UnityEngine;

    /// <summary>
    ///   Behaviour to log all events of a specific event manager.
    /// </summary>
    public class EventManagerLogBehaviour : MonoBehaviour
    {
        #region Fields

        /// <summary>
        ///   Indicates if logging is currently disabled.
        /// </summary>
        public bool Disabled;

        /// <summary>
        ///   List of disabled event types. Stored as strings, otherwise Unity doesn't serialize them.
        /// </summary>
        [SerializeField]
        [HideInInspector]
        // ReSharper disable FieldCanBeMadeReadOnly.Local
        private List<string> disabledEventTypes = new List<string>();

        private EventManager eventManager;

        #endregion

        #region Public Properties

        public EventManager EventManager
        {
            get
            {
                return this.eventManager;
            }
            set
            {
                if (value == this.eventManager)
                {
                    return;
                }

                if (this.eventManager != null)
                {
                    this.eventManager.RemoveListener(this.OnEvent);
                }

                this.eventManager = value;

                if (this.eventManager != null)
                {
                    this.eventManager.RegisterListener(this.OnEvent);
                }
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///   Logs the passed message.
        /// </summary>
        /// <param name="s">Message to log.</param>
        public void Info(string s)
        {
            if (!this.Disabled)
            {
                Debug.Log(s);
            }
        }

        /// <summary>
        ///   Checks whether the specified event type is being logged, or not.
        /// </summary>
        /// <param name="eventType">Event type to check.</param>
        /// <returns>
        ///   <c>true</c>, if the specified event type is being logged, and
        ///   <c>false</c>, otherwise.
        /// </returns>
        public bool IsDisabled(object eventType)
        {
            return this.disabledEventTypes.Contains(this.ConvertEventTypeToString(eventType));
        }

        /// <summary>
        ///   Enables or disables logging for events of the specified type.
        /// </summary>
        /// <param name="eventType">Event type to start or stop logging.</param>
        /// <param name="logDisabled">Whether to enable or disable logging.</param>
        public void SetDisabled(object eventType, bool logDisabled)
        {
            var eventTypeString = this.ConvertEventTypeToString(eventType);
            if (logDisabled)
            {
                this.disabledEventTypes.Add(eventTypeString);
            }
            else
            {
                this.disabledEventTypes.Remove(eventTypeString);
            }
        }

        #endregion

        #region Methods

        protected virtual string FormatLog(string log)
        {
            return this.WithTimestamp(log);
        }

        /// <summary>
        ///   Adds a timestamp to the specified string.
        /// </summary>
        /// <param name="message">String to add a timestamp to.</param>
        /// <returns>Timestamped message.</returns>
        protected string WithTimestamp(string message)
        {
            return string.Format("[{0:000.000}] {1}", Time.realtimeSinceStartup, message);
        }

        private void OnEvent(GameEvent e)
        {
            if (this.Disabled)
            {
                return;
            }

            string eventTypeString = this.ConvertEventTypeToString(e.EventType);
            if (!this.disabledEventTypes.Contains(eventTypeString))
            {
                this.Info(
                    e.EventData != null
                        ? this.FormatLog(string.Format("{0}: {1}", eventTypeString, e.EventData))
                        : this.FormatLog(eventTypeString));
            }
        }

        private string ConvertEventTypeToString(object eventType)
        {
            return string.Format("{0}.{1}", eventType.GetType().Name, eventType);
        }

        #endregion
    }
}