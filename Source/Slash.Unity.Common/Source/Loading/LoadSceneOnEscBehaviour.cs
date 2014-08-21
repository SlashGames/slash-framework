// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LoadSceneOnEscBehaviour.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Unity.Common.Loading
{
    using Slash.Unity.Common.Scenes;

    using UnityEngine;

    public class LoadSceneOnEscBehaviour : MonoBehaviour
    {
        #region Fields

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