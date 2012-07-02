namespace RainyGames.AI.BehaviorTrees.Implementations
{
    using System;
    using System.Collections.Generic;

    using RainyGames.AI.BehaviorTrees.Enums;
    using RainyGames.AI.BehaviorTrees.Implementations.Composites;
    using RainyGames.AI.BehaviorTrees.Interfaces;
    using RainyGames.AI.BehaviorTrees.Tree;
    using RainyGames.Math.Utils;

    /// <summary>
    ///   task which selects one of its children to be executed. If the chosen child finished execution, the task finishes.
    /// </summary>
    [Serializable]
    public class SelectorDecider : Composite<SelectorDecider.Data>
    {
        #region Constructors and Destructors

        /// <summary>
        ///   Initializes a new instance of the <see cref="SelectorDecider" /> class. Constructor.
        /// </summary>
        public SelectorDecider()
        {
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref="SelectorDecider" /> class. Constructor.
        /// </summary>
        /// <param name="children"> Child deciders. </param>
        public SelectorDecider(List<ITask> children)
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
        /// <returns> The RainyGames.AI.BehaviorTrees.Enums.ExecutionStatus. </returns>
        public override ExecutionStatus Activate(IAgentData agentData, IDecisionData decisionData)
        {
            DecisionData selectorDecisionData = decisionData as DecisionData;
            if (selectorDecisionData == null)
            {
                throw new InvalidCastException(string.Format("Decision data was null or not of correct type."));
            }

            Data data = new Data { ActiveChildIdx = selectorDecisionData.SelectedChildIdx };
            agentData.CurrentTaskData = data;

            // Activate selected child.
            IDecisionData childDecisionData = selectorDecisionData.ChildDecisionData;
            ExecutionStatus executionStatus = ExecutionStatus.None;
            while (executionStatus == ExecutionStatus.None)
            {
                // Update child.
                ExecutionStatus childExecutionStatus = this.ActivateChild(
                    data.ActiveChildIdx, agentData, childDecisionData);

                switch (childExecutionStatus)
                {
                    case ExecutionStatus.Success:
                        {
                            // Invoke callback.
                            this.InvokeOnSuccess();

                            executionStatus = ExecutionStatus.Success;
                        }

                        break;
                    case ExecutionStatus.Failed:
                    case ExecutionStatus.None:
                        {
                            // Try next child.
                            float decideValue = 0.0f;
                            int selectedChildIdx = -1;
                            if (this.CheckForTakeOverDecider(
                                agentData, data, ref selectedChildIdx, ref decideValue, ref childDecisionData))
                            {
                                data.ActiveChildIdx = selectedChildIdx;
                            }
                            else
                            {
                                executionStatus = ExecutionStatus.Failed;
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
            float decideValue = 0.0f;
            int selectedChildIdx = -1;
            IDecisionData childDecisionData = null;
            if (this.DecideForFirstPossible(agentData, ref selectedChildIdx, ref decideValue, ref childDecisionData))
            {
                // Create decision data to pass to use in Activate() method.
                decisionData = new DecisionData
                    { SelectedChildIdx = selectedChildIdx, ChildDecisionData = childDecisionData };

                return decideValue;
            }

            return 0.0f;
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
                throw new ArgumentException(string.Format("Expected selector data for task '{0}'.", this.Name));
            }

            // Check if selector has an active child.
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
                throw new InvalidCastException(string.Format("task data was null or not of correct type."));
            }

            // Check for higher prioritized task which interrupts current executing task.
            float decideValue = 0.0f;
            int selectedChildIdx = -1;
            IDecisionData childDecisionData = null;
            ExecutionStatus childExecutionStatus = ExecutionStatus.Running;
            if (this.CheckForInterruptingDecider(
                agentData, data, ref selectedChildIdx, ref decideValue, ref childDecisionData))
            {
                // Deactivate current task.
                this.DeactivateChild(data.ActiveChildIdx, agentData);

                data.ActiveChildIdx = selectedChildIdx;

                // Activate new task.
                childExecutionStatus = this.ActivateChild(data.ActiveChildIdx, agentData, childDecisionData);
            }

            // Loop while an execution status was determined.
            ExecutionStatus executionStatus = ExecutionStatus.None;
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
                    case ExecutionStatus.Success:
                        {
                            // Invoke callback.
                            this.InvokeOnSuccess();

                            executionStatus = ExecutionStatus.Success;
                        }

                        break;
                    case ExecutionStatus.Failed:
                    case ExecutionStatus.None:
                        {
                            // Try next child.
                            if (this.CheckForTakeOverDecider(
                                agentData, data, ref selectedChildIdx, ref decideValue, ref childDecisionData))
                            {
                                data.ActiveChildIdx = selectedChildIdx;
                                childExecutionStatus = ExecutionStatus.None;
                            }
                            else
                            {
                                executionStatus = ExecutionStatus.Failed;
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

        #region Methods

        /// <summary>
        ///   Checks for an interrupting task. Only checks higher prioritized deciders as they are the only ones which can interrupt the running task.
        /// </summary>
        /// <param name="agentData"> Agent data. </param>
        /// <param name="data"> task data. </param>
        /// <param name="childIdx"> Index of child which will be activated. </param>
        /// <param name="childDecideValue"> Decide value of child which will be activated. </param>
        /// <param name="childDecisionData"> Decision data of child to be used in activate method. </param>
        /// <returns> True if there's a child which wants to be activated, else false. </returns>
        private bool CheckForInterruptingDecider(
            IAgentData agentData,
            Data data,
            ref int childIdx,
            ref float childDecideValue,
            ref IDecisionData childDecisionData)
        {
            return this.DecideForFirstPossible(
                agentData, 0, data.ActiveChildIdx, ref childIdx, ref childDecideValue, ref childDecisionData);
        }

        /// <summary>
        ///   Checks for a task which will take over the control when the current active child task failed.
        /// </summary>
        /// <param name="agentData"> Agent data. </param>
        /// <param name="data"> task data. </param>
        /// <param name="childIdx"> Index of child which will be activated. </param>
        /// <param name="childDecideValue"> Decide value of child which will be activated. </param>
        /// <param name="childDecisionData"> Decision data of child to be used in activate method. </param>
        /// <returns> True if there's a child which wants to be activated, else false. </returns>
        private bool CheckForTakeOverDecider(
            IAgentData agentData,
            Data data,
            ref int childIdx,
            ref float childDecideValue,
            ref IDecisionData childDecisionData)
        {
            return this.DecideForFirstPossible(
                agentData,
                data.ActiveChildIdx + 1,
                this.Children.Count,
                ref childIdx,
                ref childDecideValue,
                ref childDecisionData);
        }

        /// <summary>
        ///   Takes first task which wants to be active.
        /// </summary>
        /// <param name="agentData"> Agent data. </param>
        /// <param name="childIdx"> Index of child which will be activated. </param>
        /// <param name="childDecideValue"> Decide value of child which will be activated. </param>
        /// <param name="childDecisionData"> Decision data of child to be used in activate method. </param>
        /// <returns> True if there's a child which wants to be activated, else false. </returns>
        private bool DecideForFirstPossible(
            IAgentData agentData, ref int childIdx, ref float childDecideValue, ref IDecisionData childDecisionData)
        {
            return this.DecideForFirstPossible(
                agentData, 0, this.Children.Count, ref childIdx, ref childDecideValue, ref childDecisionData);
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

        /// <summary>
        ///   The decision data.
        /// </summary>
        public class DecisionData : IDecisionData
        {
            #region Public Properties

            /// <summary>
            ///   Decision data of selected child.
            /// </summary>
            public IDecisionData ChildDecisionData { get; set; }

            /// <summary>
            ///   Index of selected child.
            /// </summary>
            public int SelectedChildIdx { get; set; }

            #endregion
        }
    }
}