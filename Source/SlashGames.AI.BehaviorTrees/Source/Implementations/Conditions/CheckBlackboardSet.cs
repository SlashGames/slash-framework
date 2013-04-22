namespace SlashGames.AI.BehaviorTrees.Implementations.Conditions
{
    using System;
    using System.Xml.Serialization;

    using SlashGames.AI.BehaviorTrees.Interfaces;

    /// <summary>
    ///   Checks a if a blackboard attribute is set.
    /// </summary>
    [Serializable]
    [XmlRoot(Namespace = "behaviorTree")]
    public abstract class CheckBlackboardSet : BooleanCondition
    {
        #region Properties

        /// <summary>
        ///   Key of blackboard attribute to check.
        /// </summary>
        protected abstract object BlackboardAttributeKey { get; }

        #endregion

        #region Methods

        /// <summary>
        ///   Decision function for a boolean condition.
        /// </summary>
        /// <param name="agentData"> Agent data. </param>
        /// <returns> Returns true if the condition is forfilled, else false. </returns>
        protected override bool Decide(IAgentData agentData)
        {
            object attribute = null;
            if (!agentData.Blackboard.TryGetValue(this.BlackboardAttributeKey, out attribute))
            {
                return false;
            }

            // Check that not null.
            return attribute != null;
        }

        #endregion
    }
}