// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CompoundEntities.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Application.Systems
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    using Slash.ECS.Components;
    using Slash.Reflection.Utils;

    /// <summary>
    ///   Manages all entities that have a specific component configuration.
    /// </summary>
    /// <typeparam name="T">Type that contains information about component configuration and stores entities.</typeparam>
    public sealed class CompoundEntities<T> : IEnumerable<T>
        where T : class, new()
    {
        #region Fields

        private readonly IList<ComponentProperty> componentProperties;

        /// <summary>
        ///   Existing entities.
        /// </summary>
        private readonly Dictionary<int, T> entities;

        private readonly EntityManager entityManager;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///   Constructor.
        /// </summary>
        /// <param name="entityManager">Entity manager.</param>
        public CompoundEntities(EntityManager entityManager)
        {
            this.entityManager = entityManager;
            this.entities = new Dictionary<int, T>();

            this.componentProperties = this.CollectComponentProperties(typeof(T)).ToList();
            if (this.componentProperties != null)
            {
                entityManager.EntityInitialized += this.OnEntityInitialized;
                entityManager.EntityRemoved += this.OnEntityRemoved;
            }

            this.AllowEntity = (entityId, entity) => true;
        }

        #endregion

        #region Delegates

        /// <summary>
        ///   Delegate for EntityAdded event.
        /// </summary>
        /// <param name="entityId">Id of added entity.</param>
        /// <param name="entity">Data of added entity.</param>
        public delegate void EntityAddedDelegate(int entityId, T entity);

        /// <summary>
        ///   Delegate for AllowEntity property.
        /// </summary>
        /// <param name="entityId">Id of entity that wants to be added.</param>
        /// <param name="entity">Entity that wants to be added.</param>
        /// <returns>True if the entity should be added; otherwise, false.</returns>
        public delegate bool EntityFilter(int entityId, T entity);

        /// <summary>
        ///   Delegate for EntityRemoved event.
        /// </summary>
        /// <param name="entityId">Id of removed entity.</param>
        /// <param name="entity">Data of removed entity.</param>
        public delegate void EntityRemovedDelegate(int entityId, T entity);

        #endregion

        #region Events

        /// <summary>
        ///   Called when an entity was added that matches the configuration.
        /// </summary>
        public event EntityAddedDelegate EntityAdded;

        /// <summary>
        ///   Called when an entity was removed.
        /// </summary>
        public event EntityRemovedDelegate EntityRemoved;

        #endregion

        #region Properties

        /// <summary>
        ///   Special filter to use before accepting an entity.
        /// </summary>
        public EntityFilter AllowEntity { get; set; }

        /// <summary>
        ///   Dictionary of all current matching entities.
        /// </summary>
        public IDictionary<int, T> Entities
        {
            get
            {
                return this.entities;
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///   Returns the entity with the specified id if one exists; otherwise null.
        /// </summary>
        /// <param name="entityId">Id of entity to get.</param>
        /// <returns>Returns the entity with the specified id if one exists; otherwise null.</returns>
        public T GetEntity(int entityId)
        {
            T entity;
            this.entities.TryGetValue(entityId, out entity);
            return entity;
        }

        /// <summary>
        ///   Returns an enumerator that iterates through all current entities.
        /// </summary>
        /// <returns>
        ///   A <see cref="T:System.Collections.Generic.IEnumerator`1" /> that can be used to iterate through all current entities.
        /// </returns>
        /// <filterpriority>1</filterpriority>
        public IEnumerator<T> GetEnumerator()
        {
            return this.entities.Values.GetEnumerator();
        }

        #endregion

        #region Methods

        private IEnumerable<ComponentProperty> CollectComponentProperties(Type type)
        {
            // Return all property component types.
            var propertyInfos = ReflectionUtils.GetProperties(type);
            foreach (var propertyInfo in propertyInfos)
            {
                var compoundComponentAttribute = ReflectionUtils.GetAttribute<CompoundComponentAttribute>(propertyInfo);
                if (compoundComponentAttribute != null)
                {
                    yield return new ComponentProperty(propertyInfo) { Attribute = compoundComponentAttribute };
                }
            }

            // Return all field component types.
            var fieldInfos = ReflectionUtils.GetFields(type);
            foreach (var fieldInfo in fieldInfos)
            {
                var compoundComponentAttribute = ReflectionUtils.GetAttribute<CompoundComponentAttribute>(fieldInfo);
                if (compoundComponentAttribute != null)
                {
                    yield return new ComponentProperty(fieldInfo) { Attribute = compoundComponentAttribute };
                }
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        private void OnEntityAdded(int entityid, T entity)
        {
            var handler = this.EntityAdded;
            if (handler != null)
            {
                handler(entityid, entity);
            }
        }

        private void OnEntityInitialized(int entityId)
        {
            // Check if all required components present.
            foreach (var componentProperty in this.componentProperties)
            {
                componentProperty.RecentComponent = this.entityManager.GetComponent(entityId, componentProperty.Type);

                if (componentProperty.RecentComponent == null && !componentProperty.Attribute.IsOptional)
                {
                    return;
                }
            }

            // Create compound entity.
            T entity = new T();

            // Set components.
            foreach (var componentProperty in this.componentProperties)
            {
                if (componentProperty.RecentComponent != null)
                {
                    componentProperty.SetValue(entity, componentProperty.RecentComponent);
                }
            }

            // Allow filtering.
            if (!this.AllowEntity(entityId, entity))
            {
                return;
            }

            this.entities.Add(entityId, entity);

            // Notify listeners.
            this.OnEntityAdded(entityId, entity);
        }

        private void OnEntityRemoved(int entityId)
        {
            var entity = this.GetEntity(entityId);

            if (this.entities.Remove(entityId))
            {
                this.OnEntityRemoved(entityId, entity);
            }
        }

        private void OnEntityRemoved(int entityid, T entity)
        {
            var handler = this.EntityRemoved;
            if (handler != null)
            {
                handler(entityid, entity);
            }
        }

        #endregion

        private class ComponentProperty
        {
            #region Fields

            private readonly FieldInfo fieldInfo;

            private readonly PropertyInfo propertyInfo;

            #endregion

            #region Constructors and Destructors

            public ComponentProperty(PropertyInfo propertyInfo)
            {
                this.propertyInfo = propertyInfo;
                this.Type = propertyInfo.PropertyType;
            }

            public ComponentProperty(FieldInfo fieldInfo)
            {
                this.fieldInfo = fieldInfo;
                this.Type = fieldInfo.FieldType;
            }

            #endregion

            #region Properties

            public CompoundComponentAttribute Attribute { get; set; }

            /// <summary>
            ///   Most recent component created for this property.
            /// </summary>
            public object RecentComponent { get; set; }

            public Type Type { get; private set; }

            #endregion

            #region Public Methods and Operators

            public object GetValue(object obj)
            {
                if (this.propertyInfo != null)
                {
                    return this.propertyInfo.GetValue(obj, null);
                }
                if (this.fieldInfo != null)
                {
                    return this.fieldInfo.GetValue(obj);
                }
                return null;
            }

            public void SetValue(object obj, object value)
            {
                if (this.propertyInfo != null)
                {
                    this.propertyInfo.SetValue(obj, value, null);
                }
                else if (this.fieldInfo != null)
                {
                    this.fieldInfo.SetValue(obj, value);
                }
            }

            #endregion
        }
    }
}