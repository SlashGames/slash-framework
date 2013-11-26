// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InspectorStringAttribute.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.GameBase.Attributes
{
    /// <summary>
    ///   Exposes the property to the inspector.
    /// </summary>
    public class InspectorStringAttribute : InspectorPropertyAttribute
    {
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
            return text;
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
        public override bool TryConvertFromString(string text, out object value)
        {
            value = text;
            return true;
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
            var stringValue = value as string;

            if (stringValue != null)
            {
                return stringValue.Length <= this.MaxLength;
            }

            return false;
        }

        #endregion
    }
}