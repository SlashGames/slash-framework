namespace SlashGames.AI.BehaviorTrees.Interfaces
{
    using System.Collections.Generic;

    using SlashGames.AI.BehaviorTrees.Tree;

    /// <summary>
    ///   Interface of a behavior tree. For more information about behavior trees, see https://wiki.ticking-bomb-games.de/display/SEA/Behavior+Tree.
    /// </summary>
    public interface IBehaviorTree
    {
        #region Public Properties

        /// <summary>
        ///   Root node of the behavior tree.
        /// </summary>
        ITask Root { get; set; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///   Deactivation.
        /// </summary>
        /// <param name="agentData"> Agent data. </param>
        void Deactivate(IAgentData agentData);

        /// <summary>
        ///   Generates a collection of active task nodes under this task. Used for debugging only.
        /// </summary>
        /// <param name="agentData"> Agent data. </param>
        /// <returns> Collection of active task nodes. </returns>
        ICollection<TaskNode> GetActiveTasks(IAgentData agentData);

        /// <summary>
        ///   Per frame data.
        /// </summary>
        /// <param name="agentData"> Agent data. </param>
        void Update(IAgentData agentData);

        #endregion
    }
}