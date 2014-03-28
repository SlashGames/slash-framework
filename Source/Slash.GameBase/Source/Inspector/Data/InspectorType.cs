// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InspectorType.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.GameBase.Inspector.Data
{
    using System;
    using System.Collections.Generic;

    using Slash.GameBase.Inspector.Attributes;
    using Slash.GameBase.Inspector.Utils;

    /// <summary>
    ///   Component accessible to the user in the landscape designer.
    /// </summary>
    public class InspectorType
    {
        #region Public Properties

        /// <summary>
        ///   Raw attribute.
        /// </summary>
        public InspectorTypeAttribute Attribute { get; set; }

        /// <summary>
        ///   Description of type.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        ///   Name of type.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///   Properties exposed in the inspector.
        /// </summary>
        public List<InspectorPropertyAttribute> Properties { get; set; }

        /// <summary>
        ///   C# type of the component.
        /// </summary>
        public Type Type { get; set; }

        #endregion

        #region Public Methods and Operators

        public static InspectorType GetInspectorType(Type type)
        {
            Dictionary<InspectorPropertyAttribute, InspectorConditionalPropertyAttribute> tmpConditionalInspectors =
                null;
            return GetInspectorType(type, ref tmpConditionalInspectors);
        }

        public static InspectorType GetInspectorType(
            Type type,
            ref Dictionary<InspectorPropertyAttribute, InspectorConditionalPropertyAttribute> conditionalInspectors)
        {
            List<InspectorPropertyAttribute> inspectorProperties = InspectorUtils.CollectInspectorProperties(
                type, ref conditionalInspectors);

            InspectorTypeAttribute[] inspectorTypeAttributes =
                (InspectorTypeAttribute[])type.GetCustomAttributes(typeof(InspectorTypeAttribute), false);
            if (inspectorTypeAttributes.Length == 0)
            {
                return null;
            }

            InspectorTypeAttribute inspectorTypeAttribute = inspectorTypeAttributes[0];
            var inspectorTypeData = new InspectorType
                {
                    Attribute = inspectorTypeAttribute,
                    Name = type.Name,
                    Description = inspectorTypeAttribute.Description,
                    Properties = inspectorProperties,
                    Type = type,
                };

            return inspectorTypeData;
        }

        #endregion
    }
}