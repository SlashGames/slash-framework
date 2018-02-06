// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EnumerableExtensionMethods.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Collections.Extensions
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    ///     Extension methods for enumerables.
    /// </summary>
    public static class EnumerableExtensionMethods
    {
        /// <summary>
        ///     Random number generator, used for RandomSelect methods.
        /// </summary>
        private static readonly Random Random = new Random();

        /// <summary>
        ///     Checks whether the first sequence contains all elements of the second one.
        /// </summary>
        /// <typeparam name="T">Type of the sequence to check.</typeparam>
        /// <param name="first">Containing sequence.</param>
        /// <param name="second">Contained sequence.</param>
        /// <returns>
        ///     <c>true</c>, if the first sequence contains all elements of the second one, and
        ///     <c>false</c> otherwise.
        /// </returns>
        public static bool ContainsAll<T>(this IEnumerable<T> first, IEnumerable<T> second)
        {
            return second.All(first.Contains);
        }

        ///<summary>Finds the index of the first item matching an expression in an enumerable.</summary>
        ///<param name="items">The enumerable to search.</param>
        ///<param name="predicate">The expression to test the items against.</param>
        ///<returns>The index of the first matching item, or -1 if no items match.</returns>
        public static int FindIndex<T>(this IEnumerable<T> items, Func<T, bool> predicate)
        {
            if (items == null)
            {
                throw new ArgumentNullException("items");
            }

            if (predicate == null)
            {
                throw new ArgumentNullException("predicate");
            }

            var retVal = 0;
            foreach (var item in items)
            {
                if (predicate(item))
                {
                    return retVal;
                }

                retVal++;
            }

            return -1;
        }

        ///<summary>Finds the index of the first occurrence of an item in an enumerable.</summary>
        ///<param name="items">The enumerable to search.</param>
        ///<param name="item">The item to find.</param>
        ///<returns>The index of the first matching item, or -1 if the item was not found.</returns>
        public static int IndexOf<T>(this IEnumerable<T> items, T item)
        {
            return items.FindIndex(i => EqualityComparer<T>.Default.Equals(item, i));
        }

        /// <summary>
        ///     Determines whether the collection is null or contains no elements.
        /// </summary>
        /// <typeparam name="T"> The IEnumerable type. </typeparam>
        /// <param name="enumerable"> The enumerable, which may be null or empty. </param>
        /// <returns>
        ///     <c>true</c> if the IEnumerable is null or empty; otherwise, <c>false</c> .
        /// </returns>
        public static bool IsNullOrEmpty<T>(this IEnumerable<T> enumerable)
        {
            if (enumerable == null)
            {
                return true;
            }

            return !enumerable.Any();
        }

        /// <summary>
        ///     Determines whether the collection is null or contains no elements.
        /// </summary>
        /// <param name="enumerable"> The enumerable, which may be null or empty. </param>
        /// <returns>
        ///     <c>true</c> if the IEnumerable is null or empty; otherwise, <c>false</c> .
        /// </returns>
        public static bool IsNullOrEmpty(this IEnumerable enumerable)
        {
            if (enumerable == null)
            {
                return true;
            }

            foreach (var item in enumerable)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        ///     Returns a random item from the specified sequence.
        /// </summary>
        /// <typeparam name="T">Type of sequence items.</typeparam>
        /// <param name="sequence">Sequence to get item from.</param>
        /// <returns>Time-dependent random item.</returns>
        public static T RandomItem<T>(this IEnumerable<T> sequence)
        {
            var random = new Random();

            var current = default(T);
            var count = 0;
            foreach (var element in sequence)
            {
                count++;
                if (random.Next(count) == 0)
                {
                    current = element;
                }
            }

            if (count == 0)
            {
                throw new InvalidOperationException("Sequence was empty");
            }

            return current;
        }

        /// <summary>
        ///     Returns a random item from the specified sequence or the default value of the
        ///     specified type if the sequence is null or empty.
        /// </summary>
        /// <typeparam name="T">Type of sequence items.</typeparam>
        /// <param name="sequence">Sequence to get item from.</param>
        /// <returns>Time-dependent random item.</returns>
        public static T RandomItemOrDefault<T>(this IEnumerable<T> sequence)
        {
            return sequence.RandomItemOrDefault(default(T));
        }

        /// <summary>
        ///     Returns a random item from the specified sequence or the specified default value if
        ///     the sequence is null or empty.
        /// </summary>
        /// <typeparam name="T">Type of sequence items.</typeparam>
        /// <param name="sequence">Sequence to get item from.</param>
        /// <param name="defaultValue">Default value to use if the sequence is null or empty.</param>
        /// <returns>Time-dependent random item.</returns>
        public static T RandomItemOrDefault<T>(this IEnumerable<T> sequence, T defaultValue)
        {
            if (sequence == null || !sequence.Any())
            {
                return defaultValue;
            }

            return sequence.RandomItem();
        }

        /// <summary>
        ///     Executes the specified action for the specified number of random items from the specified enumerable.
        /// </summary>
        /// <typeparam name="T"> Type of items in enumerable. </typeparam>
        /// <param name="enumerable"> Enumerable to get random items from. </param>
        /// <param name="numberOfSelections"> Number of items to get from the enumerable. </param>
        /// <param name="action"> Action to execute for selected items. </param>
        /// <exception cref="ArgumentNullException">Enumerable is null.</exception>
        public static void RandomSelect<T>(this IEnumerable<T> enumerable, int numberOfSelections, Action<T> action)
        {
            if (enumerable == null)
            {
                throw new ArgumentNullException("enumerable", "Enumerable is null.");
            }

            if (numberOfSelections <= 0)
            {
                return;
            }

            // Check if the enumerable has less items than the number which should be selected.
            IEnumerable<T> items = enumerable as T[] ?? enumerable.ToArray();
            var remainingItems = items.Count();
            if (remainingItems <= numberOfSelections)
            {
                foreach (var item in items)
                {
                    action(item);
                }
            }
            else
            {
                // Random selection of items.
                var remainingSelections = numberOfSelections;
                foreach (var item in items)
                {
                    var randomInt = Random.Next(remainingItems);
                    if (randomInt < remainingSelections)
                    {
                        action(item);
                        --remainingSelections;
                    }

                    --remainingItems;
                }
            }
        }

        /// <summary>
        ///     Selects the specified number of random items from the specified enumerable.
        ///     If the number of items in the enumerable is smaller than the specified number of selections,
        ///     the enumerable itself is returned.
        /// </summary>
        /// <typeparam name="T"> Type of items in enumerable. </typeparam>
        /// <param name="enumerable"> Enumerable to get random items from. </param>
        /// <param name="numberOfSelections"> Number of items to get from the enumerable. </param>
        /// <exception cref="ArgumentNullException">Enumerable is null.</exception>
        /// <returns> Random items from specified enumerable. </returns>
        public static IEnumerable<T> RandomSelect<T>(this IEnumerable<T> enumerable, int numberOfSelections)
        {
            if (enumerable == null)
            {
                throw new ArgumentNullException("enumerable", "Enumerable is null.");
            }

            if (numberOfSelections <= 0)
            {
                return null;
            }

            // Check if the enumerable has less items than the number which should be selected.
            IEnumerable<T> items = enumerable as T[] ?? enumerable.ToArray();
            var remainingItems = items.Count();
            if (remainingItems <= numberOfSelections)
            {
                return items;
            }

            // Random selection of items.
            var remainingSelections = numberOfSelections;
            IList<T> selectedItems = new List<T>(remainingSelections);
            foreach (var item in items)
            {
                var randomInt = Random.Next(remainingItems);
                if (randomInt < remainingSelections)
                {
                    selectedItems.Add(item);
                    --remainingSelections;
                }

                --remainingItems;
            }

            return selectedItems;
        }

        /// <summary>
        ///     Selects a random item from the specified enumerable.
        /// </summary>
        /// <typeparam name="T"> Type of items in enumerable. </typeparam>
        /// <param name="enumerable"> Enumerable to get random item from. </param>
        /// <exception cref="ArgumentNullException">Enumerable is null.</exception>
        /// <returns> Random item from specified enumerable. </returns>
        public static T RandomSelect<T>(this IEnumerable<T> enumerable)
        {
            if (enumerable == null)
            {
                throw new ArgumentNullException("enumerable", "Enumerable is null.");
            }

            IEnumerable<T> items = enumerable as T[] ?? enumerable.ToArray();
            var remainingItems = items.Count();
            if (remainingItems == 0)
            {
                return default(T);
            }

            foreach (var item in items)
            {
                var randomInt = Random.Next(remainingItems);
                if (randomInt == 0)
                {
                    return item;
                }

                --remainingItems;
            }

            // NOTE(co): The method should never get here.
            throw new Exception("Algorithm didn't work correctly.");
        }

        /// <summary>
        ///     Returns a random weighted item from the specified sequence.
        ///     Uses the specified function to get the weight of an item.
        ///     Idea was taken from
        ///     http://stackoverflow.com/questions/17912005/quick-way-of-selecting-a-random-item-from-a-list-with-varying-probabilities-ba
        /// </summary>
        /// <typeparam name="T">Type of sequence items.</typeparam>
        /// <param name="sequence">Sequence to get item from.</param>
        /// <param name="getWeight">Function to get weight of an item.</param>
        /// <param name="random">Random number generator.</param>
        /// <returns>Time-dependent random item.</returns>
        public static T RandomWeightedItem<T>(this IEnumerable<T> sequence, Func<T, float> getWeight, Random random)
        {
            var totalProbabilities = sequence.Sum(getWeight);
            var probabilityPick = (float) (random.NextDouble() * totalProbabilities);
            foreach (var item in sequence)
            {
                var weight = getWeight(item);
                if (weight < probabilityPick)
                {
                    return item;
                }

                probabilityPick -= weight;
            }

            throw new InvalidOperationException("Not supposed to reach this point, random picking went wrong.");
        }
    }
}