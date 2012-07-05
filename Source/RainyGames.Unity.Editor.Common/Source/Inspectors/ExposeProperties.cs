// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ExposeProperties.cs" company="Rainy Games">
//   Copyright (c) Rainy Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace RainyGames.Unity.Editor.Common.Inspectors
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;

    using RainyGames.Unity.Common.Attributes;

    using UnityEditor;

    using UnityEngine;

    using Object = System.Object;

    /// <summary>
    ///   Helper class to show all properties of a mono behaviour which are flagged with the ExposeProperty attribute
    ///   in the inspector.
    /// </summary>
    public static class ExposeProperties
    {
        #region Public Methods and Operators

        public static void Expose(PropertyField[] properties)
        {
            GUILayoutOption[] emptyOptions = new GUILayoutOption[0];

            EditorGUILayout.BeginVertical(emptyOptions);

            foreach (PropertyField field in properties)
            {
                EditorGUILayout.BeginHorizontal(emptyOptions);

                switch (field.Type)
                {
                    case SerializedPropertyType.Integer:
                        field.SetValue(EditorGUILayout.IntField(field.Name, (int)field.GetValue(), emptyOptions));
                        break;

                    case SerializedPropertyType.Float:
                        field.SetValue(EditorGUILayout.FloatField(field.Name, (float)field.GetValue(), emptyOptions));
                        break;

                    case SerializedPropertyType.Boolean:
                        field.SetValue(EditorGUILayout.Toggle(field.Name, (bool)field.GetValue(), emptyOptions));
                        break;

                    case SerializedPropertyType.String:
                        field.SetValue(EditorGUILayout.TextField(field.Name, (String)field.GetValue(), emptyOptions));
                        break;

                    case SerializedPropertyType.Vector2:
                        field.SetValue(
                            EditorGUILayout.Vector2Field(field.Name, (Vector2)field.GetValue(), emptyOptions));
                        break;

                    case SerializedPropertyType.Vector3:
                        field.SetValue(
                            EditorGUILayout.Vector3Field(field.Name, (Vector3)field.GetValue(), emptyOptions));
                        break;

                    case SerializedPropertyType.Enum:
                        field.SetValue(EditorGUILayout.EnumPopup(field.Name, (Enum)field.GetValue(), emptyOptions));
                        break;
                }

                EditorGUILayout.EndHorizontal();
            }

            EditorGUILayout.EndVertical();
        }

        public static PropertyField[] GetProperties(Object obj)
        {
            List<PropertyField> fields = new List<PropertyField>();

            PropertyInfo[] infos = obj.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (PropertyInfo info in infos)
            {
                // Make sure property has a public getter and setter.
                if (!info.CanRead || !info.CanWrite)
                {
                    continue;
                }

                object[] attributes = info.GetCustomAttributes(typeof(ExposePropertyAttribute), true);
                bool isExposed = attributes.Length > 0;
                if (!isExposed)
                {
                    continue;
                }

                // Check that property has a supported type.
                SerializedPropertyType type;
                if (!PropertyField.GetPropertyType(info, out type))
                {
                    continue;
                }

                PropertyField field = new PropertyField(obj, info, type);
                fields.Add(field);
            }

            return fields.ToArray();
        }

        #endregion
    }
}