// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MemberReferenceDrawer.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Unity.Editor.Common.Inspectors.Utils
{
    using System.Collections.Generic;

    using UnityEditor;

    using UnityEngine;

    public abstract class MemberReferenceDrawer : PropertyDrawer
    {
        #region Public Methods and Operators

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            SerializedProperty sourceProperty = this.GetSourceProperty(property);
            SerializedProperty memberProperty = this.GetMemberProperty(property);

            return EditorGUI.GetPropertyHeight(sourceProperty) + EditorGUI.GetPropertyHeight(memberProperty);
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            SerializedProperty sourceProperty = this.GetSourceProperty(property);
            SerializedProperty memberProperty = this.GetMemberProperty(property);

            Rect sourceFieldRect = position;
            sourceFieldRect.height = EditorGUI.GetPropertyHeight(sourceProperty);
            EditorGUI.PropertyField(sourceFieldRect, sourceProperty, label);

            position.yMin += sourceFieldRect.height;

            // Get all applicable members of the source object.
            MonoBehaviour source = sourceProperty.objectReferenceValue as MonoBehaviour;
            if (source != null)
            {
                string fieldName = memberProperty.stringValue;

                List<Entry> entries = this.GetApplicableMembers(source.gameObject);

                int index;
                string selectedName = MakeEntryName(source, fieldName);
                string[] names = GetEntryNames(entries, selectedName, out index);

                int choice = EditorGUI.Popup(position, null, index, names);
                if (choice > 0)
                {
                    if (choice != index)
                    {
                        Entry entry = entries[choice - 1];
                        sourceProperty.objectReferenceValue = entry.Target;
                        memberProperty.stringValue = entry.MemberName;
                    }
                }
            }
            else
            {
                sourceProperty.objectReferenceValue = null;
                memberProperty.stringValue = null;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        ///   Collect a list of usable routed events from the specified target game object.
        /// </summary>
        protected abstract List<Entry> GetApplicableMembers(GameObject target);

        protected SerializedProperty GetMemberProperty(SerializedProperty property)
        {
            return property.FindPropertyRelative(MemberProperty);
        }

        protected SerializedProperty GetSourceProperty(SerializedProperty property)
        {
            return property.FindPropertyRelative(SourceProperty);
        }

        protected abstract string SourceProperty { get; }

        protected abstract string MemberProperty { get; }

        /// <summary>
        ///   Convert the specified list of delegate entries into a string array.
        /// </summary>
        private static string[] GetEntryNames(List<Entry> list, string choice, out int index)
        {
            index = 0;
            string[] names = new string[list.Count + 1];
            names[0] = string.IsNullOrEmpty(choice) ? "<Choose>" : choice;

            for (int i = 0; i < list.Count;)
            {
                Entry ent = list[i];
                string del = MakeEntryName(ent.Target, ent.MemberName);
                names[++i] = del;

                if (index == 0 && string.Equals(del, choice))
                {
                    index = i;
                }
            }
            return names;
        }

        private static string MakeEntryName(MonoBehaviour source, string memberName)
        {
            if (source == null || string.IsNullOrEmpty(memberName))
            {
                return null;
            }
            string type = source.GetType().Name;
            return type + "." + memberName;
        }

        #endregion

        protected class Entry
        {
            #region Fields

            public string MemberName;

            public MonoBehaviour Target;

            #endregion
        }
    }
}