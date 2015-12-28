// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LoadSceneOnClickBehaviour.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Unity.Common.Loading
{
    using Slash.Unity.Common.Scenes;

    using UnityEngine;

    /// <summary>
    ///   Loads the specified scene when this game object is clicked.
    /// </summary>
    public class LoadSceneOnClickBehaviour : MonoBehaviour
    {
        #region Fields

        /// <summary>
        ///   Name of the scene to load.
        /// </summary>
        public string SceneName;

        #endregion

        #region Methods

        private void OnClick()
        {
            var sceneManager = SceneManager.Instance;
            sceneManager.ChangeScene(this.SceneName);
        }

        #endregion
    }
}