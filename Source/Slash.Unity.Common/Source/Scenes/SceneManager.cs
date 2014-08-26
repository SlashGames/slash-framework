// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SceneManager.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Unity.Common.Scenes
{
    using System.Collections;

    using UnityEngine;

    /// <summary>
    ///   Handles scene transitions, providing loading screens and inter-scene
    ///   communication.
    /// </summary>
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

        /// <summary>
        ///   New scene is about to be loaded.
        /// </summary>
        /// <param name="newScene">Name of the new scene.</param>
        public delegate void SceneChangingDelegate(string newScene);

        #endregion

        #region Public Events

        /// <summary>
        ///   New scene is about to be loaded.
        /// </summary>
        public event SceneChangingDelegate SceneChanging;

        #endregion

        #region Public Properties

        /// <summary>
        ///   Current scene manager instance.
        /// </summary>
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

        /// <summary>
        ///   Data to pass to the next scene that is loaded.
        /// </summary>
        public object InitParams { get; set; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///   Changes to the specified scene.
        /// </summary>
        /// <param name="scene">Scene to change to.</param>
        public void ChangeScene(string scene)
        {
            this.ChangeScene(scene, 0);
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

        /// <summary>
        ///   Loads the specified scene, passing it the passed data.
        /// </summary>
        /// <param name="scene">Name of the scene to load.</param>
        /// <param name="initParams">Data to pass to the new scene.</param>
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