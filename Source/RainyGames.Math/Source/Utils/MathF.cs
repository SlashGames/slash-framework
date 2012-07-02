// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MathF.cs" company="Rainy Games">
//   Copyright (c) Rainy Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace RainyGames.Math.Utils
{
    using System;

    using RainyGames.Math.Algebra.Vectors;

    /// <summary>
    ///   Mathematical functions which use float instead of double.
    /// </summary>
    public static class MathF
    {
        #region Constants

        public const float Pi = (float)Math.PI;

        public const float PiOver2 = Pi * 0.5f;

        public const float PiOver4 = Pi * 0.25f;

        public const float TwoPi = Pi * 2.0f;

        #endregion

        #region Public Methods and Operators

        public static float ACos(float x)
        {
            return (float)Math.Acos(x);
        }

        public static float ASin(float x)
        {
            return (float)Math.Asin(x);
        }

        public static float ATan(float x)
        {
            return (float)Math.Atan(x);
        }

        public static float Atan2(float x, float y)
        {
            return (float)Math.Atan2(x, y);
        }

        /// <summary>
        ///   Rounds the passed float value up and returns the value as integer.
        /// </summary>
        /// <param name="x"> Value to round. </param>
        /// <returns> Rounded integer value. </returns>
        public static int CeilInt(float x)
        {
            return (int)Math.Ceiling(x);
        }

        public static float Ceiling(float x)
        {
            return (float)Math.Ceiling(x);
        }

        public static float Cos(float x)
        {
            return (float)Math.Cos(x);
        }

        /// <summary>
        ///   Exponential and logarithmic functions
        /// </summary>
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
        ///   Rounds the passed float value down and returns the value as integer.
        /// </summary>
        /// <param name="x"> Value to round. </param>
        /// <returns> Rounded integer value. </returns>
        public static int FloorInt(float x)
        {
            return (int)Math.Floor(x);
        }

        public static float Log(float x)
        {
            return (float)Math.Log(x);
        }

        public static float Log(float x, float newBase)
        {
            return (float)Math.Log(x, newBase);
        }

        public static float Log10(float x)
        {
            return (float)Math.Log10(x);
        }

        public static float Log2(float x)
        {
            return (float)Math.Log(x, 2);
        }

        public static float Mod(float x, float y)
        {
            return x % y;
        }

        public static float Pow(float x, float y)
        {
            return (float)Math.Pow(x, y);
        }

        public static float Pow2(float x)
        {
            return x * x;
        }

        public static float Pow3(float x)
        {
            return x * x * x;
        }

        public static float Pow4(float x)
        {
            return Pow2(x) * Pow2(x);
        }

        public static float RSqrt(float x)
        {
            return 1.0f / Sqrt(x);
        }

        public static float Round(float x)
        {
            return (float)Math.Round(x);
        }

        public static float Round(float x, int digits)
        {
            return (float)Math.Round(x, digits);
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

        public static float Sin(float x)
        {
            return (float)Math.Sin(x);
        }

        public static float Sqrt(float x)
        {
            return (float)Math.Sqrt(x);
        }

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