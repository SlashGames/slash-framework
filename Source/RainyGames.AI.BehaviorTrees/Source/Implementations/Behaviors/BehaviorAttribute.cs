namespace RainyGames.AI.BehaviorTrees.Implementations.Behaviors
{
    using System;

    /// <summary>
    ///   Attribute to flag a property in a behavior to be a behavior parameter. Can take some meta data about the parameter like the name which is shown in the editor.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class BehaviorAttribute : Attribute
    {
        #region Public Properties

        /// <summary>
        ///   Readable name.
        /// </summary>
        public string Name { get; set; }

        #endregion
    }
}