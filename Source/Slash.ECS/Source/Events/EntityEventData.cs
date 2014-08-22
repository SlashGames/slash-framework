// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EntityEventData.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.ECS.Events
{
    /// <summary>
    ///   Base class for all events which are for a specific entity.
    /// </summary>
    public class EntityEventData
    {
        #region Public Properties

        /// <summary>
        ///   Id of the entity the component event has been fired for.
        /// </summary>
        public int EntityId { get; set; }

        #endregion

        #region Public Methods and Operators

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }
            if (ReferenceEquals(this, obj))
            {
                return true;
            }
            if (obj.GetType() != this.GetType())
            {
                return false;
            }
            return this.Equals((EntityEventData)obj);
        }

        public override int GetHashCode()
        {
            return this.EntityId;
        }

        public override string ToString()
        {
            return string.Format("EntityId: {0}", this.EntityId);
        }

        #endregion

        #region Methods

        protected bool Equals(EntityEventData other)
        {
            return this.EntityId == other.EntityId;
        }

        #endregion
    }
}