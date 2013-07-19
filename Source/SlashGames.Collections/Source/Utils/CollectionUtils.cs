// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CollectionUtils.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Collections.Utils
{
    using System.Collections.Generic;
    using System.Linq;

    public static class CollectionUtils
    {
        #region Public Methods and Operators


        /// <summary>
        ///   Compares two sequences by comparing their items instead of their references. Also checks the case that
        ///   both sequences are null. In this case the method returns true.
        /// </summary>
        /// <typeparam name = "T">Type of items inside sequences.</typeparam>
        /// <param name = "sequence1">First sequence.</param>
        /// <param name = "sequence2">Second sequence.</param>
        /// <returns>
        ///   True if the sequences are equal (i.e. the sequences contain equal items in the same order), else false.
        /// </returns>
        public static bool SequenceEqual<T>(IEnumerable<T> sequence1, IEnumerable<T> sequence2)
        {
            return sequence1 == sequence2 ||
                   sequence1 != null && sequence2 != null && sequence1.SequenceEqual(sequence2);
        }

        /// <summary>
        ///   Compares two sequences by comparing their items instead of their references. Also checks the case that
        ///   both sequences are null. In this case the method returns true.
        /// </summary>
        /// <typeparam name = "T">Type of items inside sequences.</typeparam>
        /// <param name = "sequence1">First sequence.</param>
        /// <param name = "sequence2">Second sequence.</param>
        /// <param name = "comparer">Comparer to check equality between two objects.</param>
        /// <returns>
        ///   True if the sequences are equal (i.e. the sequences contain equal items in the same order), else false.
        /// </returns>
        public static bool SequenceEqual<T>(
            IEnumerable<T> sequence1, IEnumerable<T> sequence2, IEqualityComparer<T> comparer)
        {
            return sequence1 == sequence2 ||
                   sequence1 != null && sequence2 != null && sequence1.SequenceEqual(sequence2, comparer);
        }

        #endregion
    }
}