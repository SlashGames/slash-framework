// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GameEvent.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Application.Games
{
    using Slash.ECS.Events;

    /// <summary>
    ///   <see cref="Game"/>-related events.
    /// </summary>
    [GameEventType]
    public enum ApplicationGameEvent
    {
        /// <summary>
        ///   <para>
        ///     The game starts.
        ///   </para>
        ///   <para>
        ///     Event data: <c>null</c>
        ///   </para>
        /// </summary>
        GameStarted,

        /// <summary>
        ///   <para>
        ///     The game has been paused.
        ///   </para>
        ///   <para>
        ///     Event data: <c>null</c>
        ///   </para>
        /// </summary>
        GamePaused,

        /// <summary>
        ///   <para>
        ///     The game has been resumed.
        ///   </para>
        ///   <para>
        ///     Event data: <c>null</c>
        ///   </para>
        /// </summary>
        GameResumed,

        /// <summary>
        ///   <para>
        ///     The game was stopped.
        ///   </para>
        ///   <para>
        ///     Event data: <c>null</c>
        ///   </para>
        /// </summary>
        GameStopped,
    }
}