namespace SlashGames.AI.BehaviorTrees.Implementations.Actions
{
    using SlashGames.AI.BehaviorTrees.Data;
    using SlashGames.AI.BehaviorTrees.Enums;
    using SlashGames.AI.BehaviorTrees.Interfaces;

    /// <summary>
    ///   Reads an attribute from the parent blackboard and writes it to the current blackboard.
    /// </summary>
    public abstract class BaseCopyParentBlackboardAttribute : Task
    {
        #region Properties

        /// <summary>
        ///   Key of blackboard attribute to take the value from.
        /// </summary>
        protected abstract object AttributeKey { get; }

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
            Blackboard blackboard = agentData.Blackboard;
            if (blackboard.Parents == null)
            {
                return ExecutionStatus.Failed;
            }

            // Find attribute on parent blackboard.
            object attribute = null;
            foreach (Blackboard parent in blackboard.Parents)
            {
                if (parent.TryGetValue(this.AttributeKey, out attribute))
                {
                    // Set attribute on blackboard.
                    blackboard.SetValue(this.AttributeKey, attribute);

                    return ExecutionStatus.Success;
                }
            }

            return ExecutionStatus.Failed;
        }

        #endregion
    }
}