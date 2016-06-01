// --------------------------------------------------------------------------------------------------------------------
// <copyright file="WindowManagerBehaviour.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Slash.Unity.Common.Scenes
{
    using UnityEngine;

    public class WindowManagerBehaviour : MonoBehaviour
    {

        #region Properties

        public static WindowManager Instance { get; private set; }

        #endregion

        /// <summary>
        ///   Unity callback.
        /// </summary>
        protected void Awake()
        {
            // Create window manager.
            var windowManager = new WindowManager(this);

            if (Instance == null)
            {
                Instance = windowManager;
            }
            else
            {
                Debug.LogWarning("Multiple window manager found, please use only one.");
            }
        }
    }
}