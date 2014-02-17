// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LogBehaviour.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Unity.Common.Logging
{
    using System.Collections.Generic;

    using Slash.GameBase;
    using Slash.Unity.Common.Core;
    using Slash.Unity.Common.Utils;

    using UnityEngine;

    using Event = Slash.GameBase.Events.Event;

    public class LogBehaviour : MonoBehaviour
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
        private readonly List<string> disabledEventTypes = new List<string>();

        /// <summary>
        ///   Game to log events of.
        /// </summary>
        private GameBehaviour gameBehaviour;

        #endregion

        #region Public Methods and Operators

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
            return this.disabledEventTypes.Contains(eventType.ToString());
        }

        /// <summary>
        ///   Enables or disables logging for events of the specified type.
        /// </summary>
        /// <param name="eventType">Event type to start or stop logging.</param>
        /// <param name="logDisabled">Whether to enable or disable logging.</param>
        public void SetDisabled(object eventType, bool logDisabled)
        {
            if (logDisabled)
            {
                this.disabledEventTypes.Add(eventType.ToString());
            }
            else
            {
                this.disabledEventTypes.Remove(eventType.ToString());
            }
        }

        #endregion

        #region Methods

        private void Awake()
        {
            this.gameBehaviour = (GameBehaviour)FindObjectOfType(typeof(GameBehaviour));

            if (this.gameBehaviour != null)
            {
                this.gameBehaviour.GameChanged += this.OnGameChanged;
            }
        }

        private void OnDestroy()
        {
            if (this.gameBehaviour != null)
            {
                this.gameBehaviour.GameChanged -= this.OnGameChanged;
            }
        }

        private void OnEvent(Event e)
        {
            if (this.Disabled)
            {
                return;
            }

            if (!this.disabledEventTypes.Contains(e.EventType.ToString()))
            {
                Debug.Log(UnityUtils.WithTimestamp(string.Format("{0}: {1}", e.EventType, e.EventData)));
            }
        }

        private void OnGameChanged(Game newGame, Game oldGame)
        {
            if (oldGame != null)
            {
                oldGame.EventManager.RemoveListener(this.OnEvent);
            }

            if (newGame != null)
            {
                newGame.EventManager.RegisterListener(this.OnEvent);
            }
        }

        #endregion
    }
}