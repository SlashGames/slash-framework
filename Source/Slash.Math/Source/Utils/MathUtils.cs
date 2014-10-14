// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MathUtils.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Math.Utils
{
    using System;
    using System.Linq;

    using Slash.Math.Algebra.Vectors;

    /// <summary>
    ///   Contains little math utils that are often used. If there are long methods or multiple methods of the same
    ///   type, think about moving them into a seperate class, so this file doesn't get too big.
    /// </summary>
    public static class MathUtils
    {
        #region Constants

        /// <summary>
        ///   Euler's number.
        /// </summary>
        public const float E = 2.7182818284590452f;

        /// <summary>
        ///   Ratio of a circle's circumference to its diameter.
        /// </summary>
        public const float Pi = (float)Math.PI;

        /// <summary>
        ///   Pi divided by two.
        /// </summary>
        public const float PiOver2 = Pi * 0.5f;

        /// <summary>
        ///   Pi divided by four.
        /// </summary>
        public const float PiOver4 = Pi * 0.25f;

        /// <summary>
        ///   Pi multiplied with two.
        /// </summary>
        public const float TwoPi = Pi * 2.0f;

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///   Returns the angle whose cosine is the specified number, in radians.
        /// </summary>
        /// <param name="x">Number representing a cosine.</param>
        /// <returns>Angle whose cosine is the specified number.</returns>
        public static float ACos(float x)
        {
            return (float)Math.Acos(x);
        }

        /// <summary>
        ///   Returns the angle whose sine is the specified number, in radians.
        /// </summary>
        /// <param name="x">Number representing a sine.</param>
        /// <returns>Angle whose sine is the specified number.</returns>
        public static float ASin(float x)
        {
            return (float)Math.Asin(x);
        }

        /// <summary>
        ///   Returns the angle whose tangent is the specified number, in radians.
        /// </summary>
        /// <param name="x">Number representing a tangent.</param>
        /// <returns>Angle whose tangent is the specified number.</returns>
        public static float ATan(float x)
        {
            return (float)Math.Atan(x);
        }

        /// <summary>
        ///   Returns the absolute value of the passed number.
        /// </summary>
        /// <param name="x">Number to compute the absolute value of.</param>
        /// <returns>
        ///   Absolute value of <paramref name="x" />.
        /// </returns>
        public static float Abs(float x)
        {
            return Math.Abs(x);
        }

        /// <summary>
        ///   Returns the angle in radians between the positive x-axis of a plane and the point given by the coordinates (x, y).
        /// </summary>
        /// <param name="y">y-coordinate of the point.</param>
        /// <param name="x">x-coordinate of the point.</param>
        /// <returns>Angle whose tangent is the quotient of the specified numbers.</returns>
        public static float Atan2(float y, float x)
        {
            return (float)Math.Atan2(y, x);
        }

        /// <summary>
        ///   Returns the smallest integral value that is greater than or equal to the specified value.
        /// </summary>
        /// <param name="value">Value to ceil.</param>
        /// <returns>Smallest integral value that is greater than or equal to the specified value.</returns>
        public static float Ceil(float value)
        {
            return (float)Math.Ceiling(value);
        }

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
        /// <typeparam name="T">Type of the value to clamp.</typeparam>
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
        ///   Returns the cosine of the specified angle in radians.
        /// </summary>
        /// <param name="x">Angle to compute the cosine of.</param>
        /// <returns>Cosine of the specified angle.</returns>
        public static float Cos(float x)
        {
            return (float)Math.Cos(x);
        }

        /// <summary>
        ///   Returns e raised to the specified power.
        /// </summary>
        /// <param name="x">Power to raise e to.</param>
        /// <returns>e raised to the specified power.</returns>
        public static float Exp(float x)
        {
            return (float)Math.Exp(x);
        }

        /// <summary>
        ///   Checks if the passed vector's components are "equal". I.e. checking if the difference between the components of the two is smaller than the epsilon value specified in MathUtils.Epsilon.
        /// </summary>
        /// <param name="value1"> The first vector. </param>
        /// <param name="value2"> The second vector. </param>
        /// <returns> True if the values are "equal", false otherwise. </returns>
        public static bool FloatEquals(Vector2F value1, Vector2F value2)
        {
            return Math.Abs(value1.X - value2.X) <= float.Epsilon && Math.Abs(value1.Y - value2.Y) <= float.Epsilon;
        }

        /// <summary>
        ///   Checks if the passed vector's components are "equal". I.e. checking if the difference between the components of the two is smaller than the passed delta.
        /// </summary>
        /// <param name="value1"> The first vector. </param>
        /// <param name="value2"> The second vector. </param>
        /// <param name="delta"> The floating point tolerance. </param>
        /// <returns> True if the values are "equal", false otherwise. </returns>
        public static bool FloatEquals(Vector2F value1, Vector2F value2, float delta)
        {
            return Math.Abs(value1.X - value2.X) <= delta && Math.Abs(value1.Y - value2.Y) <= delta;
        }

        /// <summary>
        ///   Checks if the passed float values are "equal". I.e. checking if the difference between the two is smaller than the epsilon value specified in MathUtils.Epsilon.
        /// </summary>
        /// <param name="value1"> The first floating point Value. </param>
        /// <param name="value2"> The second floating point Value. </param>
        /// <returns> True if the values are "equal", false otherwise. </returns>
        public static bool FloatEquals(float value1, float value2)
        {
            return Math.Abs(value1 - value2) <= float.Epsilon;
        }

        /// <summary>
        ///   Checks if a floating point Value is equal to another, within a certain tolerance.
        /// </summary>
        /// <param name="value1"> The first floating point Value. </param>
        /// <param name="value2"> The second floating point Value. </param>
        /// <param name="delta"> The floating point tolerance. </param>
        /// <returns> True if the values are "equal"; otherwise, false. </returns>
        public static bool FloatEquals(float value1, float value2, float delta)
        {
            return FloatInRange(value1, value2 - delta, value2 + delta);
        }

        /// <summary>
        ///   Checks if a floating point Value is within a specified range of values (inclusive).
        /// </summary>
        /// <param name="value"> The Value to check. </param>
        /// <param name="min"> The minimum Value. </param>
        /// <param name="max"> The maximum Value. </param>
        /// <returns> True if the Value is within the range specified, false otherwise. </returns>
        public static bool FloatInRange(float value, float min, float max)
        {
            return value >= min && value <= max;
        }

        /// <summary>
        ///   Rounds the passed float value down.
        /// </summary>
        /// <param name="x"> Value to round. </param>
        /// <returns> Rounded value. </returns>
        public static float Floor(float x)
        {
            return (float)Math.Floor(x);
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
        /// <typeparam name="T">Type of the value to check.</typeparam>
        /// <param name="value"> Value to check. </param>
        /// <param name="min"> Minimum bound. </param>
        /// <param name="max"> Maximum bound (exclusive). </param>
        /// <returns> True if value is within bounds; otherwise, false. </returns>
        public static bool IsWithinBounds<T>(T value, T min, T max) where T : IComparable<T>
        {
            return value.CompareTo(min) >= 0 && value.CompareTo(max) < 0;
        }

        /// <summary>
        ///   Checks if the specified value is within the specified bounds (i.e. bigger or equal minimum bound and
        ///   smaller or equal maximum bound).
        /// </summary>
        /// <typeparam name="T">Type of the value to check.</typeparam>
        /// <param name="value"> Value to check. </param>
        /// <param name="min"> Minimum bound. </param>
        /// <param name="max"> Maximum bound (inclusive). </param>
        /// <returns> True if value is within bounds; otherwise, false. </returns>
        public static bool IsWithinBoundsInclusive<T>(T value, T min, T max) where T : IComparable<T>
        {
            return value.CompareTo(min) >= 0 && value.CompareTo(max) <= 0;
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
        ///   Returns the natural logarithm of the specified number.
        /// </summary>
        /// <param name="x">Number to compute the logarithm of.</param>
        /// <returns>Natural logarithm of the specified number.</returns>
        public static float Log(float x)
        {
            return (float)Math.Log(x);
        }

        /// <summary>
        ///   Returns the logarithm of the specified number in the passed base.
        /// </summary>
        /// <param name="x">Number to compute the logarithm of.</param>
        /// <param name="newBase">Base of the logarithm to compute.</param>
        /// <returns>Logarithm of the specified number in the passed base.</returns>
        public static float Log(float x, float newBase)
        {
            return (float)Math.Log(x, newBase);
        }

        /// <summary>
        ///   Returns the base 10 logarithm of the specified number.
        /// </summary>
        /// <param name="x">Number to compute the logarithm of.</param>
        /// <returns>Base 10 logarithm of the specified number.</returns>
        public static float Log10(float x)
        {
            return (float)Math.Log10(x);
        }

        /// <summary>
        ///   Returns the base 2 logarithm of the specified number.
        /// </summary>
        /// <param name="x">Number to compute the logarithm of.</param>
        /// <returns>Base 2 logarithm of the specified number.</returns>
        public static float Log2(float x)
        {
            return (float)Math.Log(x, 2);
        }

        /// <summary>
        ///   Returns the maximum value of the two passed values.
        /// </summary>
        /// <typeparam name="T">Type of the values to compare.</typeparam>
        /// <param name="a"> First value. </param>
        /// <param name="b"> Second value. </param>
        /// <returns> First value if it is bigger than the second; otherwise, the second value. </returns>
        public static T Max<T>(T a, T b) where T : IComparable<T>
        {
            return a.CompareTo(b) > 0 ? a : b;
        }

        /// <summary>
        ///   Returns the maximum value of the specified values.
        /// </summary>
        /// <typeparam name="T">Type of values.</typeparam>
        /// <param name="values">Values to compare.</param>
        /// <returns>Maximum value of the specified values.</returns>
        public static T Max<T>(params T[] values)
        {
            return values.Max();
        }

        /// <summary>
        ///   Returns the minimum value of the two passed values.
        /// </summary>
        /// <typeparam name="T">Type of the values to compare.</typeparam>
        /// <param name="a"> First value. </param>
        /// <param name="b"> Second value. </param>
        /// <returns> First value if it is smaller than the second; otherwise, the second value. </returns>
        public static T Min<T>(T a, T b) where T : IComparable<T>
        {
            return a.CompareTo(b) < 0 ? a : b;
        }

        /// <summary>
        ///   Returns the minimum value of the specified values.
        /// </summary>
        /// <typeparam name="T">Type of values.</typeparam>
        /// <param name="values">Values to compare.</param>
        /// <returns>Minimum value of the specified values.</returns>
        public static T Min<T>(params T[] values)
        {
            return values.Min();
        }

        /// <summary>
        ///   Returns the remainder of the Euclidean division of <paramref name="a" /> by <paramref name="n" />.
        /// </summary>
        /// <param name="a">Dividend.</param>
        /// <param name="n">Divisor.</param>
        /// <returns>
        ///   Remainder of the Euclidean division of <paramref name="a" /> by <paramref name="n" />.
        /// </returns>
        public static float Mod(float a, float n)
        {
            return a % n;
        }

        /// <summary>
        ///   Returns the specified number raised to the passed power.
        /// </summary>
        /// <param name="x">Number to raise.</param>
        /// <param name="pow">Power to raise to.</param>
        /// <returns>Specified number raised to the passed power.</returns>
        public static int Pow(int x, uint pow)
        {
            int ret = 1;
            while (pow != 0)
            {
                if ((pow & 1) == 1)
                {
                    ret *= x;
                }
                x *= x;
                pow >>= 1;
            }
            return ret;
        }

        /// <summary>
        ///   Returns the specified number raised to the passed power.
        /// </summary>
        /// <param name="x">Number to raise.</param>
        /// <param name="y">Power to raise to.</param>
        /// <returns>Specified number raised to the passed power.</returns>
        public static float Pow(float x, float y)
        {
            return (float)Math.Pow(x, y);
        }

        /// <summary>
        ///   Returns the square of the specified number.
        /// </summary>
        /// <param name="x">Number to square.</param>
        /// <returns>Square of the specified number.</returns>
        public static int Pow2(int x)
        {
            return x * x;
        }

        /// <summary>
        ///   Returns the square of the specified number.
        /// </summary>
        /// <param name="x">Number to square.</param>
        /// <returns>Square of the specified number.</returns>
        public static float Pow2(float x)
        {
            return x * x;
        }

        /// <summary>
        ///   Returns the cube of the specified number.
        /// </summary>
        /// <param name="x">Number to cube.</param>
        /// <returns>Cube of the specified number.</returns>
        public static float Pow3(float x)
        {
            return x * x * x;
        }

        /// <summary>
        ///   Rounds the specified number to the nearest integral value.
        /// </summary>
        /// <param name="x">Number to round.</param>
        /// <returns>Specified number rounded to the nearest integral value.</returns>
        public static float Round(float x)
        {
            return (float)Math.Round(x);
        }

        /// <summary>
        ///   Rounds the specified number to the passed number of fractional digits.
        /// </summary>
        /// <param name="x">Number to round.</param>
        /// <param name="digits">Number of fractional digits in the return value.</param>
        /// <returns>Specified number rounded to the passed number of fractional digits.</returns>
        public static float Round(float x, int digits)
        {
            return (float)Math.Round(x, digits);
        }

        /// <summary>
        ///   Rounds the specified value to the nearest integral value.
        /// </summary>
        /// <param name="value">Value to round.</param>
        /// <returns>Specified value rounded to the nearest integral value.</returns>
        public static int RoundToInt(float value)
        {
            return (int)Math.Round(value);
        }

        /// <summary>
        ///   Clamps the passed value to 0...1.
        /// </summary>
        /// <param name="value"> Value to saturate. </param>
        /// <returns> Clamped value. </returns>
        public static float Saturate(float value)
        {
            return (value > 1.0f) ? 1.0f : (value < 0.0f) ? 0.0f : value;
        }

        /// <summary>
        ///   Determines the sign of the passed value. Returns -1 if value is negative, 1 if value is positive, 0 if value is zero.
        /// </summary>
        /// <param name="value"> Value to check. </param>
        /// <returns> -1 if value is negative, 1 if value is positive, 0 if value is zero. </returns>
        public static int Sign(float value)
        {
            return Math.Sign(value);
        }

        /// <summary>
        ///   Returns the sine of the specified angle in radians.
        /// </summary>
        /// <param name="x">Angle to compute the sine of.</param>
        /// <returns>Sine of the specified angle.</returns>
        public static float Sin(float x)
        {
            return (float)Math.Sin(x);
        }

        /// <summary>
        ///   Returns the square root of the specified number.
        /// </summary>
        /// <param name="x">Number to compute the square root of.</param>
        /// <returns>Square root of the specified number.</returns>
        public static float Sqrt(float x)
        {
            return (float)Math.Sqrt(x);
        }

        /// <summary>
        ///   Swaps the two specified values.
        /// </summary>
        /// <typeparam name="T">Type of the values to swap.</typeparam>
        /// <param name="value1">First value to swap.</param>
        /// <param name="value2">Second value to swap.</param>
        public static void Swap<T>(ref T value1, ref T value2)
        {
            T tmp = value1;
            value1 = value2;
            value2 = tmp;
        }

        /// <summary>
        ///   Returns the tangent of the specified angle in radians.
        /// </summary>
        /// <param name="x">Angle to compute the tangent of.</param>
        /// <returns>Tangent of the specified angle.</returns>
        public static float Tan(float x)
        {
            return (float)Math.Tan(x);
        }

        /// <summary>
        ///   Wraps value so that it is between 0.0(included) and 1.0(not included). Examples: 1.5 -> 0.5, 1.0 -> 0.0, -0.75 -> 0.25.
        /// </summary>
        /// <param name="value"> Value to wrap. </param>
        /// <returns> Wraped value. </returns>
        public static float Wrap(float value)
        {
            return value - Floor(value);
        }

        /// <summary>
        ///   Wraps a value with a specific base. E.g. 360°
        /// </summary>
        /// <param name="x"> Value to wrap </param>
        /// <param name="y"> Value to wrap around </param>
        /// <returns> Wraped value </returns>
        public static float WrapValue(float x, float y)
        {
            return y * Wrap(x / y);
        }

        #endregion
    }
}