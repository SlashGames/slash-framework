// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ViewEventDelegateDrawer.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Unity.Editor.Common.Inspectors.ViewModels
{
    using System.Collections.Generic;
    using System.Reflection;

    using Slash.Unity.Common.ViewModels;

    using UnityEditor;

    using UnityEngine;

    [CustomPropertyDrawer(typeof(ViewEventDelegate))]
    public class ViewEventDelegateDrawer : PropertyDrawer
    {
        #region Public Methods and Operators

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            SerializedProperty sourceProperty = GetSourceProperty(property);
            SerializedProperty fieldProperty = GetFieldProperty(property);

            return EditorGUI.GetPropertyHeight(sourceProperty) + EditorGUI.GetPropertyHeight(fieldProperty);
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            SerializedProperty sourceProperty = GetSourceProperty(property);
            SerializedProperty fieldProperty = GetFieldProperty(property);

            Rect sourceFieldRect = position;
            sourceFieldRect.height = EditorGUI.GetPropertyHeight(sourceProperty);
            EditorGUI.PropertyField(sourceFieldRect, sourceProperty, label);

            position.yMin += sourceFieldRect.height;

            // Get all routed events of the source object.
            MonoBehaviour source = sourceProperty.objectReferenceValue as MonoBehaviour;
            if (source != null)
            {
                string fieldName = fieldProperty.stringValue;

                List<Entry> routedEvents = GetRoutedEvents(source.gameObject);

                int index;
                string selectedName = MakeEventName(source, fieldName);
                string[] names = GetRoutedEventNames(routedEvents, selectedName, out index);

                int choice = EditorGUI.Popup(position, null, index, names);
                if (choice > 0)
                {
                    if (choice != index)
                    {
                        Entry entry = routedEvents[choice - 1];
                        sourceProperty.objectReferenceValue = entry.Target;
                        fieldProperty.stringValue = entry.FieldInfo.Name;
                    }
                }
            }
            else
            {
                sourceProperty.objectReferenceValue = null;
                fieldProperty.stringValue = null;
            }
        }

        #endregion

        #region Methods

        private static SerializedProperty GetFieldProperty(SerializedProperty property)
        {
            return property.FindPropertyRelative("field");
        }

        /// <summary>
        ///   Convert the specified list of delegate entries into a string array.
        /// </summary>
        private static string[] GetRoutedEventNames(List<Entry> list, string choice, out int index)
        {
            index = 0;
            string[] names = new string[list.Count + 1];
            names[0] = string.IsNullOrEmpty(choice) ? "<Choose>" : choice;

            for (int i = 0; i < list.Count;)
            {
                Entry ent = list[i];
                string del = MakeEventName(ent.Target, ent.FieldInfo.Name);
                names[++i] = del;

                if (index == 0 && string.Equals(del, choice))
                {
                    index = i;
                }
            }
            return names;
        }

        /// <summary>
        ///   Collect a list of usable routed events from the specified target game object.
        /// </summary>
        private static List<Entry> GetRoutedEvents(GameObject target)
        {
            MonoBehaviour[] components = target.GetComponents<MonoBehaviour>();

            List<Entry> list = new List<Entry>();

            foreach (MonoBehaviour monoBehaviour in components)
            {
                if (monoBehaviour == null)
                {
                    continue;
                }

                FieldInfo[] fields = monoBehaviour.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public);

                foreach (FieldInfo fieldInfo in fields)
                {
                    if (fieldInfo.FieldType != typeof(ViewEvent))
                    {
                        continue;
                    }

                    Entry entry = new Entry { Target = monoBehaviour, FieldInfo = fieldInfo };
                    list.Add(entry);
                }
            }
            return list;
        }

        private static SerializedProperty GetSourceProperty(SerializedProperty property)
        {
            return property.FindPropertyRelative("source");
        }

        private static string MakeEventName(MonoBehaviour source, string fieldName)
        {
            if (source == null || string.IsNullOrEmpty(fieldName))
            {
                return null;
            }
            string type = source.GetType().Name;
            return type + "." + fieldName;
        }

        #endregion

        private class Entry
        {
            #region Fields

            public FieldInfo FieldInfo;

            public MonoBehaviour Target;

            #endregion
        }
    }
}