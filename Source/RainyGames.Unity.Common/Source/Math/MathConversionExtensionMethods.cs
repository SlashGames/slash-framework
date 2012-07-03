// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MathConversionExtensionMethods.cs" company="Rainy Games">
//   Copyright (c) Rainy Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace RainyGames.Unity.Common.Math
{
    using RainyGames.Math.Algebra.Vectors;
    using RainyGames.Math.Geometry.Rectangles;

    using UnityEngine;

    /// <summary>
    ///   Contains extension methods to convert from/to unity/framework mathematical structures.
    /// </summary>
    public static class MathConversionExtensionMethods
    {
        #region Public Methods and Operators

        public static Rect ToRect(this RectangleI rectangle)
        {
            return new Rect(rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height);
        }

        public static Vector2 ToVector2(this Vector2I vector)
        {
            return new Vector2(vector.X, vector.Y);
        }

        public static Vector2F ToVector2F(this Vector2 vector)
        {
            return new Vector2F(vector.x, vector.y);
        }

        public static Vector2I ToVector2I(this Vector2 vector)
        {
            return new Vector2I((int)vector.x, (int)vector.y);
        }

        public static Vector3 ToVector3(this Vector3F vector)
        {
            return new Vector3(vector.X, vector.Y, vector.Z);
        }

        public static Vector3 ToVector3(this Vector2F vector, float height = 0.0f)
        {
            return new Vector3(vector.X, height, vector.Y);
        }

        public static Vector3 ToVector3(this Vector2I vector, float height = 0.0f, float factor = 1.0f)
        {
            return new Vector3(vector.X * factor, height, vector.Y * factor);
        }

        #endregion
    }
}