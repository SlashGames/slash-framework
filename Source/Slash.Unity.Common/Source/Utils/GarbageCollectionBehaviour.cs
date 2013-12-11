// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GarbageCollectionBehaviour.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System;

using UnityEngine;

/// <summary>
///   Reguarly triggers garbage collection to prevent spikes.
///   http://www.cratesmith.com/archives/183
/// </summary>
public class GarbageCollectionBehaviour : MonoBehaviour
{
    #region Fields

    public int FrameFrequency = 30;

    #endregion

    #region Methods

    private void Update()
    {
        if (Time.frameCount % this.FrameFrequency == 0)
        {
            GC.Collect();
        }
    }

    #endregion
}