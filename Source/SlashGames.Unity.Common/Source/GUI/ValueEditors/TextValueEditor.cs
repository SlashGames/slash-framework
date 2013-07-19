// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TextValueEditor.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Slash.Unity.Common.GUI.ValueEditors
{
    using UnityEngine;

    /// <summary>
    ///   Editor to change a string value.
    /// </summary>
    public class TextValueEditor : IValueEditor
    {
        /// <summary>
        ///   Edits the specified context.
        /// </summary>
        /// <param name="context">Editor context to work with.</param>
        public void Edit(IValueEditorContext context)
        {
            // Get value.
            object value = context.Value;
            string oldValue = value != null ? (string)value : string.Empty;

            // Show editor.
            GUILayout.BeginHorizontal();
            GUILayout.Label(context.Name);
            string newValue = GUILayout.TextField(oldValue);
            GUILayout.EndHorizontal();

            // Set new value.
            if (newValue != oldValue)
            {
                context.Value = newValue;
            }
        }
    }
}