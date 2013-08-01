// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IAttributeTable.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Collections.AttributeTables
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    ///   Table that allows storing and looking up attributes and their
    ///   respective values.
    /// </summary>
    public interface IAttributeTable
    {
        #region Public Indexers

        /// <summary>
        ///   Returns or sets the attribute with the specified key.
        ///   If no attribute is found when it should be returned, an exception is thrown.
        /// </summary>
        /// <param name="attributeKey"> Attribute key. </param>
        /// <returns> Value of attribute with the specified key. </returns>
        object this[object attributeKey] { get; set; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///   Maps the passed key to the specified value in this attribute table,
        ///   if it hasn't already been mapped before.
        /// </summary>
        /// <param name="key"> Key to map. </param>
        /// <param name="value"> Value to map the key to. </param>
        /// <exception cref="ArgumentException">An element with the same key already exists in the attribute table.</exception>
        void Add(object key, object value);

        /// <summary>
        ///   Maps the passed key to the specified value in this attribute table,
        ///   if it hasn't already been mapped before.
        /// </summary>
        /// <param name="key"> Key to map. </param>
        /// <param name="value"> Value to map the key to. </param>
        /// <exception cref="ArgumentException">An element with the same key already exists in the attribute table.</exception>
        void AddValue(object key, object value);

        /// <summary>
        ///   Returns <c>true</c> if the passed key is mapped within this
        ///   attribute table, and <c>false</c> otherwise.
        /// </summary>
        /// <param name="key"> Key to check. </param>
        /// <returns> True if the passed key is mapped within this attribute table. </returns>
        bool Contains(object key);

        /// <summary>
        ///   Returns the attribute with the specified key casted to the specified type.
        ///   If no attribute with the specified key is found or the attribute can't be casted
        ///   to the specified type, an exception is thrown.
        /// </summary>
        /// <typeparam name="T"> Expected type of attribute. </typeparam>
        /// <param name="attributeKey"> Attribute key. </param>
        /// <returns> Value of attribute of the specified type with the specified key. </returns>
        /// <exception cref="KeyNotFoundException">Specified key wasn't found.</exception>
        /// <exception cref="InvalidCastException">Attribute was found but couldn't be casted to specified type.</exception>
        T GetValue<T>(object attributeKey);

        /// <summary>
        ///   Returns the attribute with the specified key.
        ///   If no attribute is found, an exception is thrown.
        /// </summary>
        /// <param name="attributeKey"> Attribute key. </param>
        /// <returns> Value of attribute with the specified key. </returns>
        /// <exception cref="KeyNotFoundException">Specified key wasn't found.</exception>
        object GetValue(object attributeKey);

        /// <summary>
        ///   Removes the passed key from this attribute table.
        /// </summary>
        /// <param name="key"> Key to remove. </param>
        void RemoveValue(object key);

        /// <summary>
        ///   Puts the attribute with the specified key and value into the attribute table.
        ///   If already existing in attribute table, the current value is overwritten.
        /// </summary>
        /// <param name="attributeKey"> Attribute key. </param>
        /// <param name="attributeValue"> Attribute value. </param>
        void SetValue(object attributeKey, object attributeValue);

        /// <summary>
        ///   Tries to retrieve the value the passed key is mapped to within this
        ///   attribute table.
        /// </summary>
        /// <param name="key"> Key to retrieve the value of. </param>
        /// <param name="value"> Retrieved value. </param>
        /// <returns> true if a value was found, and false otherwise. </returns>
        bool TryGetValue(object key, out object value);

        /// <summary>
        ///   Tries to retrieve the value the passed key is mapped to within this
        ///   attribute table.
        /// </summary>
        /// <typeparam name="T"> Type of the value to retrieve. </typeparam>
        /// <param name="key"> Key to retrieve the value of. </param>
        /// <param name="value"> Retrieved value. </param>
        /// <returns> true if a value was found, and false otherwise. </returns>
        bool TryGetValue<T>(object key, out T value);

        #endregion
    }
}