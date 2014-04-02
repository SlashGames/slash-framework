// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SceneManager.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Unity.Common.Scenes
{
    using System.Collections;

    using UnityEngine;

    public class SceneManager : MonoBehaviour
    {
        #region Fields

        /// <summary>
        ///   Prefab to instantiate while scene is loading.
        /// </summary>
        public GameObject LoadingScreenPrefab;

        #endregion

        #region Public Properties

        public object InitParams { get; set; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///   Changes to the specified scene.
        /// </summary>
        /// <param name="scene">Scene to change to.</param>
        public void ChangeScene(string scene)
        {
            this.StartCoroutine(this.ChangeSceneWithLoadingScreen(scene));
        }

        public void LoadScene(string scene, object initParams)
        {
            this.InitParams = initParams;

            Application.LoadLevel(scene);
        }

        /// <summary>
        ///   Quits the application.
        /// </summary>
        public void Quit()
        {
#if UNITY_EDITOR
            if (Application.isEditor)
            {
                EditorApplication.isPlaying = false;
            }
#endif
            Application.Quit();
        }

        #endregion

        #region Methods

        private IEnumerator ChangeSceneWithLoadingScreen(string scene)
        {
            GameObject loadingScreen = null;

            if (this.LoadingScreenPrefab != null)
            {
                // Show loading screen.
                loadingScreen = (GameObject)Instantiate(this.LoadingScreenPrefab);
                yield return new WaitForEndOfFrame();
            }
            else
            {
                Debug.LogWarning("No loading screen set, user might experience non-reactive UI.");
            }

            // Load level.
            Application.LoadLevel(scene);

            if (loadingScreen != null)
            {
                // Hide loading screen.
                Destroy(loadingScreen);
            }
        }

        #endregion
    }
}