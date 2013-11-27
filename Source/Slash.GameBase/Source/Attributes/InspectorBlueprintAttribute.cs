// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InspectorBlueprintAttribute.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.GameBase.Attributes
{
    using System;

    using Slash.Collections.Utils;
    using Slash.GameBase.Inspector.Validation;

    /// <summary>
    ///   Exposes the property to the landscape designer inspector.
    /// </summary>
    public class InspectorBlueprintAttribute : InspectorPropertyAttribute
    {
        #region Constructors and Destructors

        /// <summary>
        ///   Exposes the property to the landscape designer inspector.
        /// </summary>
        /// <param name="name">Property name to be shown in the inspector.</param>
        public InspectorBlueprintAttribute(string name)
            : base(name)
        {
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///   Types of the components of the blueprints that are available in the inspector.
        /// </summary>
        public Type[] RequiredComponents { get; set; }

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
            return string.Format(
                "Name: {0}, Required Components: {1}", this.Name, CollectionUtils.ToString(this.RequiredComponents));
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
        ///   <c>null</c>, if the passed value is valid for this property,
        ///   and <see cref="ValidationError" /> which contains information about the error otherwise.
        /// </returns>
        public override ValidationError Validate(object value)
        {
            if (value == null)
            {
                return ValidationError.Null;
            }

            if (!(value is string))
            {
                return ValidationError.Default;
            }

            return null;
        }

        #endregion
    }
}