// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InspectorConditionalFlagsPropertyAttribute.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.ECS.Inspector.Attributes
{
    using System;

    using Slash.SystemExt.Utils;

    /// <summary>
    ///   Property inspector should only be shown if condition is met.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class InspectorConditionalFlagsPropertyAttribute : InspectorConditionalPropertyAttribute
    {
        #region Constructors and Destructors

        /// <summary>
        ///   Constructor.
        /// </summary>
        public InspectorConditionalFlagsPropertyAttribute()
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
        public override bool IsFulfilled(object value)
        {
            if (!(value is Enum) || !(this.RequiredConditionValue is Enum))
            {
                return false;
            }

            Enum enumValue = (Enum)value;
            Enum requiredEnumValue = (Enum)this.RequiredConditionValue;

#if !WINDOWS_STORE
            switch (this.Check)
            {
                case ConditionalFlagsCheck.AllSet:
                    return enumValue.IsOptionSet(requiredEnumValue);
                case ConditionalFlagsCheck.OneSet:
                    return enumValue.AnyOptionSet(requiredEnumValue);
                default:
                    return false;
            }
#else
            throw new NotImplementedException("Not implemented for Windows Store build targets.");
#endif
        }

        #endregion
    }
}