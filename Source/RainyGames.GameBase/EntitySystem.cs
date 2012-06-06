// -----------------------------------------------------------------------
// <copyright file="EntitySystem.cs" company="Rainy Games">
// Copyright (c) Rainy Games. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace RainyGames.GameBase
{
    /// <summary>
    /// Abstract base class of all systems that make up a game, e.g. physics,
    /// combat or AI.
    /// </summary>
    public abstract class EntitySystem
    {
        #region Constants and Fields

        /// <summary>
        /// Game this system is part of.
        /// </summary>
        private Game game;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Constructs a new system for the specified game.
        /// </summary>
        /// <param name="game">
        /// Game the new system is part of.
        /// </param>
        public EntitySystem(Game game)
        {
            this.game = game;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Game this system is part of.
        /// </summary>
        protected Game Game
        {
            get
            {
                return this.game;
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Ticks this system.
        /// </summary>
        /// <param name="dt">
        /// Time passed since the last tick.
        /// </param>
        public virtual void Update(float dt)
        {
        }

        #endregion
    }
}
