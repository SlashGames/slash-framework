namespace RainyGames.AI.BehaviorTrees.Implementations.Decorators
{
    using System;

    using RainyGames.AI.BehaviorTrees.Attributes;
    using RainyGames.AI.BehaviorTrees.Enums;
    using RainyGames.AI.BehaviorTrees.Interfaces;

    /// <summary>
    ///   A decorator which executes the decorated task forever. If the task succeeds or fails, it is restarted.
    /// </summary>
    [Serializable]
    [Task(Name = "Loop", IsDecorator = true)]
    public class LoopDecorator : Decorator
    {
        #region Public Methods and Operators

        /// <summary>
        ///   Activation. This method is called when the task was chosen to be executed. It's called right before the first update of the task. The task can setup its specific task data in here and do initial actions.
        /// </summary>
        /// <param name="agentData"> Agent data. </param>
        /// <param name="decisionData"> Decision data to use in activate method. </param>
        /// <returns> Execution status after activation. </returns>
        public override ExecutionStatus Activate(IAgentData agentData, IDecisionData decisionData)
        {
            Data data = new Data { Status = ExecutionStatus.Failed };

            // Check if child wants to run.
            float childDecisionValue = this.DecideChild(agentData, ref decisionData);
            if (childDecisionValue > 0.0f)
            {
                data.Status = this.ActivateChild(agentData, decisionData);
            }

            agentData.CurrentTaskData = data;

            return ExecutionStatus.Running;
        }

        /// <summary>
        ///   Deactivation.
        /// </summary>
        /// <param name="agentData"> Agent data. </param>
        public override void Deactivate(IAgentData agentData)
        {
            Data data = agentData.GetTaskData<Data>();

            // Deactivate child if running.
            if (data.Status == ExecutionStatus.Running)
            {
                this.DeactivateChild(agentData);
            }

            agentData.CurrentTaskData = null;
        }

        /// <summary>
        ///   Depending on the group policy of its parent, the floating point return value indicates whether the task will be activated.
        /// </summary>
        /// <param name="agentData"> Agent data. </param>
        /// <param name="decisionData"> Decision data to use in activate method. </param>
        /// <returns> Floating point value used to decide if the task will be activated. </returns>
        public override float Decide(IAgentData agentData, ref IDecisionData decisionData)
        {
            // Loop always runs.
            return 1.0f;
        }

        /// <summary>
        ///   Per frame update.
        /// </summary>
        /// <param name="agentData"> Agent data. </param>
        /// <returns> Execution status after this update. </returns>
        public override ExecutionStatus Update(IAgentData agentData)
        {
            Data data = agentData.GetTaskData<Data>();
            if (data.Status == ExecutionStatus.Running)
            {
                // Update.
                data.Status = this.UpdateChild(agentData);
            }
            else
            {
                // Restart if still possible.
                IDecisionData decisionData = null;
                ++agentData.CurrentDeciderLevel;
                float decisionValue = this.Task.Decide(agentData, ref decisionData);
                --agentData.CurrentDeciderLevel;
                if (decisionValue > 0.0f)
                {
                    data.Status = this.ActivateChild(agentData, decisionData);
                    if (data.Status == ExecutionStatus.Running)
                    {
                        data.Status = this.UpdateChild(agentData);
                    }
                }
                else
                {
                    data.Status = ExecutionStatus.Failed;
                }
            }

            return ExecutionStatus.Running;
        }

        #endregion

        /// <summary>
        ///   Task-specific data.
        /// </summary>
        private class Data : ITaskData
        {
            #region Public Properties

            /// <summary>
            ///   Execution status of child.
            /// </summary>
            public ExecutionStatus Status { get; set; }

            #endregion
        }
    }
}