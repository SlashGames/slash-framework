// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Entity2Data.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.GameBase.EventData
{
    /// <summary>
    ///   Event data for events concerning two entities.
    /// </summary>
    public class Entity2Data
    {
        #region Public Properties

        /// <summary>
        ///   Id of the first entity.
        /// </summary>
        public int First { get; set; }

        /// <summary>
        ///   Id of the second entity.
        /// </summary>
        public int Second { get; set; }

        #endregion
    }
}