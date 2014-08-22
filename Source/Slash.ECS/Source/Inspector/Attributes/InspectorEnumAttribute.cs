// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InspectorEnumAttribute.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.ECS.Inspector.Attributes
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    ///   Exposes the property to the inspector.
    /// </summary>
    [Serializable]
    public class InspectorEnumAttribute : InspectorPropertyAttribute
    {
        #region Constructors and Destructors

        /// <summary>
        ///   Exposes the property to the inspector.
        /// </summary>
        /// <param name="name">Property name to be shown in the inspector.</param>
        /// <param name="enumType">Type of the enum this attribute is attached to.</param>
        [Obsolete(
            "Enum type doesn't have to be specified anymore as the property type is available to all inspector properties now. So use the normal constructor and set the Default value manually."
            )]
        public InspectorEnumAttribute(string name, Type enumType)
            : base(name)
        {
            this.PropertyType = enumType;
            this.Default = Enum.GetValues(enumType).GetValue(0);
        }

        /// <summary>
        ///   Exposes the property to the inspector.
        /// </summary>
        /// <param name="name">Property name to be shown in the inspector.</param>
        public InspectorEnumAttribute(string name)
            : base(name)
        {
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///   Allowed enum values. If not set, all values are allowed.
        /// </summary>
        public object[] AllowedValues { get; set; }

        /// <summary>
        ///   Indicates if the enum has a Flags attribute.
        /// </summary>
        public bool Flags { get; set; }

        /// <summary>
        ///   Forbidden enum values. If not set, no values are forbidden.
        /// </summary>
        public object[] ForbiddenValues { get; set; }

        /// <summary>
        ///   Returns a collection of values if the property has a defined set of possible values.
        ///   Otherwise <c>null</c> is returned.
        /// </summary>
        public override IEnumerable<object> PossibleValues
        {
            get
            {
                if (this.AllowedValues != null)
                {
                    return this.AllowedValues;
                }

                // Collect all values and skip forbidden ones.
                IEnumerable<object> values = Enum.GetValues(this.PropertyType).Cast<object>();
                IEnumerable<object> allowedValues = this.ForbiddenValues == null
                                                        ? values
                                                        : values.Where(
                                                            value => Array.IndexOf(this.ForbiddenValues, value) == -1);
                return allowedValues;
            }
        }

        #endregion

        #region Public Methods and Operators

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
            try
            {
                value = Enum.Parse(this.PropertyType, text);
                return true;
            }
            catch (Exception)
            {
                value = null;
                return false;
            }
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

        #endregion
    }
}