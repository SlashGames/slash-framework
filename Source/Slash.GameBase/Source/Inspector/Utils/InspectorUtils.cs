// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InspectorUtils.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.GameBase.Inspector.Utils
{
    using System;
    using System.Collections.Generic;

    using Slash.Collections.AttributeTables;
    using Slash.GameBase.Inspector.Attributes;
    using Slash.GameBase.Inspector.Data;

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
                    // Inject property info.
                    foreach (var inspectorPropertyAttribute in inspectorPropertyAttributes)
                    {
                        inspectorPropertyAttribute.PropertyType = property.PropertyType;
                        inspectorPropertyAttribute.PropertyName = property.Name;
                    }

                    inspectorProperties.AddRange(inspectorPropertyAttributes);
                }

                if (conditionalInspectors != null)
                {
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
            }

            return inspectorProperties;
        }

        public static T CreateFromAttributeTable<T>(Game game, InspectorType inspectorType, IAttributeTable attributeTable)
            where T : class
        {
            // Create object.
            T obj = (T)Activator.CreateInstance(inspectorType.Type);

            // Init object.
            InitFromAttributeTable(game, inspectorType, obj, attributeTable);

            return obj;
        }

        /// <summary>
        ///   Initializes an object by getting its inspector properties via reflection and
        ///   look them up in the specified attribute table.
        /// </summary>
        /// <param name="obj">Object to initialize.</param>
        /// <param name="attributeTable">Attribute table to initialize from.</param>
        public static void InitFromAttributeTable(Game game, object obj, IAttributeTable attributeTable)
        {
            InspectorType inspectorType = InspectorType.GetInspectorType(obj.GetType());
            InitFromAttributeTable(game, inspectorType, obj, attributeTable);
        }

        /// <summary>
        ///   Initializes an object by getting its inspector properties from the specified inspector type
        ///   and look them up in the specified attribute table.
        /// </summary>
        /// <param name="inspectorType">Contains information about the properties of the object.</param>
        /// <param name="obj">Object to initialize.</param>
        /// <param name="attributeTable">Attribute table to initialize from.</param>
        public static void InitFromAttributeTable(Game game,
            InspectorType inspectorType, object obj, IAttributeTable attributeTable)
        {
            // Set values for all properties.
            foreach (var inspectorProperty in inspectorType.Properties)
            {
                // Get value from attribute table or default.
                object propertyValue = attributeTable.GetValueOrDefault(
                    inspectorProperty.Name, inspectorProperty.Default);

                inspectorProperty.SetPropertyValue(game, obj, propertyValue);
            }
        }

        #endregion
    }
}