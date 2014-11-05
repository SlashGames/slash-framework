// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GameLogBehaviour.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Unity.Common.Logging
{
    using Slash.ECS;
    using Slash.Unity.Common.ECS;

    using UnityEngine;

    /// <summary>
    ///   Writes all game logic events to the Unity console.
    /// </summary>
    public class GameLogBehaviour : EventManagerLogBehaviour
    {
        #region Fields

        /// <summary>
        ///   Game to log events of.
        /// </summary>
        private GameBehaviour gameBehaviour;

        #endregion

        // ReSharper restore FieldCanBeMadeReadOnly.Local

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

        protected override string FormatLog(string log)
        {
            return this.WithTimestamp(string.Format("{0} (Frame: {1})", log, this.gameBehaviour.FrameCounter));
        }

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
            this.Error(this.WithTimestamp(message));
        }

        private void OnGameChanged(Game newGame, Game oldGame)
        {
            if (oldGame != null)
            {
                oldGame.Log.InfoLogged -= this.OnInfoLogged;
                oldGame.Log.WarningLogged -= this.OnWarningLogged;
                oldGame.Log.ErrorLogged -= this.OnErrorLogged;
            }

            if (newGame != null)
            {
                newGame.Log.InfoLogged += this.OnInfoLogged;
                newGame.Log.WarningLogged += this.OnWarningLogged;
                newGame.Log.ErrorLogged += this.OnErrorLogged;
            }

            this.EventManager = newGame != null ? newGame.EventManager : null;
        }

        private void OnInfoLogged(string message)
        {
            this.Info(this.WithTimestamp(message));
        }

        private void OnWarningLogged(string message)
        {
            this.Warning(this.WithTimestamp(message));
        }

        #endregion
    }
}