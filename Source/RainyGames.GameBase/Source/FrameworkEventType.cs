// -----------------------------------------------------------------------
// <copyright file="FrameworkEventType.cs" company="Rainy Games">
// Copyright (c) Rainy Games. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace RainyGames.GameBase
{
    /// <summary>
    /// Type of an event that occurred within the Rainy Games Framework.
    /// </summary>
    public enum FrameworkEventType
    {
        /// <summary>
        /// A new entity has been created.
        /// </summary>
        EntityCreated,

        /// <summary>
        /// An entity has been removed.
        /// </summary>
        EntityRemoved,

        /// <summary>
        /// Entity components have been initialized.
        /// </summary>
        EntityInitialized,

        /// <summary>
        /// The game starts.
        /// </summary>
        GameStarted,

        /// <summary>
        /// The game has been paused.
        /// </summary>
        GamePaused,

        /// <summary>
        /// The game has been resumed.
        /// </summary>
        GameResumed,

        /// <summary>
        /// A new system has been added.
        /// </summary>
        SystemAdded,

        /// <summary>
        /// A new component has been added.
        /// </summary>
        ComponentAdded,

        /// <summary>
        /// A component has been removed.
        /// </summary>
        ComponentRemoved
    }
}
