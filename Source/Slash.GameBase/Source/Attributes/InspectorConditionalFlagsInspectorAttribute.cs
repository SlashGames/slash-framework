// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InspectorConditionalFlagsInspectorAttribute.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.GameBase.Attributes
{
    using Slash.SystemExt;

    using Slash.SystemExt.Utils;

    using global::System;

    /// <summary>
    ///   Property inspector should only be shown if condition is met.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class InspectorConditionalFlagsInspectorAttribute : InspectorConditionalInspectorAttribute
    {
        #region Constructors and Destructors

        /// <summary>
        ///   Constructor.
        /// </summary>
        public InspectorConditionalFlagsInspectorAttribute()
        {
            this.Check = ConditionalFlagsCheck.AllSet;
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///   Indicates how the flags are checked.
        /// </summary>
        public ConditionalFlagsCheck Check { get; set; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///   Checks if the specified value fulfills the condition.
        /// </summary>
        /// <param name="value">Value to check.</param>
        /// <returns>True if the specified value fulfills the condition; otherwise, false.</returns>
        public override bool isFulfilled(object value)
        {
            if (!(value is Enum) || !(this.RequiredConditionValue is Enum))
            {
                return false;
            }

            Enum enumValue = (Enum)value;
            Enum requiredEnumValue = (Enum)this.RequiredConditionValue;

            switch (this.Check)
            {
                case ConditionalFlagsCheck.AllSet:
                    return enumValue.IsOptionSet(requiredEnumValue);
                case ConditionalFlagsCheck.OneSet:
                    return enumValue.AnyOptionSet(requiredEnumValue);
                default:
                    return false;
            }
        }

        #endregion
    }
}