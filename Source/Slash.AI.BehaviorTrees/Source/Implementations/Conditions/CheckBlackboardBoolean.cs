// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CheckBlackboardBoolean.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.AI.BehaviorTrees.Implementations.Conditions
{
    using System;
    using System.Xml.Serialization;

    using Slash.AI.BehaviorTrees.Interfaces;

    /// <summary>
    ///   Checks a boolean attribute from the blackboard.
    /// </summary>
    [Serializable]
    [XmlRoot(Namespace = "behaviorTree")]
    public abstract class CheckBlackboardBoolean : BooleanCondition
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
            return agentData.Blackboard.GetValue(this.BlackboardAttributeKey, false);
        }

        #endregion
    }
}