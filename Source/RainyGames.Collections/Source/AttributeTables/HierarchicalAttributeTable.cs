// -----------------------------------------------------------------------
// <copyright file="HierarchicalAttributeTable.cs" company="Rainy Games">
// Copyright (c) Rainy Games. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace RainyGames.Collections.AttributeTables
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Table that allows storing and looking up attributes and their
    /// respective values. Provides a collection of parents that are
    /// referred to if a key can not be found in this table.
    /// </summary>
    public class HierarchicalAttributeTable : AttributeTable
    {
        #region Constants and Fields

        /// <summary>
        /// Parent tables to consult if a key can't be found in this one.
        /// </summary>
        private List<IAttributeTable> parents;

        #endregion

        #region Constructors

        /// <summary>
        /// Constructs a new, empty attribute table without any parents.
        /// </summary>
        public HierarchicalAttributeTable()
        {
            this.parents = new List<IAttributeTable>();
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Adds the passed attribute table as parent to this one.
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
        /// Removes all parents of this attribute table.
        /// </summary>
        public void ClearParents()
        {
            this.parents.Clear();
        }

        /// <summary>
        /// Checks whether the passed attribute table is a parent of this one.
        /// </summary>
        /// <param name="parent">Table to check.</param>
        /// <returns>true, if the passed table is a parent of this one, and false otherwise.</returns>
        public bool HasParent(IAttributeTable parent)
        {
            return this.parents.Contains(parent);
        }

        /// <summary>
        /// Inserts the passed attribute table as parent to be consulted with
        /// the specified priority if a key can't be found in this one.
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
        /// Removes the passed parent from this attribute table.
        /// </summary>
        /// <param name="parent">Parent to remove.</param>
        public void RemoveParent(IAttributeTable parent)
        {
            this.parents.Remove(parent);
        }

        /// <summary>
        /// Tries to retrieve the value the passed key is mapped to within this
        /// attribute table. If the key can't be found, the parents of this are
        /// checked, in order.
        /// </summary>
        /// <param name="key">Key to retrieve the value of.</param>
        /// <param name="value">Retrieved value.</param>
        /// <returns>true if a value was found, and false otherwise.</returns>
        public override bool TryGetValue(object key, out object value)
        {
            return this.TryGetValue<object>(key, out value);
        }

        /// <summary>
        /// Tries to retrieve the value the passed key is mapped to within this
        /// attribute table. If the key can't be found, the parents of this are
        /// checked, in order.
        /// </summary>
        /// <typeparam name="T">Type of the value to retrieve.</typeparam>
        /// <param name="key">Key to retrieve the value of.</param>
        /// <param name="value">Retrieved value.</param>
        /// <returns>true if a value was found, and false otherwise.</returns>
        public override bool TryGetValue<T>(object key, out T value)
        {
            if (base.TryGetValue(key, out value))
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

            value = default(T);
            return false;
        }

        #endregion
    }
}
