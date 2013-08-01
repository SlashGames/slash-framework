// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ExposeProperties.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Unity.Editor.Common.Inspectors
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;

    using Slash.Unity.Common.Attributes;

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

        public static void Expose(IEnumerable<PropertyField> properties)
        {
            GUILayoutOption[] emptyOptions = new GUILayoutOption[0];

            EditorGUILayout.BeginVertical(emptyOptions);

            foreach (PropertyField field in properties)
            {
                object value = field.GetValue();
                if (value == null)
                {
                    continue;
                }

                // Convert value if conversion function is provided.
                if (field.GetConversionFunc != null)
                {
                    value = field.GetConversionFunc(value);
                }

                EditorGUILayout.BeginHorizontal(emptyOptions);

                object newValue = null;
                switch (field.Type)
                {
                    case SerializedPropertyType.Integer:
                        newValue = EditorGUILayout.IntField(field.Name, (int)value, emptyOptions);
                        break;

                    case SerializedPropertyType.Float:
                        newValue = EditorGUILayout.FloatField(field.Name, (float)value, emptyOptions);
                        break;

                    case SerializedPropertyType.Boolean:
                        newValue = EditorGUILayout.Toggle(field.Name, (bool)value, emptyOptions);
                        break;

                    case SerializedPropertyType.String:
                        newValue = EditorGUILayout.TextField(field.Name, (String)value, emptyOptions);
                        break;

                    case SerializedPropertyType.Vector2:
                        newValue = EditorGUILayout.Vector2Field(field.Name, (Vector2)value, emptyOptions);
                        break;

                    case SerializedPropertyType.Vector3:
                        newValue = EditorGUILayout.Vector3Field(field.Name, (Vector3)value, emptyOptions);
                        break;

                    case SerializedPropertyType.Enum:
                        newValue = EditorGUILayout.EnumPopup(field.Name, (Enum)value, emptyOptions);
                        break;

                    case SerializedPropertyType.Rect:
                        {
                            newValue = EditorGUILayout.RectField(field.Name, (Rect)value, emptyOptions);
                        }
                        break;
                }

                // Convert new value if conversion function is provided.
                if (field.SetConversionFunc != null)
                {
                    newValue = field.SetConversionFunc(newValue);
                }

                // Set new value.
                field.SetValue(newValue);

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
                // Make sure property has a public getter.
                if (!info.CanRead)
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
                Func<object, object> getConversionFunc;
                Func<object, object> setConversionFunc;
                if (!PropertyField.GetPropertyType(info, out type, out getConversionFunc, out setConversionFunc))
                {
                    continue;
                }

                PropertyField field = new PropertyField(obj, info, type)
                    {
                        GetConversionFunc = getConversionFunc,
                        SetConversionFunc = setConversionFunc
                    };
                fields.Add(field);
            }

            return fields.ToArray();
        }

        #endregion
    }
}