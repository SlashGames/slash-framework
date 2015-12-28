// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EnumExtensions.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.SystemExt.Utils
{
    using System;

    /// <summary>
    ///   Utility methods for operating on enum values.
    /// </summary>
    public static class EnumExtensions
    {
        #region Public Methods and Operators

        /// <summary>
        ///   Computes the logical AND of the enum value and the complement of the specified option.
        /// </summary>
        /// <param name="value">First operand.</param>
        /// <param name="option">Second operand to take the complement of.</param>
        /// <param name="enumType">Type of the enum to compute the new value of.</param>
        /// <returns>Logical AND of the enum value and the complement of the specified option.</returns>
        public static Enum AndComplementOption(this Enum value, Enum option, Type enumType)
        {
            if (IsSignedEnumValue(value))
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

        /// <summary>
        ///   Computes the logical AND of the enum value and the specified option.
        /// </summary>
        /// <param name="value">First operand.</param>
        /// <param name="option">Second operand.</param>
        /// <param name="enumType">Type of the enum to compute the new value of.</param>
        /// <returns>Logical AND of the enum value and the specified option.</returns>
        public static Enum AndOption(this Enum value, Enum option, Type enumType)
        {
            if (IsSignedEnumValue(value))
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
            if (IsSignedEnumValue(value))
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
            if (IsSignedEnumValue(value))
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

        /// <summary>
        ///   Returns the next value for the specified enum value.
        ///   Wraps around if required.
        /// </summary>
        /// <typeparam name="T">Enum type.</typeparam>
        /// <param name="value">Enum value to get next value for.</param>
        /// <returns>Next enum value, wraps around if required.</returns>
        public static T NextValue<T>(this T value)
        {
            T[] enumValues = (T[])Enum.GetValues(typeof(T));
            int valueIndex = Array.IndexOf(enumValues, value);
            if (valueIndex < 0)
            {
                throw new ArgumentException("'" + value + "' is no value of enum '" + typeof(T) + "'.", "value");
            }
            return valueIndex >= enumValues.Length - 1 ? enumValues[0] : enumValues[valueIndex + 1];
        }

        /// <summary>
        ///   Computes the logical OR of the enum value and the specified option.
        /// </summary>
        /// <param name="value">First operand.</param>
        /// <param name="option">Second operand.</param>
        /// <param name="enumType">Type of the enum to compute the new value of.</param>
        /// <returns>Logical OR of the enum value and the specified option.</returns>
        public static Enum OrOption(this Enum value, Enum option, Type enumType)
        {
            if (IsSignedEnumValue(value))
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

        /// <summary>
        ///   Checks if the type of the specified enum value is a signed type.
        /// </summary>
        /// <param name="enumValue">Enum value to check the type of.</param>
        /// <returns>
        ///   <c>true</c>, if the type of the specified enum value is a signed type, and <c>false</c> otherwise.
        /// </returns>
        private static bool IsSignedEnumValue(Enum enumValue)
        {
#if WINDOWS_STORE
            var enumType = enumValue.GetType();

            return !(enumType == typeof(byte) || enumType == typeof(ushort) || enumType == typeof(uint)
                    || enumType == typeof(ulong));
#else
            switch (enumValue.GetTypeCode())
            {
                case TypeCode.Byte:
                case TypeCode.UInt16:
                case TypeCode.UInt32:
                case TypeCode.UInt64:
                    return false;
                default:
                    return true;
            }
#endif
        }

        #endregion
    }
}