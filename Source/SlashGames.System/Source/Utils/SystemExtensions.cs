// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SystemExtensions.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace SlashGames.System.Utils
{
    using global::System;

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

        #endregion
    }
}