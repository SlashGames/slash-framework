// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InspectorFloatAttribute.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.GameBase.Attributes
{
    /// <summary>
    ///   Exposes the property to the landscape designer inspector.
    /// </summary>
    public class InspectorFloatAttribute : InspectorPropertyAttribute
    {
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
            float.TryParse(text, out floatValue);
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
        ///   Checks whether the passed value is valid for this property.
        /// </summary>
        /// <param name="value">Value to check.</param>
        /// <returns>
        ///   <c>true</c>, if the passed value is valid for this property, and <c>false</c> otherwise.
        /// </returns>
        public override bool Validate(object value)
        {
            if (value is float)
            {
                float floatValue = (float)value;
                return floatValue >= this.Min && floatValue <= this.Max;
            }

            return false;
        }

        #endregion
    }
}