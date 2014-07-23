// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DebugBuildBehaviour.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Unity.Common.Diagnostics
{
    using UnityEngine;

    /// <summary>
    ///   Disables the game object if not in debug build.
    /// </summary>
    public class DebugBuildBehaviour : MonoBehaviour
    {
        #region Methods

        private void Awake()
        {
            this.gameObject.SetActive(Debug.isDebugBuild);
        }

        #endregion
    }
}