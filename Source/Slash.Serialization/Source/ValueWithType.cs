// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ValueWithType.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Serialization
{
    using System;

    using Slash.Reflection.Utils;

    /// <summary>
    ///   Value and its type.
    /// </summary>
    public class ValueWithType
    {
        #region Constructors and Destructors

        /// <summary>
        ///   Constructor.
        /// </summary>
        /// <param name="value">Value to encapsule.</param>
        public ValueWithType(object value)
        {
            this.Value = value;
            this.Type = value != null ? value.GetType() : null;
        }

        /// <summary>
        ///   Default constructor.
        /// </summary>
        public ValueWithType()
        {
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///   Type of the value.
        /// </summary>
        public Type Type { get; set; }

        /// <summary>
        ///   Full name of type.
        /// </summary>
        public string TypeFullName
        {
            get
            {
                return this.Type != null ? this.Type.FullName : null;
            }

            set
            {
                this.Type = !string.IsNullOrEmpty(value) ? ReflectionUtils.FindType(value) : null;
            }
        }

        /// <summary>
        ///   Value of the type.
        /// </summary>
        public object Value { get; set; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///   Determines whether the specified <see cref="T:System.Object" /> is equal to the current <see cref="T:System.Object" />.
        /// </summary>
        /// <returns>
        ///   true if the specified <see cref="T:System.Object" /> is equal to the current <see cref="T:System.Object" />; otherwise, false.
        /// </returns>
        /// <param name="obj">
        ///   The <see cref="T:System.Object" /> to compare with the current <see cref="T:System.Object" />.
        /// </param>
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
            return this.Equals((ValueWithType)obj);
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
                return ((this.Value != null ? this.Value.GetHashCode() : 0) * 397)
                       ^ (this.Type != null ? this.Type.GetHashCode() : 0);
            }
        }

        #endregion

        #region Methods

        /// <summary>
        ///   Determines whether the specified <see cref="ValueWithType" /> is equal to the current <see cref="ValueWithType" />.
        /// </summary>
        /// <returns>
        ///   true if the specified <see cref="ValueWithType" /> is equal to the current <see cref="ValueWithType" />; otherwise, false.
        /// </returns>
        /// <param name="other">
        ///   The <see cref="ValueWithType" /> to compare with the current <see cref="ValueWithType" />.
        /// </param>
        protected bool Equals(ValueWithType other)
        {
            return Equals(this.Value, other.Value) && Equals(this.Type, other.Type);
        }

        #endregion
    }
}