// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ReadOnlyCollection.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Collections.ObjectModel
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    /// <summary>
    ///   Wrapper to make sure a collection isn't changed.
    /// </summary>
    /// <typeparam name="T"> Type of objects in collection. </typeparam>
    public class ReadOnlyCollection<T> : ICollection<T>
    {
        #region Fields

        /// <summary>
        ///   Wrapped collection.
        /// </summary>
        private readonly ICollection<T> collection;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///   Wraps the specified collection with a new read-only collection.
        /// </summary>
        /// <param name="collection"> Collection to wrap. </param>
        public ReadOnlyCollection(ICollection<T> collection)
        {
            this.collection = collection;
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///   Gets the number of elements contained in the <see cref="T:System.Collections.Generic.ICollection`1" />.
        /// </summary>
        /// <returns>
        ///   The number of elements contained in the <see cref="T:System.Collections.Generic.ICollection`1" /> .
        /// </returns>
        public int Count
        {
            get
            {
                return this.collection.Count;
            }
        }

        /// <summary>
        ///   Gets a value indicating whether the <see cref="T:System.Collections.Generic.ICollection`1" /> is read-only.
        /// </summary>
        /// <returns>
        ///   true if the <see cref="T:System.Collections.Generic.ICollection`1" /> is read-only; otherwise, false.
        /// </returns>
        public bool IsReadOnly
        {
            get
            {
                return true;
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///   Adds an item to the <see cref="T:System.Collections.Generic.ICollection`1" />.
        /// </summary>
        /// <param name="item">
        ///   The object to add to the <see cref="T:System.Collections.Generic.ICollection`1" /> .
        /// </param>
        /// <exception cref="T:System.NotSupportedException">
        ///   The
        ///   <see cref="T:System.Collections.Generic.ICollection`1" />
        ///   is read-only.
        /// </exception>
        public void Add(T item)
        {
            throw new NotSupportedException("Collection is read-only.");
        }

        /// <summary>
        ///   Removes all items from the <see cref="T:System.Collections.Generic.ICollection`1" />.
        /// </summary>
        /// <exception cref="T:System.NotSupportedException">
        ///   The
        ///   <see cref="T:System.Collections.Generic.ICollection`1" />
        ///   is read-only.
        /// </exception>
        public void Clear()
        {
            throw new NotSupportedException("Collection is read-only.");
        }

        /// <summary>
        ///   Determines whether the <see cref="T:System.Collections.Generic.ICollection`1" /> contains a specific value.
        /// </summary>
        /// <returns>
        ///   true if <paramref name="item" /> is found in the <see cref="T:System.Collections.Generic.ICollection`1" /> ; otherwise, false.
        /// </returns>
        /// <param name="item">
        ///   The object to locate in the <see cref="T:System.Collections.Generic.ICollection`1" /> .
        /// </param>
        public bool Contains(T item)
        {
            return this.collection.Contains(item);
        }

        /// <summary>
        ///   Copies the elements of the <see cref="T:System.Collections.Generic.ICollection`1" /> to an
        ///   <see
        ///     cref="T:System.Array" />
        ///   , starting at a particular
        ///   <see
        ///     cref="T:System.Array" />
        ///   index.
        /// </summary>
        /// <param name="array">
        ///   The one-dimensional <see cref="T:System.Array" /> that is the destination of the elements copied from
        ///   <see
        ///     cref="T:System.Collections.Generic.ICollection`1" />
        ///   . The <see cref="T:System.Array" /> must have zero-based indexing.
        /// </param>
        /// <param name="arrayIndex">
        ///   The zero-based index in <paramref name="array" /> at which copying begins.
        /// </param>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="array" />
        ///   is null.
        /// </exception>
        /// <exception cref="T:System.ArgumentOutOfRangeException">
        ///   <paramref name="arrayIndex" />
        ///   is less than 0.
        /// </exception>
        /// <exception cref="T:System.ArgumentException">
        ///   The number of elements in the source
        ///   <see cref="T:System.Collections.Generic.ICollection`1" />
        ///   is greater than the available space from
        ///   <paramref name="arrayIndex" />
        ///   to the end of the destination
        ///   <paramref name="array" />
        ///   .
        /// </exception>
        public void CopyTo(T[] array, int arrayIndex)
        {
            this.collection.CopyTo(array, arrayIndex);
        }

        /// <summary>
        ///   Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        ///   A <see cref="T:System.Collections.Generic.IEnumerator`1" /> that can be used to iterate through the collection.
        /// </returns>
        /// <filterpriority>1</filterpriority>
        public IEnumerator<T> GetEnumerator()
        {
            return this.collection.GetEnumerator();
        }

        /// <summary>
        ///   Removes the first occurrence of a specific object from the <see cref="T:System.Collections.Generic.ICollection`1" />.
        /// </summary>
        /// <returns>
        ///   true if <paramref name="item" /> was successfully removed from the
        ///   <see
        ///     cref="T:System.Collections.Generic.ICollection`1" />
        ///   ; otherwise, false. This method also returns false if
        ///   <paramref
        ///     name="item" />
        ///   is not found in the original <see cref="T:System.Collections.Generic.ICollection`1" /> .
        /// </returns>
        /// <param name="item">
        ///   The object to remove from the <see cref="T:System.Collections.Generic.ICollection`1" /> .
        /// </param>
        /// <exception cref="T:System.NotSupportedException">
        ///   The
        ///   <see cref="T:System.Collections.Generic.ICollection`1" />
        ///   is read-only.
        /// </exception>
        public bool Remove(T item)
        {
            throw new NotSupportedException("Collection is read-only.");
        }

        #endregion

        #region Explicit Interface Methods

        /// <summary>
        ///   Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        ///   An <see cref="T:System.Collections.IEnumerator" /> object that can be used to iterate through the collection.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        #endregion
    }
}