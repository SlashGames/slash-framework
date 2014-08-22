// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Entity2Data.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.ECS.Events
{
    using System;

    /// <summary>
    ///   Event data for events concerning two entities.
    /// </summary>
    public class Entity2Data : IEquatable<Entity2Data>
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

        #region Public Methods and Operators

        public bool Equals(Entity2Data other)
        {
            return this.First == other.First && this.Second == other.Second;
        }

        public override string ToString()
        {
            return string.Format("First: {0}, Second: {1}", this.First, this.Second);
        }

        #endregion
    }
}