// -----------------------------------------------------------------------
// <copyright file="FrameworkEventType.cs" company="Slash Games">
// Copyright (c) Slash Games. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Slash.GameBase
{
    /// <summary>
    ///     Type of an event that occurred within the entity system.
    /// </summary>
    public enum FrameworkEventType
    {
        /// <summary>
        ///     <para>
        ///         A new entity has been created.
        ///     </para>
        ///     <para>
        ///         Event data: <see cref="int" />
        ///     </para>
        /// </summary>
        EntityCreated,

        /// <summary>
        ///     <para>
        ///         An entity has been removed.
        ///     </para>
        ///     <para>
        ///         Event data: <see cref="int" />
        ///     </para>
        /// </summary>
        EntityRemoved,

        /// <summary>
        ///     <para>
        ///         Entity components have been initialized.
        ///     </para>
        ///     <para>
        ///         Event data: <see cref="int" />
        ///     </para>
        /// </summary>
        EntityInitialized,

        /// <summary>
        ///     <para>
        ///         The game starts.
        ///     </para>
        ///     <para>
        ///         Event data: <c>null</c>
        ///     </para>
        /// </summary>
        GameStarted,

        /// <summary>
        ///     <para>
        ///         The game has been paused.
        ///     </para>
        ///     <para>
        ///         Event data: <c>null</c>
        ///     </para>
        /// </summary>
        GamePaused,

        /// <summary>
        ///     <para>
        ///         The game has been resumed.
        ///     </para>
        ///     <para>
        ///         Event data: <c>null</c>
        ///     </para>
        /// </summary>
        GameResumed,

        /// <summary>
        ///     <para>
        ///         A new system has been added.
        ///     </para>
        ///     <para>
        ///         Event data: <see cref="ISystem" />
        ///     </para>
        /// </summary>
        SystemAdded,

        /// <summary>
        ///     <para>
        ///         A new component has been added.
        ///     </para>
        ///     <para>
        ///         Event data: <see cref="IEntityComponent" />
        ///     </para>
        /// </summary>
        ComponentAdded,

        /// <summary>
        ///     <para>
        ///         A component has been removed.
        ///     </para>
        ///     <para>
        ///         Event data: <see cref="IEntityComponent" />
        ///     </para>
        /// </summary>
        ComponentRemoved
    }
}
