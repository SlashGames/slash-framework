// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AttributeTable.cs" company="Slash Games">
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
    public class AttributeTable : IAttributeTable
    {
        #region Fields

        /// <summary>
        ///   Dictionary to store attributes.
        /// </summary>
        private readonly Dictionary<object, object> attributes;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///   Constructs a new, empty attribute table.
        /// </summary>
        public AttributeTable()
        {
            this.attributes = new Dictionary<object, object>();
        }

        /// <summary>
        ///   Constructs a new attribute table, copying all content of the passed
        ///   original one.
        /// </summary>
        /// <param name="original"> Attribute table to copy. </param>
        public AttributeTable(AttributeTable original)
            : this()
        {
            this.CopyAttributes(original);
        }

        #endregion

        #region Public Indexers

        /// <summary>
        ///   Returns or sets the attribute with the specified key.
        ///   If no attribute is found when it should be returned, an exception is thrown.
        /// </summary>
        /// <param name="attributeKey"> Attribute key. </param>
        /// <returns> Value of attribute with the specified key. </returns>
        public object this[object attributeKey]
        {
            get
            {
                return this.GetValue(attributeKey);
            }
            set
            {
                this.SetValue(attributeKey, value);
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///   Maps the passed key to the specified value in this attribute table,
        ///   if it hasn't already been mapped before.
        /// </summary>
        /// <param name="key"> Key to map. </param>
        /// <param name="value"> Value to map the key to. </param>
        /// <exception cref="ArgumentException">An element with the same key already exists in the attribute table.</exception>
        public void Add(object key, object value)
        {
            this.AddValue(key, value);
        }

        /// <summary>
        ///   Maps the passed key to the specified value in this attribute table,
        ///   if it hasn't already been mapped before.
        /// </summary>
        /// <param name="key"> Key to map. </param>
        /// <param name="value"> Value to map the key to. </param>
        public void AddValue(object key, object value)
        {
            this.attributes.Add(key, value);
        }

        /// <summary>
        ///   Returns <c>true</c> if the passed key is mapped within this
        ///   attribute table, and <c>false</c> otherwise.
        /// </summary>
        /// <param name="key"> Key to check. </param>
        /// <returns> True if the passed key is mapped within this attribute table. </returns>
        public virtual bool Contains(object key)
        {
            return this.attributes.ContainsKey(key);
        }

        /// <summary>
        ///   Returns the attribute with the specified key.
        ///   If no attribute is found, an exception is thrown.
        /// </summary>
        /// <param name="attributeKey"> Attribute key. </param>
        /// <returns> Value of attribute with the specified key. </returns>
        /// <exception cref="KeyNotFoundException">Specified key wasn't found.</exception>
        public object GetValue(object attributeKey)
        {
            return this.attributes[attributeKey];
        }

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
        public virtual T GetValue<T>(object attributeKey)
        {
            object attributeValue = this.attributes[attributeKey];
            return (T)attributeValue;
        }

        /// <summary>
        ///   Removes the passed key from this attribute table.
        /// </summary>
        /// <param name="key"> Key to remove. </param>
        public void RemoveValue(object key)
        {
            this.attributes.Remove(key);
        }

        /// <summary>
        ///   Maps the passed key to the specified value in this attribute table,
        ///   if it has already been mapped before.
        /// </summary>
        /// <param name="key"> Key to map. </param>
        /// <param name="value"> Value to map the key to. </param>
        public void SetValue(object key, object value)
        {
            this.attributes[key] = value;
        }

        /// <summary>
        ///   Tries to retrieve the value the passed key is mapped to within this
        ///   attribute table.
        /// </summary>
        /// <param name="key"> Key to retrieve the value of. </param>
        /// <param name="value"> Retrieved value. </param>
        /// <returns> true if a value was found, and false otherwise. </returns>
        public virtual bool TryGetValue(object key, out object value)
        {
            return this.attributes.TryGetValue(key, out value);
        }

        /// <summary>
        ///   Tries to retrieve the value the passed key is mapped to within this
        ///   attribute table.
        /// </summary>
        /// <typeparam name="T"> Type of the value to retrieve. </typeparam>
        /// <param name="key"> Key to retrieve the value of. </param>
        /// <param name="value"> Retrieved value. </param>
        /// <returns> true if a value was found, and false otherwise. </returns>
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
        ///   Copyies all content of the passed attribute table to this one.
        /// </summary>
        /// <param name="original"> Table to copy. </param>
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