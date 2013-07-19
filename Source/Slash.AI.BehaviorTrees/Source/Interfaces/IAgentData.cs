namespace Slash.AI.BehaviorTrees.Interfaces
{
    using System;

    using Slash.AI.BehaviorTrees.Data;
    using Slash.AI.BehaviorTrees.Enums;

    /// <summary>
    ///   Called when the active tasks of the agent changed.
    /// </summary>
    /// <param name="agentData"> Agent data which active tasks changed. </param>
    public delegate void OnActiveTasksChangedDelegate(IAgentData agentData);

    /// <summary>
    ///   Agent data which is passed to the deciders and behaviors and contains data to make the behavior tree execution work. This is also the place to put application specific data (like a reference to the in-game actor).
    /// </summary>
    public interface IAgentData
    {
        #region Public Events

        /// <summary>
        ///   Called when the active tasks of the agent changed.
        /// </summary>
        event OnActiveTasksChangedDelegate OnActiveTasksChanged;

        #endregion

        #region Public Properties

        /// <summary>
        ///   Blackboard to exchange data between behaviors.
        /// </summary>
        Blackboard Blackboard { get; set; }

        /// <summary>
        ///   Current decider level. Incremented/Decremented while the behavior tree is walked through. Indicates how deep in the tree (on which level) the execution currently is. It's just an internal information, so don't change it from the outside.
        /// </summary>
        int CurrentDeciderLevel { get; set; }

        /// <summary>
        ///   Task-specific data. Will always return/set the task data of the current decider level.
        /// </summary>
        ITaskData CurrentTaskData { get; set; }

        /// <summary>
        ///   Behavior tree execution status.
        /// </summary>
        ExecutionStatus ExecutionStatus { get; set; }

        /// <summary>
        ///   Indicates if this behavior should log events. If not set the behaviors/deciders should only log warnings and errors to the logger.
        /// </summary>
        bool LogEnabled { get; set; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///   Tries to cast the current task data to the specified type and returns it. Throws an exception if there is no current task data or if it couldn't be casted.
        /// </summary>
        /// <typeparam name="T"> Expected type of task data. </typeparam>
        /// <returns> Current task data, casted to specified type. </returns>
        /// <exception cref="NullReferenceException">Thrown if there is no current task data available.</exception>
        /// <exception cref="InvalidCastException">Thrown if the current task data couldn't be cast to the specified type.</exception>
        T GetTaskData<T>() where T : ITaskData;

        /// <summary>
        ///   Called after behavior tree was updated.
        /// </summary>
        void PostUpdate();

        /// <summary>
        ///   Called before behavior tree is updated.
        /// </summary>
        void PreUpdate();

        #endregion
    }
}