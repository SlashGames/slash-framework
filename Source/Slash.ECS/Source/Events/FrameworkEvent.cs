﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FrameworkEvent.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.ECS.Events
{
    /// <summary>
    ///   Type of an event that occurred within the entity system.
    /// </summary>
    [GameEventType]
    public enum FrameworkEvent
    {
        /// <summary>
        ///   <para>
        ///     A new entity has been created.
        ///   </para>
        ///   <para>
        ///     Event data: <see cref="int" /> (Entity id).
        ///   </para>
        /// </summary>
        EntityCreated,

        /// <summary>
        ///   <para>
        ///     An entity has been removed.
        ///   </para>
        ///   <para>
        ///     Event data: <see cref="int" /> (Entity id).
        ///   </para>
        /// </summary>
        EntityRemoved,

        /// <summary>
        ///   <para>
        ///     Entity components have been initialized.
        ///   </para>
        ///   <para>
        ///     Event data: <see cref="int" /> (Entity id).
        ///   </para>
        /// </summary>
        EntityInitialized,

        /// <summary>
        ///   <para>
        ///     A new component has been added.
        ///   </para>
        ///   <para>
        ///     Event data: <see cref="EntityComponentData" />
        ///   </para>
        /// </summary>
        ComponentAdded,

        /// <summary>
        ///   <para>
        ///     A component has been removed.
        ///   </para>
        ///   <para>
        ///     Event data: <see cref="EntityComponentData" />
        ///   </para>
        /// </summary>
        ComponentRemoved,

        /// <summary>
        ///   <para>
        ///     A component was enabled.
        ///   </para>
        ///   <para>
        ///     Event data: <see cref="EntityComponentData" />
        ///   </para>
        /// </summary>
        ComponentEnabled,

        /// <summary>
        ///   <para>
        ///     A component was disabled.
        ///   </para>
        ///   <para>
        ///     Event data: <see cref="EntityComponentData" />
        ///   </para>
        /// </summary>
        ComponentDisabled,

        /// <summary>
        ///   <para>
        ///     A generic logging event.
        ///   </para>
        ///   <para>
        ///     Event data: <see cref="string" /> (Log message).
        ///   </para>
        /// </summary>
        Logging,

        /// <summary>
        ///   <para>
        ///     Action to submit a cheat code into the game.
        ///   </para>
        ///   <para>
        ///     Event data: <see cref="string" /> (Cheat code).
        ///   </para>
        /// </summary>
        Cheat,
    }
}