// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Vector2IValueEditor.cs" company="Rainy Games">
//   Copyright (c) Rainy Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace RainyGames.Unity.Common.GUI.ValueEditors
{
    using System.Globalization;

    using RainyGames.Math.Algebra.Vectors;

    using UnityEngine;

    public class Vector2IValueEditor : IValueEditor
    {
        #region Public Methods and Operators

        /// <summary>
        ///   Edits the specified context.
        /// </summary>
        /// <param name="context"> Editor context to work with. </param>
        public void Edit(IValueEditorContext context)
        {
            // Get value.
            Vector2I oldValue = context.GetValue<Vector2I>();

            // Show editor.
            GUILayout.BeginHorizontal();
            GUILayout.Label(context.Name);
            int newX;
            string newXString = GUILayout.TextField(oldValue.X.ToString(CultureInfo.InvariantCulture));
            if (string.IsNullOrEmpty(newXString))
            {
                newX = 0;
            }
            else if (!int.TryParse(newXString, out newX))
            {
                newX = oldValue.X;
            }

            int newY;
            string newYString = GUILayout.TextField(oldValue.Y.ToString(CultureInfo.InvariantCulture));
            if (string.IsNullOrEmpty(newYString))
            {
                newY = 0;
            }
            else if (!int.TryParse(newYString, out newY))
            {
                newY = oldValue.Y;
            }
            Vector2I newValue = new Vector2I(newX, newY);
            GUILayout.EndHorizontal();

            // Set new value.
            if (newValue != oldValue)
            {
                context.Value = newValue;
            }
        }

        #endregion
    }
}