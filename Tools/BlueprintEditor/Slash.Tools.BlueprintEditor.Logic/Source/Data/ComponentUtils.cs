// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ComponentUtils.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Tools.BlueprintEditor.Logic.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    using Slash.GameBase.Attributes;

    public static class ComponentUtils
    {
        #region Public Methods and Operators

        /// <summary>
        ///   Searches all loaded assemblies and returns the component types which have the LandscapeDesignerComponent attribute.
        /// </summary>
        /// <returns>List of found component types.</returns>
        public static IEnumerable<Type> FindComponentTypes(IEnumerable<Assembly> assemblies)
        {
            List<Type> componentTypes = new List<Type>();
            foreach (Assembly assembly in assemblies)
            {
                componentTypes.AddRange(
                    assembly.GetTypes()
                            .Where(type => Attribute.IsDefined(type, typeof(InspectorComponentAttribute))));
            }

            return componentTypes;
        }

        public static List<InspectorPropertyAttribute> CollectInspectorProperties(Type designerComponentType, ref Dictionary<InspectorPropertyAttribute, InspectorConditionalPropertyAttribute> conditionalInspectors)
        {
            // Access inspector properties.
            var properties = designerComponentType.GetProperties();
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