namespace RainyGames.AI.BehaviorTrees.Implementations.Actions
{
    using RainyGames.AI.BehaviorTrees.Enums;
    using RainyGames.AI.BehaviorTrees.Interfaces;

    /// <summary>
    ///   Removes a blackboard attribute. Runtime: Instant. Fails: Never.
    /// </summary>
    public abstract class BaseRemoveBlackboardAttribute : Task
    {
        #region Public Methods and Operators

        /// <summary>
        ///   The equals.
        /// </summary>
        /// <param name="other"> The other. </param>
        /// <returns> The System.Boolean. </returns>
        public bool Equals(BaseRemoveBlackboardAttribute other)
        {
            return base.Equals(other);
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

            return this.Equals(obj as BaseRemoveBlackboardAttribute);
        }

        /// <summary>
        ///   The get hash code.
        /// </summary>
        /// <returns> The System.Int32. </returns>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        /// <summary>
        ///   Per frame update.
        /// </summary>
        /// <param name="agentData"> Agent data. </param>
        /// <returns> Execution status after this update. </returns>
        public override ExecutionStatus Update(IAgentData agentData)
        {
            // Get key of attribute to remove.
            object key = this.GetBlackboardAttributeKey(agentData);
            if (key != null)
            {
                // Remove attribute.
                agentData.Blackboard.RemoveValue(key);
            }

            return ExecutionStatus.Success;
        }

        #endregion

        #region Methods

        /// <summary>
        ///   Should return the key of the blackboard attribute to remove.
        /// </summary>
        /// <param name="agentData"> Agent data. </param>
        /// <returns> Key of the blackboard attribute to remove. </returns>
        protected abstract object GetBlackboardAttributeKey(IAgentData agentData);

        #endregion
    }
}