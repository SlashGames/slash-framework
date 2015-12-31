// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GameEventTypeAttribute.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.ECS.Events
{
    using System;

    /// <summary>
    ///   Attribute to flag an enum that it contains identifiers for game events.
    ///   This way all game events can be found, e.g. for logging.
    /// </summary>
    public class GameEventTypeAttribute : Attribute
    {
    }
}