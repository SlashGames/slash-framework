// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InspectorEntityAttribute.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.ECS.Inspector.Attributes
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Slash.ECS.Configurations;

    [Serializable]
    public class InspectorEntityAttribute : InspectorPropertyAttribute
    {
        #region Constructors and Destructors

        public InspectorEntityAttribute(string name)
            : base(name)
        {
            this.AttributeType = typeof(EntityConfiguration);
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///   Initializes the specified object via reflection with the specified property value.
        /// </summary>
        /// <param name="game">Game the object exists in.</param>
        /// <param name="obj">Object to set property value for.</param>
        /// <param name="propertyValue">Property value to set.</param>
        public override void SetPropertyValue(Game game, object obj, object propertyValue)
        {
            if (this.IsList)
            {
                List<int> entityIds = null;
                IList<EntityConfiguration> entityConfigurations = propertyValue as IList<EntityConfiguration>;
                if (entityConfigurations != null)
                {
                    entityIds = new List<int>();
                    entityIds.AddRange(
                        entityConfigurations.Select(entityConfiguration => CreateEntity(game, entityConfiguration))
                                            .Where(entityId => entityId != 0));
                }
                else
                {
                    // Create entity from value (backwards compatibility).
                    EntityConfiguration entityConfiguration = (EntityConfiguration)propertyValue;
                    if (entityConfiguration != null)
                    {
                        int entityId = CreateEntity(game, entityConfiguration);
                        entityIds = new List<int> { entityId };
                    }
                }

                propertyValue = entityIds;
            }
            else
            {
                // Create entity from value.
                EntityConfiguration entityConfiguration = (EntityConfiguration)propertyValue;
                int entityId = CreateEntity(game, entityConfiguration);

                propertyValue = entityId;
            }

            base.SetPropertyValue(game, obj, propertyValue);
        }

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

        private static int CreateEntity(Game game, EntityConfiguration entityConfiguration)
        {
            int entityId = 0;
            if (entityConfiguration != null)
            {
                if (entityConfiguration.BlueprintId != null)
                {
                    entityId =
                        game.EntityManager.CreateEntity(
                            game.BlueprintManager.GetBlueprint(entityConfiguration.BlueprintId),
                            entityConfiguration.Configuration);
                }
            }
            return entityId;
        }

        #endregion
    }
}