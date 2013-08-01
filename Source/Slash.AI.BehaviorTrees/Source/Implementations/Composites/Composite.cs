// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Composite.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.AI.BehaviorTrees.Implementations.Composites
{
    using System;
    using System.Collections.Generic;
    using System.Xml.Serialization;

    using Slash.AI.BehaviorTrees.Editor;
    using Slash.AI.BehaviorTrees.Enums;
    using Slash.AI.BehaviorTrees.Interfaces;
    using Slash.AI.BehaviorTrees.Tree;
    using Slash.Collections.Extensions;
    using Slash.Collections.Utils;

    /// <summary>
    ///   Task which contains array of references to other tasks.
    /// </summary>
    /// <typeparam name="TTaskData"> Task data. </typeparam>
    [Serializable]
    public abstract class Composite<TTaskData> : BaseTask<TTaskData>, IComposite
        where TTaskData : ITaskData, new()
    {
        #region Fields

        /// <summary>
        ///   Children of this group task.
        /// </summary>
        private List<ITask> children;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///   Constructor.
        /// </summary>
        protected Composite()
        {
            this.children = new List<ITask>();
        }

        /// <summary>
        ///   Constructor.
        /// </summary>
        /// <param name="deciders"> Child deciders. </param>
        protected Composite(List<ITask> deciders)
        {
            this.children = deciders;
        }

        #endregion

        #region Public Events

        /// <summary>
        ///   Called when a child was added to the composite.
        /// </summary>
        public event CompositeChildAddedDelegate ChildAdded;

        /// <summary>
        ///   Called when a child was removed from the composite.
        /// </summary>
        public event CompositeChildRemovedDelegate ChildRemoved;

        #endregion

        #region Public Properties

        /// <summary>
        ///   Maximum number of children that the composite can take.
        /// </summary>
        [XmlIgnore]
        public int Capacity
        {
            get
            {
                return int.MaxValue;
            }
        }

        /// <summary>
        ///   Children of this group task. Read-only.
        /// </summary>
        [XmlIgnore]
        public IList<ITask> Children
        {
            get
            {
                return this.children.AsReadOnly();
            }
        }

        /// <summary>
        ///   Xml Serialization for decorated children.
        /// </summary>
        [XmlElement("Children")]
        public XmlWrapperList ChildrenSerialized
        {
            get
            {
                return new XmlWrapperList(this.children);
            }

            set
            {
                this.children = value.List;
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///   Adds a child to this group task.
        /// </summary>
        /// <param name="child"> Child to add. </param>
        public void AddChild(ITask child)
        {
            this.children.Add(child);

            this.InvokeChildAdded(child);
        }

        public bool Equals(Composite<TTaskData> other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return CollectionUtils.SequenceEqual(this.children, other.children);
        }

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

            if (!obj.GetType().IsSubclassOf(typeof(Composite<TTaskData>)))
            {
                return false;
            }

            return this.Equals((Composite<TTaskData>)obj);
        }

        /// <summary>
        ///   Searches for tasks which forfill the passed predicate.
        /// </summary>
        /// <param name="taskNode"> Task node of this task. </param>
        /// <param name="predicate"> Predicate to forfill. </param>
        /// <param name="tasks"> List of tasks which forfill the passed predicate. </param>
        public override void FindTasks(TaskNode taskNode, Func<ITask, bool> predicate, ref ICollection<TaskNode> tasks)
        {
            // Check children.
            for (int index = 0; index < this.children.Count; index++)
            {
                ITask child = this.children[index];
                TaskNode childTaskNode = taskNode.CreateChildNode(child, index);
                if (predicate(child))
                {
                    tasks.Add(childTaskNode);
                }

                // Find tasks in child.
                child.FindTasks(childTaskNode, predicate, ref tasks);
            }
        }

        public override int GetHashCode()
        {
            return this.children != null ? this.children.GetHashCode() : 0;
        }

        /// <summary>
        ///   Inserts a child to this group task at the passed index.
        /// </summary>
        /// <param name="index"> Position to add child to. </param>
        /// <param name="child"> Child to insert. </param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if passed index isn't between 0 and Children.Count (inclusive).</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if passed index isn't between 0 and Capacity (exclusive).</exception>
        public void InsertChild(int index, ITask child)
        {
            this.children.Insert(index, child);

            this.InvokeChildAdded(child);
        }

        /// <summary>
        ///   Moves a child to the passed position inside the group.
        /// </summary>
        /// <param name="oldIndex"> Old position of the child. </param>
        /// <param name="newIndex"> New position of the child. </param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if passed old index isn't between 0 and Children.Count (exclusive).</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if passed new index isn't between 0 and Children.Count (exclusive).</exception>
        public void MoveChild(int oldIndex, int newIndex)
        {
            this.children.Move(oldIndex, newIndex);
        }

        /// <summary>
        ///   Removes a child from this group task.
        /// </summary>
        /// <param name="child"> Child to remove. </param>
        /// <returns> Indicates if the child was removed. </returns>
        public bool RemoveChild(ITask child)
        {
            if (!this.children.Remove(child))
            {
                return false;
            }

            this.InvokeChildRemoved(child);

            return true;
        }

        #endregion

        #region Methods

        /// <summary>
        ///   Activates the passed child. Does no index range checking.
        /// </summary>
        /// <param name="childIdx"> Child index. Has to be in correct range. </param>
        /// <param name="agentData"> Agent data. </param>
        /// <param name="decisionData"> Decision data from the decide function. </param>
        /// <returns> Execution status after the activation. </returns>
        protected ExecutionStatus ActivateChild(int childIdx, IAgentData agentData, IDecisionData decisionData)
        {
            ++agentData.CurrentDeciderLevel;
            ExecutionStatus executionStatus = this.children[childIdx].Activate(agentData, decisionData);
            --agentData.CurrentDeciderLevel;
            return executionStatus;
        }

        /// <summary>
        ///   Deactivates the passed child. Does no index range checking.
        /// </summary>
        /// <param name="childIdx"> Child index. Has to be in correct range. </param>
        /// <param name="agentData"> Agent data. </param>
        protected void DeactivateChild(int childIdx, IAgentData agentData)
        {
            ++agentData.CurrentDeciderLevel;
            this.children[childIdx].Deactivate(agentData);
            --agentData.CurrentDeciderLevel;
        }

        /// <summary>
        ///   Takes first task which wants to be active. Checks only a subset of the children from index 0 to passed lastChildIdx (exclusive).
        /// </summary>
        /// <param name="agentData"> Agent data. </param>
        /// <param name="firstChildIdx"> First child index to check (inclusive). </param>
        /// <param name="lastChildIdx"> Last child index to check (exclusive). </param>
        /// <param name="childIdx"> Index of child which will be activated. </param>
        /// <param name="childDecideValue"> Decide value of child which will be activated. </param>
        /// <param name="childDecisionData"> Decision data of child to be used in activate method. </param>
        /// <returns> True if there's a child which wants to be activated, else false. </returns>
        protected bool DecideForFirstPossible(
            IAgentData agentData,
            int firstChildIdx,
            int lastChildIdx,
            ref int childIdx,
            ref float childDecideValue,
            ref IDecisionData childDecisionData)
        {
            for (int idx = firstChildIdx; idx < lastChildIdx; idx++)
            {
                ITask task = this.Children[idx];
                float decideValue = task.Decide(agentData, ref childDecisionData);
                if (decideValue <= 0.0f)
                {
                    continue;
                }

                childIdx = idx;
                childDecideValue = decideValue;
                return true;
            }

            return false;
        }

        /// <summary>
        ///   Computes the relevancy for the decide method by taking the maximum relevancy of all children.
        /// </summary>
        /// <param name="agentData"> Agent data. </param>
        /// <param name="decisionData"> Decision data. </param>
        /// <returns> Maximum relevancy of the children. </returns>
        protected float DelegateDecideToChildren(IAgentData agentData, ref IDecisionData decisionData)
        {
            float maxDecisionValue = 0.0f;
            foreach (ITask decider in this.children)
            {
                IDecisionData childDecisionData = null;
                float decisionValue = decider.Decide(agentData, ref childDecisionData);
                if (decisionValue > maxDecisionValue)
                {
                    maxDecisionValue = decisionValue;
                    decisionData = childDecisionData;
                }
            }

            return maxDecisionValue;
        }

        /// <summary>
        ///   Gets the active tasks of the passed child. Does no index range checking.
        /// </summary>
        /// <param name="childIdx"> Child index. Has to be in correct range. </param>
        /// <param name="agentData"> Agent data. </param>
        /// <param name="childTaskNode"> Task node of child. </param>
        /// <param name="activeTasks"> Collection of active tasks. </param>
        protected void GetActiveChildTasks(
            int childIdx, IAgentData agentData, TaskNode childTaskNode, ref ICollection<TaskNode> activeTasks)
        {
            ++agentData.CurrentDeciderLevel;
            this.children[childIdx].GetActiveTasks(agentData, childTaskNode, ref activeTasks);
            --agentData.CurrentDeciderLevel;
        }

        /*
        /// <summary>
        ///   Called when the configuration should be read from xml. Can be overridden in derived classes to read
        ///   additional attributes from xml.
        /// </summary>
        /// <param name = "reader">Xml reader.</param>
        protected override void OnReadXml(XmlReader reader)
        {
            int numChildren = Convert.ToInt32(reader.GetValue("Count"));
            reader.ReadStartElement("Children");
            for (int idx = 0; idx < numChildren; ++idx)
            {
                // Read child.
                ITask task;
                BehaviorTreeHelpers.ReadXml(reader, out task, "Child");

                this.children.Add(task);
            }

            reader.ReadEndElement();
        }

        /// <summary>
        ///   Called when the configuration should be write to xml. Can be overridden in derived classes to write 
        ///   additional attributes to xml.
        /// </summary>
        /// <param name = "writer">Xml writer.</param>
        protected override void OnWriteXml(XmlWriter writer)
        {
            writer.WriteStartElement("Children");
            writer.WriteAttributeString("Count", this.children.Count.ToString());
            foreach (ITask task in this.children)
            {
                BehaviorTreeHelpers.WriteXml(writer, task, "Child");
            }

            writer.WriteEndElement();
        }*/

        /// <summary>
        ///   Updates the passed child. Does no index range checking.
        /// </summary>
        /// <param name="childIdx"> Child index. Has to be in correct range. </param>
        /// <param name="agentData"> Agent data. </param>
        /// <returns> New execution status. </returns>
        protected ExecutionStatus UpdateChild(int childIdx, IAgentData agentData)
        {
            ++agentData.CurrentDeciderLevel;
            ExecutionStatus executionStatus = this.children[childIdx].Update(agentData);
            --agentData.CurrentDeciderLevel;
            return executionStatus;
        }

        private void InvokeChildAdded(ITask childTask)
        {
            CompositeChildAddedDelegate handler = this.ChildAdded;
            if (handler != null)
            {
                handler(this, childTask);
            }
        }

        private void InvokeChildRemoved(ITask childTask)
        {
            CompositeChildRemovedDelegate handler = this.ChildRemoved;
            if (handler != null)
            {
                handler(this, childTask);
            }
        }

        #endregion
    }
}