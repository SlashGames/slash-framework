// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SystemExtensions.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.SystemExt.Utils
{
    using System;
    using System.Text.RegularExpressions;

    /// <summary>
    ///   Extension methods for classes of the System namespace.
    /// </summary>
    public static class SystemExtensions
    {
        #region Public Methods and Operators

        /// <summary>
        ///   Checks if the specified value is between the specified lower and higher bound (exclusive).
        /// </summary>
        /// <typeparam name="T"> Type of objects to compare. </typeparam>
        /// <param name="value"> Value to compare. </param>
        /// <param name="low"> Lower bound. </param>
        /// <param name="high"> Higher bound (exclusive). </param>
        /// <returns> True if the value is between the lower and higher bound (exclusive); otherwise, false. </returns>
        public static bool IsBetween<T>(this T value, T low, T high) where T : IComparable<T>
        {
            return value.CompareTo(low) >= 0 && value.CompareTo(high) < 0;
        }

        /// <summary>
        ///   Splits the string, inserting spaces before each capital letter except the first one.
        /// </summary>
        /// <param name="s">String to split.</param>
        /// <returns>Split string.</returns>
        public static string SplitByCapitalLetters(this string s)
        {
            var r = new Regex(
               @"(?<=[A-Z])(?=[A-Z][a-z]) |
                 (?<=[^A-Z])(?=[A-Z]) |
                 (?<=[A-Za-z])(?=[^A-Za-z])",
               RegexOptions.IgnorePatternWhitespace);
            return r.Replace(s, " ");
        }

        #endregion
    }
}