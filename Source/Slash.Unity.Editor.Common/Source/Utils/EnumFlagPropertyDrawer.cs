// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EnumFlagPropertyDrawer.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Unity.Editor.Common.Utils
{
    using System;

    using Slash.Unity.Common.Utils;

    using UnityEditor;

    using UnityEngine;

    [CustomPropertyDrawer(typeof(EnumFlagAttribute))]
    public class EnumFlagDrawer : PropertyDrawer
    {
        #region Public Methods and Operators

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var flagSettings = (EnumFlagAttribute)this.attribute;
            var targetEnum = GetBaseProperty<Enum>(property);

            var propName = flagSettings.Name;
            if (string.IsNullOrEmpty(propName))
            {
                propName = property.name;
            }

            EditorGUI.BeginProperty(position, label, property);
            var enumNew = EditorGUI.EnumMaskField(position, propName, targetEnum);
            var convertedType = Convert.ChangeType(enumNew, targetEnum.GetType());
            if (convertedType != null)
            {
                property.intValue = (int)convertedType;
            }
            EditorGUI.EndProperty();
        }

        #endregion

        #region Methods

        private static T GetBaseProperty<T>(SerializedProperty prop)
        {
            // Separate the steps it takes to get to this property
            var separatedPaths = prop.propertyPath.Split('.');

            // Go down to the root of this serialized property
            var reflectionTarget = prop.serializedObject.targetObject as object;
            // Walk down the path to get the target object
            foreach (var path in separatedPaths)
            {
                var fieldInfo = reflectionTarget.GetType().GetField(path);
                reflectionTarget = fieldInfo.GetValue(reflectionTarget);
            }
            return (T)reflectionTarget;
        }

        #endregion
    }
}