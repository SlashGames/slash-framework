namespace RainyGames.AI.BehaviorTrees.Implementations.Actions
{
    using RainyGames.AI.BehaviorTrees.Data;
    using RainyGames.AI.BehaviorTrees.Enums;
    using RainyGames.AI.BehaviorTrees.Implementations.Decorators;
    using RainyGames.AI.BehaviorTrees.Interfaces;

    /// <summary>
    ///   Task to create a child blackboard of the current active blackboard and use that instead of the original one.
    /// </summary>
    public class CreateBlackboard : Decorator
    {
        #region Public Properties

        /// <summary>
        ///   Initial blackboard to use.
        /// </summary>
        public Blackboard Blackboard { get; set; }

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
            DecisionData taskDecisionData = (DecisionData)decisionData;

            // Create child blackboard.
            Data data = new Data { Blackboard = taskDecisionData.Blackboard, PreviousBlackboard = agentData.Blackboard };
            agentData.CurrentTaskData = data;

            // Setup blackboard.
            Setup(agentData, data);

            // Activate child.
            ExecutionStatus result = this.ActivateChild(agentData, taskDecisionData.ChildDecisionData);
            if (result != ExecutionStatus.Running)
            {
                // Tear down.
                TearDown(agentData, data);
            }

            return result;
        }

        /// <summary>
        ///   Deactivation.
        /// </summary>
        /// <param name="agentData"> Agent data. </param>
        public override void Deactivate(IAgentData agentData)
        {
            Data data = agentData.GetTaskData<Data>();

            // Setup blackboard.
            Setup(agentData, data);

            // Deactivate child.
            this.DeactivateChild(agentData);

            // Tear down.
            TearDown(agentData, data);
        }

        /// <summary>
        ///   Depending on the group policy of its parent, the floating point return value indicates whether the decider will be activated.
        /// </summary>
        /// <param name="agentData"> Agent data. </param>
        /// <param name="decisionData"> Decision data to use in activate method. </param>
        /// <returns> Floating point value used to decide if the decider will be activated. </returns>
        public override float Decide(IAgentData agentData, ref IDecisionData decisionData)
        {
            // Create blackboard.
            Blackboard blackboard = new Blackboard();

            // Add parent blackboards.
            if (this.Blackboard != null)
            {
                blackboard.Parents.Add(this.Blackboard);
            }

            blackboard.Parents.Add(agentData.Blackboard);

            // Setup blackboard.
            Blackboard previousBlackboard = agentData.Blackboard;
            agentData.Blackboard = blackboard;

            // Deactivate child.
            IDecisionData childDecisionData = null;
            float decisionValue = this.DecideChild(agentData, ref childDecisionData);

            // Tear down.
            agentData.Blackboard = previousBlackboard;

            // Create decision data.
            if (decisionValue > 0.0f)
            {
                decisionData = new DecisionData { Blackboard = blackboard, ChildDecisionData = childDecisionData };
            }

            return decisionValue;
        }

        /// <summary>
        ///   Per frame update.
        /// </summary>
        /// <param name="agentData"> Agent data. </param>
        /// <returns> Execution status after this update. </returns>
        public override ExecutionStatus Update(IAgentData agentData)
        {
            Data data = agentData.GetTaskData<Data>();

            // Setup blackboard.
            Setup(agentData, data);

            ExecutionStatus result = base.Update(agentData);
            if (result != ExecutionStatus.Running)
            {
                // Tear down.
                TearDown(agentData, data);
            }

            return result;
        }

        #endregion

        #region Methods

        /// <summary>
        ///   Sets up the agent data to use the blackboard of this decorator.
        /// </summary>
        /// <param name="agentData"> Agent data. </param>
        /// <param name="data"> Task data. </param>
        private static void Setup(IAgentData agentData, Data data)
        {
            agentData.Blackboard = data.Blackboard;
        }

        /// <summary>
        ///   Removes the blackboard from the agent data and uses the parent blackboard again.
        /// </summary>
        /// <param name="agentData"> Agent data. </param>
        /// <param name="data"> Task data. </param>
        private static void TearDown(IAgentData agentData, Data data)
        {
            agentData.Blackboard = data.PreviousBlackboard;
            agentData.CurrentTaskData = null;
        }

        #endregion

        /// <summary>
        ///   Task-specific data.
        /// </summary>
        private class Data : ITaskData
        {
            #region Public Properties

            /// <summary>
            ///   Created blackboard.
            /// </summary>
            public Blackboard Blackboard { get; set; }

            /// <summary>
            ///   Blackboard which was replaced by the created one.
            /// </summary>
            public Blackboard PreviousBlackboard { get; set; }

            #endregion
        }

        /// <summary>
        ///   Task-specific decision data.
        /// </summary>
        private class DecisionData : IDecisionData
        {
            #region Public Properties

            /// <summary>
            ///   Blackboard to use.
            /// </summary>
            public Blackboard Blackboard { get; set; }

            /// <summary>
            ///   Child decision data.
            /// </summary>
            public IDecisionData ChildDecisionData { get; set; }

            #endregion
        }
    }
}