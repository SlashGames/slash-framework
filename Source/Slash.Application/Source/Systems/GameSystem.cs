namespace Slash.Application.Systems
{
    using Slash.Collections.AttributeTables;
    using Slash.ECS.Blueprints;
    using Slash.ECS.Components;
    using Slash.ECS.Events;
    using Slash.ECS.Inspector.Utils;
    using Slash.ECS.Logging;
    using Slash.ECS.Processes;

    /// <summary>
    ///   Base system class.
    /// </summary>
    public class GameSystem : ISystem
    {
        #region Public Properties

        /// <summary>
        ///   Blueprint manager for this system.
        /// </summary>
        public IBlueprintManager BlueprintManager { get; set; }

        /// <summary>
        ///   Entity manager for this system.
        /// </summary>
        public EntityManager EntityManager { get; set; }

        /// <summary>
        ///   Event manager for this system.
        /// </summary>
        public EventManager EventManager { get; set; }

        /// <summary>
        ///   Logger.
        /// </summary>
        public GameLogger Log { get; set; }

        /// <summary>
        ///   Allows ticking and queueing timed processes. Good examples are
        ///   animations, tweens, or "Go to that point, and open the door after."
        /// </summary>
        public ProcessManager ProcessManager { get; set; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///   Deinitializes this system.
        /// </summary>
        public virtual void Deinit()
        {
        }

        /// <summary>
        ///   Initializes this system with the data stored in the specified
        ///   attribute table.
        /// </summary>
        /// <param name="configuration">System configuration data.</param>
        public virtual void Init(IAttributeTable configuration)
        {
            // Initialize from configuration.
            InspectorUtils.InitFromAttributeTable(this.EntityManager, this, configuration);
        }

        /// <summary>
        ///   Late update of this system. The late update is performed after
        ///   all events of the tick were processed.
        /// </summary>
        /// <param name="dt">
        ///   Time passed since the last tick (in s).
        /// </param>
        public virtual void LateUpdate(float dt)
        {
        }

        /// <summary>
        ///   Ticks this system.
        /// </summary>
        /// <param name="dt">
        ///   Time passed since the last tick, in seconds.
        /// </param>
        public virtual void Update(float dt)
        {
        }

        #endregion
    }
}