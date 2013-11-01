// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AttributeTable.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Collections.AttributeTables
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    using Slash.Collections.Utils;

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
        public AttributeTable(IAttributeTable original)
            : this()
        {
            this.AddRange(original);
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///   Number of attribute values stored in this table.
        /// </summary>
        public int Count
        {
            get
            {
                return this.attributes.Count;
            }
        }

        #endregion

        #region Public Indexers

        /// <summary>
        ///   Gets or sets the attribute with the specified key.
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
        ///   Adds all content of the passed attribute table to this one.
        /// </summary>
        /// <param name="attributeTable"> Table to add the content of. </param>
        public void AddRange(IAttributeTable attributeTable)
        {
            foreach (KeyValuePair<object, object> keyValuePair in attributeTable)
            {
                this.attributes[keyValuePair.Key] = keyValuePair.Value;
            }
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
        ///   Clears the attribute table.
        /// </summary>
        public void Clear()
        {
            this.attributes.Clear();
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

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            if (obj.GetType() != this.GetType())
            {
                return false;
            }

            return this.Equals((AttributeTable)obj);
        }

        /// <summary>
        ///   Gets an enumerator over all attributes of this table.
        /// </summary>
        /// <returns>All attributes of this table.</returns>
        public IEnumerator GetEnumerator()
        {
            return this.attributes.GetEnumerator();
        }

        public override int GetHashCode()
        {
            return this.attributes != null ? this.attributes.GetHashCode() : 0;
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
            try
            {
                return this.attributes[attributeKey];
            }
            catch (KeyNotFoundException)
            {
                throw new KeyNotFoundException(string.Format("Attribute key not found: {0}", attributeKey));
            }
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
        public virtual T GetValue<T>(object attributeKey) where T : class
        {
            object attributeValue = this.GetValue(attributeKey);
            return (T)attributeValue;
        }

        /// <summary>
        ///   Removes the passed key from this attribute table.
        /// </summary>
        /// <param name="key"> Key to remove. </param>
        /// <returns>
        ///   <c>true</c>, if the key has been removed, and <c>false</c> otherwise.
        /// </returns>
        public bool RemoveValue(object key)
        {
            return this.attributes.Remove(key);
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

        #endregion

        #region Methods

        protected bool Equals(AttributeTable other)
        {
            return CollectionUtils.DictionaryEqual(this.attributes, other.attributes);
        }

        #endregion
    }
}