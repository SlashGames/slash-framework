// -----------------------------------------------------------------------
// <copyright file="Game.cs" company="Slash Games">
// Copyright (c) Slash Games. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Slash.GameBase
{
    /// <summary>
    /// Base class of most Slash Games games. Provides default functionality
    /// that is common across many games, such as components that are attached
    /// to entities, or systems working on these components.
    /// </summary>
    public class Game
    {
        #region Constants and Fields

        /// <summary>
        /// Manager responsible for creating and removing entities in this game.
        /// </summary>
        private readonly EntityManager entityManager;

        /// <summary>
        /// Manager responsible for updating all game systems in each tick.
        /// </summary>
        private readonly SystemManager systemManager;

        /// <summary>
        /// Manager allowing listeners to register for game-related events.
        /// </summary>
        private readonly EventManager eventManager;

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
            this.Running = false;
            this.TimeElapsed = 0.0f;
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
        public bool Running { get; private set; }

        /// <summary>
        /// Total time since this game has started, in seconds.
        /// </summary>
        public float TimeElapsed { get; private set; }

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
            if (!this.Running)
            {
                return;
            }

            this.systemManager.Update(dt);
            this.eventManager.ProcessEvents(dt);
            this.entityManager.CleanUpEntities();

            this.TimeElapsed += dt;
        }

        /// <summary>
        /// Starts this game, beginning to tick all systems.
        /// </summary>
        public virtual void StartGame()
        {
            this.Running = true;
            this.eventManager.QueueEvent(FrameworkEventType.GameStarted);
        }

        /// <summary>
        /// Pauses this game, stopping ticking all systems.
        /// </summary>
        public virtual void PauseGame()
        {
            this.Running = false;
            this.eventManager.QueueEvent(FrameworkEventType.GamePaused);
        }

        /// <summary>
        /// Resumes this game, continuing to tick all systems.
        /// </summary>
        public virtual void ResumeGame()
        {
            this.Running = true;
            this.eventManager.QueueEvent(FrameworkEventType.GameResumed);
        }
        #endregion
    }
}
