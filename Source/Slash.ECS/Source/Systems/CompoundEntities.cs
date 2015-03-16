// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CompoundEntities.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.ECS.Source.Systems
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    using Slash.ECS.Components;
    using Slash.Reflection.Extensions;
    using Slash.Reflection.Utils;

    public sealed class CompoundEntities<T> : IEnumerable<T>
        where T : class, new()
    {
        #region Fields

        private readonly IEnumerable<ComponentProperty> componentProperties;

        /// <summary>
        ///   Existing entities.
        /// </summary>
        private readonly Dictionary<int, T> entities;

        /// <summary>
        ///   Entities which are not complete (yet).
        /// </summary>
        private readonly Dictionary<int, T> incompleteEntities;

        #endregion

        #region Constructors and Destructors

        public CompoundEntities(EntityManager entityManager)
        {
            this.entities = new Dictionary<int, T>();
            this.incompleteEntities = new Dictionary<int, T>();

            this.componentProperties = this.CollectComponentProperties(typeof(T));
            if (this.componentProperties != null)
            {
                foreach (var componentProperty in this.componentProperties)
                {
                    entityManager.RegisterComponentListeners(
                        componentProperty.Type,
                        this.OnComponentAdded,
                        this.OnComponentRemoved);
                }
            }
        }

        #endregion

        #region Delegates

        public delegate void EntityAddedDelegate(int entityId, T entity);

        public delegate void EntityRemovedDelegate(int entityId, T entity);

        #endregion

        #region Events

        public event EntityAddedDelegate EntityAdded;

        public event EntityRemovedDelegate EntityRemoved;

        #endregion

        #region Properties

        public IDictionary<int, T> Entities
        {
            get
            {
                return this.entities;
            }
        }

        #endregion

        #region Public Methods and Operators

        public T GetEntity(int entityId)
        {
            T entity;
            this.entities.TryGetValue(entityId, out entity);
            return entity;
        }

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

        private T GetIncompleteEntity(int entityId, bool createIfNecessary = false)
        {
            T entity;
            if (!this.incompleteEntities.TryGetValue(entityId, out entity) && createIfNecessary)
            {
                entity = new T();
                this.incompleteEntities[entityId] = entity;
            }
            return entity;
        }

        private bool IsComplete(T entity)
        {
            foreach (var componentProperty in this.componentProperties)
            {
                if (!componentProperty.Attribute.IsOptional && componentProperty.GetValue(entity) == null)
                {
                    return false;
                }
            }
            return true;
        }

        private void OnComponentAdded(int entityId, object component)
        {
            // Get entity.
            T entity = this.GetEntity(entityId);
            if (entity == null)
            {
                // Get or create incomplete entity.
                entity = this.GetIncompleteEntity(entityId, true);
            }

            // Get component property for this component.
            ComponentProperty componentProperty =
                this.componentProperties.FirstOrDefault(property => property.Type == component.GetType());
            if (componentProperty == null)
            {
                return;
            }

            // Set component.
            componentProperty.SetValue(entity, component);

            // Check if complete, i.e. all necessary components are set.
            if (!componentProperty.Attribute.IsOptional && this.IsComplete(entity))
            {
                this.incompleteEntities.Remove(entityId);
                this.entities.Add(entityId, entity);

                this.OnEntityAdded(entityId, entity);
            }
        }

        private void OnComponentRemoved(int entityId, object component)
        {
            // Get entity.
            T entity = this.GetEntity(entityId);
            if (entity == null)
            {
                return;
            }

            // Get component property for this component.
            ComponentProperty componentProperty =
                this.componentProperties.FirstOrDefault(property => property.Type == component.GetType());
            if (componentProperty == null)
            {
                return;
            }

            // Remove component.
            componentProperty.SetValue(entity, null);

            // Check if to remove entity completely.
            if (!componentProperty.Attribute.IsOptional)
            {
                this.entities.Remove(entityId);
                this.OnEntityRemoved(entityId, entity);
            }
        }

        private void OnEntityAdded(int entityid, T entity)
        {
            var handler = this.EntityAdded;
            if (handler != null)
            {
                handler(entityid, entity);
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