// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UntilFailDecorator.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.AI.BehaviorTrees.Implementations.Decorators
{
    using System;

    using Slash.AI.BehaviorTrees.Attributes;
    using Slash.AI.BehaviorTrees.Enums;
    using Slash.AI.BehaviorTrees.Interfaces;

    /// <summary>
    ///   A decorator which executes the decorated decider until it fails. If the task succeeds, it is restarted.
    /// </summary>
    [Serializable]
    [Task(Name = "Until Fail", IsDecorator = true)]
    public class UntilFailDecorator : Decorator
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
            Data data = new Data();
            data.Status = this.ActivateChild(agentData, decisionData);
            if (data.Status == ExecutionStatus.Failed)
            {
                return ExecutionStatus.Failed;
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
            Data data = agentData.CurrentTaskData as Data;

            // Deactivate child if running.
            if (data.Status == ExecutionStatus.Running)
            {
                this.DeactivateChild(agentData);
            }

            agentData.CurrentTaskData = null;
        }

        /// <summary>
        ///   Per frame update.
        /// </summary>
        /// <param name="agentData"> Agent data. </param>
        /// <returns> Execution status after this update. </returns>
        public override ExecutionStatus Update(IAgentData agentData)
        {
            Data data = agentData.CurrentTaskData as Data;
            if (data.Status == ExecutionStatus.Running)
            {
                data.Status = this.UpdateChild(agentData);
            }
            else
            {
                // Restart if still possible.
                IDecisionData decisionData = null;
                float decisionValue = this.Task.Decide(agentData, ref decisionData);
                if (decisionValue > 0.0f)
                {
                    data.Status = this.ActivateChild(agentData, decisionData);
                    if (data.Status == ExecutionStatus.Running)
                    {
                        data.Status = this.UpdateChild(agentData);
                    }
                }
            }

            return data.Status == ExecutionStatus.Failed ? ExecutionStatus.Failed : ExecutionStatus.Running;
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