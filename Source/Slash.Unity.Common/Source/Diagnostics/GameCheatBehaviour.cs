// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GameCheatBehaviour.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Unity.Common.Diagnostics
{
    using UnityEngine;

    public abstract class GameCheatBehaviour : MonoBehaviour
    {
        #region Public Methods and Operators

        public abstract void DrawCheats();

        #endregion
    }
}