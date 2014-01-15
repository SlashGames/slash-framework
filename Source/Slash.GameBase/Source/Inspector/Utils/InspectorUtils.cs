// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InspectorUtils.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.GameBase.Inspector.Utils
{
    using System;
    using System.Collections.Generic;

    using Slash.GameBase.Inspector.Attributes;

    public static class InspectorUtils
    {
        #region Public Methods and Operators

        public static List<InspectorPropertyAttribute> CollectInspectorProperties(
            Type inspectorType,
            ref Dictionary<InspectorPropertyAttribute, InspectorConditionalPropertyAttribute> conditionalInspectors)
        {
            // Access inspector properties.
            var properties = inspectorType.GetProperties();
            var inspectorProperties = new List<InspectorPropertyAttribute>();

            foreach (var property in properties)
            {
                // Find all properties that are to be exposed in the inspector.
                var inspectorPropertyAttributes =
                    property.GetCustomAttributes(typeof(InspectorPropertyAttribute), true) as
                    InspectorPropertyAttribute[];

                if (inspectorPropertyAttributes != null)
                {
                    inspectorProperties.AddRange(inspectorPropertyAttributes);
                }

                // Find all properties whose inspectors are shown only if certain conditions are met.
                var conditionInspector =
                    (InspectorConditionalPropertyAttribute)
                    Attribute.GetCustomAttribute(property, typeof(InspectorConditionalPropertyAttribute));

                if (conditionInspector != null && inspectorPropertyAttributes != null)
                {
                    foreach (var attribute in inspectorPropertyAttributes)
                    {
                        conditionalInspectors.Add(attribute, conditionInspector);
                    }
                }
            }

            return inspectorProperties;
        }

        #endregion
    }
}