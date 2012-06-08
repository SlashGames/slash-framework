// -----------------------------------------------------------------------
// <copyright file="ISystem.cs" company="Rainy Games">
// Copyright (c) Rainy Games. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace RainyGames.GameBase
{
    /// <summary>
    /// Contract that all systems that make up a game have to fulfill,
    /// e.g. physics, combat or AI.
    /// </summary>
    public interface ISystem
    {
        #region Public Methods

        /// <summary>
        /// Ticks this system.
        /// </summary>
        /// <param name="dt">
        /// Time passed since the last tick, in seconds.
        /// </param>
        void Update(float dt);

        #endregion
    }
}
