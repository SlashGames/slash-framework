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
        #region Static Fields

        private static SceneManager instance;

        #endregion

        #region Fields

        /// <summary>
        ///   Prefab to instantiate while scene is loading.
        /// </summary>
        public GameObject LoadingScreenPrefab;

        #endregion

        #region Delegates

        public delegate void SceneChangingDelegate(string newScene);

        #endregion

        #region Public Events

        public event SceneChangingDelegate SceneChanging;

        #endregion

        #region Public Properties

        public static SceneManager Instance
        {
            get
            {
                if (instance == null)
                {
                    GameObject newGameObject = new GameObject();
                    instance = newGameObject.AddComponent<SceneManager>();
                }
                return instance;
            }
        }

        public object InitParams { get; set; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///   Changes to the specified scene.
        /// </summary>
        /// <param name="scene">Scene to change to.</param>
        public void ChangeScene(string scene)
        {
            ChangeScene(scene, 0);
        }

        /// <summary>
        ///   Changes to the specified scene.
        /// </summary>
        /// <param name="scene">Scene to change to.</param>
        /// <param name="delay">Delay before scene change (e.g. to play animations) (in s).</param>
        public void ChangeScene(string scene, float delay)
        {
            this.OnSceneChanging(scene);
            this.StartCoroutine(this.ChangeSceneWithLoadingScreen(scene, delay));
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

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
        }

        private IEnumerator ChangeSceneWithLoadingScreen(string scene, float delay)
        {
            if (delay > 0)
            {
                // Delay changing.
                yield return new WaitForSeconds(delay);
            }

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

        private void OnSceneChanging(string newScene)
        {
            var handler = this.SceneChanging;
            if (handler != null)
            {
                handler(newScene);
            }
        }

        #endregion
    }
}