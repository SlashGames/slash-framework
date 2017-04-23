// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PlayerPrefsDeleteAll.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Unity.Editor.Common.MenuItems.Util
{
    using UnityEditor;

    using UnityEngine;

    /// <summary>
    ///   Finds prefabs who have the specified MonoBehaviour attached.
    /// </summary>
    public static class PlayerPrefsDeleteAll
    {
        #region Public Methods and Operators

        /// <summary>
        ///   Deletes all player prefs.
        /// </summary>
        [MenuItem("Slash/Util/Delete All Player Prefs")]
        public static void DeleteAllPlayerPrefs()
        {
            PlayerPrefs.DeleteAll();
            Debug.Log("Player preferences deleted.");
        }

        #endregion
    }
}