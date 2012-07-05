// -----------------------------------------------------------------------
// <copyright file="IAttributeTable.cs" company="Rainy Games">
// Copyright (c) Rainy Games. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace RainyGames.Collections.AttributeTables
{
    /// <summary>
    /// Table that allows storing and looking up attributes and their
    /// respective values.
    /// </summary>
    public interface IAttributeTable
    {
        /// <summary>
        /// Maps the passed key to the specified value in this attribute table,
        /// if it has already been mapped before.
        /// </summary>
        /// <param name="key">Key to map.</param>
        /// <param name="value">Value to map the key to.</param>
        void SetValue(object key, object value);

        /// <summary>
        /// Removes the passed key from this attribute table.
        /// </summary>
        /// <param name="key">Key to remove.</param>
        void Remove(object key);

        /// <summary>
        /// Maps the passed key to the specified value in this attribute table,
        /// if it hasn't already been mapped before.
        /// </summary>
        /// <param name="key">Key to map.</param>
        /// <param name="value">Value to map the key to.</param>
        void Add(object key, object value);

        /// <summary>
        /// Returns <c>true</c> if the passed key is mapped within this
        /// attribute table, and <c>false</c> otherwise.
        /// </summary>
        /// <param name="key">Key to check.</param>
        /// <returns>True if the passed key is mapped within this attribute table.</returns>
        bool ContainsKey(object key);

        /// <summary>
        /// Tries to retrieve the value the passed key is mapped to within this
        /// attribute table.
        /// </summary>
        /// <param name="key">Key to retrieve the value of.</param>
        /// <param name="value">Retrieved value.</param>
        /// <returns>true if a value was found, and false otherwise.</returns>
        bool TryGetValue(object key, out object value);

        /// <summary>
        /// Tries to retrieve the value the passed key is mapped to within this
        /// attribute table.
        /// </summary>
        /// <typeparam name="T">Type of the value to retrieve.</typeparam>
        /// <param name="key">Key to retrieve the value of.</param>
        /// <param name="value">Retrieved value.</param>
        /// <returns>true if a value was found, and false otherwise.</returns>
        bool TryGetValue<T>(object key, out T value);
    }
}
