// --------------------------------------------------------------------------------------------------------------------
// <copyright file="VectorExtensions.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Math.Algebra.Vectors
{
    /// <summary>
    ///   Conversions between 2- and 3-dimensional vectors.
    /// </summary>
    public static class VectorExtensions
    {
        #region Public Methods and Operators

        /// <summary>
        ///   Converts a integer vector to a float vector.
        /// </summary>
        /// <param name="vector">Vector to convert.</param>
        /// <returns>Float vector which represents the specified integer vector.</returns>
        public static Vector2F ToVector2F(this Vector2I vector)
        {
            return new Vector2F(vector.X, vector.Y);
        }

        /// <summary>
        ///   Returns the 2D representation of this 3D vector (i.e. x and y coordinates).
        /// </summary>
        /// <param name="vector">Vector to get the 2D representation of.</param>
        /// <returns>2D vector of this vector.</returns>
        public static Vector2F ToVector2F(this Vector3F vector)
        {
            return new Vector2F(vector.X, vector.Y);
        }

        /// <summary>
        ///   Converts a 3D vector to a 2D vector by removing the Z coordinate.
        /// </summary>
        /// <param name="vector">3D vector to convert.</param>
        /// <returns>2D vector which consists of the X and Y coordinate of the 3D vector.</returns>
        public static Vector2I ToVector2I(this Vector3I vector)
        {
            return new Vector2I(vector.X, vector.Y);
        }

        /// <summary>
        ///   Returns the 3D representation of this 2D vector and z coordinate.
        /// </summary>
        /// <param name="vector">Vector to get the 3D representation of.</param>
        /// <param name="z">z-coordinate of the 3D representation of the vector.</param>
        /// <returns>3D vector of this vector.</returns>
        public static Vector3F ToVector3F(this Vector2F vector, float z)
        {
            return new Vector3F(vector.X, vector.Y, z);
        }

        #endregion
    }
}