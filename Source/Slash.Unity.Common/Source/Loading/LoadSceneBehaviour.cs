// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LoadSceneBehaviour.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using Slash.Unity.Common.Scenes;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Slash.Unity.Common.Loading
{
    public class LoadSceneBehaviour : MonoBehaviour
    {
        #region Public Methods and Operators

        public void LoadScene()
        {
            if (this.WindowManager == null)
            {
                this.WindowManager = WindowManagerBehaviour.Instance;
            }

            if (this.LoadAdditive)
            {
                if (this.WindowManager != null)
                {
                    this.WindowManager.OpenWindow(this.SceneName);
                }
                else
                {
                    SceneManager.LoadScene(this.SceneName, LoadSceneMode.Additive);
                }
            }
            else
            {
                SceneManager.LoadScene(this.SceneName);
            }
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Unity callback.
        /// </summary>
        protected void Start()
        {
            this.LoadScene();
        }

        #endregion

        #region Fields

        /// <summary>
        ///     Indicates if the scene should be loaded additive to existing one.
        ///     If loaded additive the old scene objects are not removed.
        /// </summary>
        public bool LoadAdditive;

        /// <summary>
        ///     Name of scene to load.
        /// </summary>
        public string SceneName;

        /// <summary>
        ///     Window manager to use. If not set, the windows are loaded directly.
        /// </summary>
        public WindowManager WindowManager;

        #endregion
    }
}