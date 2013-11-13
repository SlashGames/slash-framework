// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HierarchicalAttributeTable.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Collections.AttributeTables
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    ///   Table that allows storing and looking up attributes and their
    ///   respective values. Provides a collection of parents that are
    ///   referred to if a key can not be found in this table.
    /// </summary>
    public class HierarchicalAttributeTable : IAttributeTable
    {
        #region Fields

        /// <summary>
        ///   Table storing the values of this hierarchical attribute table.
        /// </summary>
        private readonly AttributeTable attributeTable;

        /// <summary>
        ///   Parent tables to consult if a key can't be found in this one.
        /// </summary>
        private readonly List<IAttributeTable> parents;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///   Constructs a new, empty attribute table without any parents.
        /// </summary>
        public HierarchicalAttributeTable()
        {
            this.attributeTable = new AttributeTable();
            this.parents = new List<IAttributeTable>();
        }

        /// <summary>
        ///   Constructs a new, empty attribute table with the specified parents.
        /// </summary>
        /// <param name="parents">Parent tables to consult if a key can't be found.</param>
        public HierarchicalAttributeTable(params IAttributeTable[] parents)
            : this()
        {
            foreach (IAttributeTable parent in parents.Where(parent => parent != null))
            {
                this.parents.Add(parent);
            }
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///   Returns the number of attributes in the attribute table.
        /// </summary>
        public int Count
        {
            get
            {
                return this.attributeTable.Count + this.parents.Sum(parent => parent.Count);
            }
        }

        #endregion

        #region Public Indexers

        /// <summary>
        ///   Gets or sets the attribute with the specified key. If the key is looked up and can't be found, the parents of this are
        ///   checked, in order.
        ///   If no attribute is found when it should be returned, an exception is thrown.
        /// </summary>
        /// <param name="key"> Attribute key. </param>
        /// <returns> Value of attribute with the specified key. </returns>
        /// <exception cref="KeyNotFoundException">If the key is looked up and can't be found in this table or any parents.</exception>
        public object this[object key]
        {
            get
            {
                return this.GetValue(key);
            }

            set
            {
                this.SetValue(key, value);
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
            this.attributeTable.Add(key, value);
        }

        /// <summary>
        ///   Adds the passed attribute table as parent to this one.
        /// </summary>
        /// <param name="parent">Parent to add.</param>
        public void AddParent(IAttributeTable parent)
        {
            if (parent == null)
            {
                throw new ArgumentNullException("parent", "Parent is null.");
            }

            this.parents.Add(parent);
        }

        /// <summary>
        ///   Adds all content of the passed attribute table to this one.
        /// </summary>
        /// <param name="other"> Table to add the content of. </param>
        public void AddRange(IAttributeTable other)
        {
            this.attributeTable.AddRange(other);
        }

        /// <summary>
        ///   Maps the passed key to the specified value in this attribute table,
        ///   if it hasn't already been mapped before.
        /// </summary>
        /// <param name="key"> Key to map. </param>
        /// <param name="value"> Value to map the key to. </param>
        public void AddValue(object key, object value)
        {
            this.attributeTable.AddValue(key, value);
        }

        /// <summary>
        ///   Removes all parents of this attribute table.
        /// </summary>
        public void ClearParents()
        {
            this.parents.Clear();
        }

        /// <summary>
        ///   Returns <c>true</c> if the passed key is mapped within this
        ///   attribute table, and <c>false</c> otherwise. If the key is looked up and can't be found, the parents of this are
        ///   checked, in order.
        /// </summary>
        /// <param name="key"> Key to check. </param>
        /// <returns> True if the passed key is mapped within this attribute table or any of its parents. </returns>
        public bool Contains(object key)
        {
            if (this.attributeTable.Contains(key))
            {
                return true;
            }

            return this.parents.Any(parent => parent.Contains(key));
        }

        /// <summary>
        ///   Gets an enumerator over all attributes of this table and its children.
        /// </summary>
        /// <returns>All attributes of this table and its children.</returns>
        public IEnumerator GetEnumerator()
        {
            HashSet<object> returnedKeys = new HashSet<object>();

            foreach (KeyValuePair<object, object> attribute in this.attributeTable)
            {
                returnedKeys.Add(attribute.Key);
                yield return attribute;
            }

            foreach (IAttributeTable parent in this.parents)
            {
                foreach (KeyValuePair<object, object> attribute in parent)
                {
                    if (!returnedKeys.Contains(attribute.Key))
                    {
                        returnedKeys.Add(attribute.Key);
                        yield return attribute;
                    }
                }
            }
        }

        /// <summary>
        ///   Returns the attribute with the specified key. If the key is looked up and can't be found, the parents of this are
        ///   checked, in order.
        ///   If no attribute is found, an exception is thrown.
        /// </summary>
        /// <param name="key"> Attribute key. </param>
        /// <returns> Value of attribute with the specified key. </returns>
        /// <exception cref="KeyNotFoundException">Specified key wasn't found.</exception>
        public object GetValue(object key)
        {
            object value;

            if (this.attributeTable.TryGetValue(key, out value))
            {
                return value;
            }

            if (this.parents.Any(parent => parent.TryGetValue(key, out value)))
            {
                return value;
            }

            throw new KeyNotFoundException(key.ToString());
        }

        /// <summary>
        ///   Checks whether the passed attribute table is a parent of this one.
        /// </summary>
        /// <param name="parent">Table to check.</param>
        /// <returns>true, if the passed table is a parent of this one, and false otherwise.</returns>
        public bool HasParent(IAttributeTable parent)
        {
            return this.parents.Contains(parent);
        }

        /// <summary>
        ///   Inserts the passed attribute table as parent to be consulted with
        ///   the specified priority if a key can't be found in this one.
        /// </summary>
        /// <param name="priority">Priority of the new parent.</param>
        /// <param name="parent">Parent to add.</param>
        public void InsertParent(int priority, IAttributeTable parent)
        {
            if (parent == null)
            {
                throw new ArgumentNullException("parent", "Parent is null.");
            }

            this.parents.Insert(priority, parent);
        }

        /// <summary>
        ///   Removes the passed parent from this attribute table.
        /// </summary>
        /// <param name="parent">Parent to remove.</param>
        public void RemoveParent(IAttributeTable parent)
        {
            this.parents.Remove(parent);
        }

        /// <summary>
        ///   Removes the passed key from this attribute table. Note that the value is not removed from any parent tables.
        /// </summary>
        /// <param name="key"> Key to remove. </param>
        /// <returns>
        ///   <c>true</c>, if the key has been removed, and <c>false</c> otherwise.
        /// </returns>
        public bool RemoveValue(object key)
        {
            return this.attributeTable.RemoveValue(key);
        }

        /// <summary>
        ///   Maps the passed key to the specified value in this attribute table,
        ///   if it has already been mapped before.
        /// </summary>
        /// <param name="key"> Key to map. </param>
        /// <param name="value"> Value to map the key to. </param>
        public void SetValue(object key, object value)
        {
            this.attributeTable.SetValue(key, value);
        }

        /// <summary>
        ///   Tries to retrieve the value the passed key is mapped to within this
        ///   attribute table. If the key can't be found, the parents of this table are
        ///   checked, in order.
        /// </summary>
        /// <param name="key">Key to retrieve the value of.</param>
        /// <param name="value">Retrieved value.</param>
        /// <returns>true if a value was found, and false otherwise.</returns>
        public bool TryGetValue(object key, out object value)
        {
            if (this.attributeTable.TryGetValue(key, out value))
            {
                return true;
            }

            foreach (IAttributeTable parent in this.parents)
            {
                if (parent.TryGetValue(key, out value))
                {
                    return true;
                }
            }

            value = null;
            return false;
        }

        #endregion
    }
}