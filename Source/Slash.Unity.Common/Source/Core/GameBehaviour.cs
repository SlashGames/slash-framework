// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GameBehaviour.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Unity.Common.Core
{
    using Slash.GameBase;
    using Slash.Unity.Common.Configuration;

    using UnityEngine;

    /// <summary>
    ///   Interface between Unity and the core game logic.
    /// </summary>
    public class GameBehaviour : MonoBehaviour
    {
        #region Delegates

        /// <summary>
        ///   Current game has changed.
        /// </summary>
        /// <param name="newGame">New game instance.</param>
        /// <param name="oldGame">Previous game instance.</param>
        public delegate void GameChangedDelegate(Game newGame, Game oldGame);

        #endregion

        #region Public Events

        /// <summary>
        ///   Current game has changed.
        /// </summary>
        public event GameChangedDelegate GameChanged;

        #endregion

        #region Public Properties

        /// <summary>
        ///   Current game instance.
        /// </summary>
        public Game Game { get; set; }

        /// <summary>
        ///   Behaviour to manage game configuration.
        /// </summary>
        public GameConfigurationBehaviour GameConfiguration { get; private set; }

        #endregion

        #region Methods

        protected virtual void Awake()
        {
            if (this.GameConfiguration == null)
            {
                this.GameConfiguration =
                    (GameConfigurationBehaviour)FindObjectOfType(typeof(GameConfigurationBehaviour));
            }
        }

        protected void OnGameChanged(Game newGame, Game oldGame)
        {
            var handler = this.GameChanged;
            if (handler != null)
            {
                handler(newGame, oldGame);
            }
        }

        protected virtual void Update()
        {
            if (this.Game != null)
            {
                this.Game.Update(Time.deltaTime);
            }
        }

        #endregion
    }
}