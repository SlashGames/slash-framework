// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LogDeviceIdBehaviour.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Unity.Common.Utils
{
    using UnityEngine;

    /// <summary>
    ///   Logs the unique device identifier on awake.
    /// </summary>
    public class LogDeviceIdBehaviour : MonoBehaviour
    {
        #region Methods

        private void Awake()
        {
            Debug.Log("Unique device ID: " + SystemInfo.deviceUniqueIdentifier);
        }

        #endregion
    }
}