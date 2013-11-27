// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InspectorFloatAttribute.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.GameBase.Attributes
{
    using System.Globalization;

    using Slash.GameBase.Inspector.Validation;

    /// <summary>
    ///   Exposes the property to the landscape designer inspector.
    /// </summary>
    public class InspectorFloatAttribute : InspectorPropertyAttribute
    {
        #region Constants

        /// <summary>
        ///   Validation message to use for strings which are too long.
        /// </summary>
        private const string ValidationMessageOutOfRange = "Value is out of range (min: {0}, max: {1}).";

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///   Exposes the property to the landscape designer inspector.
        /// </summary>
        /// <param name="name">Property name to be shown in the inspector.</param>
        public InspectorFloatAttribute(string name)
            : base(name)
        {
            this.Min = float.MinValue;
            this.Max = float.MaxValue;
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///   Maximum property value.
        /// </summary>
        public float Max { get; set; }

        /// <summary>
        ///   Minimum property value.
        /// </summary>
        public float Min { get; set; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///   Converts the passed text to a value of the correct type for this property.
        /// </summary>
        /// <param name="text">Text to convert.</param>
        /// <returns>
        ///   Value of the correct type for this property, if the conversion was successful, and <c>null</c> otherwise.
        /// </returns>
        public override object ConvertFromString(string text)
        {
            float floatValue;
            float.TryParse(text, NumberStyles.Any, CultureInfo.InvariantCulture, out floatValue);
            return floatValue;
        }

        /// <summary>
        ///   Converts the passed value to a string that can be converted back to a value of the correct type for this property.
        /// </summary>
        /// <param name="value">Value to convert.</param>
        /// <returns>String that can be converted back to a value of the correct type for this property.</returns>
        /// <see cref="InspectorPropertyAttribute.ConvertFromString" />
        public override string ConvertToString(object value)
        {
            return value.ToString();
        }

        public override string ToString()
        {
            return string.Format(
                "Name: {0}, Max: {1}, Min: {2}, Default: {3}", this.Name, this.Max, this.Min, this.Default);
        }

        /// <summary>
        ///   Tries to convert the specified text to a value of the correct type for this property.
        /// </summary>
        /// <param name="text">Text to convert.</param>
        /// <param name="value">Value of the correct type for this property, if the conversion was successful.</param>
        /// <returns>
        ///   True if the conversion was successful; otherwise, false.
        /// </returns>
        public override bool TryConvertFromString(string text, out object value)
        {
            float floatValue;
            bool success = float.TryParse(text, NumberStyles.Any, CultureInfo.InvariantCulture, out floatValue);
            value = floatValue;
            return success;
        }

        /// <summary>
        ///   Tries to convert the specified value to a string that can be converted back to a value of the correct type for this property.
        /// </summary>
        /// <param name="value">Value to convert.</param>
        /// <param name="text">String that can be converted back to a value of the correct type for this property.</param>
        /// <see cref="InspectorPropertyAttribute.TryConvertFromString" />
        /// <returns>
        ///   True if the conversion was successful; otherwise, false.
        /// </returns>
        public override bool TryConvertToString(object value, out string text)
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

            if (!(value is float))
            {
                return ValidationError.WrongType;
            }

            float floatValue = (float)value;
            if (floatValue < this.Min || floatValue > this.Max)
            {
                return new ValidationError { Message = string.Format(ValidationMessageOutOfRange, this.Min, this.Max) };
            }

            return null;
        }

        #endregion
    }
}