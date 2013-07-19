namespace Slash.AI.BehaviorTrees.Implementations
{
    using System;
    using System.Xml.Serialization;

    using Slash.AI.BehaviorTrees.Data;
    using Slash.AI.BehaviorTrees.Enums;
    using Slash.AI.BehaviorTrees.Interfaces;

    /// <summary>
    ///   References a sub tree to reuse trees.
    /// </summary>
    [Serializable]
    public class SubTreeReference : Task
    {
        #region Public Properties

        /// <summary>
        ///   Initial blackboard to use when entering the sub tree. May be null.
        /// </summary>
        public Blackboard Blackboard { get; set; }

        /// <summary>
        ///   Sub tree this decider references.
        /// </summary>
        [XmlIgnore]
        public ITask SubTree { get; set; }

        /// <summary>
        ///   Name of sub tree this decider references.
        /// </summary>
        [XmlElement("SubTree")]
        public string SubTreeName { get; set; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///   The ==.
        /// </summary>
        /// <param name="left"> The left. </param>
        /// <param name="right"> The right. </param>
        /// <returns> </returns>
        public static bool operator ==(SubTreeReference left, SubTreeReference right)
        {
            return Equals(left, right);
        }

        /// <summary>
        ///   The !=.
        /// </summary>
        /// <param name="left"> The left. </param>
        /// <param name="right"> The right. </param>
        /// <returns> </returns>
        public static bool operator !=(SubTreeReference left, SubTreeReference right)
        {
            return !Equals(left, right);
        }

        /// <summary>
        ///   Activation. This method is called when the task was chosen to be executed. It's called right before the first update of the task. The task can setup its specific task data in here and do initial actions.
        /// </summary>
        /// <param name="agentData"> Agent data. </param>
        /// <param name="decisionData"> Decision data to use in activate method. </param>
        /// <returns> The SlashGames.AI.BehaviorTrees.Enums.ExecutionStatus. </returns>
        public override ExecutionStatus Activate(IAgentData agentData, IDecisionData decisionData)
        {
            return this.SubTree.Activate(agentData, decisionData);
        }

        /// <summary>
        ///   Deactivation.
        /// </summary>
        /// <param name="agentData"> Agent data. </param>
        public override void Deactivate(IAgentData agentData)
        {
            this.SubTree.Deactivate(agentData);
        }

        /// <summary>
        ///   Depending on the group policy of its parent, the floating point return value indicates whether the decider will be activated.
        /// </summary>
        /// <param name="agentData"> Agent data. </param>
        /// <param name="decisionData"> Decision data to use in activate method. </param>
        /// <returns> Floating point value used to decide if the decider will be activated. </returns>
        public override float Decide(IAgentData agentData, ref IDecisionData decisionData)
        {
            return this.SubTree != null ? this.SubTree.Decide(agentData, ref decisionData) : 0.0f;
        }

        /// <summary>
        ///   The equals.
        /// </summary>
        /// <param name="other"> The other. </param>
        /// <returns> The System.Boolean. </returns>
        public bool Equals(SubTreeReference other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return base.Equals(other) && Equals(other.SubTreeName, this.SubTreeName);
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

            return this.Equals(obj as SubTreeReference);
        }

        /// <summary>
        ///   The get hash code.
        /// </summary>
        /// <returns> The System.Int32. </returns>
        public override int GetHashCode()
        {
            unchecked
            {
                return (base.GetHashCode() * 397) ^ (this.SubTreeName != null ? this.SubTreeName.GetHashCode() : 0);
            }
        }

        /// <summary>
        ///   Per frame update.
        /// </summary>
        /// <param name="agentData"> Agent data. </param>
        /// <returns> Execution status after this update. </returns>
        public override ExecutionStatus Update(IAgentData agentData)
        {
            return this.SubTree.Update(agentData);
        }

        #endregion
    }
}