// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GameSystemAttribute.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.ECS.Systems
{
    using System;

    /// <summary>
    ///   System that is automatically added to the game.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class GameSystemAttribute : Attribute
    {
        #region Constructors and Destructors

        public GameSystemAttribute()
        {
            this.Enabled = true;
            this.Order = 0;
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///   Whether the system should be added to the game, or not.
        /// </summary>
        public bool Enabled { get; set; }

        /// <summary>
        ///   Order to add the systems in. Systems with higher order will be added later.
        /// </summary>
        public int Order { get; set; }

        #endregion
    }
}