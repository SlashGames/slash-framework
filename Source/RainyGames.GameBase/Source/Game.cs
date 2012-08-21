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
        /// Whether this game is running, or not (e.g. not yet started,
        /// paused, or already over).
        /// </summary>
        private bool running;

        /// <summary>
        /// Total time since this game has started, in seconds.
        /// </summary>
        private float timeElapsed;
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
            this.running = false;
            this.timeElapsed = 0.0f;
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
        /// Total time since this game has started, in seconds.
        /// </summary>
        public float TimeElapsed
        {
            get { return this.timeElapsed; }
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
        /// Ticks this game, allowing all systems to update themselves and
        /// processing all game events.
        /// </summary>
        /// <param name="dt">
        /// Time passed since the last tick, in seconds.
        /// </param>
        public virtual void Update(float dt)
        {
            if (this.running)
            {
                this.systemManager.Update(dt);
                this.eventManager.ProcessEvents();
                this.entityManager.CleanUpEntities();

                this.timeElapsed += dt;
            }
        }

        /// <summary>
        /// Starts this game, beginning to tick all systems.
        /// </summary>
        public virtual void StartGame()
        {
            this.running = true;
            this.eventManager.QueueEvent(FrameworkEventType.GameStarted);
        }

        /// <summary>
        /// Pauses this game, stopping ticking all systems.
        /// </summary>
        public virtual void PauseGame()
        {
            this.running = false;
            this.eventManager.QueueEvent(FrameworkEventType.GamePaused);
        }

        /// <summary>
        /// Resumes this game, continuing to tick all systems.
        /// </summary>
        public virtual void ResumeGame()
        {
            this.running = true;
            this.eventManager.QueueEvent(FrameworkEventType.GameResumed);
        }
        #endregion
    }
}
