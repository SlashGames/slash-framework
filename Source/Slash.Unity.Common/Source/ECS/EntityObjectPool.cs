// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EntityObjectPool.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Unity.Common.ECS
{
    using System.Collections.Generic;

    using Slash.Unity.Common.Utils;

    using UnityEngine;
#if UNITY_5_4_OR_NEWER
    using UnityEngine.Profiling;
#endif

    /// <summary>
    ///   Pool of game objects for entities. Manages the allocation and destruction of the entity objects.
    /// </summary>
    public class EntityObjectPool : MonoBehaviour
    {
#region Fields

        /// <summary>
        ///   Prefab to visualize an entity with.
        /// </summary>
        public GameObject entityPrefab;

        /// <summary>
        ///   Initial capacity of pool. This many entity objects are created on startup.
        /// </summary>
        public int initialCapacity = 500;

        /// <summary>
        ///   Maximum capacity of pool.
        /// </summary>
        public int maxCapacity = 500;

        /// <summary>
        ///   Pool of unused game objects which are ready for using.
        /// </summary>
        private Stack<GameObject> pool;

#endregion

#region Public Methods and Operators

        public GameObject Alloc()
        {
            if (this.entityPrefab == null)
            {
                return null;
            }

            // Get from pool if there is one.
            GameObject entityObject;
            if (this.pool.Count > 0)
            {
                entityObject = this.pool.Pop();
                entityObject.SetActive(true);
            }
            else
            {
                Profiler.BeginSample("Instantiate EntityPrefab");
                entityObject = this.gameObject.AddChild(this.entityPrefab);
                Profiler.EndSample();
            }

            return entityObject;
        }

        public void Free(GameObject entityObject)
        {
            // Put into pool if hasn't reached maximum capacity yet.
            if (this.pool.Count < this.maxCapacity)
            {
                entityObject.transform.localPosition = Vector3.zero;
                entityObject.transform.localRotation = Quaternion.identity;
                entityObject.transform.localScale = Vector3.one;

                entityObject.SetActive(false);
                this.PushPoolObject(entityObject);
            }
            else
            {
                if (Application.isEditor)
                {
                    DestroyImmediate(entityObject);
                }
                else
                {
                    Destroy(entityObject);
                }
            }
        }

        /// <summary>
        ///   Changes the maximum capacity of this pool. If the maximum capacity is increased, an amount of objects equal to the capacity difference is added to the pool. If the maximum capacity is decreased, an amount of objets equal to the capacity difference is removed from the pool of available objects.
        /// </summary>
        /// <param name="newMaxCapacity">New maximum capacity of this pool.</param>
        public void SetMaxCapacity(int newMaxCapacity)
        {
            int capacityDifference = newMaxCapacity - this.maxCapacity;

            if (capacityDifference > 0)
            {
                Debug.Log(string.Format("Adjusting object pool size, adding {0} objects.", capacityDifference));

                // Capacity increased. Increase pool size.
                for (int i = 0; i < capacityDifference; i++)
                {
                    this.AddPoolObject();
                }
            }
            else
            {
                // Capacity decreased. Decrease pool size.
                int spareObjects = Mathf.Min(this.pool.Count, Mathf.Abs(capacityDifference));

                Debug.Log(string.Format("Adjusting object pool size, removing {0} objects.", spareObjects));

                for (int i = 0; i < spareObjects; i++)
                {
                    GameObject entityObject = this.pool.Pop();
                    Destroy(entityObject);
                }
            }

            this.maxCapacity = newMaxCapacity;
        }

#endregion

#region Methods

        private void AddPoolObject()
        {
            GameObject entityObject = this.gameObject.AddChild(this.entityPrefab);
            this.PushPoolObject(entityObject);
        }

        private void Awake()
        {
            // Create pool.
            this.pool = new Stack<GameObject>(this.maxCapacity);

            // Create initial entity objects.
            for (int i = 0; i < this.initialCapacity; i++)
            {
                this.AddPoolObject();
            }
        }

        private void PushPoolObject(GameObject entityObject)
        {
#if UNITY_EDITOR
        Profiler.BeginSample("Change parent transform");
        entityObject.transform.parent = this.transform;
        Profiler.EndSample();
#endif

            entityObject.SetActive(false);
            this.pool.Push(entityObject);
        }

#endregion
    }
}