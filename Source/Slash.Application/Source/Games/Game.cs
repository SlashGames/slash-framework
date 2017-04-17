namespace Slash.Application.Games
{
    using System;
    using System.Linq;

    using Slash.Application.Systems;
    using Slash.Collections.AttributeTables;
    using Slash.ECS.Blueprints;
    using Slash.ECS.Components;
    using Slash.ECS.Events;
    using Slash.ECS.Logging;
    using Slash.ECS.Processes;
    using Slash.Reflection.Extensions;
    using Slash.Reflection.Utils;

    /// <summary>
    ///   Core game class. Provides default functionality
    ///   that is common across many games, such as components that are attached
    ///   to entities, or systems working on these components.
    /// </summary>
    public class Game
    {
        #region Fields

        /// <summary>
        ///   Manager responsible for creating and removing entities in this game.
        /// </summary>
        private readonly EntityManager entityManager;

        /// <summary>
        ///   Manager allowing listeners to register for game-related events.
        /// </summary>
        private readonly EventManager eventManager;

        /// <summary>
        ///   Manager allowing ticking and queueing timed processes.
        /// </summary>
        private readonly ProcessManager processManager;

        /// <summary>
        ///   Manager responsible for updating all game systems in each tick.
        /// </summary>
        private readonly SystemManager systemManager;

        /// <summary>
        ///   Accumulated time for fixed updates (in sec).
        /// </summary>
        private float accumulatedTime;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///   Constructs a new game without players.
        /// </summary>
        public Game()
        {
            this.Log = new GameLogger();
            this.eventManager = new EventManager();
            this.entityManager = new EntityManager(this.eventManager, this.Log);
            this.systemManager = new SystemManager(this);
            this.processManager = new ProcessManager(this.entityManager, this.eventManager);
            this.Running = false;
            this.TimeElapsed = 0.0f;
            this.AddSystemsViaReflection = true;
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///   Indicates if game systems should be added which are flagged with the
        ///   GameSystem attribute.
        /// </summary>
        public bool AddSystemsViaReflection { get; set; }

        /// <summary>
        ///   Manages all blueprints available in the game.
        /// </summary>
        public IBlueprintManager BlueprintManager { get; set; }

        /// <summary>
        ///   Manager responsible for creating and removing entities in this game.
        /// </summary>
        public EntityManager EntityManager
        {
            get
            {
                return this.entityManager;
            }
        }

        /// <summary>
        ///   Manager allowing listeners to register for game-related events.
        /// </summary>
        public EventManager EventManager
        {
            get
            {
                return this.eventManager;
            }
        }

        /// <summary>
        ///   Name of this game.
        /// </summary>
        public string GameName { get; set; }

        /// <summary>
        ///   Logger for logic events.
        /// </summary>
        public GameLogger Log { get; set; }

        /// <summary>
        ///   Manager allowing ticking and queueing timed processes.
        /// </summary>
        public ProcessManager ProcessManager
        {
            get
            {
                return this.processManager;
            }
        }

        /// <summary>
        ///   Whether this game is running, or not (e.g. not yet started,
        ///   paused, or already over).
        /// </summary>
        public bool Running { get; private set; }

        /// <summary>
        ///   Manager responsible for updating all game systems in each tick.
        /// </summary>
        public SystemManager SystemManager
        {
            get
            {
                return this.systemManager;
            }
        }

        /// <summary>
        ///   Total time since this game has started, in seconds.
        /// </summary>
        public float TimeElapsed { get; private set; }

        /// <summary>
        ///   Fixed time between two game ticks (in sec).
        /// </summary>
        public float UpdatePeriod { get; set; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///   Adds and initializes the system of the specified type.
        /// </summary>
        /// <typeparam name="T">Type of the system to add.</typeparam>
        public void AddSystem<T>() where T : ISystem, new()
        {
            var system = new T();
            this.AddSystem(system);
        }

        /// <summary>
        ///   Pauses this game, stopping ticking all systems.
        /// </summary>
        public virtual void PauseGame()
        {
            if (!this.Running)
            {
                return;
            }

            this.Running = false;
            this.eventManager.QueueEvent(ApplicationGameEvent.GamePaused);

            // Process events till the paused event before stopping the game.
            // TODO(co): Introduce a SendEvent method for event manager and handle both cases: Currently inside event handling and not inside.
            this.eventManager.ProcessEvents();
        }

        /// <summary>
        ///   Resumes this game, continuing to tick all systems.
        /// </summary>
        public void ResumeGame()
        {
            if (this.Running)
            {
                return;
            }

            this.Running = true;
            this.eventManager.QueueEvent(ApplicationGameEvent.GameResumed);
        }

        /// <summary>
        ///   Starts this game, beginning to tick all systems.
        /// </summary>
        public void StartGame()
        {
            this.StartGame(new AttributeTable());
        }

        /// <summary>
        ///   Starts this game, beginning to tick all systems.
        /// </summary>
        /// <param name="gameConfiguration">Configuration for game systems.</param>
        public void StartGame(IAttributeTable gameConfiguration)
        {
            // Init game.
            this.InitGame();

            // Init systems.
            foreach (ISystem system in this.systemManager)
            {
                system.Init(gameConfiguration);
            }

            // Send event.
            this.Running = true;
            this.eventManager.QueueEvent(ApplicationGameEvent.GameStarted);

            // Give the derived game the chance to do game start things.
            this.OnGameStarted();
        }

        /// <summary>
        ///   Deinitializes and stops the game.
        /// </summary>
        public void StopGame()
        {
            // Deinit systems.
            foreach (var system in this.systemManager)
            {
                system.Deinit();
            }

            // Send event.
            this.eventManager.FireImmediately(ApplicationGameEvent.GameStopped);
            this.Running = false;
        }

        /// <summary>
        ///   Ticks this game, allowing all systems to update themselves and
        ///   processing all game events.
        /// </summary>
        /// <param name="dt">
        ///   Time passed since the last tick, in seconds.
        /// </param>
        public void Update(float dt)
        {
            if (!this.Running)
            {
                return;
            }

            if (this.UpdatePeriod > 0)
            {
                // Fixed update.
                this.accumulatedTime += dt;

                while (this.accumulatedTime >= this.UpdatePeriod)
                {
                    this.UpdateGame(this.UpdatePeriod);
                    this.accumulatedTime -= this.UpdatePeriod;
                }
            }
            else
            {
                this.UpdateGame(dt);
            }

            this.TimeElapsed += dt;
        }

        #endregion

        #region Methods

        /// <summary>
        ///   Called after initialization of game was done and the game was started.
        /// </summary>
        protected virtual void OnGameStarted()
        {
        }

        private void AddSystem(ISystem system)
        {
            this.SystemManager.AddSystem(system);

            system.EntityManager = this.EntityManager;
            system.BlueprintManager = this.BlueprintManager;
            system.EventManager = this.EventManager;
            system.Log = this.Log;
            system.ProcessManager = this.ProcessManager;
        }

        /// <summary>
        ///   Initialization of the game. Can be used for game-specific initialization steps.
        /// </summary>
        private void InitGame()
        {
            if (this.AddSystemsViaReflection)
            {            
#if !WINDOWS_STORE && !WINDOWS_PHONE
                // Make sure all referenced assemblies are loaded.
                AssemblyUtils.CheckReferencedAssembliesAreLoaded();
#endif

                // Add game systems using reflection.
                var systemTypes = ReflectionUtils.FindTypesWithBase<ISystem>();

                // Check if enabled and order by index.
                var gameSystemTypes = from systemType in systemTypes
                                      let systemTypeAttribute = systemType.GetAttribute<GameSystemAttribute>()
                                      where systemTypeAttribute != null && systemTypeAttribute.Enabled
                                      orderby systemTypeAttribute.Order
                                      select systemType;

                // Attach systems.
                foreach (var gameSystemType in gameSystemTypes)
                {
                    var system = (ISystem)Activator.CreateInstance(gameSystemType);
                    this.AddSystem(system);
                }
            }
        }

        /// <summary>
        ///   Ticks this game, allowing all systems to update themselves and
        ///   processing all game events.
        /// </summary>
        /// <param name="dt">
        ///   Time passed since the last tick, in seconds.
        /// </param>
        private void UpdateGame(float dt)
        {
            this.systemManager.Update(dt);
            this.eventManager.ProcessEvents(dt);

            this.processManager.UpdateProcesses(dt);
            this.eventManager.ProcessEvents();

            this.entityManager.CleanUpEntities();
            this.eventManager.ProcessEvents();

            int processedEvents;
            do
            {
                this.systemManager.LateUpdate(dt);
                processedEvents = this.eventManager.ProcessEvents();

                // Reset time so the late update method doesn't get multiple deltas.
                dt = 0;
            }
            while (processedEvents > 0);
        }

        #endregion
    }
}