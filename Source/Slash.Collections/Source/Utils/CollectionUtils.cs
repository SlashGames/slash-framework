// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CollectionUtils.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Collections.Utils
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public static class CollectionUtils
    {
        #region Public Methods and Operators

        /// <summary>
        ///   Checks whether the first sequence contains all elements of the second one.
        /// </summary>
        /// <typeparam name="T">Type of the sequence to check.</typeparam>
        /// <param name="first">Containing sequence.</param>
        /// <param name="second">Contained sequence.</param>
        /// <returns>
        ///   <c>true</c>, if the first sequence contains all elements of the second one, and
        ///   <c>false</c> otherwise.
        /// </returns>
        public static bool ContainsAll<T>(this IEnumerable<T> first, IEnumerable<T> second)
        {
            return second.All(first.Contains);
        }

        /// <summary>
        ///   Compares two dictionary for equality.
        /// </summary>
        /// <typeparam name="TKey">Type of dictionary keys.</typeparam>
        /// <typeparam name="TValue">Type of dictionary values.</typeparam>
        /// <param name="first">First dictionary.</param>
        /// <param name="second">Second dictionary.</param>
        /// <returns>True if the two dictionaries are equal; otherwise, false.</returns>
        public static bool DictionaryEqual<TKey, TValue>(
            IDictionary<TKey, TValue> first, IDictionary<TKey, TValue> second)
        {
            if (first == second)
            {
                return true;
            }
            if ((first == null) || (second == null))
            {
                return false;
            }
            if (first.Count != second.Count)
            {
                return false;
            }

            EqualityComparer<TValue> comparer = EqualityComparer<TValue>.Default;

            foreach (KeyValuePair<TKey, TValue> kvp in first)
            {
                TValue secondValue;
                if (!second.TryGetValue(kvp.Key, out secondValue))
                {
                    return false;
                }

                if (kvp.Value is IEnumerable && secondValue is IEnumerable)
                {
                    IEnumerable enumerable1 = (IEnumerable)kvp.Value;
                    IEnumerable enumerable2 = (IEnumerable)secondValue;
                    if (!SequenceEqual(enumerable1.Cast<object>(), enumerable2.Cast<object>()))
                    {
                        return false;
                    }
                }
                else
                {
                    if (!comparer.Equals(kvp.Value, secondValue))
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        /// <summary>
        ///   Returns a random item from the specified sequence.
        /// </summary>
        /// <typeparam name="T">Type of sequence items.</typeparam>
        /// <param name="sequence">Sequence to get item from.</param>
        /// <returns>Time-dependent random item.</returns>
        public static T RandomItem<T>(this IEnumerable<T> sequence)
        {
            Random random = new Random();

            T current = default(T);
            int count = 0;
            foreach (T element in sequence)
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
        ///   Returns a random weighted item from the specified sequence.
        ///   Uses the specified function to get the weight of an item.
        ///   Idea was taken from http://stackoverflow.com/questions/17912005/quick-way-of-selecting-a-random-item-from-a-list-with-varying-probabilities-ba
        /// </summary>
        /// <typeparam name="T">Type of sequence items.</typeparam>
        /// <param name="sequence">Sequence to get item from.</param>
        /// <param name="getWeight">Function to get weight of an item.</param>
        /// <param name="random">Random number generator.</param>
        /// <returns>Time-dependent random item.</returns>
        public static T RandomWeightedItem<T>(this ICollection<T> sequence, Func<T, float> getWeight, Random random)
        {
            float totalProbabilities = sequence.Sum(getWeight);
            float probabilityPick = (float)(random.NextDouble() * totalProbabilities);
            foreach (var item in sequence)
            {
                float weight = getWeight(item);
                if (weight < probabilityPick)
                {
                    return item;
                }

                probabilityPick -= weight;
            }

            throw new InvalidOperationException("Not supposed to reach this point, random picking went wrong.");
        }

        /// <summary>
        ///   Compares two sequences by comparing their items instead of their references. Also checks the case that
        ///   both sequences are null. In this case the method returns true.
        /// </summary>
        /// <typeparam name="T">Type of items inside sequences.</typeparam>
        /// <param name="sequence1">First sequence.</param>
        /// <param name="sequence2">Second sequence.</param>
        /// <returns>
        ///   True if the sequences are equal (i.e. the sequences contain equal items in the same order), else false.
        /// </returns>
        public static bool SequenceEqual<T>(IEnumerable<T> sequence1, IEnumerable<T> sequence2)
        {
            return sequence1 == sequence2
                   || sequence1 != null && sequence2 != null && sequence1.SequenceEqual(sequence2);
        }

        /// <summary>
        ///   Compares two sequences by comparing their items instead of their references. Also checks the case that
        ///   both sequences are null. In this case the method returns true.
        /// </summary>
        /// <typeparam name="T">Type of items inside sequences.</typeparam>
        /// <param name="sequence1">First sequence.</param>
        /// <param name="sequence2">Second sequence.</param>
        /// <param name="comparer">Comparer to check equality between two objects.</param>
        /// <returns>
        ///   True if the sequences are equal (i.e. the sequences contain equal items in the same order), else false.
        /// </returns>
        public static bool SequenceEqual<T>(
            IEnumerable<T> sequence1, IEnumerable<T> sequence2, IEqualityComparer<T> comparer)
        {
            return sequence1 == sequence2
                   || sequence1 != null && sequence2 != null && sequence1.SequenceEqual(sequence2, comparer);
        }

        /// <summary>
        ///   Swaps the elements with the specified indices in the passed list.
        /// </summary>
        /// <typeparam name="T">Type of the list to swap the items of.</typeparam>
        /// <param name="list">List to swap the items of.</param>
        /// <param name="first">Index of the first item to swap.</param>
        /// <param name="second">Index of the second item to swap.</param>
        public static void Swap<T>(IList<T> list, int first, int second)
        {
            T temp = list[first];
            list[first] = list[second];
            list[second] = temp;
        }

        /// <summary>
        ///   Enumerates the elements of the specified list in random order.
        /// </summary>
        /// <typeparam name="T">Type of list to enumerate the items of.</typeparam>
        /// <param name="list">List to enumerate the items of.</param>
        /// <returns>List with the same items in random order.</returns>
        public static List<T> ToRandomOrder<T>(IList<T> list)
        {
            List<T> copy = new List<T>(list);
            Random random = new Random();
            int count = copy.Count;

            while (count > 0)
            {
                int next = random.Next(count);
                Swap(copy, next, count - 1);
                count--;
            }

            return copy;
        }

        /// <summary>
        ///   Returns a comma-seperated list of the elements of the passed sequence.
        /// </summary>
        /// <typeparam name="T">Type of the elements of the sequence.</typeparam>
        /// <param name="sequence">Sequence to get a comma-seperated list of.</param>
        /// <returns>Comma-seperated list of the elements of the passed sequence.</returns>
        public static string ToString(IEnumerable sequence)
        {
            if (sequence == null)
            {
                return "null";
            }

            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.Append("[");

            foreach (var element in sequence)
            {
                string elementString;
                IEnumerable elementEnumerable = element as IEnumerable;
                if (elementEnumerable != null)
                {
                    elementString = ToString(elementEnumerable);
                }
                else
                {
                    elementString = element.ToString();
                }

                stringBuilder.AppendFormat("{0}, ", elementString);
            }

            if (stringBuilder.Length > 1)
            {
                stringBuilder[stringBuilder.Length - 2] = ']';
                return stringBuilder.ToString().Substring(0, stringBuilder.Length - 1);
            }

            return "[]";
        }

        #endregion
    }
}