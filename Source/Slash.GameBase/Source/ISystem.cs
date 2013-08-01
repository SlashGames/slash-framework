// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ISystem.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.GameBase
{
    /// <summary>
    ///   Contract that all systems that make up a game have to fulfill,
    ///   e.g. physics, combat or AI.
    /// </summary>
    public interface ISystem
    {
        #region Public Methods and Operators

        /// <summary>
        ///   Ticks this system.
        /// </summary>
        /// <param name="dt">
        ///   Time passed since the last tick, in seconds.
        /// </param>
        void UpdateSystem(float dt);

        #endregion
    }
}