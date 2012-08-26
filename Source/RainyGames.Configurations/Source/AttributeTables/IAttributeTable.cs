// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IAttributeTable.cs" company="Rainy Games">
//   Copyright (c) Rainy Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace RainyGames.Configurations.AttributeTables
{
    using System;

    /// <summary>
    ///   Interface for an attribute table which contains key/value pairs. An attribute table is used
    ///   to configure an object in the most generic way.
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
        ///   Puts the attribute with the specified key and value into the attribute table.
        ///   If already existing in attribute table, an exception is thrown.
        /// </summary>
        /// <param name="attributeKey"> Attribute key. </param>
        /// <param name="attributeValue"> Attribute value. </param>
        /// <exception cref="ArgumentException">An element with the same key already exists in the attribute table.</exception>
        void AddValue(object attributeKey, object attributeValue);

        /// <summary>
        ///   Indicates if the attribute table contains a value with the specified key.
        /// </summary>
        /// <param name="attributeKey"> Attribute key. </param>
        /// <returns> True if an attribute with the specified key is stored in the attribute table; otherwise, false. </returns>
        bool Contains(object attributeKey);

        /// <summary>
        ///   Returns the attribute with the specified key casted to the specified type.
        ///   If no attribute with the specified key is found or the attribute can't be casted
        ///   to the specified type, an exception is thrown.
        /// </summary>
        /// <typeparam name="T"> Expected type of attribute. </typeparam>
        /// <param name="attributeKey"> Attribute key. </param>
        /// <returns> Value of attribute of the specified type with the specified key. </returns>
        T GetValue<T>(object attributeKey);

        /// <summary>
        ///   Returns the attribute with the specified key.
        ///   If no attribute is found, an exception is thrown.
        /// </summary>
        /// <param name="attributeKey"> Attribute key. </param>
        /// <returns> Value of attribute with the specified key. </returns>
        object GetValue(object attributeKey);

        /// <summary>
        ///   Puts the attribute with the specified key and value into the attribute table.
        ///   If already existing in attribute table, the current value is overwritten.
        /// </summary>
        /// <param name="attributeKey"> Attribute key. </param>
        /// <param name="attributeValue"> Attribute value. </param>
        void SetValue(object attributeKey, object attributeValue);

        /// <summary>
        ///   Tries to return the attribute with the specified key casted to the specified type.
        ///   If no attribute with the specified key is found or the attribute can't be casted
        ///   to the specified type, false is returned.
        /// </summary>
        /// <typeparam name="T"> Expected type of attribute. </typeparam>
        /// <param name="attributeKey"> Attribute key. </param>
        /// <param name="attributeValue"> Value of attribute of the specified type with the specified key. Contains the default value of the specified type if attribute wasn't found. </param>
        /// <returns> True if attribute was found; otherwise, false. </returns>
        bool TryGetValue<T>(object attributeKey, out T attributeValue);

        /// <summary>
        ///   Tries to return the attribute with the specified key.
        ///   If no attribute with the specified key is found, false is returned.
        /// </summary>
        /// <param name="attributeKey"> Attribute key. </param>
        /// <param name="attributeValue"> Value of attribute with the specified key. Contains null if attribute wasn't found. </param>
        /// <returns> True if attribute was found; otherwise, false. </returns>
        bool TryGetValue(object attributeKey, out object attributeValue);

        #endregion
    }
}