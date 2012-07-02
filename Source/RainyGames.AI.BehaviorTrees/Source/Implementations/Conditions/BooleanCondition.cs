namespace RainyGames.AI.BehaviorTrees.Implementations.Conditions
{
    using RainyGames.AI.BehaviorTrees.Interfaces;

    /// <summary>
    ///   Base class of a boolean condition.
    /// </summary>
    public abstract class BooleanCondition : Condition
    {
        #region Public Methods and Operators

        /// <summary>
        ///   Depending on the group policy of its parent, the floating point return value indicates whether the decider will be activated.
        /// </summary>
        /// <param name="agentData"> Agent data. </param>
        /// <param name="decisionData"> Decision data to use in activate method. </param>
        /// <returns> Floating point value used to decide if the decider will be activated. </returns>
        public override float Decide(IAgentData agentData, ref IDecisionData decisionData)
        {
            return this.Decide(agentData) ? 1.0f : 0.0f;
        }

        #endregion

        #region Methods

        /// <summary>
        ///   Decision function for a boolean condition.
        /// </summary>
        /// <param name="agentData"> Agent data. </param>
        /// <returns> Returns true if the condition is forfilled, else false. </returns>
        protected abstract bool Decide(IAgentData agentData);

        #endregion
    }
}