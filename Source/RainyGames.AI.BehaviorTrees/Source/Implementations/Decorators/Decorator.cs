// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Decorator.cs" company="Rainy Games">
//   Copyright (c) Rainy Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace RainyGames.AI.BehaviorTrees.Implementations.Decorators
{
    using System;
    using System.Collections.Generic;
    using System.Xml.Serialization;

    using RainyGames.AI.BehaviorTrees.Editor;
    using RainyGames.AI.BehaviorTrees.Enums;
    using RainyGames.AI.BehaviorTrees.Interfaces;
    using RainyGames.AI.BehaviorTrees.Tree;

    /// <summary>
    ///   Base class for a decorator.
    /// </summary>
    [Serializable]
    public class Decorator : Task, IComposite
    {
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
                return 1;
            }
        }

        /// <summary>
        ///   Collection of children. Read-only.
        /// </summary>
        [XmlIgnore]
        public IList<ITask> Children
        {
            get
            {
                return this.Task != null ? new List<ITask> { this.Task } : new List<ITask>();
            }
        }

        /// <summary>
        ///   Xml serialization for decorated task.
        /// </summary>
        [XmlElement("Child")]
        public XmlWrapper DeciderSerialized
        {
            get
            {
                return new XmlWrapper(this.Task);
            }

            set
            {
                this.Task = value.Task;
            }
        }

        /// <summary>
        ///   Decorated task.
        /// </summary>
        [XmlIgnore]
        public ITask Task { get; set; }

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
            return this.ActivateChild(agentData, decisionData);
        }

        /// <summary>
        ///   Adds a child to this group task.
        /// </summary>
        /// <param name="child"> Child to add. </param>
        /// <exception cref="Exception">Thrown if child couldn't be added because capacity was reached.</exception>
        public void AddChild(ITask child)
        {
            if (this.Task != null)
            {
                throw new Exception("Decorator already has a child.");
            }

            this.Task = child;

            this.InvokeChildAdded(child);
        }

        /// <summary>
        ///   Deactivation.
        /// </summary>
        /// <param name="agentData"> Agent data. </param>
        public override void Deactivate(IAgentData agentData)
        {
            this.DeactivateChild(agentData);
        }

        /// <summary>
        ///   Depending on the group policy of its parent, the floating point return value indicates whether the task will be activated.
        /// </summary>
        /// <param name="agentData"> Agent data. </param>
        /// <param name="decisionData"> Decision data to use in activate method. </param>
        /// <returns> Floating point value used to decide if the task will be activated. </returns>
        public override float Decide(IAgentData agentData, ref IDecisionData decisionData)
        {
            return this.Task != null ? this.DecideChild(agentData, ref decisionData) : 0.0f;
        }

        /// <summary>
        ///   The equals.
        /// </summary>
        /// <param name="other"> The other. </param>
        /// <returns> The System.Boolean. </returns>
        public bool Equals(Decorator other)
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

            return this.Equals(obj as Decorator);
        }

        /// <summary>
        ///   Searches for tasks which forfill the passed predicate.
        /// </summary>
        /// <param name="taskNode"> Task node of this task. </param>
        /// <param name="predicate"> Predicate to forfill. </param>
        /// <param name="tasks"> List of tasks which forfill the passed predicate. </param>
        public override void FindTasks(TaskNode taskNode, Func<ITask, bool> predicate, ref ICollection<TaskNode> tasks)
        {
            if (this.Task == null)
            {
                return;
            }

            // Check child.
            TaskNode childTaskNode = taskNode.CreateChildNode(this.Task, 0);
            if (predicate(this.Task))
            {
                tasks.Add(childTaskNode);
            }

            // Find tasks in child.
            this.Task.FindTasks(childTaskNode, predicate, ref tasks);
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
        ///   Inserts a child to this group task at the passed index.
        /// </summary>
        /// <param name="index"> Position to add child to. </param>
        /// <param name="child"> Child to insert. </param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if passed index isn't between 0 and Children.Count (inclusive).</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if passed index isn't between 0 and Capacity (exclusive).</exception>
        /// <exception cref="Exception">Thrown if child couldn't be inserted because capacity was reached.</exception>
        public void InsertChild(int index, ITask child)
        {
            if (this.Task != null)
            {
                throw new Exception("Decorator already has a child.");
            }

            if (index != 0)
            {
                throw new ArgumentOutOfRangeException("Decorator only has slot 0 to store a child in.");
            }

            this.Task = child;

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
            if (oldIndex != 0 || newIndex != 0)
            {
                throw new ArgumentOutOfRangeException("Decorator only has slot 0 to store a child in.");
            }
        }

        /// <summary>
        ///   Removes a child from this group task.
        /// </summary>
        /// <param name="child"> Child to remove. </param>
        /// <returns> Indicates if the child was removed. </returns>
        public bool RemoveChild(ITask child)
        {
            if (this.Task != child)
            {
                return false;
            }

            this.Task = null;

            this.InvokeChildRemoved(child);

            return true;
        }

        /// <summary>
        ///   Per frame update.
        /// </summary>
        /// <param name="agentData"> Agent data. </param>
        /// <returns> Execution status after this update. </returns>
        public override ExecutionStatus Update(IAgentData agentData)
        {
            return this.UpdateChild(agentData);
        }

        #endregion

        #region Methods

        /// <summary>
        ///   Activates the child of the decorator.
        /// </summary>
        /// <param name="agentData"> Agent data. </param>
        /// <param name="decisionData"> Decision data. </param>
        /// <returns> Execution status after activation. </returns>
        protected ExecutionStatus ActivateChild(IAgentData agentData, IDecisionData decisionData)
        {
            ++agentData.CurrentDeciderLevel;
            ExecutionStatus result = this.Task.Activate(agentData, decisionData);
            --agentData.CurrentDeciderLevel;
            return result;
        }

        /// <summary>
        ///   Deactivates the child of the decorator.
        /// </summary>
        /// <param name="agentData"> Agent data. </param>
        protected void DeactivateChild(IAgentData agentData)
        {
            ++agentData.CurrentDeciderLevel;
            this.Task.Deactivate(agentData);
            --agentData.CurrentDeciderLevel;
        }

        /// <summary>
        ///   Let's the child decide.
        /// </summary>
        /// <param name="agentData"> Agent data. </param>
        /// <param name="decisionData"> Decision data to use in activate method. </param>
        /// <returns> Floating point value used to decide if the task will be activated. </returns>
        protected float DecideChild(IAgentData agentData, ref IDecisionData decisionData)
        {
            ++agentData.CurrentDeciderLevel;
            float decisionValue = this.Task.Decide(agentData, ref decisionData);
            --agentData.CurrentDeciderLevel;
            return decisionValue;
        }

        /// <summary>
        ///   Updates the child of the decorator.
        /// </summary>
        /// <param name="agentData"> Agent data. </param>
        /// <returns> Execution status after update. </returns>
        protected ExecutionStatus UpdateChild(IAgentData agentData)
        {
            ++agentData.CurrentDeciderLevel;
            ExecutionStatus result = this.Task.Update(agentData);
            --agentData.CurrentDeciderLevel;
            return result;
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