// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ListExtensionMethods.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Collections.Extensions
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    using Slash.Collections.Utils;

    /// <summary>
    ///   Extension methods for lists.
    /// </summary>
    public static class ListExtensionMethods
    {
        #region Public Methods and Operators

        /// <summary>
        ///   Moves an item inside the list from an old to a new index.
        /// </summary>
        /// <param name="list"> List to work on. </param>
        /// <param name="oldIndex"> Old index of the item. </param>
        /// <param name="newIndex"> New index of the item. </param>
        public static void Move(this IList list, int oldIndex, int newIndex)
        {
            if (oldIndex < 0 || oldIndex >= list.Count)
            {
                throw new ArgumentOutOfRangeException(
                    "oldIndex", string.Format("Old index {0} out of range.", oldIndex));
            }

            if (newIndex < 0 || newIndex >= list.Count)
            {
                throw new ArgumentOutOfRangeException(
                    "newIndex", string.Format("New index {0} out of range.", newIndex));
            }

            if (oldIndex == newIndex)
            {
                return;
            }

            object item = list[oldIndex];

            list.RemoveAt(oldIndex);

            list.Insert(newIndex, item);
        }

        /// <summary>
        ///   Shuffle a list randomly.
        ///   From https://stackoverflow.com/a/4262134
        /// </summary>
        /// <param name="list">List to shuffle.</param>
        /// <param name="random">Random number generator.</param>
        public static void Shuffle(this IList list, Random random = null)
        {
            if (random == null)
            {
                random = new Random();
            }
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = random.Next(n + 1);
                object value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }

        /// <summary>
        ///   Shuffle a list randomly.
        ///   From https://stackoverflow.com/a/4262134
        /// </summary>
        /// <param name="list">List to shuffle.</param>
        /// <param name="random">Random number generator.</param>
        public static void Shuffle<T>(this IList<T> list, Random random = null)
        {
            if (random == null)
            {
                random = new Random();
            }
            var n = list.Count;
            while (n > 1)
            {
                n--;
                var k = random.Next(n + 1);
                var value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }

        /// <summary>
        ///   Enumerates the elements of the specified list in random order.
        /// </summary>
        /// <typeparam name="T">Type of list to enumerate the items of.</typeparam>
        /// <param name="list">List to enumerate the items of.</param>
        /// <returns>List with the same items in random order.</returns>
        public static List<T> ToRandomOrder<T>(this IList<T> list)
        {
            return ToRandomOrder(list, new Random());
        }

        /// <summary>
        ///   Enumerates the elements of the specified list in random order.
        /// </summary>
        /// <typeparam name="T">Type of list to enumerate the items of.</typeparam>
        /// <param name="list">List to enumerate the items of.</param>
        /// <param name="random">Random number generator to use.</param>
        /// <returns>List with the same items in random order.</returns>
        public static List<T> ToRandomOrder<T>(this IList<T> list, Random random)
        {
            List<T> copy = new List<T>(list);
            int count = copy.Count;

            while (count > 0)
            {
                int next = random.Next(count);
                CollectionUtils.Swap(copy, next, count - 1);
                count--;
            }

            return copy;
        }

        #endregion
    }
}