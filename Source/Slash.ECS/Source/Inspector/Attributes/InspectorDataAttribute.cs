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

    /// <summary>
    ///   Exposes the property to the inspector.
    /// </summary>
#if !WINDOWS_STORE
    [Serializable]
#endif
    public class InspectorDataAttribute : InspectorPropertyAttribute
    {
        #region Constructors and Destructors

        /// <summary>
        ///   Exposes the property to the inspector.
        /// </summary>
        /// <param name="name">Property name to be shown in the inspector.</param>
        public InspectorDataAttribute(string name)
            : base(name)
        {
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///   Not implemented.
        /// </summary>
        /// <param name="text">Not implemented.</param>
        /// <returns>
        ///   Throws a <see cref="NotImplementedException"/>.
        /// </returns>
        public override object ConvertStringToValue(string text)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///   Initializes the specified object via reflection with the specified property value.
        /// </summary>
        /// <param name="game">The parameter is not used.</param>
        /// <param name="obj">Object to set property value for.</param>
        /// <param name="propertyValue">Property value to set.</param>
        public override void SetPropertyValue(Game game, object obj, object propertyValue)
        {
            IAttributeTable propertyAttributeTable = (IAttributeTable)propertyValue;

            propertyValue = Activator.CreateInstance(this.PropertyType);
            InspectorType propertyInspectorType = InspectorType.GetInspectorType(this.PropertyType);
            InspectorUtils.InitFromAttributeTable(game, propertyInspectorType, propertyValue, propertyAttributeTable);

            base.SetPropertyValue(game, obj, propertyValue);
        }

        /// <summary>
        ///   Not implemented.
        /// </summary>
        /// <param name="text">Not implemented..</param>
        /// <param name="value">Not implemented.</param>
        /// <returns>
        ///   Throws a <see cref="NotImplementedException"/>.
        /// </returns>
        public override bool TryConvertStringToValue(string text, out object value)
        {
            value = null;
            return false;
        }

        #endregion
    }
}