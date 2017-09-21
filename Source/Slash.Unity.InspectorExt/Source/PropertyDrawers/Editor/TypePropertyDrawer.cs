// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TypePropertyDrawer.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Unity.InspectorExt.PropertyDrawers.Editor
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Slash.Reflection.Utils;
    using Slash.Unity.InspectorExt.PropertyDrawers;
    using UnityEditor;
    using UnityEngine;

    /// <summary>
    ///   Property drawer to select a type that is derived from a base type from a popup.
    /// </summary>
    [CustomPropertyDrawer(typeof(TypePropertyAttribute))]
    public class TypePropertyDrawer : PropertyDrawer
    {
        /// <inheritdoc />
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var typeAttribute = this.attribute as TypePropertyAttribute;
            if (typeAttribute == null)
            {
                return;
            }

            if (typeAttribute.BaseType == null)
            {
                EditorGUILayout.HelpBox("No base type specified", MessageType.Warning);
                return;
            }

            // Get types.
            List<Type> types;
            GUIContent[] typeNames;
            FindTypes(typeAttribute.BaseType, typeAttribute.UseFullName, out types, out typeNames);

            var typeString = property.stringValue;
            Type type = null;
            try
            {
                type = ReflectionUtils.FindType(typeString);
            }
            catch (TypeLoadException)
            {
                Debug.LogWarningFormat("Couldn't load type '{0}', reset type", typeString);
                property.stringValue = string.Empty;
            }

            // Find all available context classes.
            var contextTypeIndex = types.IndexOf(type);
            var newContextTypeIndex = EditorGUI.Popup(position, label, contextTypeIndex, typeNames);
            if (newContextTypeIndex != contextTypeIndex)
            {
                type = types[newContextTypeIndex];
                property.stringValue = type.FullName;
            }
        }

        private static void FindTypes(Type baseType, bool useFullName, out List<Type> types, out GUIContent[] typeNames)
        {
            types = new List<Type> { null };
            var availableConcreteTypes =
                ReflectionUtils.FindTypesWithBase(baseType).Where(type => !type.IsAbstract).ToList();
            availableConcreteTypes.Sort(
                (typeA, typeB) => string.Compare(typeA.FullName, typeB.FullName, StringComparison.Ordinal));
            types.AddRange(availableConcreteTypes);
            typeNames =
                types.Select(type => new GUIContent(type != null ? (useFullName ? type.FullName : type.Name) : "None"))
                    .ToArray();
        }
    }
}