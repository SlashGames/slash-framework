// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GameSystemAttribute.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.GameBase.Systems
{
    using System;

    [AttributeUsage(AttributeTargets.Class)]
    public class GameSystemAttribute : Attribute
    {
        public GameSystemAttribute()
        {
            this.Enabled = true;
        }

        public bool Enabled { get; set; }
    }
}