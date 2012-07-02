namespace RainyGames.AI.BehaviorTrees.Interfaces
{
    using System;
    using System.Collections.Generic;

    using RainyGames.AI.BehaviorTrees.Enums;
    using RainyGames.AI.BehaviorTrees.Tree;

    /// <summary>
    ///   The on success.
    /// </summary>
    public delegate void OnSuccess();

    /// <summary>
    ///   Interface which defines methods for a decider inside the behavior tree.
    /// </summary>
    public interface ITask
    {
        #region Public Events

        /// <summary>
        ///   Called when decider finished successful.
        /// </summary>
        event OnSuccess OnSuccess;

        #endregion

        #region Public Properties

        /// <summary>
        ///   Debug name.
        /// </summary>
        string Name { get; set; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///   Activation. This method is called when the task was chosen to be executed. It's called right before the first update of the task. The task can setup its specific task data in here and do initial actions.
        /// </summary>
        /// <param name="agentData"> Agent data. </param>
        /// <param name="decisionData"> Decision data to use in activate method. </param>
        /// <returns> Execution status after activation. </returns>
        ExecutionStatus Activate(IAgentData agentData, IDecisionData decisionData);

        /// <summary>
        ///   Deactivation.
        /// </summary>
        /// <param name="agentData"> Agent data. </param>
        void Deactivate(IAgentData agentData);

        /// <summary>
        ///   Depending on the group policy of its parent, the floating point return value indicates whether the decider will be activated.
        /// </summary>
        /// <param name="agentData"> Agent data. </param>
        /// <param name="decisionData"> Decision data to use in activate method. </param>
        /// <returns> Floating point value used to decide if the decider will be activated. </returns>
        float Decide(IAgentData agentData, ref IDecisionData decisionData);

        /// <summary>
        ///   Searches for tasks which forfill the passed predicate.
        /// </summary>
        /// <param name="taskNode"> Task node of this task. </param>
        /// <param name="predicate"> Predicate to forfill. </param>
        /// <param name="tasks"> List of tasks which forfill the passed predicate. </param>
        void FindTasks(TaskNode taskNode, Func<ITask, bool> predicate, ref ICollection<TaskNode> tasks);

        /// <summary>
        ///   Generates a collection of active task nodes under this task. Used for debugging only.
        /// </summary>
        /// <param name="agentData"> Agent data. </param>
        /// <param name="taskNode"> Task node of this task. </param>
        /// <param name="activeTasks"> Collection of active task nodes. </param>
        void GetActiveTasks(IAgentData agentData, TaskNode taskNode, ref ICollection<TaskNode> activeTasks);

        /// <summary>
        ///   Per frame update.
        /// </summary>
        /// <param name="agentData"> Agent data. </param>
        /// <returns> Execution status after this update. </returns>
        ExecutionStatus Update(IAgentData agentData);

        #endregion
    }
}