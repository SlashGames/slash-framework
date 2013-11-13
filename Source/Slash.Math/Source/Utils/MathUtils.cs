// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MathUtils.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Math.Utils
{
    using System;

    /// <summary>
    ///   Contains little math utils that are often used. If there are long methods or multiple methods of the same
    ///   type, think about moving them into a seperate class, so this file doesn't get too big.
    /// </summary>
    public static class MathUtils
    {
        #region Constants

        /// <summary>
        ///   Euler's number. See http://en.wikipedia.org/wiki/E_%28mathematical_constant%29 for more details.
        /// </summary>
        public const float E = 2.7182818284590452f;

        public const float Log10E = 0.4342944819032518f;

        public const float Log2E = 1.4426950408889634f;

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///   Converts the specified float value to the ceil value as an integer.
        ///   The smallest integer bigger than or equal the specified value is returned.
        /// </summary>
        /// <param name="value">Value to convert.</param>
        /// <returns>Ceil integer for the specified float value.</returns>
        public static int CeilToInt(float value)
        {
            return (int)Math.Ceiling(value);
        }

        /// <summary>
        ///   Clamps the passed value to the passed bounds (i.e. if value is smaller than min bound it's set to min bound,
        ///   if bigger than max bound it's set to max bound).
        /// </summary>
        /// <param name="value"> Value to clamp. </param>
        /// <param name="min"> Minimum bound. </param>
        /// <param name="max"> Maximum bound (inclusive). </param>
        /// <returns> Clamped value. </returns>
        public static T Clamp<T>(T value, T min, T max) where T : IComparable<T>
        {
            if (value.CompareTo(min) < 0)
            {
                return min;
            }
            if (value.CompareTo(max) > 0)
            {
                return max;
            }
            return value;
        }

        /// <summary>
        ///   Converts the specified float value to the floor value as an integer.
        ///   The largest integer less than or equal the specified value is returned.
        /// </summary>
        /// <param name="value">Value to convert.</param>
        /// <returns>Floor integer for the specified float value.</returns>
        public static int FloorToInt(float value)
        {
            return (int)Math.Floor(value);
        }

        /// <summary>
        ///   Checks if the passed value is within the passed bounds (i.e. bigger or equal minimum bound and
        ///   smaller maximum bound).
        /// </summary>
        /// <param name="value"> Value to check. </param>
        /// <param name="min"> Minimum bound. </param>
        /// <param name="max"> Maximum bound (exclusive). </param>
        /// <returns> True if value is within bounds; otherwise, false. </returns>
        public static bool IsWithinBounds<T>(T value, T min, T max) where T : IComparable<T>
        {
            return value.CompareTo(min) >= 0 && value.CompareTo(max) < 0;
        }

        /// <summary>
        ///   Linear interpolation of a float. TODO: generic
        /// </summary>
        /// <param name="x"> First value. </param>
        /// <param name="y"> Second value. </param>
        /// <param name="step"> Weight. </param>
        /// <returns> Interpolated value. </returns>
        public static float Lerp(float x, float y, float step)
        {
            return x + (step * (y - x));
        }

        /// <summary>
        ///   Returns the maximum value of the two passed values.
        /// </summary>
        /// <param name="a"> First value. </param>
        /// <param name="b"> Second value. </param>
        /// <returns> First value if it is bigger than the second; otherwise, the second value. </returns>
        public static T Max<T>(T a, T b) where T : IComparable<T>
        {
            return a.CompareTo(b) > 0 ? a : b;
        }

        /// <summary>
        ///   Returns the minimum value of the two passed values.
        /// </summary>
        /// <param name="a"> First value. </param>
        /// <param name="b"> Second value. </param>
        /// <returns> First value if it is smaller than the second; otherwise, the second value. </returns>
        public static T Min<T>(T a, T b) where T : IComparable<T>
        {
            return a.CompareTo(b) < 0 ? a : b;
        }

        public static void Swap<T>(ref T value1, ref T value2)
        {
            T tmp = value1;
            value1 = value2;
            value2 = tmp;
        }

        #endregion
    }
}