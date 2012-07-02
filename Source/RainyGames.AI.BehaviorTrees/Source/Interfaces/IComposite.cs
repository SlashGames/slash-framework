namespace RainyGames.AI.BehaviorTrees.Interfaces
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    ///   task which contains array of references to other deciders.
    /// </summary>
    public interface IComposite : ITask
    {
        #region Public Properties

        /// <summary>
        ///   Maximum number of children that the composite can take.
        /// </summary>
        int Capacity { get; }

        /// <summary>
        ///   Collection of children. Read-only.
        /// </summary>
        IList<ITask> Children { get; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///   Adds a child to this group task.
        /// </summary>
        /// <param name="child"> Child to add. </param>
        /// <exception cref="Exception">Thrown if child couldn't be added because capacity was reached.</exception>
        void AddChild(ITask child);

        /// <summary>
        ///   Inserts a child to this group task at the passed index.
        /// </summary>
        /// <param name="index"> Position to add child to. </param>
        /// <param name="child"> Child to insert. </param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if passed index isn't between 0 and Children.Count (inclusive).</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if passed index isn't between 0 and Capacity (exclusive).</exception>
        /// <exception cref="Exception">Thrown if child couldn't be inserted because capacity was reached.</exception>
        void InsertChild(int index, ITask child);

        /// <summary>
        ///   Moves a child to the passed position inside the group.
        /// </summary>
        /// <param name="oldIndex"> Old position of the child. </param>
        /// <param name="newIndex"> New position of the child. </param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if passed old index isn't between 0 and Children.Count (exclusive).</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if passed new index isn't between 0 and Children.Count (exclusive).</exception>
        void MoveChild(int oldIndex, int newIndex);

        /// <summary>
        ///   Removes a child from this group task.
        /// </summary>
        /// <param name="child"> Child to remove. </param>
        /// <returns> Indicates if the child was removed. </returns>
        bool RemoveChild(ITask child);

        #endregion
    }
}