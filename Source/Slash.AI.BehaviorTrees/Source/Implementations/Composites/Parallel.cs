namespace Slash.AI.BehaviorTrees.Implementations.Composites
{
    using System;
    using System.Collections.Generic;
    using System.Xml.Serialization;

    using Slash.AI.BehaviorTrees.Enums;
    using Slash.AI.BehaviorTrees.Interfaces;
    using Slash.AI.BehaviorTrees.Tree;

    /// <summary>
    ///   The Parallel composite acts in a similar way to the Sequence composite. It has a set of child tasks, and it runs them until one of them fails. At that point, the Parallel task as a whole fails. If all of the child tasks complete successfully, the Parallel task returns with success. In this way, it is identical to the Sequence task and its non-deterministic variations. The difference is the way it runs those tasks. Rather than running them one at a time, it runs them all simultaneously. We can think of it as creating a bunch of new threads, one per child, and setting the child tasks off together.
    /// </summary>
    [Serializable]
    public class Parallel : Composite<Parallel.Data>
    {
        #region Constructors and Destructors

        /// <summary>
        ///   Initializes a new instance of the <see cref="Parallel" /> class. Constructor.
        /// </summary>
        /// <param name="children"> Child tasks. </param>
        public Parallel(List<ITask> children)
            : base(children)
        {
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref="Parallel" /> class. Constructor.
        /// </summary>
        public Parallel()
        {
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///   Main task. If set the parallel will run as long as the main task runs. When the main task ends, all other tasks which still run are aborted. If not set the parallel will run as long as any task runs.
        /// </summary>
        [XmlIgnore]
        public ITask MainTask { get; set; }

        /// <summary>
        ///   Xml serialization of the main task index.
        /// </summary>
        [XmlElement("MainTask")]
        public int MainTaskIndex
        {
            get
            {
                return this.Children.IndexOf(this.MainTask);
            }

            set
            {
                this.MainTask = value >= 0 && value < this.Children.Count ? this.Children[value] : null;
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///   Activation. This method is called when the task was chosen to be executed. It's called right before the first update of the task. The task can setup its specific task data in here and do initial actions.
        /// </summary>
        /// <param name="agentData"> Agent data. </param>
        /// <param name="decisionData"> Decision data to use in activate method. </param>
        /// <returns> The SlashGames.AI.BehaviorTrees.Enums.ExecutionStatus. </returns>
        public override ExecutionStatus Activate(IAgentData agentData, IDecisionData decisionData)
        {
            DecisionData parallelDecisionData = decisionData as DecisionData;
            if (parallelDecisionData == null)
            {
                throw new ArgumentException("Got invalid decision data, expected data for Parallel task.");
            }

            Data data = new Data();

            // Activate all children.
            ExecutionStatus executionStatus = ExecutionStatus.Running;
            data.ChildrenData = new ITaskData[this.Children.Count];
            ++agentData.CurrentDeciderLevel;
            for (int childIdx = 0; childIdx < this.Children.Count; childIdx++)
            {
                ExecutionStatus childExecutionStatus = this.Children[childIdx].Activate(
                    agentData, parallelDecisionData.ChildrenDecisionData[childIdx]);
                if (childExecutionStatus == ExecutionStatus.Failed)
                {
                    // If one fails, all others also fail.
                    executionStatus = ExecutionStatus.Failed;

                    // Don't continue as we already have a result.
                    break;
                }

                // If main task already succeeded, sub tasks don't have to be activated.
                if (childExecutionStatus == ExecutionStatus.Success && childIdx == this.MainTaskIndex)
                {
                    executionStatus = ExecutionStatus.Success;
                    break;
                }

                // If not running, the child task don't have to be added to running children.
                if (childExecutionStatus != ExecutionStatus.Running)
                {
                    continue;
                }

                data.ChildrenData[childIdx] = agentData.CurrentTaskData;
                data.RunningChildren.Add(childIdx);
            }

            if (executionStatus != ExecutionStatus.Running)
            {
                // Deactivate remaining running children.
                foreach (int childIdx in data.RunningChildren)
                {
                    agentData.CurrentTaskData = data.ChildrenData[childIdx];
                    this.Children[childIdx].Deactivate(agentData);
                }
            }

            --agentData.CurrentDeciderLevel;

            // Store task data.
            agentData.CurrentTaskData = data;

            return executionStatus;
        }

        /// <summary>
        ///   Deactivation.
        /// </summary>
        /// <param name="agentData"> Agent data. </param>
        public override void Deactivate(IAgentData agentData)
        {
            Data data = agentData.CurrentTaskData as Data;
            if (data == null)
            {
                throw new ArgumentException("Got invalid task data, expected data for Parallel task.");
            }

            // Deactivate all running children.
            ++agentData.CurrentDeciderLevel;
            foreach (int childIdx in data.RunningChildren)
            {
                agentData.CurrentTaskData = data.ChildrenData[childIdx];
                this.Children[childIdx].Deactivate(agentData);
            }

            --agentData.CurrentDeciderLevel;

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
            List<IDecisionData> childrenDecisionData = new List<IDecisionData>();

            // Check if all children want to run.
            float maxChildDecisionValue = 0.0f;
            foreach (ITask child in this.Children)
            {
                IDecisionData childDecisionData = null;
                float childDecisionValue = child.Decide(agentData, ref childDecisionData);
                maxChildDecisionValue = Math.Max(maxChildDecisionValue, childDecisionValue);

                // If one doesn't want to run, we don't run the whole task.
                if (childDecisionValue <= 0.0f)
                {
                    return 0.0f;
                }

                childrenDecisionData.Add(childDecisionData);
            }

            decisionData = new DecisionData { ChildrenDecisionData = childrenDecisionData };

            return maxChildDecisionValue;
        }

        /// <summary>
        ///   The equals.
        /// </summary>
        /// <param name="other"> The other. </param>
        /// <returns> The System.Boolean. </returns>
        public bool Equals(Parallel other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return base.Equals(other) && Equals(other.MainTask, this.MainTask);
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

            return this.Equals(obj as Parallel);
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
                throw new ArgumentException(string.Format("Expected parallel data for task '{0}'.", this.Name));
            }

            // Check if selector has an active child.
            if (data.RunningChildren.Count == 0)
            {
                return;
            }

            // Add task to active tasks and collect active tasks of active child.
            ++agentData.CurrentDeciderLevel;
            foreach (int childIdx in data.RunningChildren)
            {
                TaskNode childTaskNode = taskNode.CreateChildNode(this.Children[childIdx], childIdx);
                activeTasks.Add(childTaskNode);

                agentData.CurrentTaskData = data.ChildrenData[childIdx];
                this.Children[childIdx].GetActiveTasks(agentData, childTaskNode, ref activeTasks);
            }

            --agentData.CurrentDeciderLevel;
        }

        /// <summary>
        ///   The get hash code.
        /// </summary>
        /// <returns> The System.Int32. </returns>
        public override int GetHashCode()
        {
            unchecked
            {
                return (base.GetHashCode() * 397) ^ (this.MainTask != null ? this.MainTask.GetHashCode() : 0);
            }
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
                throw new ArgumentException("Got invalid task data, expected data for Parallel task.");
            }

            if (data.RunningChildren.Count == 0)
            {
                return ExecutionStatus.Success;
            }

            ExecutionStatus executionStatus = ExecutionStatus.Running;
            List<int> runningChildren = new List<int>(data.RunningChildren);
            ++agentData.CurrentDeciderLevel;
            foreach (int childIdx in runningChildren)
            {
                agentData.CurrentTaskData = data.ChildrenData[childIdx];
                ExecutionStatus childExecutionStatus = this.Children[childIdx].Update(agentData);

                if (childExecutionStatus == ExecutionStatus.Failed)
                {
                    // Remove from running children.
                    data.RunningChildren.Remove(childIdx);

                    // If one fails, all others also fail.
                    executionStatus = ExecutionStatus.Failed;

                    // Don't continue as we already have a result.
                    break;
                }

                if (childExecutionStatus == ExecutionStatus.Success)
                {
                    // Remove from running children.
                    data.RunningChildren.Remove(childIdx);

                    if (data.RunningChildren.Count == 0 || this.MainTaskIndex == childIdx)
                    {
                        // If all succeeded or if main task succeeded, parallel is done.
                        executionStatus = ExecutionStatus.Success;

                        break;
                    }
                }
            }

            if (executionStatus != ExecutionStatus.Running)
            {
                // Deactivate remaining running children.
                foreach (int childIdx in data.RunningChildren)
                {
                    agentData.CurrentTaskData = data.ChildrenData[childIdx];
                    this.Children[childIdx].Deactivate(agentData);
                }
            }

            --agentData.CurrentDeciderLevel;

            return executionStatus;
        }

        #endregion

        /// <summary>
        ///   task data of a parallel task.
        /// </summary>
        public class Data : ITaskData
        {
            #region Fields

            /// <summary>
            ///   Data of running children.
            /// </summary>
            public IList<ITaskData> ChildrenData;

            /// <summary>
            ///   List of running children.
            /// </summary>
            public List<int> RunningChildren = new List<int>();

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
            public List<IDecisionData> ChildrenDecisionData { get; set; }

            #endregion
        }
    }
}