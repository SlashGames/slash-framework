// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InspectorUtils.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.ECS.Inspector.Utils
{
    using System;
    using System.Collections.Generic;

    using Slash.Collections.AttributeTables;
    using Slash.ECS.Components;
    using Slash.ECS.Inspector.Attributes;
    using Slash.ECS.Inspector.Data;
    using Slash.Reflection.Extensions;

#if WINDOWS_STORE
    using System.Reflection;
#endif

    /// <summary>
    ///   Utility methods for collecting inspector data and initializing objects.
    /// </summary>
    public static class InspectorUtils
    {
        #region Public Methods and Operators

        /// <summary>
        ///   Returns inspector data for all inspectable properties of the specified type.
        /// </summary>
        /// <param name="inspectorType">Type to get inspector data for.</param>
        /// <param name="conditionalInspectors">Dictionary to be filled with conditions for inspectors to be shown.</param>
        /// <returns>Inspector data for all inspectable properties of the specified type.</returns>
        public static List<InspectorPropertyAttribute> CollectInspectorProperties(
            Type inspectorType,
            ref Dictionary<InspectorPropertyAttribute, InspectorConditionalPropertyAttribute> conditionalInspectors)
        {
            // Access inspector properties.
            var properties = inspectorType.GetInstanceProperties();
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
                    var conditionInspector = property.GetCustomAttribute<InspectorConditionalPropertyAttribute>();

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

        /// <summary>
        ///   Creates an object of the specified type and initializes it with
        ///   values from the passed attribute table, or default values of the
        ///   respective inspector properties, if no attribute value is present.
        /// </summary>
        /// <typeparam name="T">Type of the object to create.</typeparam>
        /// <param name="entityManager">
        ///   Entity manager to use for initializing the object, e.g. for creating entities from entity
        ///   configuration attributes.
        /// </param>
        /// <param name="inspectorType">Inspector data of the type of the object to create.</param>
        /// <param name="attributeTable">Attribute table to initialize the object with.</param>
        /// <returns>Initialized new object of the specified type.</returns>
        public static T CreateFromAttributeTable<T>(
            EntityManager entityManager,
            InspectorType inspectorType,
            IAttributeTable attributeTable) where T : class
        {
            // Create object.
            T obj = (T)Activator.CreateInstance(inspectorType.Type);

            // Init object.
            InitFromAttributeTable(entityManager, inspectorType, obj, attributeTable);

            return obj;
        }

        /// <summary>
        ///   Deinitializes the specified object that is of the specified inspector type.
        /// </summary>
        /// <param name="entityManager">Entity manager.</param>
        /// <param name="inspectorType">Inspector type of the specified object.</param>
        /// <param name="obj">Object to deinitialize.</param>
        public static void Deinit(EntityManager entityManager, InspectorType inspectorType, object obj)
        {
            // Unset values for all properties.
            foreach (var inspectorProperty in inspectorType.Properties)
            {
                inspectorProperty.Deinit(entityManager, obj);
            }
        }

        /// <summary>
        ///   Initializes an object by getting its inspector properties via reflection and
        ///   look them up in the specified attribute table.
        /// </summary>
        /// <param name="entityManager">
        ///   Entity manager to use for initializing the object, e.g. for creating entities from entity
        ///   configuration attributes.
        /// </param>
        /// <param name="obj">Object to initialize.</param>
        /// <param name="attributeTable">Attribute table to initialize from.</param>
        public static void InitFromAttributeTable(
            EntityManager entityManager,
            object obj,
            IAttributeTable attributeTable)
        {
            InspectorType inspectorType = InspectorType.GetInspectorType(obj.GetType());
            if (inspectorType == null)
            {
                return;
            }

            InitFromAttributeTable(entityManager, inspectorType, obj, attributeTable);
        }

        /// <summary>
        ///   Initializes an object by getting its inspector properties from the specified inspector type
        ///   and look them up in the specified attribute table.
        /// </summary>
        /// <param name="entityManager">
        ///   Entity manager to use for initializing the object, e.g. for creating entities from entity
        ///   configuration attributes.
        /// </param>
        /// <param name="inspectorType">Contains information about the properties of the object.</param>
        /// <param name="obj">Object to initialize.</param>
        /// <param name="attributeTable">Attribute table to initialize from.</param>
        public static void InitFromAttributeTable(
            EntityManager entityManager,
            InspectorType inspectorType,
            object obj,
            IAttributeTable attributeTable)
        {
            // Set values for all properties.
            foreach (var inspectorProperty in inspectorType.Properties)
            {
                // Get value from attribute table or default.
                object propertyValue = attributeTable != null
                    ? attributeTable.GetValueOrDefault(inspectorProperty.Name, inspectorProperty.Default)
                    : inspectorProperty.Default;

                inspectorProperty.SetPropertyValue(entityManager, obj, propertyValue);
            }
        }

        #endregion
    }
}