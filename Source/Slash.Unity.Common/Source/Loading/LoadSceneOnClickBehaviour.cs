// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LoadSceneOnClickBehaviour.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Unity.Common.Loading
{
    using Slash.Unity.Common.Scenes;

    using UnityEngine;

    public class LoadSceneOnClickBehaviour : MonoBehaviour
    {
        #region Fields

        public string SceneName;

        #endregion

        #region Methods

        private void OnClick()
        {
            var sceneManager = FindObjectOfType<SceneManager>();
            sceneManager.ChangeScene(this.SceneName);
        }

        #endregion
    }
}