namespace RainyGames.AI.BehaviorTrees.Implementations
{
    using System;

    using RainyGames.AI.BehaviorTrees.Data;
    using RainyGames.AI.BehaviorTrees.Enums;
    using RainyGames.AI.BehaviorTrees.Interfaces;

    /// <summary>
    ///   The agent data flags.
    /// </summary>
    [Flags]
    public enum AgentDataFlags
    {
        /// <summary>
        ///   The none.
        /// </summary>
        None = 0x0,

        /// <summary>
        ///   The active tasks changed.
        /// </summary>
        ActiveTasksChanged = 0x1,
    }

    /// <summary>
    ///   Base agent data implementation.
    /// </summary>
    public class AgentData : IAgentData
    {
        #region Constants

        /// <summary>
        ///   Maximum number of task levels.
        /// </summary>
        public const int MaxTaskLevels = 10;

        #endregion

        #region Fields

        /// <summary>
        ///   Storage for task data.
        /// </summary>
        private readonly ITaskData[] taskData = new ITaskData[MaxTaskLevels];

        /// <summary>
        ///   Flags which indicate the state of the agent data. Used for debugging only.
        /// </summary>
        private AgentDataFlags flags;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///   Initializes a new instance of the <see cref="AgentData" /> class. Constructor.
        /// </summary>
        public AgentData()
        {
            this.Blackboard = new Blackboard();
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref="AgentData" /> class. Constructor.
        /// </summary>
        /// <param name="blackboard"> Initial blackboard. </param>
        public AgentData(Blackboard blackboard)
        {
            this.Blackboard = blackboard;
        }

        #endregion

        #region Public Events

        /// <summary>
        ///   Called when the active tasks of the agent changed.
        /// </summary>
        public event OnActiveTasksChangedDelegate OnActiveTasksChanged;

        #endregion

        #region Public Properties

        /// <summary>
        ///   Blackboard to exchange data between behaviors.
        /// </summary>
        public Blackboard Blackboard { get; set; }

        /// <summary>
        ///   Current decider level. Incremented/Decremented while the behavior tree is walked through. Indicates how deep in the tree (on which level) the execution currently is. It's just an internal information, so don't change it from the outside.
        /// </summary>
        public int CurrentDeciderLevel { get; set; }

        /// <summary>
        ///   Task specific data.
        /// </summary>
        public ITaskData CurrentTaskData
        {
            get
            {
                return this.taskData[this.CurrentDeciderLevel];
            }

            set
            {
                // Check if current task level is in range.
                if (this.CurrentDeciderLevel >= MaxTaskLevels)
                {
                    throw new ArgumentOutOfRangeException(
                        string.Format(
                            "Can't store task data because the current task level ({0}) >= maximum task level ({1}). Either the behavior tree has to be changed or the maximum task level has to be increased.",
                            this.CurrentDeciderLevel,
                            MaxTaskLevels));
                }

                if (value == this.taskData[this.CurrentDeciderLevel])
                {
                    return;
                }

                this.taskData[this.CurrentDeciderLevel] = value;

                // Set flag that active tasks changed.
                this.flags = this.flags | AgentDataFlags.ActiveTasksChanged;
            }
        }

        /// <summary>
        ///   Behavior tree execution status.
        /// </summary>
        public ExecutionStatus ExecutionStatus { get; set; }

        /// <summary>
        ///   Indicates if this behavior should log events. If not set the behaviors/deciders should only log warnings and errors to the logger.
        /// </summary>
        public bool LogEnabled { get; set; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///   Tries to cast the current task data to the specified type and returns it. Throws an exception if there is no current task data or if it couldn't be casted.
        /// </summary>
        /// <typeparam name="T"> Expected type of task data. </typeparam>
        /// <returns> Current task data, casted to specified type. </returns>
        /// <exception cref="NullReferenceException">Thrown if there is no current task data available.</exception>
        /// <exception cref="InvalidCastException">Thrown if the current task data couldn't be cast to the specified type.</exception>
        public T GetTaskData<T>() where T : ITaskData
        {
            ITaskData currentTaskData = this.CurrentTaskData;
            if (currentTaskData == null)
            {
                throw new NullReferenceException("No current task data available.");
            }

            return (T)currentTaskData;
        }

        /// <summary>
        ///   Called after behavior tree was updated.
        /// </summary>
        public void PostUpdate()
        {
            // Check flags.
            if ((this.flags & AgentDataFlags.ActiveTasksChanged) != 0)
            {
                this.InvokeOnActiveTasksChanged();
            }
        }

        /// <summary>
        ///   Called before behavior tree is updated.
        /// </summary>
        public void PreUpdate()
        {
            // Clear flags.
            this.flags = AgentDataFlags.None;
        }

        #endregion

        #region Methods

        /// <summary>
        ///   The invoke on active tasks changed.
        /// </summary>
        private void InvokeOnActiveTasksChanged()
        {
            OnActiveTasksChangedDelegate handler = this.OnActiveTasksChanged;
            if (handler != null)
            {
                handler(this);
            }
        }

        #endregion
    }
}