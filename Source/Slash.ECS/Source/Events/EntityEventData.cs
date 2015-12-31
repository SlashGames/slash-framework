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
        #region Properties

        /// <summary>
        ///   Id of the entity the component event has been fired for.
        /// </summary>
        public int EntityId { get; set; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///   Determines whether the specified <see cref="T:System.Object" /> is equal to the current
        ///   <see cref="T:System.Object" />.
        /// </summary>
        /// <returns>
        ///   true if the specified <see cref="T:System.Object" /> is equal to the current <see cref="T:System.Object" />;
        ///   otherwise, false.
        /// </returns>
        /// <param name="obj">The <see cref="T:System.Object" /> to compare with the current <see cref="T:System.Object" />. </param>
        /// <filterpriority>2</filterpriority>
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

        /// <summary>
        ///   Serves as a hash function for a particular type.
        /// </summary>
        /// <returns>
        ///   A hash code for the current <see cref="T:System.Object" />.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        public override int GetHashCode()
        {
            return this.EntityId;
        }

        /// <summary>
        ///   Returns a string that represents the current object.
        /// </summary>
        /// <returns>
        ///   A string that represents the current object.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        public override string ToString()
        {
            return string.Format("EntityId: {0}", this.EntityId);
        }

        #endregion

        #region Methods

        /// <summary>
        ///   Determines whether the specified <see cref="T:EntityEventData" /> is equal to the current
        ///   <see cref="T:EntityEventData" />.
        /// </summary>
        /// <returns>
        ///   true if the specified <see cref="T:EntityEventData" /> is equal to the current <see cref="T:EntityEventData" />;
        ///   otherwise, false.
        /// </returns>
        /// <param name="other">The <see cref="T:EntityEventData" /> to compare with the current <see cref="T:EntityEventDatat" />. </param>
        protected bool Equals(EntityEventData other)
        {
            return this.EntityId == other.EntityId;
        }

        #endregion
    }
}