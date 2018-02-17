﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Utils.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Unity.Editor.Common.MenuItems.Util
{
    using System.IO;

    using UnityEditor;

    using UnityEngine;

    public static class Utils
    {
        #region Public Methods and Operators

        /// <summary>
        ///   Deletes all persistent data.
        /// </summary>
        [MenuItem("Slash/Util/Delete All Persistent Data")]
        public static void DeleteAllPerstistentData()
        {
            if (EditorUtility.DisplayDialog(
                "Confirm",
                "Are you sure you want to delete all persistent data?",
                "Yes",
                "No"))
            {
                DirectoryInfo directoryInfo = new DirectoryInfo(Application.persistentDataPath);
                foreach (FileInfo file in directoryInfo.GetFiles())
                {
                    file.Delete();
                }
                foreach (DirectoryInfo dir in directoryInfo.GetDirectories())
                {
                    dir.Delete(true);
                }
                Debug.Log("Persistent data deleted.");
            }
        }

        /// <summary>
        ///   Opens the persistent data path in OS explorer.
        /// </summary>
        [MenuItem("Slash/Util/Open persistent data path")]
        public static void OpenPersistentDataPath()
        {
            EditorUtility.RevealInFinder(Application.persistentDataPath);
        }

        /// <summary>
        ///   Clears the asset bundle cache.
        /// </summary>
        [MenuItem("Slash/Util/Clear asset bundle cache")]
        public static void ClearCache()
        {
            if (Caching.ClearCache())
            {
                Debug.Log("Successfully cleaned the cache.");
            }
            else
            {
                Debug.Log("Cache is being used.");
            }
        }

        #endregion
    }
}