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

    /// <summary>
    ///   Custom editor for selecting a member of a specified source MonoBehaviour attached to a Unity game object.
    /// </summary>
    public abstract class MemberReferenceDrawer : PropertyDrawer
    {
        #region Properties

        /// <summary>
        ///   Name of the serialized property that makes up the selected member.
        /// </summary>
        protected abstract string MemberProperty { get; }

        /// <summary>
        ///   Name of the serialized property that makes up the selected source.
        /// </summary>
        protected abstract string SourceProperty { get; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///   Gets the total height of this editor.
        /// </summary>
        /// <param name="property">Property to get the total height of the editor for.</param>
        /// <param name="label">The parameter is not used.</param>
        /// <returns>Total height of this editor.</returns>
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            SerializedProperty sourceProperty = this.GetSourceProperty(property);
            SerializedProperty memberProperty = this.GetMemberProperty(property);

            return EditorGUI.GetPropertyHeight(sourceProperty) + EditorGUI.GetPropertyHeight(memberProperty);
        }

        /// <summary>
        ///   Draws the custom editor for selecting a member of a specified source MonoBehaviour attached to a Unity game object.
        /// </summary>
        /// <param name="position">Position to draw the editor at.</param>
        /// <param name="property">Property to draw the editor for.</param>
        /// <param name="label">Text to show next to the property editor.</param>
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
                string[] names = GetEntryNames(entries, source, fieldName, out index);

                int choice = EditorGUI.Popup(position, null, index, names);
                if (choice > 0)
                {
                    Entry entry = entries[choice - 1];
                    sourceProperty.objectReferenceValue = entry.Target;
                    memberProperty.stringValue = entry.MemberName;
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
        ///   Collects a list of members of the source MonoBehaviour that can be selected in the editor.
        /// </summary>
        protected abstract List<Entry> GetApplicableMembers(GameObject target);

        /// <summary>
        ///   Convert the specified list of applicable entries into a string array.
        /// </summary>
        private static string[] GetEntryNames(
            List<Entry> list, MonoBehaviour selectedSource, string selectedField, out int index)
        {
            int selectedEntry = -1;

            // Fallback is used if no entry matches selected one exactly. 
            // Fallback has same member name and same MonoBehaviour type.
            int fallbackSelectedEntry = -1;

            string[] names = new string[list.Count + 1];

            for (int i = 0; i < list.Count; ++i)
            {
                Entry entry = list[i];
                string entryName = MakeEntryName(entry.Target, entry.MemberName);
                names[i + 1] = entryName;

                if (selectedEntry == -1 && selectedSource != null)
                {
                    if (entry.MemberName == selectedField)
                    {
                        if (entry.Target == selectedSource)
                        {
                            selectedEntry = i;
                        }
                        else if (entry.Target.GetType() == selectedSource.GetType())
                        {
                            fallbackSelectedEntry = i;
                        }
                    }
                }
            }

            if (selectedEntry != -1)
            {
                Entry entry = list[selectedEntry];
                names[0] = MakeEntryName(entry.Target, entry.MemberName);
                index = selectedEntry + 1;
            }
            else if (fallbackSelectedEntry != -1)
            {
                Entry entry = list[fallbackSelectedEntry];
                names[0] = MakeEntryName(entry.Target, entry.MemberName);
                index = fallbackSelectedEntry + 1;
            }
            else
            {
                names[0] = "<Choose>";
                index = 0;
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

        private SerializedProperty GetMemberProperty(SerializedProperty property)
        {
            return property.FindPropertyRelative(this.MemberProperty);
        }

        private SerializedProperty GetSourceProperty(SerializedProperty property)
        {
            return property.FindPropertyRelative(this.SourceProperty);
        }

        #endregion

        /// <summary>
        ///   Applicable member of the specified source MonoBehaviour attached to a Unity game object.
        /// </summary>
        protected class Entry
        {
            #region Fields

            /// <summary>
            ///   Name of the member.
            /// </summary>
            public string MemberName;

            /// <summary>
            ///   Source MonoBehaviour attached to a Unity game object.
            /// </summary>
            public MonoBehaviour Target;

            #endregion
        }
    }
}