// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SwitchFullscreenBehaviour.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Unity.Common.GUI
{
    using UnityEngine;

    /// <summary>
    ///   Allows switching between full-screen and windowed mode.
    /// </summary>
    public class SwitchFullscreenBehaviour : MonoBehaviour
    {
        #region Fields

        /// <summary>
        ///   Key to switch fullscreen.
        /// </summary>
        public KeyCode KeyCode = KeyCode.F10;

        #endregion

        #region Methods

        /// <summary>
        ///   Per frame update.
        /// </summary>
        private void Update()
        {
            if (Input.GetKeyDown(this.KeyCode))
            {
                bool toFullscreen = !Screen.fullScreen;

                if (toFullscreen)
                {
                    // Set resolution to current screen resolution.
                    Screen.SetResolution(Screen.currentResolution.width, Screen.currentResolution.height, true);
                }
                else
                {
                    Screen.fullScreen = false;
                }
            }
        }

        #endregion
    }
}