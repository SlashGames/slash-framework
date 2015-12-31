// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BehaviorTree.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.AI.BehaviorTrees
{
    using System;
    using System.Collections.Generic;
    using System.Xml.Serialization;

    using Slash.AI.BehaviorTrees.Editor;
    using Slash.AI.BehaviorTrees.Enums;
    using Slash.AI.BehaviorTrees.Interfaces;
    using Slash.AI.BehaviorTrees.Tree;
    using Slash.Serialization.Utils;

    /// <summary>
    ///   Implementation of a behavior tree.
    /// </summary>
    [Serializable]
    public class BehaviorTree : IBehaviorTree, ICloneable
    {
        #region Fields

        /// <summary>
        ///   Root of behavior tree.
        /// </summary>
        private ITask root;

        #endregion

        #region Properties

        /// <summary>
        ///   Gets or sets the root.
        /// </summary>
        [XmlIgnore]
        public ITask Root
        {
            get
            {
                return this.root;
            }

            set
            {
                this.root = value;
            }
        }

        /// <summary>
        ///   Xml serialization for root decider.
        /// </summary>
        [XmlElement("Root")]
        public XmlWrapper RootSerialized
        {
            get
            {
                return new XmlWrapper(this.root);
            }

            set
            {
                this.root = value.Task;
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///   Creates a new object that is a copy of the current instance.
        /// </summary>
        /// <returns> A new object that is a copy of this instance. </returns>
        /// <filterpriority>2</filterpriority>
        public object Clone()
        {
            // Serialize and deserialize again to clone.
            return SerializationUtils.DeepCopy(this);
        }

        /// <summary>
        ///   Deactivation.
        /// </summary>
        /// <param name="agentData"> Agent data. </param>
        public void Deactivate(IAgentData agentData)
        {
            if (this.root == null)
            {
                return;
            }

            agentData.CurrentDeciderLevel = 0;

            switch (agentData.ExecutionStatus)
            {
                case ExecutionStatus.Running:
                {
                    // Deactivate root task.
                    this.root.Deactivate(agentData);
                }

                    break;
            }
        }

        /// <summary>
        ///   Determines whether the specified <see cref="T:BehaviorTree" /> is equal to the current <see cref="T:BehaviorTree" />.
        /// </summary>
        /// <returns>
        ///   true if the specified <see cref="T:BehaviorTree" /> is equal to the current <see cref="T:BehaviorTree" />; otherwise,
        ///   false.
        /// </returns>
        /// <param name="other">The <see cref="T:BehaviorTree" /> to compare with the current <see cref="T:BehaviorTree" />.</param>
        public bool Equals(BehaviorTree other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return Equals(other.root, this.root);
        }

        /// <summary>
        ///   Determines whether the specified <see cref="T:System.Object" /> is equal to the current
        ///   <see cref="T:System.Object" />.
        /// </summary>
        /// <returns>
        ///   true if the specified <see cref="T:System.Object" /> is equal to the current <see cref="T:System.Object" />;
        ///   otherwise, false.
        /// </returns>
        /// <param name="obj">The <see cref="T:System.Object" /> to compare with the current <see cref="T:System.Object" />. </param>
        /// <filterpriority>2</filterpriority>
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

            if (obj.GetType() != typeof(BehaviorTree))
            {
                return false;
            }

            return this.Equals((BehaviorTree)obj);
        }

        /// <summary>
        ///   Generates a collection of active task nodes under this task. Used for debugging only.
        /// </summary>
        /// <param name="agentData"> Agent data. </param>
        /// <returns> Collection of active task nodes. </returns>
        public ICollection<TaskNode> GetActiveTasks(IAgentData agentData)
        {
            if (this.root == null || agentData.ExecutionStatus != ExecutionStatus.Running)
            {
                return null;
            }

            agentData.CurrentDeciderLevel = 0;

            TaskNode taskNode = new TaskNode { Task = this.root };
            ICollection<TaskNode> activeTasks = new List<TaskNode> { taskNode };
            this.root.GetActiveTasks(agentData, taskNode, ref activeTasks);
            return activeTasks;
        }

        /// <summary>
        ///   Serves as a hash function for a particular type.
        /// </summary>
        /// <returns>
        ///   A hash code for the current <see cref="T:System.Object" />.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        public override int GetHashCode()
        {
            return this.root != null ? this.root.GetHashCode() : 0;
        }

        /// <summary>
        ///   Per frame data.
        /// </summary>
        /// <param name="agentData"> Agent data. </param>
        public void Update(IAgentData agentData)
        {
            if (this.Root == null)
            {
                return;
            }

            agentData.CurrentDeciderLevel = 0;

            agentData.PreUpdate();

            switch (agentData.ExecutionStatus)
            {
                case ExecutionStatus.None:
                case ExecutionStatus.Failed:
                case ExecutionStatus.Success:
                {
                    IDecisionData decisionData = null;
                    if (this.Root.Decide(agentData, ref decisionData) > 0.0f)
                    {
                        // Activate decider.
                        agentData.ExecutionStatus = this.Root.Activate(agentData, decisionData);

                        if (agentData.ExecutionStatus == ExecutionStatus.Running)
                        {
                            // Update decider.
                            agentData.ExecutionStatus = this.Root.Update(agentData);
                        }
                    }
                }

                    break;
                case ExecutionStatus.Running:
                {
                    // Update decider.
                    agentData.ExecutionStatus = this.Root.Update(agentData);
                }

                    break;
            }

            agentData.PostUpdate();
        }

        #endregion
    }
}