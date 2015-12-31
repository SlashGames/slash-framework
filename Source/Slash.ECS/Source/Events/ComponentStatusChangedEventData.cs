// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ComponentStatusChangedEventData.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.ECS.Events
{
    using Slash.ECS.Components;

    /// <summary>
    ///   Event data for ComponentEnabled and ComponentDisabled events.
    /// </summary>
    public sealed class ComponentStatusChangedEventData
    {
        #region Properties

        /// <summary>
        ///   Component which status changed.
        /// </summary>
        public IEntityComponent Component { get; set; }

        /// <summary>
        ///   Duration the status change lasts (in s).
        ///   Null means the duration isn't determined.
        /// </summary>
        public float? Duration { get; set; }

        /// <summary>
        ///   Id of entity the component belongs to.
        /// </summary>
        public int EntityId { get; set; }

        /// <summary>
        ///   Indicates if the component was enabled or disabled.
        /// </summary>
        public bool IsEnabled { get; set; }

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
            return obj is ComponentStatusChangedEventData && this.Equals((ComponentStatusChangedEventData)obj);
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
            unchecked
            {
                int hashCode = (this.Component != null ? this.Component.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ this.Duration.GetHashCode();
                hashCode = (hashCode * 397) ^ this.EntityId;
                hashCode = (hashCode * 397) ^ this.IsEnabled.GetHashCode();
                return hashCode;
            }
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
            return string.Format(
                "Component: {0}, Duration: {1}, EntityId: {2}, IsEnabled: {3}",
                this.Component,
                this.Duration,
                this.EntityId,
                this.IsEnabled);
        }

        #endregion

        #region Methods

        private bool Equals(ComponentStatusChangedEventData other)
        {
            return Equals(this.Component, other.Component) && this.Duration.Equals(other.Duration)
                   && this.EntityId == other.EntityId && this.IsEnabled.Equals(other.IsEnabled);
        }

        #endregion
    }
}