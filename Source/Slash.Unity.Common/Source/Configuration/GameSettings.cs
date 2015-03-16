// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GameSettings.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Unity.Common.Configuration
{
#if UNITY_EDITOR
    using UnityEditor;
#endif
    using System.IO;

    using UnityEngine;

    public class GameSettings : ScriptableObject
    {
        #region Constants

        private const string DefaultGameFolder = "Game";

        #endregion

        #region Methods

        protected static string BuildResourceFilename(string filename)
        {
            return string.Format("{0}.asset", filename);
        }

        protected static string BuildResourceFilename<T>()
        {
            return BuildResourceFilename(typeof(T).Name);
        }

        protected static string BuildResourceFolder()
        {
            return BuildResourceFolder(DefaultGameFolder);
        }

        protected static string BuildResourceFolder(string gameFolder)
        {
            return string.Format("{0}/Resources/", gameFolder);
        }

        protected static T LoadSettings<T>() where T : GameSettings
        {
            return LoadSettings<T>(BuildResourceFolder(), BuildResourceFilename<T>());
        }

        /// <summary>
        ///   Loads the setting resource from the specified resource folder and filename.
        ///   Within the editor a new resource file is created if the file wasn't found.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="resourceFolder">Path of the Resources folder.</param>
        /// <param name="filePath">Path to the resource file within the Resources folder.</param>
        /// <returns>Loaded settings resource file.</returns>
        protected static T LoadSettings<T>(string resourceFolder, string filePath) where T : GameSettings
        {
#if UNITY_EDITOR
            string resourcePath = resourceFolder + filePath;
            string assetResourcePath = "Assets/" + resourcePath;
            T settings = Resources.LoadAssetAtPath<T>(assetResourcePath);
#else
            T settings = Resources.Load<T>(Path.GetFileNameWithoutExtension(filePath));
#endif

#if UNITY_EDITOR
            if (settings == null)
            {
                Debug.LogWarning(string.Format("Creating new '{0}' settings at '{1}'", typeof(T).Name, resourcePath));

                // If the settings asset directory doesn't exist, then create it.
                string fullResourcePath = Application.dataPath + "/" + resourcePath;
                string fullResourceDirectory = Path.GetDirectoryName(fullResourcePath);
                if (fullResourceDirectory != null && !Directory.Exists(fullResourceDirectory))
                {
                    Directory.CreateDirectory(fullResourceDirectory);
                }

                settings = CreateInstance<T>();

                AssetDatabase.CreateAsset(settings, assetResourcePath);
                AssetDatabase.SaveAssets();

                Selection.activeObject = settings;
            }
#endif

            return settings;
        }

        #endregion
    }
}