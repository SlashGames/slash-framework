// -----------------------------------------------------------------------
// <copyright file="IPriorityQueue.cs" company="Slash Games">
// Copyright (c) Slash Games. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace SlashGames.Collections.PriorityQueues
{
    /// <summary>
    /// Ordered data structure that is optimized for finding the minimum item.
    /// </summary>
    /// <typeparam name="T">Type of items held by this queue.</typeparam>
    public interface IPriorityQueue<T>
    {
        /// <summary>
        /// Number of elements of this priority queue.
        /// </summary>
        int Count { get; }

        /// <summary>
        /// Checks whether this priority queue is empty, or not.
        /// </summary>
        /// <returns>
        /// <c>true</c>, if this priority queue is empty, and <c>false</c>
        /// otherwise.
        /// </returns>
        bool IsEmpty();

        /// <summary>
        /// Clears this priority queue, removing all items.
        /// </summary>
        void Clear();

        /// <summary>
        /// Inserts the passed item with the specified key into this priority queue.
        /// </summary>
        /// <param name="item">Item to insert.</param>
        /// <param name="key">Key of the item to insert.</param>
        /// <returns>Container that holds the passed item.</returns>
        IPriorityQueueItem<T> Insert(T item, double key);

        /// <summary>
        /// Returns the item with the minimum key in this priority queue.
        /// </summary>
        /// <returns>Item with the minimum key in this priority queue.</returns>
        /// <exception cref="InvalidOperationException">
        /// This priority queue is empty.
        /// </exception>
        IPriorityQueueItem<T> FindMin();

        /// <summary>
        /// Deletes the item with the minimum key in this priority queue and returns it.
        /// </summary>
        /// <returns>Item with the minimum key in this priority queue.</returns>
        /// <exception cref="InvalidOperationException">
        /// This priority queue is empty.
        /// </exception>
        IPriorityQueueItem<T> DeleteMin();

        /// <summary>
        /// Decreases the key of the specified item in this priority queue by subtracting
        /// the passed non-negative real number <c>delta</c>.
        /// </summary>
        /// <param name="item">Item to decrease the key of.</param>
        /// <param name="delta">Non-negative real number to be subtracted from the item's key.</param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <c>delta</c> is negative.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// This priority queue is empty.
        /// </exception>
        void DecreaseKey(IPriorityQueueItem<T> item, double delta);

        /// <summary>
        /// Decreases the key of the specified item in this priority queue to the passed
        /// non-negative real number.
        /// </summary>
        /// <param name="item">Item to decrease the key of.</param>
        /// <param name="newKey">Item's new key.</param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// The resulting key would be greater than the current one.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// This priority queue is empty.
        /// </exception>
        void DecreaseKeyTo(IPriorityQueueItem<T> item, double newKey);

        /// <summary>
        /// Deletes the specified item from this priority queue.
        /// </summary>
        /// <param name="item">Item to be deleted.</param>
        /// <exception cref="InvalidOperationException">
        /// This priority queue is empty.
        /// </exception>
        void Delete(IPriorityQueueItem<T> item);
    }
}
