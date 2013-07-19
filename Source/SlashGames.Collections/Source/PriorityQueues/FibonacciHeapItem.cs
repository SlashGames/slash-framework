// -----------------------------------------------------------------------
// <copyright file="FibonacciHeapItem.cs" company="Slash Games">
// Copyright (c) Slash Games. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Slash.Collections.PriorityQueues
{
    /// <summary>
    /// Container for an item that can be inserted into a Fibonacci heap.
    /// Provides a pointer to its heap position and a key for comparing it to
    /// other heap items for order.
    /// </summary>
    /// <typeparam name="T">Type of the item held by this container.</typeparam>
    public class FibonacciHeapItem<T> : IPriorityQueueItem<T>
    {
        #region Constructors

        /// <summary>
        /// Constructs a new container for the passed item that can be inserted
        /// into a Fibonacci heap with the specified key for comparing it to
        /// other heap items for order.
        /// </summary>
        /// <param name="item">Item held by the new container.</param>
        /// <param name="key">
        /// Key of the item which is used for comparing it to other
        /// heap items for order.
        /// </param>
        protected internal FibonacciHeapItem(T item, double key)
        {
            this.Item = item;
            this.Key = key;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Item held by this container.
        /// </summary>
        public T Item { get; private set; }

        /// <summary>
        /// Key of the item which is used for comparing it to other
        /// heap items for order.
        /// </summary>
        public double Key { get; internal set; }

        /// <summary>
        /// Heap node which contains the item.
        /// </summary>
        internal FibonacciHeapNode<T> ContainingNode { get; set; }

        #endregion
    }
}
