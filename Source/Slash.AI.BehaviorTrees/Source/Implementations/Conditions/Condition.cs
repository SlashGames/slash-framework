namespace Slash.AI.BehaviorTrees.Implementations.Conditions
{
    using Slash.AI.BehaviorTrees.Enums;
    using Slash.AI.BehaviorTrees.Interfaces;

    /// <summary>
    ///   Base class for a condition.
    /// </summary>
    public abstract class Condition : Task
    {
        #region Public Methods and Operators

        /// <summary>
        ///   Per frame update.
        /// </summary>
        /// <param name="agentData"> Agent data. </param>
        /// <returns> Execution status after this update. </returns>
        public override ExecutionStatus Update(IAgentData agentData)
        {
            // Conditions are always successful after they decided to run.
            return ExecutionStatus.Success;
        }

        #endregion
    }
}