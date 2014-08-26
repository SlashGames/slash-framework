// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ActiveOnPlatformBehaviour.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Unity.Common.Utils
{
    using UnityEngine;

    /// <summary>
    ///   (De-)Activates this game object depending on the current runtime platform.
    /// </summary>
    public class ActiveOnPlatformBehaviour : MonoBehaviour
    {
        #region Fields

        /// <summary>
        ///   Whether to activate or deactive this game object.
        /// </summary>
        public bool Active;

        /// <summary>
        ///   Runtime platform causing the game object to be activated or deactivated.
        /// </summary>
        public RuntimePlatform Platform;

        #endregion

        #region Methods

        private void Awake()
        {
            if (Application.platform == this.Platform)
            {
                this.gameObject.SetActive(this.Active);
            }
        }

        #endregion
    }
}