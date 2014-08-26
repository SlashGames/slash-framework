// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DontDestroyOnLoadBehaviour.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Unity.Common.Loading
{
    using UnityEngine;

    /// <summary>
    ///   The method DontDestroyOnLoad preserves the specified game object when
    ///   the scene is changed.
    ///   You can use it for game objects which are required throughout the game.
    /// </summary>
    public class DontDestroyOnLoadBehaviour : MonoBehaviour
    {
        #region Methods

        private void Start()
        {
            DontDestroyOnLoad(this);
        }

        #endregion
    }
}