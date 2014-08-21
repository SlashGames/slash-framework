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

        public bool Active;

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