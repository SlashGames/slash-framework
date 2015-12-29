// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GameBehaviour.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Unity.Common.ECS
{
    using Slash.Application.Games;
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
        ///   Number of frames while behaviour was enabled.
        /// </summary>
        public int FrameCounter;

        /// <summary>
        ///   Whether to start the game immediately when the scene is loaded.
        /// </summary>
        public bool StartImmediately = true;

        /// <summary>
        ///   Update period of game (in s).
        ///   0 if no fixed update.
        /// </summary>
        [Tooltip("Update period of game (in s). 0 if no fixed update.")]
        public float UpdatePeriod;

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

        #region Events

        /// <summary>
        ///   Current game has changed.
        /// </summary>
        public event GameChangedDelegate GameChanged;

        #endregion

        #region Properties

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

        public static GameBehaviour Instance { get; private set; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///   Starts the specified game with the current configuration, notifying
        ///   interested listeners of the new game.
        /// </summary>
        /// <param name="newGame">Game to start.</param>
        /// <seealso cref="GameConfiguration" />
        public void StartGame(Game newGame)
        {
            // Get game configuration.
            var gameConfiguration = this.GameConfiguration != null
                ? new AttributeTable(this.GameConfiguration.Configuration)
                : null;

            // Load game data.
            if (this.GameConfiguration != null)
            {
                newGame.BlueprintManager = this.GameConfiguration.BlueprintManager;
            }
            else
            {
                Debug.LogError("No game configuration set.");
                newGame.BlueprintManager = new BlueprintManager();
            }

            // Create game.
            this.Game = newGame;

            // Setup game.
            this.Game.UpdatePeriod = this.UpdatePeriod;

            // Start game.
            this.Game.StartGame(gameConfiguration);
        }

        #endregion

        #region Methods

        /// <summary>
        ///   Loads the game configuration, if available.
        /// </summary>
        private void Awake()
        {
            Instance = this;

            if (this.GameConfiguration == null)
            {
                this.GameConfiguration =
                    (GameConfigurationBehaviour)FindObjectOfType(typeof(GameConfigurationBehaviour));
            }
        }

        protected void OnDestroy()
        {
            if (this.Game != null)
            {
                this.Game.StopGame();
                this.Game = null;
            }
        }

        /// <summary>
        ///   Notifies interested listeners of new games.
        /// </summary>
        /// <param name="newGame">New game instance.</param>
        /// <param name="oldGame">Previous game instance.</param>
        private void OnGameChanged(Game newGame, Game oldGame)
        {
            var handler = this.GameChanged;
            if (handler != null)
            {
                handler(newGame, oldGame);
            }
        }

        private void Start()
        {
            if (this.StartImmediately)
            {
                this.StartGame(new Game());
            }
        }

        /// <summary>
        ///   Ticks the current game and increases the frame counter.
        /// </summary>
        private void Update()
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