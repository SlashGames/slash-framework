// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SystemGameEvent.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Slash.ECS.Systems
{
    using Slash.ECS.Events;

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