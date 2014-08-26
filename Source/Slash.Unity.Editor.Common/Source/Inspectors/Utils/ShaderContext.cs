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

        private readonly Material dummyMaterial;

        private MenuCommand mc;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///   Creates a new, empty wrapper for exposing shaders in the Unity inspector.
        /// </summary>
        public ShaderContext()
        {
            // Create dummy material to make it not highlight any shaders inside:
            const string TmpStr = "Shader \"Hidden/tmp_shdr\"{SubShader{Pass{}}}";
            this.dummyMaterial = new Material(TmpStr);
        }

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
            Material temp = shader != null ? new Material(shader) : this.dummyMaterial;

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