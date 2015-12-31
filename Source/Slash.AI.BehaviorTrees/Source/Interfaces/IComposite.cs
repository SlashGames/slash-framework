// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IComposite.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.AI.BehaviorTrees.Interfaces
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    ///   Delegate for IComposite.ChildAdded event.
    /// </summary>
    /// <param name="composite">Composite a child was added to.</param>
    /// <param name="childTask">Child that was added to composite.</param>
    public delegate void CompositeChildAddedDelegate(IComposite composite, ITask childTask);

    /// <summary>
    ///   Delegate for IComposite.ChildRemoved event.
    /// </summary>
    /// <param name="composite">Composite a child was removed from.</param>
    /// <param name="childTask">Child that was removed from composite.</param>
    public delegate void CompositeChildRemovedDelegate(IComposite composite, ITask childTask);

    /// <summary>
    ///   task which contains array of references to other deciders.
    /// </summary>
    public interface IComposite : ITask
    {
        #region Events

        /// <summary>
        ///   Called when a child was added to the composite.
        /// </summary>
        event CompositeChildAddedDelegate ChildAdded;

        /// <summary>
        ///   Called when a child was removed from the composite.
        /// </summary>
        event CompositeChildRemovedDelegate ChildRemoved;

        #endregion

        #region Properties

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
        /// <exception cref="ArgumentOutOfRangeException">
        ///   Thrown if passed old index isn't between 0 and Children.Count
        ///   (exclusive).
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        ///   Thrown if passed new index isn't between 0 and Children.Count
        ///   (exclusive).
        /// </exception>
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