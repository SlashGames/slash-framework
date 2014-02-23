// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InspectorStringAttribute.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.GameBase.Inspector.Attributes
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    using Slash.GameBase.Inspector.Validation;

    /// <summary>
    ///   Exposes the property to the inspector.
    /// </summary>
    [Serializable]
    public class InspectorStringAttribute : InspectorPropertyAttribute
    {
        #region Constants

        /// <summary>
        ///   Validation message to use for strings which are too long.
        /// </summary>
        private const string ValidationMessageTooLong = "String is too long.";

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///   Constructor.
        /// </summary>
        /// <param name="name">Property name to be shown in the inspector.</param>
        public InspectorStringAttribute(string name)
            : base(name)
        {
            this.MaxLength = int.MaxValue;
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///   Maximum length of the string.
        /// </summary>
        public int MaxLength { get; set; }

        /// <summary>
        ///   Whether this string should be localized to different languages and just represents a localization id.
        /// </summary>
        public bool Localized { get; set; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///   Converts the passed text to a value of the correct type for this property.
        /// </summary>
        /// <param name="text">Text to convert.</param>
        /// <returns>
        ///   Value of the correct type for this property, if the conversion was successful, and <c>null</c> otherwise.
        /// </returns>
        public override object ConvertStringToValue(string text)
        {
            return text;
        }

        /// <summary>
        ///   Converts the passed value to a string that can be converted back to a value of the correct type for this property.
        /// </summary>
        /// <param name="value">Value to convert.</param>
        /// <returns>String that can be converted back to a value of the correct type for this property.</returns>
        /// <see cref="InspectorPropertyAttribute.ConvertStringToValue" />
        public override string ConvertValueToString(object value)
        {
            return value.ToString();
        }

        /// <summary>
        ///   Gets an empty list for elements of the type of the property the attribute is attached to.
        /// </summary>
        /// <returns>Empty list of matching type.</returns>
        public override IList GetEmptyList()
        {
            return new List<string>();
        }

        public override string ToString()
        {
            return string.Format("Name: {0}, MaxLength: {1}, Default: {2}", this.Name, this.MaxLength, this.Default);
        }

        /// <summary>
        ///   Tries to convert the specified text to a value of the correct type for this property.
        /// </summary>
        /// <param name="text">Text to convert.</param>
        /// <param name="value">Value of the correct type for this property, if the conversion was successful.</param>
        /// <returns>
        ///   True if the conversion was successful; otherwise, false.
        /// </returns>
        public override bool TryConvertStringToValue(string text, out object value)
        {
            value = text;
            return true;
        }

        /// <summary>
        ///   Tries to convert the specified value to a string that can be converted back to a value of the correct type for this property.
        /// </summary>
        /// <param name="value">Value to convert.</param>
        /// <param name="text">String that can be converted back to a value of the correct type for this property.</param>
        /// <see cref="InspectorPropertyAttribute.TryConvertStringToValue" />
        /// <returns>
        ///   True if the conversion was successful; otherwise, false.
        /// </returns>
        public override bool TryConvertValueToString(object value, out string text)
        {
            text = value.ToString();
            return true;
        }

        /// <summary>
        ///   Checks whether the passed value is valid for this property.
        /// </summary>
        /// <param name="value">Value to check.</param>
        /// <returns>
        ///   <c>null</c>, if the passed value is valid for this property,
        ///   and <see cref="ValidationError" /> which contains information about the error otherwise.
        /// </returns>
        public override ValidationError Validate(object value)
        {
            if (value == null)
            {
                return ValidationError.Null;
            }

            var stringValue = value as string;
            if (stringValue == null)
            {
                return ValidationError.WrongType;
            }

            if (stringValue.Length > this.MaxLength)
            {
                return new ValidationError { Message = ValidationMessageTooLong };
            }

            return null;
        }

        #endregion
    }
}