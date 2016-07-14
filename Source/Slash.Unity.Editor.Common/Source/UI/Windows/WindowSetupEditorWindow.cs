// --------------------------------------------------------------------------------------------------------------------
// <copyright file="WindowSetupEditorWindow.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Unity.Editor.Common.UI.Windows
{
    using UnityEditor;

    using UnityEngine;

    public class WindowSetupEditorWindow : EditorWindow
    {
        #region Fields

        private string windowId = "NewWindow";

        #endregion

        #region Methods

        protected void OnGUI()
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label("Window Id:", GUILayout.Width(100));
            this.windowId = GUILayout.TextField(this.windowId);
            GUILayout.EndHorizontal();

            if (GUILayout.Button("Create Window"))
            {
                var windowRoot = SetupWindowUtils.CreateWindow(this.windowId);
                Selection.activeGameObject = windowRoot;
                this.Close();
            }
        }

        #endregion
    }
}