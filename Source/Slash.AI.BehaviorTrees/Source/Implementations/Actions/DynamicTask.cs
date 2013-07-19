namespace Slash.AI.BehaviorTrees.Implementations.Actions
{
    using System;

    using Slash.AI.BehaviorTrees.Attributes;
    using Slash.AI.BehaviorTrees.Data;
    using Slash.AI.BehaviorTrees.Enums;
    using Slash.AI.BehaviorTrees.Interfaces;

    /// <summary>
    ///   Fetches the task to execute from the blackboard or from a user value.
    /// </summary>
    [Serializable]
    [Task(Name = "Dynamic Task")]
    public class DynamicTask : Task
    {
        #region Public Properties

        /// <summary>
        ///   Task to execute.
        /// </summary>
        [TaskParameter(Name = "Task", Description = "Information about where to get task to execute from.")]
        public TaskParameterTask Task { get; set; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///   Activation. This method is called when the task was chosen to be executed. It's called right before the first update of the task. The task can setup its specific task data in here and do initial actions.
        /// </summary>
        /// <param name="agentData"> Agent data. </param>
        /// <param name="decisionData"> Decision data to use in activate method. </param>
        /// <returns> Execution status after activation. </returns>
        public override ExecutionStatus Activate(IAgentData agentData, IDecisionData decisionData)
        {
            ITask task = this.GetTask(agentData);
            return task == null ? ExecutionStatus.Failed : task.Activate(agentData, decisionData);
        }

        /// <summary>
        ///   Deactivation.
        /// </summary>
        /// <param name="agentData"> Agent data. </param>
        public override void Deactivate(IAgentData agentData)
        {
            ITask task = this.GetTask(agentData);
            if (task != null)
            {
                task.Deactivate(agentData);
            }
        }

        /// <summary>
        ///   Depending on the group policy of its parent, the floating point return value indicates whether the decider will be activated.
        /// </summary>
        /// <param name="agentData"> Agent data. </param>
        /// <param name="decisionData"> Decision data to use in activate method. </param>
        /// <returns> Floating point value used to decide if the decider will be activated. </returns>
        public override float Decide(IAgentData agentData, ref IDecisionData decisionData)
        {
            ITask task = this.GetTask(agentData);
            return task == null ? 0.0f : task.Decide(agentData, ref decisionData);
        }

        /// <summary>
        ///   The equals.
        /// </summary>
        /// <param name="other"> The other. </param>
        /// <returns> The System.Boolean. </returns>
        public bool Equals(DynamicTask other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return base.Equals(other) && Equals(other.Task, this.Task);
        }

        /// <summary>
        ///   The equals.
        /// </summary>
        /// <param name="obj"> The obj. </param>
        /// <returns> The System.Boolean. </returns>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            return this.Equals(obj as DynamicTask);
        }

        /// <summary>
        ///   The get hash code.
        /// </summary>
        /// <returns> The System.Int32. </returns>
        public override int GetHashCode()
        {
            unchecked
            {
                return (base.GetHashCode() * 397) ^ (this.Task != null ? this.Task.GetHashCode() : 0);
            }
        }

        /// <summary>
        ///   Per frame update.
        /// </summary>
        /// <param name="agentData"> Agent data. </param>
        /// <returns> Execution status after this update. </returns>
        public override ExecutionStatus Update(IAgentData agentData)
        {
            ITask task = this.GetTask(agentData);
            return task == null ? ExecutionStatus.Failed : task.Update(agentData);
        }

        #endregion

        #region Methods

        /// <summary>
        ///   Searches the task to execute.
        /// </summary>
        /// <param name="agentData"> Agent data. </param>
        /// <returns> Task to execute. </returns>
        private ITask GetTask(IAgentData agentData)
        {
            // Get task.
            ITask task = null;
            if (this.Task != null)
            {
                this.Task.TryGetValue(agentData, out task);
            }

            return task;
        }

        #endregion
    }
}