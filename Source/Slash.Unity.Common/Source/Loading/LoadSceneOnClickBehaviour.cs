// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LoadSceneOnClickBehaviour.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using UnityEngine;
using UnityEngine.SceneManagement;

namespace Slash.Unity.Common.Loading
{
    /// <summary>
    ///     Loads the specified scene when this game object is clicked.
    /// </summary>
    public class LoadSceneOnClickBehaviour : MonoBehaviour
    {
        #region Fields

        /// <summary>
        ///     Name of the scene to load.
        /// </summary>
        public string SceneName;

        #endregion

        #region Methods

        private void OnClick()
        {
            SceneManager.LoadScene(this.SceneName);
        }

        #endregion
    }
}