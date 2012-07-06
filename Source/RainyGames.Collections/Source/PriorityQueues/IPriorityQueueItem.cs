// -----------------------------------------------------------------------
// <copyright file="IPriorityQueueItem.cs" company="Rainy Games">
// Copyright (c) Rainy Games. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace RainyGames.Collections.PriorityQueues
{
    /// <summary>
    /// Container for an item that can be inserted into a priority queue.
    /// Provides a key for comparing it to other items for order.
    /// </summary>
    /// <typeparam name="T">Type of the item held by this container.</typeparam>
    public interface IPriorityQueueItem<T>
    {
        /// <summary>
        /// Item held by this container.
        /// </summary>
        T Item { get; }

        /// <summary>
        /// Key of the item which is used for comparing it to other
        /// items for order.
        /// </summary>
        double Key { get; }
    }
}
