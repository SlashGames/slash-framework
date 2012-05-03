using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RainyGames.Math.Algebra.Vectors;
using UnityEngine;

namespace RainyGames.Unity.Common.Math
{
    /// <summary>
    ///   Contains extension methods to convert from/to unity/framework mathematical structures.
    /// </summary>
    public static class MathConversionExtensionMethods
    {
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
    }
}
