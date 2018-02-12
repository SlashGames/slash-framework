﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MathConversionExtensions.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Unity.Common.Math
{
    using System;

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
        [Obsolete("Deprecated to make conversion clearer. Use ToVector2FXY instead.")]
        public static Vector2F ToVector2F(this Vector3 vector)
        {
            return new Vector2F(vector.x, vector.y);
        }

        /// <summary>
        ///   Converts the specified Unity vector to a framework vector.
        /// </summary>
        /// <param name="vector">Vector to convert.</param>
        /// <returns>Corresponding framework vector.</returns>
        public static Vector2F ToVector2FXY(this Vector3 vector)
        {
            return new Vector2F(vector.x, vector.y);
        }

        /// <summary>
        ///   Converts the specified Unity vector to a framework vector.
        /// </summary>
        /// <param name="vector">Vector to convert.</param>
        /// <returns>Corresponding framework vector.</returns>
        public static Vector2F ToVector2FXZ(this Vector3 vector)
        {
            return new Vector2F(vector.x, vector.z);
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
        ///   Converts the specified Unity vector to a framework vector.
        /// </summary>
        /// <param name="vector">Vector to convert.</param>
        /// <returns>Corresponding framework vector.</returns>
        public static Vector2I ToVector2IXY(this Vector3 vector)
        {
            return new Vector2I((int)vector.x, (int)vector.y);
        }

        /// <summary>
        ///   Converts the specified Unity vector to a framework vector.
        /// </summary>
        /// <param name="vector">Vector to convert.</param>
        /// <returns>Corresponding framework vector.</returns>
        public static Vector2I ToVector2IXZ(this Vector3 vector)
        {
            return new Vector2I((int)vector.x, (int)vector.z);
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
        /// <param name="z">Z value.</param>
        /// <returns>Corresponding Unity vector.</returns>
        [Obsolete("Deprecated to make conversion clearer. Use ToVector3XY instead.")]
        public static Vector3 ToVector3(this Vector2F vector, float z = 0)
        {
            return new Vector3(vector.X, vector.Y, z);
        }

        /// <summary>
        ///   Converts the specified framework vector to an Unity vector.
        /// </summary>
        /// <param name="vector">Vector to convert.</param>
        /// <param name="z">Z value.</param>
        /// <returns>Corresponding Unity vector.</returns>
        public static Vector3 ToVector3XY(this Vector2F vector, float z = 0)
        {
            return new Vector3(vector.X, vector.Y, z);
        }

        /// <summary>
        ///   Converts the specified framework vector to an Unity vector.
        /// </summary>
        /// <param name="vector">Vector to convert.</param>
        /// <param name="y">Y value.</param>
        /// <returns>Corresponding Unity vector.</returns>
        public static Vector3 ToVector3XZ(this Vector2F vector, float y = 0)
        {
            return new Vector3(vector.X, y, vector.Y);
        }

        #endregion
    }
}