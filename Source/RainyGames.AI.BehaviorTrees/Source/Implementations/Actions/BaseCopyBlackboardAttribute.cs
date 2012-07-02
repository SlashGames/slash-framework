namespace RainyGames.AI.BehaviorTrees.Implementations.Actions
{
    using RainyGames.AI.BehaviorTrees.Enums;
    using RainyGames.AI.BehaviorTrees.Interfaces;

    /// <summary>
    ///   Task to copy the value of a blackboard attribute into another attribute.
    /// </summary>
    public abstract class BaseCopyBlackboardAttribute : Task
    {
        #region Properties

        /// <summary>
        ///   Key of blackboard attribute to take the value from.
        /// </summary>
        protected abstract object SourceAttributeKey { get; }

        /// <summary>
        ///   Key of blackboard attribute to copy the value to.
        /// </summary>
        protected abstract object TargetAttributeKey { get; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///   The equals.
        /// </summary>
        /// <param name="other"> The other. </param>
        /// <returns> The System.Boolean. </returns>
        public bool Equals(BaseCopyBlackboardAttribute other)
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

            return this.Equals(obj as BaseCopyBlackboardAttribute);
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
            // Get value from blackboard.
            object attribute = null;
            if (!agentData.Blackboard.TryGetValue(this.SourceAttributeKey, out attribute))
            {
                return ExecutionStatus.Failed;
            }

            // Set value to target key.
            agentData.Blackboard.SetValue(this.TargetAttributeKey, attribute);

            return ExecutionStatus.Success;
        }

        #endregion
    }
}