// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InspectorDataAttribute.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.ECS.Inspector.Attributes
{
    using System;

    using Slash.Collections.AttributeTables;
    using Slash.ECS.Inspector.Data;
    using Slash.ECS.Inspector.Utils;

    [Serializable]
    public class InspectorDataAttribute : InspectorPropertyAttribute
    {
        #region Constructors and Destructors

        public InspectorDataAttribute(string name)
            : base(name)
        {
        }

        #endregion

        #region Public Methods and Operators

        public override object ConvertStringToValue(string text)
        {
            throw new NotImplementedException();
        }

        public override void SetPropertyValue(Game game, object obj, object propertyValue)
        {
            IAttributeTable propertyAttributeTable = (IAttributeTable)propertyValue;

            propertyValue = Activator.CreateInstance(this.PropertyType);
            InspectorType propertyInspectorType = InspectorType.GetInspectorType(this.PropertyType);
            InspectorUtils.InitFromAttributeTable(game, propertyInspectorType, propertyValue, propertyAttributeTable);

            base.SetPropertyValue(game, obj, propertyValue);
        }

        public override bool TryConvertStringToValue(string text, out object value)
        {
            value = null;
            return false;
        }

        #endregion
    }
}