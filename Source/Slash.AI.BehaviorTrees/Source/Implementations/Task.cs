// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Task.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.AI.BehaviorTrees.Implementations
{
    using System;
    using System.Collections.Generic;

    using Slash.AI.BehaviorTrees.Enums;
    using Slash.AI.BehaviorTrees.Interfaces;
    using Slash.AI.BehaviorTrees.Tree;

    /// <summary>
    ///   Base class for tasks to have no need to implement empty methods in classes which implement the ITask interface.
    /// </summary>
    [Serializable]
    public class Task : ITask
    {
        #region Constructors and Destructors

        /// <summary>
        ///   Initializes a new instance of the <see cref="Task" /> class. Constructor.
        /// </summary>
        public Task()
        {
            this.Name = string.Empty;
        }

        #endregion

        #region Events

        /// <summary>
        ///   Called when decider finished successful.
        /// </summary>
        public event OnSuccess OnSuccess;

        #endregion

        #region Properties

        /// <summary>
        ///   Debug name.
        /// </summary>
        public string Name { get; set; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///   Activation. This method is called when the task was chosen to be executed. It's called right before the first update
        ///   of the task. The task can setup its specific task data in here and do initial actions.
        /// </summary>
        /// <param name="agentData"> Agent data. </param>
        /// <param name="decisionData"> Decision data to use in activate method. </param>
        /// <returns> Execution status after activation. </returns>
        public virtual ExecutionStatus Activate(IAgentData agentData, IDecisionData decisionData)
        {
            return ExecutionStatus.Running;
        }

        /// <summary>
        ///   Deactivation.
        /// </summary>
        /// <param name="agentData"> Agent data. </param>
        public virtual void Deactivate(IAgentData agentData)
        {
        }

        /// <summary>
        ///   Depending on the group policy of its parent, the floating point return value indicates whether the decider will be
        ///   activated.
        /// </summary>
        /// <param name="agentData"> Agent data. </param>
        /// <param name="decisionData"> Decision data to use in activate method. </param>
        /// <returns> Floating point value used to decide if the decider will be activated. </returns>
        public virtual float Decide(IAgentData agentData, ref IDecisionData decisionData)
        {
            return 1.0f;
        }

        /// <summary>
        ///   Determines whether the specified <see cref="T:Task" /> is equal to the current
        ///   <see cref="T:Task" />.
        /// </summary>
        /// <returns>
        ///   true if the specified <see cref="T:Task" /> is equal to the current <see cref="T:Task" />; otherwise, false.
        /// </returns>
        /// <param name="other">The <see cref="T:Task" /> to compare with the current <see cref="T:Task" />. </param>
        public bool Equals(Task other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return Equals(other.Name, this.Name);
        }

        /// <summary>
        ///   Determines whether the specified <see cref="T:BaseTask" /> is equal to the current
        ///   <see cref="T:BaseTask" />.
        /// </summary>
        /// <returns>
        ///   true if the specified <see cref="T:BaseTask" /> is equal to the current <see cref="T:BaseTask" />; otherwise, false.
        /// </returns>
        /// <param name="obj">The <see cref="T:BaseTask" /> to compare with the current <see cref="T:BaseTask" />. </param>
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

            if (obj.GetType() != typeof(Task))
            {
                return false;
            }

            return this.Equals((Task)obj);
        }

        /// <summary>
        ///   Searches for tasks which forfill the passed predicate.
        /// </summary>
        /// <param name="taskNode"> Task node of this task. </param>
        /// <param name="predicate"> Predicate to forfill. </param>
        /// <param name="tasks"> List of tasks which forfill the passed predicate. </param>
        public virtual void FindTasks(TaskNode taskNode, Func<ITask, bool> predicate, ref ICollection<TaskNode> tasks)
        {
        }

        /// <summary>
        ///   Generates a collection of active task nodes under this task. Used for debugging only.
        /// </summary>
        /// <param name="agentData"> Agent data. </param>
        /// <param name="taskNode"> Task node of this task. </param>
        /// <param name="activeTasks"> Collection of active task nodes. </param>
        public virtual void GetActiveTasks(
            IAgentData agentData,
            TaskNode taskNode,
            ref ICollection<TaskNode> activeTasks)
        {
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
            return this.Name != null ? this.Name.GetHashCode() : 0;
        }

        /// <summary>
        ///   Per frame update.
        /// </summary>
        /// <param name="agentData"> Agent data. </param>
        /// <returns> Execution status after this update. </returns>
        public virtual ExecutionStatus Update(IAgentData agentData)
        {
            return ExecutionStatus.Running;
        }

        #endregion

        #region Methods

        /// <summary>
        ///   Invoke callback when decider finished successful.
        /// </summary>
        protected void InvokeOnSuccess()
        {
            OnSuccess handler = this.OnSuccess;
            if (handler != null)
            {
                handler();
            }
        }

        #endregion
    }

    /// <summary>
    ///   Base class for tasks to have no need to implement empty methods in classes which implement the ITask interface.
    /// </summary>
    /// <typeparam name="TTaskData"> </typeparam>
    [Serializable]
    public class BaseTask<TTaskData> : Task
        where TTaskData : ITaskData, new()
    {
        #region Public Methods and Operators

        /// <summary>
        ///   Activation. This method is called when the task was chosen to be executed. It's called right before the first update
        ///   of the task. The task can setup its specific task data in here and do initial actions.
        /// </summary>
        /// <param name="agentData"> Agent data. </param>
        /// <param name="decisionData"> Decision data to use in activate method. </param>
        /// <returns> Execution status after activation. </returns>
        public override ExecutionStatus Activate(IAgentData agentData, IDecisionData decisionData)
        {
            agentData.CurrentTaskData = new TTaskData();

            return ExecutionStatus.Running;
        }

        /// <summary>
        ///   Deactivation.
        /// </summary>
        /// <param name="agentData"> Agent data. </param>
        public override void Deactivate(IAgentData agentData)
        {
            agentData.CurrentTaskData = null;
        }

        /// <summary>
        ///   Determines whether the specified <see cref="T:BaseTask" /> is equal to the current <see cref="T:BaseTask" />.
        /// </summary>
        /// <returns>
        ///   true if the specified <see cref="T:System.Object" /> is equal to the current <see cref="T:System.Object" />;
        ///   otherwise, false.
        /// </returns>
        /// <param name="other">The <see cref="T:System.Object" /> to compare with the current <see cref="T:System.Object" />. </param>
        /// <filterpriority>2</filterpriority>
        public bool Equals(BaseTask<TTaskData> other)
        {
            return base.Equals(other);
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

            return this.Equals(obj as BaseTask<TTaskData>);
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
            return base.GetHashCode();
        }

        #endregion
    }
}