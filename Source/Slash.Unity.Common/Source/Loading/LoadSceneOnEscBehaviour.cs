// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LoadSceneOnEscBehaviour.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Unity.Common.Loading
{
    using Slash.Unity.Common.Scenes;

    using UnityEngine;

    /// <summary>
    ///   Loads the specified scene on ESC or back button.
    /// </summary>
    public class LoadSceneOnEscBehaviour : MonoBehaviour
    {
        #region Fields

        /// <summary>
        ///   Name of the scene to load.
        /// </summary>
        public string SceneName;

        #endregion

        #region Methods

        private void Update()
        {
            // Load scene on ESC or back button.
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                var sceneManager = FindObjectOfType<SceneManager>();
                sceneManager.ChangeScene(this.SceneName);
            }
        }

        #endregion
    }
}