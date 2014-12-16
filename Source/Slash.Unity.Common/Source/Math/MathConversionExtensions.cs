// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MathConversionExtensions.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Unity.Common.Math
{
    using Slash.Math.Algebra.Vectors;
    using Slash.Math.Geometry.Rectangles;

    using UnityEngine;

    /// <summary>
    ///   Contains extension methods to convert from/to Unity/framework mathematical structures.
    /// </summary>
    public static class MathConversionExtensions
    {
        #region Public Methods and Operators

        /// <summary>
        ///   Converts the specified framework rectangle to an Unity rectangle.
        /// </summary>
        /// <param name="rectangle">Rectangle to convert.</param>
        /// <returns>Corresponding Unity rectangle.</returns>
        public static Rect ToRect(this RectangleI rectangle)
        {
            return new Rect(rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height);
        }

        /// <summary>
        ///   Converts the specified framework rectangle to an Unity rectangle.
        /// </summary>
        /// <param name="rectangle">Rectangle to convert.</param>
        /// <returns>Corresponding Unity rectangle.</returns>
        public static Rect ToRect(this RectangleF rectangle)
        {
            return new Rect(rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height);
        }

        /// <summary>
        ///   Converts the specified Unity rectangle to a framework rectangle.
        /// </summary>
        /// <param name="rectangle">Rectangle to convert.</param>
        /// <returns>Corresponding framework rectangle.</returns>
        public static RectangleF ToRectangleF(this Rect rectangle)
        {
            return new RectangleF(rectangle.x, rectangle.y, rectangle.width, rectangle.height);
        }

        /// <summary>
        ///   Converts the specified Unity rectangle to a framework rectangle.
        /// </summary>
        /// <param name="rectangle">Rectangle to convert.</param>
        /// <returns>Corresponding framework rectangle.</returns>
        public static RectangleI ToRectangleI(this Rect rectangle)
        {
            return new RectangleI((int)rectangle.x, (int)rectangle.y, (int)rectangle.width, (int)rectangle.height);
        }

        /// <summary>
        ///   Converts the specified framework vector to an Unity vector.
        /// </summary>
        /// <param name="vector">Vector to convert.</param>
        /// <returns>Corresponding Unity vector.</returns>
        public static Vector2 ToVector2(this Vector2I vector)
        {
            return new Vector2(vector.X, vector.Y);
        }

        /// <summary>
        ///   Converts the specified framework vector to an Unity vector.
        /// </summary>
        /// <param name="vector">Vector to convert.</param>
        /// <returns>Corresponding Unity vector.</returns>
        public static Vector2 ToVector2(this Vector2F vector)
        {
            return new Vector2(vector.X, vector.Y);
        }

        /// <summary>
        ///   Converts the specified Unity vector to a framework vector.
        /// </summary>
        /// <param name="vector">Vector to convert.</param>
        /// <returns>Corresponding framework vector.</returns>
        public static Vector2F ToVector2F(this Vector2 vector)
        {
            return new Vector2F(vector.x, vector.y);
        }

        /// <summary>
        ///   Converts the specified Unity vector to a framework vector.
        /// </summary>
        /// <param name="vector">Vector to convert.</param>
        /// <returns>Corresponding framework vector.</returns>
        public static Vector2F ToVector2F(this Vector3 vector)
        {
            return new Vector2F(vector.x, vector.y);
        }

        /// <summary>
        ///   Converts the specified Unity vector to a framework vector.
        /// </summary>
        /// <param name="vector">Vector to convert.</param>
        /// <returns>Corresponding framework vector.</returns>
        public static Vector2I ToVector2I(this Vector2 vector)
        {
            return new Vector2I((int)vector.x, (int)vector.y);
        }

        /// <summary>
        ///   Converts the specified framework vector to an Unity vector.
        /// </summary>
        /// <param name="vector">Vector to convert.</param>
        /// <returns>Corresponding Unity vector.</returns>
        public static Vector3 ToVector3(this Vector3F vector)
        {
            return new Vector3(vector.X, vector.Y, vector.Z);
        }

        /// <summary>
        ///   Converts the specified framework vector to an Unity vector.
        /// </summary>
        /// <param name="vector">Vector to convert.</param>
        /// <param name="height">y value of converted vector.</param>
        /// <returns>Corresponding Unity vector.</returns>
        public static Vector3 ToVector3(this Vector2F vector, float height = 0.0f)
        {
            return new Vector3(vector.X, height, vector.Y);
        }

        /// <summary>
        ///   Converts the specified framework vector to an Unity vector,
        ///   multiplying it with the passed scalar.
        /// </summary>
        /// <param name="vector">Vector to convert.</param>
        /// <param name="height">y value of converted vector.</param>
        /// <param name="factor">Scalar to multiply the input vector with before converting it.</param>
        /// <returns>Corresponding Unity vector.</returns>
        public static Vector3 ToVector3(this Vector2I vector, float height = 0.0f, float factor = 1.0f)
        {
            return new Vector3(vector.X * factor, height, vector.Y * factor);
        }

        #endregion
    }
}