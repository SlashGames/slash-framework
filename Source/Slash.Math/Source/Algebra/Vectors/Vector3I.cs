// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Vector3I.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Math.Algebra.Vectors
{
    using System;
    using System.Globalization;

    using Slash.Math.Utils;
    using Slash.Serialization.Dictionary;

    /// <summary>
    ///   2-dimensional integer vector.
    /// </summary>
    [Serializable]
    [DictionarySerializable]
    public class Vector3I
    {
        #region Static Fields

        /// <summary>
        ///   Unrotated forward vector.
        /// </summary>
        public static Vector3I Forward = new Vector3I(0, 0, 1);

        /// <summary>
        ///   All vector components are 1.
        /// </summary>
        public static Vector3I One = new Vector3I(1, 1, 1);

        /// <summary>
        ///   Unrotated side vector.
        /// </summary>
        public static Vector3I Right = new Vector3I(1, 0, 0);

        /// <summary>
        ///   Unrotated up vector.
        /// </summary>
        public static Vector3I Up = new Vector3I(0, 1, 0);

        /// <summary>
        ///   All vector components are 0.
        /// </summary>
        public static Vector3I Zero = new Vector3I(0, 0, 0);

        #endregion

        #region Fields

        /// <summary>
        ///   X component.
        /// </summary>
        [DictionarySerializable]
        public readonly int X;

        /// <summary>
        ///   Y component.
        /// </summary>
        [DictionarySerializable]
        public readonly int Y;

        /// <summary>
        ///   Z component.
        /// </summary>
        [DictionarySerializable]
        public readonly int Z;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///   Constructs a new zero vector.
        /// </summary>
        public Vector3I()
        {
            this.X = 0;
            this.Y = 0;
            this.Z = 0;
        }

        /// <summary>
        ///   Constructor.
        /// </summary>
        /// <param name="vector"> Initial vector. </param>
        public Vector3I(Vector3I vector)
        {
            this.X = vector.X;
            this.Y = vector.Y;
            this.Z = vector.Z;
        }

        /// <summary>
        ///   Constructor.
        /// </summary>
        /// <param name="x"> Initial x value. </param>
        /// <param name="y"> Initial y value. </param>
        /// <param name="z"> Initial z value. </param>
        public Vector3I(int x, int y, int z)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
        }

        /// <summary>
        ///   Constructor.
        /// </summary>
        /// <param name="values"> int array which contains the initial vector value. Value at index 0 is taken as the initial x value, value at index 1 is taken as the initial y value, value at index 2 is taken as the initial z value. </param>
        public Vector3I(params int[] values)
        {
            if (values == null)
            {
                throw new ArgumentNullException("values");
            }

            if (values.Length < 3)
            {
                throw new ArgumentException("Expected a int array which size is at least 3.", "values");
            }

            this.X = values[0];
            this.Y = values[1];
            this.Z = values[2];
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///   Indicates if at least one vector component is not zero.
        /// </summary>
        public bool IsNonZero
        {
            get
            {
                return this.X != 0 || this.Y != 0 || this.Z != 0;
            }
        }

        /// <summary>
        ///   Indicates if all vector components are zero.
        /// </summary>
        public bool IsZero
        {
            get
            {
                return this.X == 0 && this.Y == 0 && this.Z == 0;
            }
        }

        /// <summary>
        ///   Magnitude of the vector.
        /// </summary>
        public float Magnitude
        {
            get
            {
                return MathUtils.Sqrt(this.SquareMagnitude);
            }
        }

        /// <summary>
        ///   Square magnitude of the vector.
        /// </summary>
        public int SquareMagnitude
        {
            get
            {
                return (this.X * this.X) + (this.Y * this.Y) + (this.Z * this.Z);
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///   Calculates the dot product of the two passed vectors. See http://en.wikipedia.org/wiki/Dot_product for more details.
        /// </summary>
        /// <param name="a"> First vector. </param>
        /// <param name="b"> Second vector. </param>
        /// <returns> Dot product of the two passed vectors. </returns>
        public static int Dot(Vector3I a, Vector3I b)
        {
            return (a.X * b.X) + (a.Y * b.Y) + (a.Z * b.Z);
        }

        /// <summary>
        ///   Returns a vector that is made from the largest components of two vectors.
        /// </summary>
        /// <param name="lhs"> First vector. </param>
        /// <param name="rhs"> Second vector. </param>
        /// <returns>Vector that is made from the largest components of two vectors.</returns>
        public static Vector3I Max(Vector3I lhs, Vector3I rhs)
        {
            return new Vector3I(MathUtils.Max(lhs.X, rhs.X), MathUtils.Max(lhs.Y, rhs.Y), MathUtils.Max(lhs.Z, rhs.Z));
        }

        /// <summary>
        ///   Returns a vector that is made from the smallest components of two vectors.
        /// </summary>
        /// <param name="lhs"> First vector. </param>
        /// <param name="rhs"> Second vector. </param>
        /// <returns>Vector that is made from the smallest components of two vectors.</returns>
        public static Vector3I Min(Vector3I lhs, Vector3I rhs)
        {
            return new Vector3I(MathUtils.Min(lhs.X, rhs.X), MathUtils.Min(lhs.Y, rhs.Y), MathUtils.Min(lhs.Z, rhs.Z));
        }

        /// <summary>
        ///   Sums the components of the passed vectors and returns the resulting vector.
        /// </summary>
        /// <param name="a"> First vector. </param>
        /// <param name="b"> Second vector. </param>
        /// <returns> Vector which components are the sum of the respective components of the two passed vectors. </returns>
        public static Vector3I operator +(Vector3I a, Vector3I b)
        {
            return new Vector3I(a.X + b.X, a.Y + b.Y, a.Z + b.Z);
        }

        /// <summary>
        ///   Divides each component of the passed vector by the passed value.
        /// </summary>
        /// <param name="a"> Vector to divide by the value. </param>
        /// <param name="b"> Value to divide by. </param>
        /// <returns> Vector where each component is the result of the particular component of the passed vector divided by the passed int value. </returns>
        public static Vector3F operator /(Vector3I a, float b)
        {
            return new Vector3F(a.X / b, a.Y / b, a.Z / b);
        }

        /// <summary>
        ///   Multiplies each vector component of the two passed vectors.
        /// </summary>
        /// <param name="a"> First vector. </param>
        /// <param name="b"> Second vector. </param>
        /// <returns> Vector which components are the product of the respective components of the passed vectors. </returns>
        public static Vector3I operator *(Vector3I a, Vector3I b)
        {
            return new Vector3I(a.X * b.X, a.Y * b.Y, a.Z * b.Z);
        }

        /// <summary>
        ///   Multiplies each vector component with the passed int value.
        /// </summary>
        /// <param name="a"> Vector to multiply. </param>
        /// <param name="b"> int value to multiply by. </param>
        /// <returns> Vector which components are the product of the respective component of the passed vector and the int value. </returns>
        public static Vector3I operator *(Vector3I a, int b)
        {
            return new Vector3I(a.X * b, a.Y * b, a.Z * b);
        }

        /// <summary>
        ///   Multiplies each vector component with the passed int value.
        /// </summary>
        /// <param name="a"> int value to multiply by. </param>
        /// <param name="b"> Vector to multiply. </param>
        /// <returns> Vector which components are the product of the respective component of the passed vector and the int value. </returns>
        public static Vector3I operator *(int a, Vector3I b)
        {
            return new Vector3I(a * b.X, a * b.Y, a * b.Z);
        }

        /// <summary>
        ///   Subtracts the components of the second passed vector from the first passed.
        /// </summary>
        /// <param name="a"> First vector. </param>
        /// <param name="b"> Second vector. </param>
        /// <returns> Vector which components are the difference of the respective components of the two passed vectors. </returns>
        public static Vector3I operator -(Vector3I a, Vector3I b)
        {
            return new Vector3I(a.X - b.X, a.Y - b.Y, a.Z - b.Z);
        }

        /// <summary>
        ///   Negates each component of the passed vector.
        /// </summary>
        /// <param name="a"> Vector to negate. </param>
        /// <returns> Vector which components have the negated value of the respective components of the passed vector. </returns>
        public static Vector3I operator -(Vector3I a)
        {
            return new Vector3I(-a.X, -a.Y, -a.Z);
        }

        /// <summary>
        ///   Calculates the dot product of this and the passed vector. See http://en.wikipedia.org/wiki/Dot_product for more details.
        /// </summary>
        /// <param name="vector"> Vector to calculate dot product with. </param>
        /// <returns> Dot product of this and the passed vector. </returns>
        public int CalculateDotProduct(Vector3I vector)
        {
            return Dot(this, vector);
        }

        /// <summary>
        ///   Determines whether the specified <see cref="T:System.Object" /> is equal to the current <see cref="T:System.Object" />.
        /// </summary>
        /// <returns>
        ///   true if the specified <see cref="T:System.Object" /> is equal to the current <see cref="T:System.Object" />; otherwise, false.
        /// </returns>
        /// <param name="obj">
        ///   The <see cref="T:System.Object" /> to compare with the current <see cref="T:System.Object" />.
        /// </param>
        /// <filterpriority>2</filterpriority>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }
            if (ReferenceEquals(this, obj))
            {
                return true;
            }
            if (obj.GetType() != this.GetType())
            {
                return false;
            }
            return this.Equals((Vector3I)obj);
        }

        /// <summary>
        ///   Calculates the distance between this and the passed vector.
        /// </summary>
        /// <param name="vector"> Vector to compute distance to. </param>
        /// <returns> Distance between this and the passed vector. </returns>
        public float GetDistance(Vector3I vector)
        {
            return MathUtils.Sqrt(this.GetSquareDistance(vector));
        }

        /// <summary>
        ///   Serves as a hash function for a particular type.
        /// </summary>
        /// <returns>
        ///   A hash code for the current <see cref="T:System.Object" />.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = this.X;
                hashCode = (hashCode * 397) ^ this.Y;
                hashCode = (hashCode * 397) ^ this.Z;
                return hashCode;
            }
        }

        /// <summary>
        ///   Returns the normalized vector.
        /// </summary>
        /// <returns> Normalized vector. </returns>
        public Vector3F GetNormalized()
        {
            float magnitude = this.Magnitude;
            if (magnitude != 0.0f)
            {
                return new Vector3F(this.X / magnitude, this.Y / magnitude, this.Z / magnitude);
            }

            return Vector3F.Zero;
        }

        /// <summary>
        ///   Calculates the square distance between this and the passed vector.
        /// </summary>
        /// <param name="vector"> Vector to compute square distance to. </param>
        /// <returns> Square distance between this and the passed vector. </returns>
        public int GetSquareDistance(Vector3I vector)
        {
            return MathUtils.Pow2(vector.X - this.X) + MathUtils.Pow2(vector.Y - this.Y)
                   + MathUtils.Pow2(vector.Z - this.Z);
        }

        /// <summary>
        ///   Returns a string that represents the current object.
        /// </summary>
        /// <returns>
        ///   A string that represents the current object.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        public override string ToString()
        {
            return string.Format(
                "({0},{1},{2})",
                this.X.ToString(CultureInfo.InvariantCulture.NumberFormat),
                this.Y.ToString(CultureInfo.InvariantCulture.NumberFormat),
                this.Z.ToString(CultureInfo.InvariantCulture.NumberFormat));
        }

        #endregion

        #region Methods

        /// <summary>
        ///   Determines whether the specified <see cref="Vector3I" /> is equal to the current <see cref="Vector3I" />.
        /// </summary>
        /// <returns>
        ///   true if the specified <see cref="Vector3I" /> is equal to the current <see cref="Vector3I" />; otherwise, false.
        /// </returns>
        /// <param name="other">
        ///   The <see cref="Vector3I" /> to compare with the current <see cref="Vector3I" />.
        /// </param>
        protected bool Equals(Vector3I other)
        {
            return this.X == other.X && this.Y == other.Y && this.Z == other.Z;
        }

        #endregion
    }
}