// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LogBehaviour.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Unity.Common.Logging
{
    using System.Collections.Generic;

    using Slash.GameBase;
    using Slash.GameBase.Events;
    using Slash.Unity.Common.Core;
    using Slash.Unity.Common.Utils;

    using UnityEngine;

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
        // ReSharper disable FieldCanBeMadeReadOnly.Local
        private List<string> disabledEventTypes = new List<string>();

        // ReSharper restore FieldCanBeMadeReadOnly.Local

        /// <summary>
        ///   Game to log events of.
        /// </summary>
        private GameBehaviour gameBehaviour;

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///   Logs the passed message as error.
        /// </summary>
        /// <param name="s">Error to log.</param>
        public void Error(string s)
        {
            if (!this.Disabled)
            {
                Debug.LogError(s);
            }
        }

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

        /// <summary>
        ///   Logs the passed message as warning.
        /// </summary>
        /// <param name="s">Warning to log.</param>
        public void Warning(string s)
        {
            if (!this.Disabled)
            {
                Debug.LogWarning(s);
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

        private void OnErrorLogged(string message)
        {
            this.Error(UnityUtils.WithTimestamp(message));
        }

        private void OnEvent(GameEvent e)
        {
            if (this.Disabled)
            {
                return;
            }

            if (!this.disabledEventTypes.Contains(e.EventType.ToString()))
            {
                this.Info(
                    UnityUtils.WithTimestamp(
                        string.Format(
                            "{0}: {1} (Frame: {2})", e.EventType, e.EventData, this.gameBehaviour.FrameCounter)));
            }
        }

        private void OnGameChanged(Game newGame, Game oldGame)
        {
            if (oldGame != null)
            {
                oldGame.EventManager.RemoveListener(this.OnEvent);

                oldGame.Log.InfoLogged -= this.OnInfoLogged;
                oldGame.Log.WarningLogged -= this.OnWarningLogged;
                oldGame.Log.ErrorLogged -= this.OnErrorLogged;
            }

            if (newGame != null)
            {
                newGame.EventManager.RegisterListener(this.OnEvent);

                newGame.Log.InfoLogged += this.OnInfoLogged;
                newGame.Log.WarningLogged += this.OnWarningLogged;
                newGame.Log.ErrorLogged += this.OnErrorLogged;
            }
        }

        private void OnInfoLogged(string message)
        {
            this.Info(UnityUtils.WithTimestamp(message));
        }

        private void OnWarningLogged(string message)
        {
            this.Warning(UnityUtils.WithTimestamp(message));
        }

        #endregion
    }
}