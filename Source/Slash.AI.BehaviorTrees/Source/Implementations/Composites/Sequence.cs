// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Sequence.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.AI.BehaviorTrees.Implementations.Composites
{
    using System;
    using System.Collections.Generic;

    using Slash.AI.BehaviorTrees.Attributes;
    using Slash.AI.BehaviorTrees.Enums;
    using Slash.AI.BehaviorTrees.Interfaces;
    using Slash.AI.BehaviorTrees.Tree;
    using Slash.Math.Utils;

    /// <summary>
    ///   Task which executes its children one after another. The task finishes when all children finished.
    /// </summary>
    [Serializable]
    [Task(Name = "Sequence",
        Description =
            "Task which executes its children one after another. The task finishes when all children finished.")]
    public class Sequence : Composite<Sequence.Data>
    {
        #region Constructors and Destructors

        /// <summary>
        ///   Constructor.
        /// </summary>
        public Sequence()
        {
        }

        /// <summary>
        ///   Constructor.
        /// </summary>
        /// <param name="children"> Child tasks. </param>
        public Sequence(List<ITask> children)
            : base(children)
        {
        }

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
            base.Activate(agentData, decisionData);

            Data data = agentData.GetTaskData<Data>();

            // Activate first child.
            for (data.ActiveChildIdx = 0; data.ActiveChildIdx < this.Children.Count; ++data.ActiveChildIdx)
            {
                ExecutionStatus childResult = this.ActivateChild(data.ActiveChildIdx, agentData, decisionData);
                switch (childResult)
                {
                    case ExecutionStatus.None:
                    case ExecutionStatus.Failed:
                        return ExecutionStatus.Failed;
                    case ExecutionStatus.Running:
                        return ExecutionStatus.Running;
                }
            }

            // All children succeeded.
            return ExecutionStatus.Success;
        }

        /// <summary>
        ///   Deactivation.
        /// </summary>
        /// <param name="agentData"> Agent data. </param>
        public override void Deactivate(IAgentData agentData)
        {
            // Deactivate current active child.
            Data data = agentData.GetTaskData<Data>();
            if (data.ActiveChildIdx >= 0)
            {
                this.DeactivateChild(data.ActiveChildIdx, agentData);
            }

            base.Deactivate(agentData);
        }

        /// <summary>
        ///   Depending on the group policy of its parent, the floating point return value indicates whether the task will be activated.
        /// </summary>
        /// <param name="agentData"> Agent data. </param>
        /// <param name="decisionData"> Decision data to use in activate method. </param>
        /// <returns> Floating point value used to decide if the task will be activated. </returns>
        public override float Decide(IAgentData agentData, ref IDecisionData decisionData)
        {
            if (this.Children.Count == 0)
            {
                return 0.0f;
            }

            // Check if all children want to run.
            float maxChildDecisionValue = 0.0f;
            for (int index = 0; index < this.Children.Count; index++)
            {
                ITask child = this.Children[index];
                IDecisionData childDecisionData = null;
                float childDecisionValue = child.Decide(agentData, ref childDecisionData);
                maxChildDecisionValue = Math.Max(maxChildDecisionValue, childDecisionValue);

                // If one doesn't want to run, we don't run the whole task.
                if (childDecisionValue <= 0.0f)
                {
                    return 0.0f;
                }

                // Store decision data of first child to pass it to it in Activate.
                if (index == 0)
                {
                    decisionData = childDecisionData;
                }
            }

            return maxChildDecisionValue;
        }

        /// <summary>
        ///   Generates a collection of active task nodes under this task. Used for debugging only.
        /// </summary>
        /// <param name="agentData"> Agent data. </param>
        /// <param name="taskNode"> Task node this task is located in. </param>
        /// <param name="activeTasks"> Collection of active task nodes. </param>
        public override void GetActiveTasks(
            IAgentData agentData, TaskNode taskNode, ref ICollection<TaskNode> activeTasks)
        {
            Data data = agentData.CurrentTaskData as Data;
            if (data == null)
            {
                throw new ArgumentException(string.Format("Expected sequence data for task '{0}'.", this.Name));
            }

            // Check if sequence has an active child.
            if (!MathUtils.IsWithinBounds(data.ActiveChildIdx, 0, this.Children.Count))
            {
                return;
            }

            // Add task to active tasks and collect active tasks of active child.
            TaskNode childTaskNode = taskNode.CreateChildNode(this.Children[data.ActiveChildIdx], data.ActiveChildIdx);
            activeTasks.Add(childTaskNode);
            this.GetActiveChildTasks(data.ActiveChildIdx, agentData, childTaskNode, ref activeTasks);
        }

        /// <summary>
        ///   Per frame update.
        /// </summary>
        /// <param name="agentData"> Agent data. </param>
        /// <returns> Execution status after this update. </returns>
        public override ExecutionStatus Update(IAgentData agentData)
        {
            Data data = agentData.CurrentTaskData as Data;
            if (data == null)
            {
                throw new ArgumentException(string.Format("Expected sequence data for task '{0}'.", this.Name));
            }

            // Loop while an execution status was determined.
            ExecutionStatus executionStatus = ExecutionStatus.None;
            ExecutionStatus childExecutionStatus = ExecutionStatus.Running;
            IDecisionData childDecisionData = null;
            while (executionStatus == ExecutionStatus.None)
            {
                // Activate child if necessary.
                if (childExecutionStatus == ExecutionStatus.None)
                {
                    // Activate new task.
                    childExecutionStatus = this.ActivateChild(data.ActiveChildIdx, agentData, childDecisionData);
                }

                if (childExecutionStatus == ExecutionStatus.Running)
                {
                    // Update child.
                    childExecutionStatus = this.UpdateChild(data.ActiveChildIdx, agentData);
                }

                switch (childExecutionStatus)
                {
                    case ExecutionStatus.Failed:
                    case ExecutionStatus.None:
                        {
                            executionStatus = ExecutionStatus.Failed;
                        }

                        break;
                    case ExecutionStatus.Success:
                        {
                            // Go to next child.
                            ++data.ActiveChildIdx;
                            if (data.ActiveChildIdx < this.Children.Count)
                            {
                                // Check that the child task is ready to be activated.
                                ITask childTask = this.Children[data.ActiveChildIdx];
                                if (childTask.Decide(agentData, ref childDecisionData) <= 0.0f)
                                {
                                    executionStatus = ExecutionStatus.Failed;
                                }
                                else
                                {
                                    childExecutionStatus = ExecutionStatus.None;
                                }
                            }
                            else
                            {
                                // Invoke callback.
                                this.InvokeOnSuccess();

                                executionStatus = ExecutionStatus.Success;
                            }
                        }

                        break;
                    default:
                        {
                            executionStatus = childExecutionStatus;
                        }

                        break;
                }
            }

            return executionStatus;
        }

        #endregion

        /// <summary>
        ///   Task data.
        /// </summary>
        public class Data : ITaskData
        {
            #region Public Properties

            /// <summary>
            ///   Index of active child.
            /// </summary>
            public int ActiveChildIdx { get; set; }

            #endregion
        }
    }
}