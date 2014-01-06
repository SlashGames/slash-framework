// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ListExtensionMethods.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Collections.Extensions
{
    using System;
    using System.Collections;

    /// <summary>
    ///   Extension methods for IList and IList{T}.
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

        #endregion
    }
}