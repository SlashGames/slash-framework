// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DictionaryExtensionMethods.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Collections.Extensions
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    ///   Extension methods for dictionaries.
    /// </summary>
    public static class DictionaryExtensionMethods
    {
        #region Public Methods and Operators

        /// <summary>
        ///   Tries to get the value with the specified key from the
        ///   dictionary, and returns the passed default value if the key
        ///   could not be found.
        /// </summary>
        /// <param name="dictionary">
        ///   Dictionary to get the value from.
        /// </param>
        /// <param name="key">
        ///   Key of the value to get.
        /// </param>
        /// <param name="defaultValue">
        ///   Default value to return if the specified key could not be found.
        /// </param>
        /// <returns>
        ///   Value with the specified <paramref name="key" />, if found,
        ///   and <paramref name="defaultValue" /> otherwise.
        /// </returns>
        public static TValue GetValueOrDefault<TKey, TValue>(
            this IDictionary<TKey, TValue> dictionary, TKey key, TValue defaultValue)
        {
            TValue value;
            return dictionary.TryGetValue(key, out value) ? value : defaultValue;
        }

        /// <summary>
        ///   Tries to get the value with the specified key from the
        ///   dictionary, and uses the passed default value provider to return
        ///   a value if the key could not be found.
        /// </summary>
        /// <param name="dictionary">
        ///   Dictionary to get the value from.
        /// </param>
        /// <param name="key">
        ///   Key of the value to get.
        /// </param>
        /// <param name="defaultValueProvider">
        ///   Default value provider to use to return a value if the key could not be found.
        /// </param>
        /// <returns>
        ///   Value with the specified <paramref name="key" />, if found,
        ///   and a value provided by <paramref name="defaultValueProvider" /> otherwise.
        /// </returns>
        public static TValue GetValueOrDefault<TKey, TValue>(
            this IDictionary<TKey, TValue> dictionary, TKey key, Func<TValue> defaultValueProvider)
        {
            TValue value;
            return dictionary.TryGetValue(key, out value) ? value : defaultValueProvider();
        }

        #endregion
    }
}