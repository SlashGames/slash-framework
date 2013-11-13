// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InspectorEnumAttribute.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.GameBase.Attributes
{
    using System;
    
    /// <summary>
    ///   Exposes the property to the landscape designer inspector.
    /// </summary>
    public class InspectorEnumAttribute : InspectorPropertyAttribute
    {
        #region Constructors and Destructors

        /// <summary>
        ///   Exposes the property to the landscape designer inspector.
        /// </summary>
        /// <param name="name">Property name to be shown in the inspector.</param>
        /// <param name="enumType">Type of the enum this attribute is attached to.</param>
        public InspectorEnumAttribute(string name, Type enumType)
            : base(name)
        {
            this.EnumType = enumType;
            this.Default = Enum.GetValues(enumType).GetValue(0);
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///   Allowed enum values. If not set, all values are allowed.
        /// </summary>
        public object[] AllowedValues { get; set; }

        /// <summary>
        ///   Type of the enum this attribute is attached to.
        /// </summary>
        public Type EnumType { get; private set; }

        /// <summary>
        ///   Indicates if the enum has a Flags attribute.
        /// </summary>
        public bool Flags { get; set; }

        /// <summary>
        ///   Forbidden enum values. If not set, no values are forbidden.
        /// </summary>
        public object[] ForbiddenValues { get; set; }

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
            return Enum.Parse(this.EnumType, text);
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

        /// <summary>
        ///   Indicates if the specified value is allowed for the property.
        /// </summary>
        /// <param name="value">Value to check.</param>
        /// <returns>True if the specified value is allowed; otherwise, false.</returns>
        public override bool IsAllowed(object value)
        {
            // Check if forbidden.
            if (this.ForbiddenValues != null && Array.IndexOf(this.ForbiddenValues, value) != -1)
            {
                return false;
            }

            // Check if allowed.
            return this.AllowedValues == null || Array.IndexOf(this.AllowedValues, value) != -1;
        }

        public override string ToString()
        {
            return string.Format("Name: {0}, Enum: {1}, Default: {2}", this.Name, this.EnumType.Name, this.Default);
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
            return value.GetType() == this.EnumType;
        }

        #endregion
    }
}