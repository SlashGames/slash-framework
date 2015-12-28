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

        /// <summary>
        ///   Target to activate/deactivate.
        /// </summary>
        public GameObject Target;

        #endregion

        #region Methods

        private void Awake()
        {
            if (this.Target == null)
            {
                this.Target = this.gameObject;
            }

            if (Application.platform == this.Platform)
            {
                this.Target.SetActive(this.Active);
            }
        }

        #endregion
    }
}