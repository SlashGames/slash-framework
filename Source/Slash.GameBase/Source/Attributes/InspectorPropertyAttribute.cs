// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InspectorPropertyAttribute.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.GameBase.Attributes
{
    using System;

    /// <summary>
    ///   Exposes the property to the landscape designer inspector.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public abstract class InspectorPropertyAttribute : Attribute
    {
        #region Constructors and Destructors

        /// <summary>
        ///   Exposes the property to the landscape designer inspector.
        /// </summary>
        /// <param name="name">Property name to be shown in the inspector.</param>
        protected InspectorPropertyAttribute(string name)
        {
            this.Name = name;
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///   Default property value.
        /// </summary>
        public object Default { get; set; }

        /// <summary>
        ///   Property name to be shown in the inspector.
        /// </summary>
        public string Name { get; set; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///   Converts the passed text to a value of the correct type for this property.
        /// </summary>
        /// <param name="text">Text to convert.</param>
        /// <returns>
        ///   Value of the correct type for this property, if the conversion was successful, and <c>null</c> otherwise.
        /// </returns>
        public abstract object ConvertFromString(string text);

        /// <summary>
        ///   Converts the passed value to a string that can be converted back to a value of the correct type for this property.
        /// </summary>
        /// <param name="value">Value to convert.</param>
        /// <returns>String that can be converted back to a value of the correct type for this property.</returns>
        /// <see cref="ConvertFromString" />
        public abstract string ConvertToString(object value);

        /// <summary>
        ///   Indicates if the specified value is allowed for the property.
        /// </summary>
        /// <param name="value">Value to check.</param>
        /// <returns>True if the specified value is allowed; otherwise, false.</returns>
        public virtual bool IsAllowed(object value)
        {
            return true;
        }

        public override string ToString()
        {
            return string.Format("Name: {0}, Default: {1}", this.Name, this.Default);
        }

        /// <summary>
        ///   Tries to convert the specified text to a value of the correct type for this property.
        /// </summary>
        /// <param name="text">Text to convert.</param>
        /// <param name="value">Value of the correct type for this property, if the conversion was successful.</param>
        /// <returns>
        ///   True if the conversion was successful; otherwise, false.
        /// </returns>
        public abstract bool TryConvertFromString(string text, out object value);

        /// <summary>
        ///   Tries to convert the specified value to a string that can be converted back to a value of the correct type for this property.
        /// </summary>
        /// <param name="value">Value to convert.</param>
        /// <param name="text">String that can be converted back to a value of the correct type for this property.</param>
        /// <see cref="TryConvertFromString" />
        /// <returns>
        ///   True if the conversion was successful; otherwise, false.
        /// </returns>
        public abstract bool TryConvertToString(object value, out string text);

        /// <summary>
        ///   Checks whether the passed value is valid for this property.
        /// </summary>
        /// <param name="value">Value to check.</param>
        /// <returns>
        ///   <c>true</c>, if the passed value is valid for this property, and <c>false</c> otherwise.
        /// </returns>
        public abstract bool Validate(object value);

        #endregion
    }
}