// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TypePropertyDrawer.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Unity.Editor.Common.PropertyDrawers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Slash.Reflection.Utils;
    using Slash.Unity.Common.PropertyDrawers;

    using UnityEditor;

    using UnityEngine;

    [CustomPropertyDrawer(typeof(TypePropertyAttribute))]
    public class TypePropertyDrawer : PropertyDrawer
    {
        #region Public Methods and Operators

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var typeAttribute = this.attribute as TypePropertyAttribute;
            if (typeAttribute == null)
            {
                return;
            }

            // Get types.
            List<Type> types;
            string[] typeNames;
            this.FindTypes(typeAttribute.BaseType, out types, out typeNames);

            string typeString = property.stringValue;
            Type type = ReflectionUtils.FindType(typeString);

            // Find all available context classes.
            int contextTypeIndex = types.IndexOf(type);
            int newContextTypeIndex = EditorGUI.Popup(position, "Context", contextTypeIndex, typeNames);
            if (newContextTypeIndex != contextTypeIndex)
            {
                type = types[newContextTypeIndex];
                property.stringValue = type.FullName;
            }
        }

        #endregion

        #region Methods

        private void FindTypes(Type baseType, out List<Type> types, out string[] typeNames)
        {
            types = new List<Type> { null };
            var availableContextTypes =
                ReflectionUtils.FindTypesWithBase(baseType).Where(type => !type.IsAbstract).ToList();
            availableContextTypes.Sort(
                (typeA, typeB) => String.Compare(typeA.FullName, typeB.FullName, StringComparison.Ordinal));
            types.AddRange(availableContextTypes);
            typeNames = types.Select(type => type != null ? type.FullName : "None").ToArray();
        }

        #endregion
    }
}