namespace SlashGames.AI.BehaviorTrees.Attributes
{
    using System;

    /// <summary>
    ///   Attribute to flag a task class which description should be exported into the editor configuration. Stores some meta data about the task.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class TaskAttribute : Attribute
    {
        #region Public Properties

        /// <summary>
        ///   Detailed description.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        ///   Indicates if the task is a decorator and thus can have a child task.
        /// </summary>
        public bool IsDecorator { get; set; }

        /// <summary>
        ///   Readable name.
        /// </summary>
        public string Name { get; set; }

        #endregion
    }
}