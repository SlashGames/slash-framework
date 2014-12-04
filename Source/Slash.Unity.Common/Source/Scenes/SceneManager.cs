// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SceneManager.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Unity.Common.Scenes
{
#if UNITY_EDITOR
    using UnityEditor;
#endif
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    using UnityEngine;

    public sealed class SceneManager : MonoBehaviour
    {
        #region Fields

        /// <summary>
        ///   Prefab to instantiate while scene is loading.
        /// </summary>
        public GameObject LoadingScreenPrefab;

        private string loadingSceneId;

        /// <summary>
        ///   Root of main scene.
        /// </summary>
        private SceneRoot mainScene;

        #endregion

        #region Delegates

        public delegate void SceneChangedDelegate(SceneRoot newSceneRoot);

        public delegate void SceneChangingDelegate(string newScene);

        #endregion

        #region Public Events

        public event SceneChangedDelegate SceneChanged;

        public event SceneChangingDelegate SceneChanging;

        #endregion

        #region Public Properties

        public static SceneManager Instance { get; private set; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///   Changes to the specified scene.
        /// </summary>
        /// <param name="scene">Scene to change to.</param>
        public void ChangeScene(string scene)
        {
            this.OnSceneChanging(scene);
            this.StartCoroutine(this.DoChangeScene(scene));
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
            if (Instance == null)
            {
                Instance = this;
            }

            // Consider case where content and scene manager are in one scene.
            this.mainScene = FindObjectOfType<SceneRoot>();
        }

        private IEnumerator DoChangeScene(string scene)
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
            this.loadingSceneId = scene;
            yield return Application.LoadLevelAsync(this.loadingSceneId);

            if (loadingScreen != null)
            {
                // Hide loading screen.
                Destroy(loadingScreen);
            }
        }

        private SceneRoot FindSceneRoot(string sceneId)
        {
            Debug.Log("Setup scene " + sceneId);
            // Get new scene root.
            IEnumerable<SceneRoot> sceneRoots =
                FindObjectsOfType<SceneRoot>().Where(existingSceneRoot => existingSceneRoot != this.mainScene).ToList();
            if (!sceneRoots.Any())
            {
                Debug.LogError(
                    "No scene root found in loaded scene '" + sceneId
                    + "'. Please make sure to add a SceneRoot component to the root game object of the scene.",
                    this);
                return null;
            }

            if (sceneRoots.Count() > 1)
            {
                Debug.LogError(
                    "Multiple scene roots found in loaded scene '" + sceneId
                    + "'. Please make sure to use only one SceneRoot component per scene.",
                    this);
                return null;
            }

            return sceneRoots.First();
        }

        private void OnLevelLoaded()
        {
            // Find new scene root.
            SceneRoot sceneRoot = this.FindSceneRoot(this.loadingSceneId);
            this.mainScene = sceneRoot;
            Debug.Log("Changed to scene '" + this.loadingSceneId + "'.");

            // Dispatch event.
            this.OnSceneChanged(this.mainScene);
        }

        private void OnLevelWasLoaded(int levelIndex)
        {
            Debug.Log("Level was loaded: " + levelIndex);
            if (string.IsNullOrEmpty(this.loadingSceneId))
            {
                Debug.LogWarning(
                    "Loaded scene " + levelIndex
                    + " without scene manager. Make sure to use ChangeScene to make sure scene manager works as intended.");
            }
            this.OnLevelLoaded();
        }

        private void OnSceneChanged(SceneRoot newSceneRoot)
        {
            SceneChangedDelegate handler = this.SceneChanged;
            if (handler != null)
            {
                handler(newSceneRoot);
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