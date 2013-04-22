namespace SlashGames.AI.BehaviorTrees.Data
{
    using SlashGames.AI.BehaviorTrees.Interfaces;

    public class TaskOutParameter<T>
    {
        /// <summary>
        ///   Key of blackboard attribute to store task parameter value to.
        /// </summary>
        public object BlackboardAttribute { get; set; }

        /// <summary>
        ///   Sets the task parameter value.
        /// </summary>
        /// <param name="agentData">Agent data to set task parameter to.</param>
        /// <param name="value">Value of task parameter.</param>
        public void SetValue(IAgentData agentData, T value)
        {
            agentData.Blackboard.SetValue(this.BlackboardAttribute, value);
        }
    }
}