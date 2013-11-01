// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ValueWithType.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Serialization.Dictionary
{
    using System;

    using Slash.Reflection.Utils;

    /// <summary>
    ///   Value and its type.
    /// </summary>
    public class ValueWithType
    {
        #region Fields

        /// <summary>
        ///   Value of the type.
        /// </summary>
        public object value;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///   Constructor.
        /// </summary>
        /// <param name="value">Value to encapsule.</param>
        public ValueWithType(object value)
        {
            this.value = value;
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
        public Type Type
        {
            get
            {
                return string.IsNullOrEmpty(this.TypeFullName) ? null : ReflectionUtils.FindType(this.TypeFullName);
            }
            set
            {
                this.TypeFullName = value != null ? value.FullName : null;
            }
        }

        /// <summary>
        ///   Full name of type.
        /// </summary>
        public string TypeFullName { get; set; }

        #endregion
    }
}