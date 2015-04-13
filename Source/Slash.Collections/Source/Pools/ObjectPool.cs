// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ObjectPool.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Collections.Pools
{
    using System.Collections.Generic;

    /// <summary>
    ///   Pool of re-usable objects.
    /// </summary>
    /// <typeparam name="T">Type of the objects of this pool.</typeparam>
    public class ObjectPool<T>
        where T : IPoolable, new()
    {
        #region Fields

        /// <summary>
        ///   Pool of unused objects which are ready for use.
        /// </summary>
        private readonly Stack<T> pool;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///   Creates a new pool with the specified initial and maximum capacity.
        /// </summary>
        /// <param name="capacity">Maximum capacity of the pool.</param>
        public ObjectPool(int capacity)
            : this(capacity, () => new T())
        {
        }

        /// <summary>
        ///   Creates a new pool with the specified initial and maximum capacity.
        /// </summary>
        /// <param name="capacity">Maximum capacity of the pool.</param>
        /// <param name="createPoolObject">Factory method for creating new pool objects.</param>
        public ObjectPool(int capacity, CreatePoolObjectDelegate createPoolObject)
        {
            // Create pool.
            this.Capacity = capacity;
            this.pool = new Stack<T>(this.Capacity);

            // Register factory method.
            this.CreatePoolObject = createPoolObject;

            // Create initial objects.
            for (var i = 0; i < capacity; ++i)
            {
                var obj = this.CreatePoolObject();
                this.pool.Push(obj);
            }
        }

        #endregion

        #region Delegates

        /// <summary>
        ///   Creates a new object to be added to the pool.
        /// </summary>
        /// <returns>New object to be added to the pool.</returns>
        public delegate T CreatePoolObjectDelegate();

        #endregion

        #region Properties

        /// <summary>
        ///   Maximum capacity of the pool.
        /// </summary>
        public int Capacity { get; set; }

        /// <summary>
        ///   Creates a new object to be added to the pool.
        /// </summary>
        /// <returns>New object to be added to the pool.</returns>
        public CreatePoolObjectDelegate CreatePoolObject { get; set; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///   Returns a pooled object, if there are any left, or a new one, otherwise.
        /// </summary>
        /// <returns>Object of this pool.</returns>
        public T Alloc()
        {
            // Get object from pool if there is one.
            return this.pool.Count > 0 ? this.pool.Pop() : this.CreatePoolObject();
        }

        /// <summary>
        ///   Returns the passed object to this pool, if it's not already at maximum capacity.
        /// </summary>
        /// <param name="obj">Object to return.</param>
        public void Free(T obj)
        {
            // Put into pool if hasn't reached maximum capacity yet.
            if (this.pool.Count < this.Capacity)
            {
                obj.Reset();
                this.pool.Push(obj);
            }
        }

        #endregion
    }
}