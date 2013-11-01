// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EnumExtensions.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.System.Utils
{
    using global::System;

    public static class EnumExtensions
    {
        #region Public Methods and Operators

        public static Enum AndComplementOption(this Enum value, Enum option, Type enumType)
        {
            if (IsSignedTypeCode(value.GetTypeCode()))
            {
                long longVal = Convert.ToInt64(value);
                long longOpt = Convert.ToInt64(option);
                return (Enum)Enum.ToObject(enumType, longVal & ~longOpt);
            }
            else
            {
                ulong longVal = Convert.ToUInt64(value);
                ulong longOpt = Convert.ToUInt64(option);
                return (Enum)Enum.ToObject(enumType, longVal & ~longOpt);
            }
        }

        public static Enum AndOption(this Enum value, Enum option, Type enumType)
        {
            if (IsSignedTypeCode(value.GetTypeCode()))
            {
                long longVal = Convert.ToInt64(value);
                long longOpt = Convert.ToInt64(option);
                return (Enum)Enum.ToObject(enumType, longVal & longOpt);
            }
            else
            {
                ulong longVal = Convert.ToUInt64(value);
                ulong longOpt = Convert.ToUInt64(option);
                return (Enum)Enum.ToObject(enumType, longVal & longOpt);
            }
        }

        /// <summary>
        ///   Checks if any of the options is set in the specified value.
        /// </summary>
        /// <param name="value">Value to check.</param>
        /// <param name="options">Options which are checked.</param>
        /// <returns>True if the at least one option is set for the specified value.</returns>
        public static bool AnyOptionSet(this Enum value, Enum options)
        {
            if (IsSignedTypeCode(value.GetTypeCode()))
            {
                long longVal = Convert.ToInt64(value);
                long longOpt = Convert.ToInt64(options);
                return (longVal & longOpt) != 0;
            }
            else
            {
                ulong longVal = Convert.ToUInt64(value);
                ulong longOpt = Convert.ToUInt64(options);
                return (longVal & longOpt) != 0;
            }
        }

        /// <summary>
        ///   Replacement for Enum.HasFlag in .NET before 4.0.
        /// </summary>
        /// <param name="value">Value to check.</param>
        /// <param name="option">Option which is checked.</param>
        /// <returns>True if the specified option is set for the specified value.</returns>
        public static bool IsOptionSet(this Enum value, Enum option)
        {
            if (IsSignedTypeCode(value.GetTypeCode()))
            {
                long longVal = Convert.ToInt64(value);
                long longOpt = Convert.ToInt64(option);
                return (longVal & longOpt) == longOpt;
            }
            else
            {
                ulong longVal = Convert.ToUInt64(value);
                ulong longOpt = Convert.ToUInt64(option);
                return (longVal & longOpt) == longOpt;
            }
        }

        public static Enum OrOption(this Enum value, Enum option, Type enumType)
        {
            if (IsSignedTypeCode(value.GetTypeCode()))
            {
                long longVal = Convert.ToInt64(value);
                long longOpt = Convert.ToInt64(option);
                return (Enum)Enum.ToObject(enumType, longVal | longOpt);
            }
            else
            {
                ulong longVal = Convert.ToUInt64(value);
                ulong longOpt = Convert.ToUInt64(option);
                return (Enum)Enum.ToObject(enumType, longVal | longOpt);
            }
        }

        #endregion

        #region Methods

        private static bool IsSignedTypeCode(TypeCode code)
        {
            switch (code)
            {
                case TypeCode.Byte:
                case TypeCode.UInt16:
                case TypeCode.UInt32:
                case TypeCode.UInt64:
                    return false;
                default:
                    return true;
            }
        }

        #endregion
    }
}