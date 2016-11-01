// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LoadSceneOnEscBehaviour.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using UnityEngine;
using UnityEngine.SceneManagement;

namespace Slash.Unity.Common.Loading
{
    /// <summary>
    ///     Loads the specified scene on ESC or back button.
    /// </summary>
    public class LoadSceneOnEscBehaviour : MonoBehaviour
    {
        #region Fields

        /// <summary>
        ///     Name of the scene to load.
        /// </summary>
        public string SceneName;

        #endregion

        #region Methods

        private void Update()
        {
            // Load scene on ESC or back button.
            if (UnityEngine.Input.GetKeyDown(KeyCode.Escape))
            {
                SceneManager.LoadScene(this.SceneName);
            }
        }

        #endregion
    }
}