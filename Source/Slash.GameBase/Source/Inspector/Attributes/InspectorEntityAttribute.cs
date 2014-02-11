// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InspectorEntityAttribute.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.GameBase.Inspector.Attributes
{
    using Slash.GameBase.Configurations;

    public class InspectorEntityAttribute : InspectorPropertyAttribute
    {
        #region Constructors and Destructors

        public InspectorEntityAttribute(string name)
            : base(name)
        {
        }

        #endregion

        #region Public Methods and Operators

        public override void SetPropertyValue(Game game, object obj, object propertyValue)
        {
            // Create entity from value.
            EntityConfiguration entityConfiguration = (EntityConfiguration)propertyValue;
            int entityId = -1;
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
            propertyValue = entityId;

            base.SetPropertyValue(game, obj, propertyValue);
        }

        #endregion
    }
}