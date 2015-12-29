// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SerializationAction.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Slash.Application.Features.Serialization.Events
{
    using Slash.ECS.Events;

    [GameEventType]
    public enum SerializationAction
    {
        /// <summary>
        ///   <para>
        ///     Action to save the game to a file.
        ///   </para>
        ///   <para>
        ///     Event data: <see cref="string" /> (File path).
        ///   </para>
        /// </summary>
        Save,

        /// <summary>
        ///   <para>
        ///     Action to load the game from a file.
        ///   </para>
        ///   <para>
        ///     Event data: <see cref="string" /> (File path).
        ///   </para>
        /// </summary>
        Load,
    }
}