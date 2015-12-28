// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InspectorType.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.ECS.Inspector.Data
{
    using System;
    using System.Collections.Generic;

    using Slash.ECS.Inspector.Attributes;
    using Slash.ECS.Inspector.Utils;
    using Slash.Reflection.Extensions;

    /// <summary>
    ///   Component accessible to the user in the inspector.
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

        /// <summary>
        ///   Gets inspector data such as name and description for the
        ///   specified type.
        /// </summary>
        /// <param name="type">Type to get inspector data for.</param>
        /// <returns>Inspector data for the specified type.</returns>
        public static InspectorType GetInspectorType(Type type)
        {
            Dictionary<InspectorPropertyAttribute, InspectorConditionalPropertyAttribute> tmpConditionalInspectors =
                null;
            return GetInspectorType(type, ref tmpConditionalInspectors);
        }

        /// <summary>
        ///   Gets inspector data such as name and description for the
        ///   specified type.
        /// </summary>
        /// <param name="type">Type to get inspector data for.</param>
        /// <param name="conditionalInspectors">Dictionary to be filled with conditions for inspectors to be shown.</param>
        /// <returns>Inspector data for the specified type.</returns>
        public static InspectorType GetInspectorType(
            Type type,
            ref Dictionary<InspectorPropertyAttribute, InspectorConditionalPropertyAttribute> conditionalInspectors)
        {
            List<InspectorPropertyAttribute> inspectorProperties = InspectorUtils.CollectInspectorProperties(
                type, ref conditionalInspectors);

            var inspectorTypeAttribute = type.GetAttribute<InspectorTypeAttribute>();

            if (inspectorTypeAttribute == null)
            {
                return null;
            }

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