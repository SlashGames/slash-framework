// -----------------------------------------------------------------------
// <copyright file="AttributeTable.cs" company="Rainy Games">
// Copyright (c) Rainy Games. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace RainyGames.Collections.AttributeTables
{
    using System.Collections.Generic;

    /// <summary>
    /// Table that allows storing and looking up attributes and their
    /// respective values.
    /// </summary>
    public class AttributeTable : IAttributeTable
    {
        #region Constants and Fields

        /// <summary>
        ///   Dictionary to store attributes.
        /// </summary>
        private readonly Dictionary<object, object> attributes;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Constructs a new, empty attribute table.
        /// </summary>
        public AttributeTable()
        {
            this.attributes = new Dictionary<object, object>();
        }

        /// <summary>
        /// Constructs a new attribute table, copying all content of the passed
        /// original one.
        /// </summary>
        /// <param name="original">Attribute table to copy.</param>
        public AttributeTable(AttributeTable original)
            : this()
        {
            this.CopyAttributes(original);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Maps the passed key to the specified value in this attribute table,
        /// if it has already been mapped before.
        /// </summary>
        /// <param name="key">Key to map.</param>
        /// <param name="value">Value to map the key to.</param>
        public void SetValue(object key, object value)
        {
            this.attributes[key] = value;
        }

        /// <summary>
        /// Removes the passed key from this attribute table.
        /// </summary>
        /// <param name="key">Key to remove.</param>
        public void Remove(object key)
        {
            this.attributes.Remove(key);
        }

        /// <summary>
        /// Maps the passed key to the specified value in this attribute table,
        /// if it hasn't already been mapped before.
        /// </summary>
        /// <param name="key">Key to map.</param>
        /// <param name="value">Value to map the key to.</param>
        public void Add(object key, object value)
        {
            this.attributes.Add(key, value);
        }

        /// <summary>
        /// Returns <c>true</c> if the passed key is mapped within this
        /// attribute table, and <c>false</c> otherwise.
        /// </summary>
        /// <param name="key">Key to check.</param>
        /// <returns>True if the passed key is mapped within this attribute table.</returns>
        public bool ContainsKey(object key)
        {
            return this.attributes.ContainsKey(key);
        }

        /// <summary>
        /// Tries to retrieve the value the passed key is mapped to within this
        /// attribute table.
        /// </summary>
        /// <param name="key">Key to retrieve the value of.</param>
        /// <param name="value">Retrieved value.</param>
        /// <returns>true if a value was found, and false otherwise.</returns>
        public virtual bool TryGetValue(object key, out object value)
        {
            return this.attributes.TryGetValue(key, out value);
        }

        /// <summary>
        /// Tries to retrieve the value the passed key is mapped to within this
        /// attribute table.
        /// </summary>
        /// <typeparam name="T">Type of the value to retrieve.</typeparam>
        /// <param name="key">Key to retrieve the value of.</param>
        /// <param name="value">Retrieved value.</param>
        /// <returns>true if a value was found, and false otherwise.</returns>
        public virtual bool TryGetValue<T>(object key, out T value)
        {
            object o;

            if (this.attributes.TryGetValue(key, out o) && o is T)
            {
                value = (T)o;
                return true;
            }
            else
            {
                value = default(T);
                return false;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Copyies all content of the passed attribute table to this one.
        /// </summary>
        /// <param name="original">Table to copy.</param>
        private void CopyAttributes(AttributeTable original)
        {
            foreach (KeyValuePair<object, object> keyValuePair in original.attributes)
            {
                this.attributes.Add(keyValuePair.Key, keyValuePair.Value);
            }
        }

        #endregion
    }
}