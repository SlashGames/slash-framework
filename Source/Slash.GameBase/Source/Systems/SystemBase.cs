// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SystemBase.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.GameBase.Systems
{
    using Slash.Collections.AttributeTables;
    using Slash.GameBase.Inspector.Utils;

    /// <summary>
    ///   Base system class.
    /// </summary>
    public class SystemBase : ISystem
    {
        #region Public Properties

        /// <summary>
        ///   Game this system belongs to.
        /// </summary>
        public Game Game { get; set; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///   Initializes this system with the data stored in the specified
        ///   attribute table.
        /// </summary>
        /// <param name="configuration">System configuration data.</param>
        public virtual void Init(IAttributeTable configuration)
        {
            // Initialize from configuration.
            InspectorUtils.InitFromAttributeTable(this.Game, this, configuration);
        }

        /// <summary>
        ///   Late update of this system. The late update is performed after
        ///   all events of the tick were processed.
        /// </summary>
        /// <param name="dt">
        ///   Time passed since the last tick (in s).
        /// </param>
        public virtual void LateUpdate(float dt)
        {
        }

        /// <summary>
        ///   Ticks this system.
        /// </summary>
        /// <param name="dt">
        ///   Time passed since the last tick, in seconds.
        /// </param>
        public virtual void UpdateSystem(float dt)
        {
        }

        #endregion
    }
}