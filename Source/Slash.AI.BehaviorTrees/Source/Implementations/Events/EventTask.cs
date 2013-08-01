// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EventTask.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.AI.BehaviorTrees.Implementations.Events
{
    using System;
    using System.Collections.Generic;

    using Slash.AI.BehaviorTrees.Enums;
    using Slash.AI.BehaviorTrees.Interfaces;

    /// <summary>
    ///   Base class for event tasks.
    /// </summary>
    /// <typeparam name="T"> Event-specific data. </typeparam>
    public abstract class EventTask<T> : Task
    {
        #region Fields

        /// <summary>
        ///   Event data for registered entities.
        /// </summary>
        private readonly Dictionary<IAgentData, AgentEventData> agentEventData =
            new Dictionary<IAgentData, AgentEventData>();

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///   Finalizes an instance of the <see cref="EventTask{T}" /> class. Destructor.
        /// </summary>
        ~EventTask()
        {
            // Unregister all entities.
            foreach (KeyValuePair<IAgentData, AgentEventData> eventDataPair in this.agentEventData)
            {
                this.UnregisterAgent(eventDataPair.Key);
            }

            this.agentEventData.Clear();
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///   Activation. This method is called when the task was chosen to be executed. It's called right before the first update of the task. The task can setup its specific task data in here and do initial actions.
        /// </summary>
        /// <param name="agentData"> Agent data. </param>
        /// <param name="decisionData"> Decision data to use in activate method. </param>
        /// <returns> Execution status after the activation. </returns>
        public override ExecutionStatus Activate(IAgentData agentData, IDecisionData decisionData)
        {
            // Get event data.
            AgentEventData eventData;
            if (!this.agentEventData.TryGetValue(agentData, out eventData))
            {
                return ExecutionStatus.Failed;
            }

            if (!eventData.EventOccured)
            {
                return ExecutionStatus.Failed;
            }

            // Execute event task code.
            this.Execute(agentData, eventData.EventData);

            // Reset flag.
            eventData.EventOccured = false;

            return ExecutionStatus.Success;
        }

        /// <summary>
        ///   Depending on the group policy of its parent, the floating point return value indicates whether the decider will be activated.
        /// </summary>
        /// <param name="agentData"> Agent data. </param>
        /// <param name="decisionData"> Decision data to use in activate method. </param>
        /// <returns> Floating point value used to decide if the decider will be activated. </returns>
        public override float Decide(IAgentData agentData, ref IDecisionData decisionData)
        {
            // Check if event data for agent available.
            AgentEventData eventData;
            if (this.agentEventData.TryGetValue(agentData, out eventData))
            {
                // Check if event occured.
                if (eventData.EventOccured)
                {
                    return 1.0f;
                }
            }
            else
            {
                // Register agent for event.
                this.RegisterAgent(agentData);
                this.agentEventData.Add(agentData, new AgentEventData());
            }

            // Not execute event task.
            return 0.0f;
        }

        #endregion

        #region Methods

        /// <summary>
        ///   Execution of the event task specific code.
        /// </summary>
        /// <param name="agentData"> Agent data. </param>
        /// <param name="eventData"> Event-specific data. </param>
        protected abstract void Execute(IAgentData agentData, T eventData);

        /// <summary>
        ///   Called from derived event tasks to indicate that the event for the passed agent occured.
        /// </summary>
        /// <param name="agentData"> Data of agent for which the event occured. </param>
        /// <param name="eventSpecificData"> Event-specific data. </param>
        protected void OnEventOccured(IAgentData agentData, T eventSpecificData)
        {
            if (agentData == null)
            {
                throw new NullReferenceException(string.Format("Event '{0}' occured for null agent.", this.GetType()));
            }

            AgentEventData eventData;
            if (!this.agentEventData.TryGetValue(agentData, out eventData))
            {
                return;
            }

            eventData.EventOccured = true;
            eventData.EventData = eventSpecificData;
        }

        /// <summary>
        ///   Registers an agent, so the event task will be informed when the observed event occurs.
        /// </summary>
        /// <param name="agentData"> Data of agent to register. </param>
        protected abstract void RegisterAgent(IAgentData agentData);

        /// <summary>
        ///   Unregisters an agent.
        /// </summary>
        /// <param name="agentData"> Data of agent to unregister. </param>
        protected abstract void UnregisterAgent(IAgentData agentData);

        #endregion

        /// <summary>
        ///   Event data for an agent.
        /// </summary>
        private class AgentEventData
        {
            #region Public Properties

            /// <summary>
            ///   Event-specific data.
            /// </summary>
            public T EventData { get; set; }

            /// <summary>
            ///   Indicates if the event occured.
            /// </summary>
            public bool EventOccured { get; set; }

            #endregion
        }
    }
}