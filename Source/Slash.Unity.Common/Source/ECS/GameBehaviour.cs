// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GameBehaviour.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Unity.Common.ECS
{
    using Slash.Collections.AttributeTables;
    using Slash.ECS;
    using Slash.ECS.Blueprints;
    using Slash.Unity.Common.Configuration;

    using UnityEngine;

    /// <summary>
    ///   Interface between Unity and the core game logic.
    /// </summary>
    public class GameBehaviour : MonoBehaviour
    {
        #region Fields

        /// <summary>
        ///   Number of frames till behaviour was enabled.
        /// </summary>
        public int FrameCounter;

        /// <summary>
        ///   Current game instance.
        /// </summary>
        private Game game;

        #endregion

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
        public Game Game
        {
            get
            {
                return this.game;
            }
            set
            {
                if (this.game == value)
                {
                    return;
                }

                var oldGame = this.game;
                this.game = value;

                this.OnGameChanged(this.game, oldGame);
            }
        }

        /// <summary>
        ///   Behaviour to manage game configuration.
        /// </summary>
        public GameConfigurationBehaviour GameConfiguration { get; private set; }

        #endregion

        #region Public Methods and Operators

        public void StartGame(Game newGame)
        {
            // Create game.
            this.Game = newGame;

            // Get game configuration.
            var gameConfiguration = this.GameConfiguration != null
                                        ? new AttributeTable(this.GameConfiguration.Configuration)
                                        : null;

            // Load game data.
            if (this.GameConfiguration != null)
            {
                this.Game.BlueprintManager = this.GameConfiguration.BlueprintManager;
            }
            else
            {
                Debug.LogError("No game configuration set.");
                this.Game.BlueprintManager = new BlueprintManager();
            }

            // Start game.
            this.Game.StartGame(gameConfiguration);
        }

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

            ++this.FrameCounter;
        }

        #endregion
    }
}