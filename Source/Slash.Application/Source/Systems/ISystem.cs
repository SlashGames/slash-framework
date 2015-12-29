namespace Slash.Application.Systems
{
    using Slash.Collections.AttributeTables;
    using Slash.ECS.Blueprints;
    using Slash.ECS.Components;
    using Slash.ECS.Events;
    using Slash.ECS.Logging;
    using Slash.ECS.Processes;

    /// <summary>
    ///   Contract that all systems that make up a game have to fulfill,
    ///   e.g. physics, combat or AI.
    /// </summary>
    public interface ISystem
    {
        #region Public Properties

        /// <summary>
        ///   Blueprint manager for this system.
        /// </summary>
        IBlueprintManager BlueprintManager { get; set; }

        /// <summary>
        ///   Entity manager for this system.
        /// </summary>
        EntityManager EntityManager { get; set; }

        /// <summary>
        ///   Event manager for this system.
        /// </summary>
        EventManager EventManager { get; set; }

        /// <summary>
        ///   Logger for logic events.
        /// </summary>
        GameLogger Log { get; set; }

        /// <summary>
        ///   Allows ticking and queueing timed processes. Good examples are
        ///   animations, tweens, or "Go to that point, and open the door after."
        /// </summary>
        ProcessManager ProcessManager { get; set; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///   Deinitializes this system.
        /// </summary>
        void Deinit();

        /// <summary>
        ///   Initializes this system with the data stored in the specified
        ///   attribute table.
        /// </summary>
        /// <param name="configuration">System configuration data.</param>
        void Init(IAttributeTable configuration);

        /// <summary>
        ///   Late update of this system. The late update is performed after
        ///   all events of the tick were processed.
        /// </summary>
        /// <param name="dt">
        ///   Time passed since the last tick (in s).
        /// </param>
        void LateUpdate(float dt);

        /// <summary>
        ///   Ticks this system.
        /// </summary>
        /// <param name="dt">
        ///   Time passed since the last tick, in seconds.
        /// </param>
        void Update(float dt);

        #endregion
    }
}