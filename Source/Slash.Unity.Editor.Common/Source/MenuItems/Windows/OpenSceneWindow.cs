// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OpenSceneWindow.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Unity.Editor.Common.MenuItems.Windows
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    using UnityEditor;
    using UnityEditor.SceneManagement;

    using UnityEngine;

    /// <summary>
    ///   Menu for changing scenes in the editor.
    /// </summary>
    public class OpenSceneWindow : EditorWindow
    {
        #region Constants

        /// <summary>
        ///   Scenes accessible from the Open Scene window.
        /// </summary>
        private static IEnumerable<FileInfo> gameScenes;

        /// <summary>
        ///   Asset folders to ignore when building the scene list.
        /// </summary>
        private static readonly string[] IgnoredFolders = new[] { "NGUI", "NData" };

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///   Shows the menu for changing scenes in the editor.
        /// </summary>
        [MenuItem("Slash Games/Windows/Open Scene")]
        public static void OpenScene()
        {
            GetWindow(typeof(OpenSceneWindow), false, "Open Scene").Show();
        }

        #endregion

        #region Methods

        private void OnGUI()
        {
            if (gameScenes == null)
            {
                // Search all non-ignored directories for scene files.
                DirectoryInfo directory = new DirectoryInfo(Application.dataPath);
                FileInfo[] sceneFiles = directory.GetFiles("*.unity", SearchOption.AllDirectories);
                gameScenes =
                    sceneFiles.Where(
                        file => IgnoredFolders.All(ignoredFolder => !file.FullName.Contains(ignoredFolder)));
            }

            foreach (var gameScene in gameScenes)
            {
                if (GUILayout.Button(gameScene.Name.Substring(0, gameScene.Name.IndexOf('.'))))
                {
                    EditorSceneManager.OpenScene(gameScene.FullName);
                }
            }
        }

        #endregion
    }
}