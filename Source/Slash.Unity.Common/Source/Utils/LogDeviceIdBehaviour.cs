// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LogDeviceIdBehaviour.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Unity.Common.Utils
{
    using UnityEngine;

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