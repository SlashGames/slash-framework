// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ListExtensionMethods.cs" company="Rainy Games">
//   Copyright (c) Rainy Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace RainyGames.Collections.Utils
{
    using System;
    using System.Collections.Generic;

    public static class ListExtensionMethods
    {
        #region Public Methods and Operators

        /// <summary>
        ///   Moves an item inside the list from an old to a new index.
        /// </summary>
        /// <typeparam name="T">Type of the items in the list.</typeparam>
        /// <param name="list">List to work on.</param>
        /// <param name="oldIndex">Old index of the item.</param>
        /// <param name="newIndex">New index of the item.</param>
        public static void Move<T>(this List<T> list, int oldIndex, int newIndex)
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

            T item = list[oldIndex];

            list.RemoveAt(oldIndex);

            // The actual index could have shifted due to the removal.
            if (newIndex > oldIndex)
            {
                --newIndex;
            }

            list.Insert(newIndex, item);
        }

        #endregion
    }
}