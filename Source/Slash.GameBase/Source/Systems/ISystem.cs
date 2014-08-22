// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ISystem.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.ECS.Systems
{
    using Slash.Collections.AttributeTables;

    /// <summary>
    ///   Contract that all systems that make up a game have to fulfill,
    ///   e.g. physics, combat or AI.
    /// </summary>
    public interface ISystem
    {
        #region Public Properties

        /// <summary>
        ///   Game this system belongs to.
        /// </summary>
        Game Game { get; set; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///   Initializes this system with the data stored in the specified
        ///   attribute table.
        /// </summary>
        /// <param name="configuration">System configuration data.</param>
        void Init(IAttributeTable configuration);

        /// <summary>
        ///   Late update of this system. The late update is performed after
        ///   all events of the tick were processed.
        /// </summary>
        /// <param name="dt">
        ///   Time passed since the last tick (in s).
        /// </param>
        void LateUpdate(float dt);

        /// <summary>
        ///   Ticks this system.
        /// </summary>
        /// <param name="dt">
        ///   Time passed since the last tick, in seconds.
        /// </param>
        void UpdateSystem(float dt);

        #endregion
    }
}