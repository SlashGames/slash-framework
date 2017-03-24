// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ShaderContext.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Unity.Editor.Common.Inspectors.Utils
{
    using UnityEditor;

    using UnityEditorInternal;

    using UnityEngine;

    /// <summary>
    ///   Wrapper for exposing shaders in the Unity inspector.
    /// </summary>
    public class ShaderContext : ScriptableObject
    {
        #region Fields

        /// <summary>
        ///   Name of the shader.
        /// </summary>
        public string SelectedShader;

        private MenuCommand mc;

        #endregion
        
        #region Public Methods and Operators

        /// <summary>
        ///   Draws an inspector for this shader.
        /// </summary>
        /// <param name="r">GUI rectangle to draw the inspector to.</param>
        public void DisplayShaderContext(Rect r)
        {
            if (this.mc == null)
            {
                this.mc = new MenuCommand(this, 0);
            }

            Shader shader = string.IsNullOrEmpty(this.SelectedShader) ? null : Shader.Find(this.SelectedShader);
            Material temp = shader != null ? new Material(shader) : null;

            // Rebuild shader menu:
            InternalEditorUtility.SetupShaderMenu(temp);

            // Destroy temporary material.
            if (shader != null)
            {
                DestroyImmediate(temp, true);
            }

            // Display shader popup:
            EditorUtility.DisplayPopupMenu(r, "CONTEXT/ShaderPopup", this.mc);
        }

        #endregion
    }
}