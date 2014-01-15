// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InspectorComponentTable.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Tools.BlueprintEditor.Logic.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Slash.GameBase.Inspector.Attributes;
    using Slash.GameBase.Inspector.Utils;

    /// <summary>
    ///   Lookup table for designer components. Avoids expensive reflection
    ///   calls at runtime.
    /// </summary>
    public class InspectorComponentTable
    {
        #region Static Fields

        /// <summary>
        ///   Attributes whose inspectors are shown only if a certain condition is satisfied.
        /// </summary>
        private static Dictionary<InspectorPropertyAttribute, InspectorConditionalPropertyAttribute>
            conditionalInspectors;

        /// <summary>
        ///   Components accessible to the user in the landscape designer.
        /// </summary>
        private static Dictionary<Type, InspectorComponent> inspectorComponents;

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///   Types of components accessible to the user in the landscape designer.
        /// </summary>
        /// <returns>All types of components accessible to the user in the landscape designer.</returns>
        public static IEnumerable<Type> ComponentTypes()
        {
            return inspectorComponents.Keys;
        }

        /// <summary>
        ///   Gets the designer component for the specified type.
        /// </summary>
        /// <param name="type">Type to get the designer component for.</param>
        /// <returns>Designer component for the specified type.</returns>
        public static InspectorComponent GetComponent(Type type)
        {
            return inspectorComponents[type];
        }

        /// <summary>
        ///   Gets the condition for the specified attribute to have its inspector shown,
        ///   or <c>null</c> if the inspector should always be shown.
        /// </summary>
        /// <param name="attribute">Attribute to get the inspector condition of.</param>
        /// <returns>
        ///   Condition for the specified attribute to have its inspector shown,
        ///   or <c>null</c> if the inspector should always be shown.
        /// </returns>
        public static InspectorConditionalPropertyAttribute GetCondition(InspectorPropertyAttribute attribute)
        {
            InspectorConditionalPropertyAttribute condition;
            conditionalInspectors.TryGetValue(attribute, out condition);
            return condition;
        }

        /// <summary>
        ///   Whether the specified component type is accessible to the user in the landscape designer.
        /// </summary>
        /// <param name="type">Type to check.</param>
        /// <returns>
        ///   <c>true</c>, if the specified component type is accessible to the user in the landscape designer, and
        ///   <c>false</c>, otherwise.
        /// </returns>
        public static bool HasComponent(Type type)
        {
            return inspectorComponents.ContainsKey(type);
        }

        /// <summary>
        ///   Finds all components accessible to the user in the landscape designer via reflection.
        /// </summary>
        public static void LoadComponents()
        {
            inspectorComponents = new Dictionary<Type, InspectorComponent>();
            conditionalInspectors = new Dictionary<InspectorPropertyAttribute, InspectorConditionalPropertyAttribute>();

            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                var designerComponentTypes =
                    assembly.GetTypes().Where(type => Attribute.IsDefined(type, typeof(InspectorComponentAttribute)));

                foreach (var designerComponentType in designerComponentTypes)
                {
                    List<InspectorPropertyAttribute> inspectorProperties =
                        InspectorUtils.CollectInspectorProperties(designerComponentType, ref conditionalInspectors);

                    var component = new InspectorComponent
                        {
                            Type = designerComponentType,
                            Component =
                                designerComponentType.GetCustomAttributes(typeof(InspectorComponentAttribute), false)[0] as
                                InspectorComponentAttribute,
                            Properties = inspectorProperties
                        };

                    inspectorComponents.Add(designerComponentType, component);
                }
            }
        }


        #endregion
    }
}