// -----------------------------------------------------------------------
// <copyright file="Game.cs" company="Rainy Games">
// Copyright (c) Rainy Games. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace RainyGames.GameBase
{
    using System.Collections.Generic;

    /// <summary>
    /// Base class of most Rainy Games games. Provides default functionality
    /// that is common across many games, such as components that are attached
    /// to entities, or systems working on these components.
    /// </summary>
    public class Game
    {
        #region Constants and Fields

        /// <summary>
        /// Manager responsible for creating and removing entities in this game.
        /// </summary>
        private EntityManager entityManager;

        /// <summary>
        /// Manager responsible for updating all game systems in each tick.
        /// </summary>
        private SystemManager systemManager;

        /// <summary>
        /// Manager allowing listeners to register for game-related events.
        /// </summary>
        private EventManager eventManager;

        /// <summary>
        /// Players participating in this game.
        /// </summary>
        private List<Player> players;

        /// <summary>
        /// Whether this game is running, or not (e.g. not yet started,
        /// paused, or already over).
        /// </summary>
        private bool running;
        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Constructs a new game without players.
        /// </summary>
        public Game()
        {
            this.entityManager = new EntityManager(this);
            this.systemManager = new SystemManager(this);
            this.eventManager = new EventManager();
            this.players = new List<Player>();
            this.running = false;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Name of this game.
        /// </summary>
        public string GameName { get; set; }

        /// <summary>
        /// Whether this game is running, or not (e.g. not yet started,
        /// paused, or already over).
        /// </summary>
        public bool Running
        {
            get { return this.running; }
        }

        /// <summary>
        /// Manager responsible for creating and removing entities in this game.
        /// </summary>
        public EntityManager EntityManager
        {
            get { return this.entityManager; }
        }

        /// <summary>
        /// Manager responsible for updating all game systems in each tick.
        /// </summary>
        public SystemManager SystemManager
        {
            get { return this.systemManager; }
        }

        /// <summary>
        /// Manager allowing listeners to register for game-related events.
        /// </summary>
        public EventManager EventManager
        {
            get { return this.eventManager; }
        }
        #endregion

        #region Public Methods

        /// <summary>
        /// Ticks this game, allowing all systems to update themselves.
        /// </summary>
        /// <param name="dt">
        /// Time passed since the last tick.
        /// </param>
        public void Update(float dt)
        {
            if (this.running)
            {
                this.systemManager.Update(dt);
            }
        }

        /// <summary>
        /// Adds the passed player to this game.
        /// </summary>
        /// <param name="player">
        /// Player to add to this game.
        /// </param>
        public void AddPlayer(Player player)
        {
            this.players.Add(player);
            this.eventManager.InvokePlayerAdded(player);
        }

        /// <summary>
        /// Gets the player with the specified index, if there's one
        /// participating in this game.
        /// </summary>
        /// <param name="playerIndex">
        /// Index of the player to get.
        /// </param>
        /// <returns>
        /// Player with the specified index, if there's one
        /// participating in this game, and <c>null</c> otherwise.
        /// </returns>
        public Player GetPlayer(int playerIndex)
        {
            return this.players.Find(p => p.PlayerIndex == playerIndex);
        }

        /// <summary>
        /// Removes the specified player from this game.
        /// </summary>
        /// <param name="player">
        /// Player to remove.
        /// </param>
        /// <returns>
        /// <c>true</c>, if the player has been removed, and <c>false</c>
        /// otherwise.
        /// </returns>
        public bool RemovePlayer(Player player)
        {
            bool removed = this.players.Remove(player);

            if (removed)
            {
                this.eventManager.InvokePlayerRemoved(player);
            }

            return removed;
        }

        /// <summary>
        /// Starts this game, beginning to tick all systems.
        /// </summary>
        public void StartGame()
        {
            this.running = true;
            this.eventManager.InvokeGameStarted();
        }

        /// <summary>
        /// Pauses this game, stopping ticking all systems.
        /// </summary>
        public void PauseGame()
        {
            this.running = false;
            this.eventManager.InvokeGamePaused();
        }

        /// <summary>
        /// Resumes this game, continuing to tick all systems.
        /// </summary>
        public void ResumeGame()
        {
            this.running = true;
            this.eventManager.InvokeGameResumed();
        }
        #endregion
    }
}
