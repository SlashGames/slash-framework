// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DisableWithSceneManager.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Unity.Common.Diagnostics
{
    using Slash.Unity.Common.Scenes;

    using UnityEngine;

    /// <summary>
    ///   Disables the game object the script is on if a scene manager is found.
    ///   Useful if you test scenes separately and need e.g. a camera which is normally
    ///   initialized in a root scene.
    /// </summary>
    public class DisableWithSceneManager : MonoBehaviour
    {
        #region Methods

        private void Awake()
        {
            SceneManager sceneManager = FindObjectOfType<SceneManager>();
            if (sceneManager != null)
            {
                this.gameObject.SetActive(false);
            }
        }

        #endregion
    }
}