// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SystemGameEvent.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Application.Systems
{
    using Slash.ECS.Events;

    /// <summary>
    ///   <see cref="System" />-related events.
    /// </summary>
    [GameEventType]
    public enum SystemGameEvent
    {
        /// <summary>
        ///   <para>
        ///     A new system has been added.
        ///   </para>
        ///   <para>
        ///     Event data: <see cref="ISystem" />
        ///   </para>
        /// </summary>
        SystemAdded,
    }
}