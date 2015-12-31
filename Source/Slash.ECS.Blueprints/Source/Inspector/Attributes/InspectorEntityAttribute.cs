// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InspectorEntityAttribute.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.ECS.Blueprints.Inspector.Attributes
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Slash.ECS.Blueprints.Configurations;
    using Slash.ECS.Components;
    using Slash.ECS.Inspector.Attributes;

    /// <summary>
    ///   Exposes the property to the inspector.
    /// </summary>
    [Serializable]
    public class InspectorEntityAttribute : InspectorPropertyAttribute
    {
        #region Constructors and Destructors

        /// <summary>
        ///   Exposes the property to the inspector.
        /// </summary>
        /// <param name="name">Property name to be shown in the inspector.</param>
        public InspectorEntityAttribute(string name)
            : base(name)
        {
            this.AttributeType = typeof(EntityConfiguration);
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///   Performs required deinitialization steps for this property of the specified object.
        /// </summary>
        /// <param name="entityManager">Entity manager the object belongs to.</param>
        /// <param name="obj">Object the property belongs to.</param>
        public override void Deinit(EntityManager entityManager, object obj)
        {
            // Get property value.
            int entityId = (int)this.GetPropertyValue(entityManager, obj);
            if (entityId > 0)
            {
                // Remove entity.
                entityManager.RemoveEntity(entityId);
            }
        }

        /// <summary>
        ///   Initializes the specified object via reflection with the specified property value.
        /// </summary>
        /// <param name="entityManager">Entity manager containing the specified object.</param>
        /// <param name="obj">Object to set property value for.</param>
        /// <param name="propertyValue">Property value to set.</param>
        public override void SetPropertyValue(EntityManager entityManager, object obj, object propertyValue)
        {
            if (this.IsList)
            {
                List<int> entityIds = null;
                IList<EntityConfiguration> entityConfigurations = propertyValue as IList<EntityConfiguration>;
                if (entityConfigurations != null)
                {
                    entityIds = new List<int>();
                    entityIds.AddRange(
                        entityConfigurations.Select(
                            entityConfiguration => CreateEntity(entityManager, entityConfiguration))
                            .Where(entityId => entityId != 0));
                }
                else
                {
                    entityIds = propertyValue as List<int>;
                    if (entityIds == null)
                    {
                        // Create entity from value (backwards compatibility).
                        EntityConfiguration entityConfiguration = (EntityConfiguration)propertyValue;
                        if (entityConfiguration != null)
                        {
                            int entityId = CreateEntity(entityManager, entityConfiguration);
                            entityIds = new List<int> { entityId };
                        }
                    }
                }

                propertyValue = entityIds;
            }
            else
            {
                if (!(propertyValue is int))
                {
                    // Create entity from value.
                    EntityConfiguration entityConfiguration = (EntityConfiguration)propertyValue;
                    int entityId = CreateEntity(entityManager, entityConfiguration);
                    propertyValue = entityId;
                }
            }

            base.SetPropertyValue(entityManager, obj, propertyValue);
        }

        /// <summary>
        ///   Tries to convert the specified text to a value of the correct type for this property.
        /// </summary>
        /// <param name="text">Text to convert.</param>
        /// <param name="value">Value of the correct type for this property, if the conversion was successful.</param>
        /// <returns>
        ///   True if the conversion was successful; otherwise, false.
        /// </returns>
        public override bool TryConvertStringToValue(string text, out object value)
        {
            if (string.IsNullOrEmpty(text))
            {
                value = null;
            }
            else
            {
                EntityConfiguration entityConfiguration = new EntityConfiguration { BlueprintId = text };
                value = entityConfiguration;
            }
            return true;
        }

        #endregion

        #region Methods

        private static int CreateEntity(EntityManager entityManager, EntityConfiguration entityConfiguration)
        {
            int entityId = 0;
            if (entityConfiguration != null)
            {
                if (entityConfiguration.BlueprintId != null)
                {
                    throw new Exception(
                        "InspectorEntityAttribute not usable right now, see github issue #24 (https://github.com/npruehs/slash-framework/issues/24)");
                    // TODO(co): How to get blueprint at this place?
                    //entityId = entityManager.CreateEntity(
                    //    entityConfiguration.BlueprintId,
                    //    entityConfiguration.Configuration);
                }
            }
            return entityId;
        }

        #endregion
    }
}