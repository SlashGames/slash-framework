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
            var propertyInfos = type.GetProperties();
            foreach (var propertyInfo in propertyInfos)
            {
                var compoundComponentAttribute = propertyInfo.GetCustomAttribute<CompoundComponentAttribute>();
                if (compoundComponentAttribute != null)
                {
                    yield return
                        new ComponentProperty()
                        {
                            PropertyInfo = propertyInfo,
                            Type = propertyInfo.PropertyType,
                            Attribute = compoundComponentAttribute
                        };
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
                if (!componentProperty.Attribute.IsOptional
                    && componentProperty.PropertyInfo.GetValue(entity, null) == null)
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
            componentProperty.PropertyInfo.SetValue(entity, component, null);

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
            componentProperty.PropertyInfo.SetValue(entity, null, null);

            // TODO(co): Check if to remove entity completely.
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
            #region Properties

            public CompoundComponentAttribute Attribute { get; set; }

            public PropertyInfo PropertyInfo { get; set; }

            public Type Type { get; set; }

            #endregion
        }
    }
}