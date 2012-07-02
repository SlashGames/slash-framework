namespace RainyGames.AI.BehaviorTrees.Implementations.Decorators
{
    using System;

    using RainyGames.AI.BehaviorTrees.Attributes;
    using RainyGames.AI.BehaviorTrees.Interfaces;

    /// <summary>
    ///   Inverts the result of the decorated task.
    /// </summary>
    [Serializable]
    [Task(Name = "Not", IsDecorator = true)]
    public class NotDecorator : Decorator
    {
        #region Public Methods and Operators

        /// <summary>
        ///   Depending on the group policy of its parent, the floating point return value indicates whether the task will be activated.
        /// </summary>
        /// <param name="agentData"> Agent data. </param>
        /// <param name="decisionData"> Decision data to use in activate method. </param>
        /// <returns> Floating point value used to decide if the task will be activated. </returns>
        public override float Decide(IAgentData agentData, ref IDecisionData decisionData)
        {
            // Inverts the result to run of the decorated task.
            return 1.0f - this.Task.Decide(agentData, ref decisionData);
        }

        #endregion
    }
}