// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InspectorEntityAttribute.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.GameBase.Inspector.Attributes
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Slash.GameBase.Configurations;

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
            if (this.List)
            {
                List<int> entityIds = null;
                IList<EntityConfiguration> entityConfigurations = (IList<EntityConfiguration>)propertyValue;
                if (entityConfigurations != null)
                {
                    entityIds = new List<int>();
                    entityIds.AddRange(
                        entityConfigurations.Select(entityConfiguration => CreateEntity(game, entityConfiguration))
                                            .Where(entityId => entityId != -1));
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