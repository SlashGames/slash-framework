// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LoadSceneBehaviour.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Unity.Common.Loading
{
    using Slash.Unity.Common.Scenes;

    using UnityEngine;
    using UnityEngine.SceneManagement;

    using SceneManager = Slash.Unity.Common.Scenes.SceneManager;

    public class LoadSceneBehaviour : MonoBehaviour
    {
        #region Fields

        /// <summary>
        ///   Indicates if the scene should be loaded additive to existing one.
        ///   If loaded additive the old scene objects are not removed.
        /// </summary>
        public bool LoadAdditive;

        /// <summary>
        ///   Scene manager to use. If not set, the levels are loaded directly.
        /// </summary>
        public SceneManager SceneManager;

        /// <summary>
        ///   Name of scene to load.
        /// </summary>
        public string SceneName;

        /// <summary>
        ///   Window manager to use. If not set, the windows are loaded directly.
        /// </summary>
        public WindowManager WindowManager;

        #endregion

        #region Public Methods and Operators

        public void LoadScene()
        {
            if (this.WindowManager == null)
            {
                this.WindowManager = WindowManager.Instance;
            }
            if (this.SceneManager == null)
            {
                this.SceneManager = SceneManager.Instance;
            }

            if (this.LoadAdditive)
            {
                if (this.WindowManager != null)
                {
                    this.WindowManager.OpenWindow(this.SceneName);
                }
                else
                {
                    UnityEngine.SceneManagement.SceneManager.LoadScene(this.SceneName, LoadSceneMode.Additive);
                }
            }
            else
            {
                if (this.SceneManager != null)
                {
                    this.SceneManager.ChangeScene(this.SceneName);
                }
                else
                {
                    UnityEngine.SceneManagement.SceneManager.LoadScene(this.SceneName);
                }
            }
        }

        #endregion

        #region Methods

        /// <summary>
        ///   Unity callback.
        /// </summary>
        protected void Start()
        {
            this.LoadScene();
        }

        #endregion
    }
}