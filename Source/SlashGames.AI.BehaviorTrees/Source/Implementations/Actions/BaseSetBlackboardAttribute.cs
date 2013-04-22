namespace SlashGames.AI.BehaviorTrees.Implementations.Actions
{
    using SlashGames.AI.BehaviorTrees.Enums;
    using SlashGames.AI.BehaviorTrees.Interfaces;

    /// <summary>
    ///   Sets a blackboard attribute.
    /// </summary>
    public abstract class BaseSetBlackboardAttribute : Task
    {
        #region Properties

        /// <summary>
        ///   Key of blackboard attribute to set.
        /// </summary>
        protected abstract object BlackboardAttributeKey { get; }

        /// <summary>
        ///   Value to set.
        /// </summary>
        protected abstract object BlackboardAttributeValue { get; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///   The equals.
        /// </summary>
        /// <param name="other"> The other. </param>
        /// <returns> The System.Boolean. </returns>
        public bool Equals(BaseSetBlackboardAttribute other)
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

            return this.Equals(obj as BaseSetBlackboardAttribute);
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
            // Set value to attribute.
            agentData.Blackboard.SetValue(this.BlackboardAttributeKey, this.BlackboardAttributeValue);
            return ExecutionStatus.Success;
        }

        #endregion
    }
}