// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InspectorIntAttribute.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.GameBase.Attributes
{
    /// <summary>
    ///   Exposes the property to the landscape designer inspector.
    /// </summary>
    public class InspectorIntAttribute : InspectorPropertyAttribute
    {
        #region Constructors and Destructors

        /// <summary>
        ///   Exposes the property to the landscape designer inspector.
        /// </summary>
        /// <param name="name">Property name to be shown in the inspector.</param>
        public InspectorIntAttribute(string name)
            : base(name)
        {
            this.Min = 0;
            this.Max = 31;
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///   Maximum property value.
        /// </summary>
        public int Max { get; set; }

        /// <summary>
        ///   Minimum property value.
        /// </summary>
        public int Min { get; set; }

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
            return text == string.Empty ? 0 : int.Parse(text);
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
            int intValue = 0;

            // Treat empty string as 0.
            bool success = text == string.Empty || int.TryParse(text, out intValue);
            value = intValue;
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
        ///   <c>true</c>, if the passed value is valid for this property, and <c>false</c> otherwise.
        /// </returns>
        public override bool Validate(object value)
        {
            if (value is int)
            {
                int intValue = (int)value;
                return intValue >= this.Min && intValue <= this.Max;
            }

            return false;
        }

        #endregion
    }
}