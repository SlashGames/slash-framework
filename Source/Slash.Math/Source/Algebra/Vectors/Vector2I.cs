// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Vector2I.cs" company="Slash Games">
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
    public struct Vector2I
    {
        #region Constants

        /// <summary>
        ///   Unrotated forward vector.
        /// </summary>
        public static Vector2I Forward = new Vector2I(0, 1);

        /// <summary>
        ///   Both vector components are 1.
        /// </summary>
        public static Vector2I One = new Vector2I(1, 1);

        /// <summary>
        ///   Unrotated side vector.
        /// </summary>
        public static Vector2I Side = new Vector2I(1, 0);

        /// <summary>
        ///   X component is 1, Y component is 0.
        /// </summary>
        public static Vector2I UnitX = new Vector2I(1, 0);

        /// <summary>
        ///   X component is 0, Y component is 1.
        /// </summary>
        public static Vector2I UnitY = new Vector2I(0, 1);

        /// <summary>
        ///   Both vector components are 0.
        /// </summary>
        public static Vector2I Zero = new Vector2I(0, 0);

        #endregion

        #region Fields

        /// <summary>
        ///   X component.
        /// </summary>
        [DictionarySerializable]
        public int X;

        /// <summary>
        ///   Y component.
        /// </summary>
        [DictionarySerializable]
        public int Y;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///   Constructor.
        /// </summary>
        /// <param name="vector"> Initial vector. </param>
        public Vector2I(Vector2I vector)
        {
            this.X = vector.X;
            this.Y = vector.Y;
        }

        /// <summary>
        ///   Constructor.
        /// </summary>
        /// <param name="x"> Initial x value. </param>
        /// <param name="y"> Initial y value. </param>
        public Vector2I(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }

        /// <summary>
        ///   Constructor.
        /// </summary>
        /// <param name="values">
        ///   Array which contains the initial vector value. Value at index 0 is taken as the initial x value,
        ///   value at index 1 is taken as the initial y value.
        /// </param>
        public Vector2I(params int[] values)
        {
            if (values == null)
            {
                throw new ArgumentNullException("values");
            }

            if (values.Length < 2)
            {
                throw new ArgumentException("Expected a array which size is at least 2.", "values");
            }

            this.X = values[0];
            this.Y = values[1];
        }

        #endregion

        #region Properties

        /// <summary>
        ///   Indicates if at least one vector component is not zero.
        /// </summary>
        public bool IsNonZero
        {
            get
            {
                return this.X != 0 || this.Y != 0;
            }
        }

        /// <summary>
        ///   Indicates if all vector components are zero.
        /// </summary>
        public bool IsZero
        {
            get
            {
                return this.X == 0 && this.Y == 0;
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
                return (this.X * this.X) + (this.Y * this.Y);
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///   Returns a vector whose components represent the absolute values of
        ///   the components of the specified vector.
        /// </summary>
        /// <param name="vector">Vector to compute the absolute component values of.</param>
        /// <returns>
        ///   Vector whose components represent the absolute values of
        ///   the components of the specified vector.
        /// </returns>
        public static Vector2I Abs(Vector2I vector)
        {
            return new Vector2I(Math.Abs(vector.X), Math.Abs(vector.Y));
        }

        /// <summary>
        ///   Adds the passed values to the x and y value.
        /// </summary>
        /// <param name="addX"> Value to add to x. </param>
        /// <param name="addY"> Value to add to y. </param>
        /// <returns>Sum of <paramref name="addX" /> and <paramref name="addY" />.</returns>
        public Vector2I Add(int addX, int addY)
        {
            return new Vector2I(this.X + addX, this.Y + addY);
        }

        /// <summary>
        ///   Returns a positive number if c is to the left of the line going from a to b.
        /// </summary>
        /// <param name="a">First point of the line.</param>
        /// <param name="b">Second point of the line.</param>
        /// <param name="c">Point to check against the line.</param>
        /// <returns> Positive number if point is left, negative if point is right, and 0 if points are collinear. </returns>
        public static int Area(Vector2I a, Vector2I b, Vector2I c)
        {
            return a.X * (b.Y - c.Y) + b.X * (c.Y - a.Y) + c.X * (a.Y - b.Y);
        }

        /// <summary>
        ///   Calculates the angle between two vectors on a plane.
        /// </summary>
        /// <param name="vector1"> First vector. </param>
        /// <param name="vector2"> Second vector. </param>
        /// <returns>
        ///   Return the angle between two vectors on a plane. The angle is from vector 1 to vector 2, positive
        ///   counter-clockwise. The result is between -pi -> pi.
        /// </returns>
        public static float CalculateAngle(Vector2I vector1, Vector2I vector2)
        {
            float theta1 = MathUtils.Atan2(vector1.Y, vector1.X);
            float theta2 = MathUtils.Atan2(vector2.Y, vector2.X);
            float dtheta = theta2 - theta1;
            while (dtheta > MathUtils.Pi)
            {
                dtheta -= (2 * MathUtils.Pi);
            }
            while (dtheta < -MathUtils.Pi)
            {
                dtheta += (2 * MathUtils.Pi);
            }

            return (dtheta);
        }

        /// <summary>
        ///   Calculates the dot product of this and the passed vector. See http://en.wikipedia.org/wiki/Dot_product for more
        ///   details.
        /// </summary>
        /// <param name="vector"> Vector to calculate dot product with. </param>
        /// <returns> Dot product of this and the passed vector. </returns>
        public int CalculateDotProduct(Vector2I vector)
        {
            return Dot(this, vector);
        }

        /// <summary>
        ///   Determines if three vectors are collinear (ie. on a straight line).
        /// </summary>
        /// <param name="a"> First vector. </param>
        /// <param name="b"> Second vector. </param>
        /// <param name="c"> Third vector. </param>
        /// <returns> True if the three vectors are collinear; otherwise, false. </returns>
        public static bool Collinear(ref Vector2I a, ref Vector2I b, ref Vector2I c)
        {
            return Collinear(ref a, ref b, ref c, 0);
        }

        /// <summary>
        ///   Determines if three vectors are collinear (ie. on a straight line).
        /// </summary>
        /// <param name="a"> First vector. </param>
        /// <param name="b"> Second vector. </param>
        /// <param name="c"> Third vector. </param>
        /// <param name="tolerance"> Tolerance to allow. </param>
        /// <returns> True if the three vectors are collinear; otherwise, false. </returns>
        public static bool Collinear(ref Vector2I a, ref Vector2I b, ref Vector2I c, float tolerance)
        {
            return MathUtils.FloatInRange(Area(a, b, c), -tolerance, tolerance);
        }

        /// <summary>
        ///   Calculates the cross product of the two passed vectors. See http://en.wikipedia.org/wiki/Cross_product for more
        ///   details.
        /// </summary>
        /// <param name="a"> First vector. </param>
        /// <param name="b"> Second vector. </param>
        /// <returns> Cross product of the two passed vectors. </returns>
        public static int Cross(Vector2I a, Vector2I b)
        {
            return a.X * b.Y - a.Y * b.X;
        }

        /// <summary>
        ///   Returns the distance between the two passed vectors.
        /// </summary>
        /// <param name="a"> First vector. </param>
        /// <param name="b"> Second vector. </param>
        /// <returns> Distance betwwen the two passed vectors. </returns>
        public static float Distance(Vector2I a, Vector2I b)
        {
            return (a - b).Magnitude;
        }

        /// <summary>
        ///   Calculates the dot product of the two passed vectors. See http://en.wikipedia.org/wiki/Dot_product for more details.
        /// </summary>
        /// <param name="a"> First vector. </param>
        /// <param name="b"> Second vector. </param>
        /// <returns> Dot product of the two passed vectors. </returns>
        public static int Dot(Vector2I a, Vector2I b)
        {
            return (a.X * b.X) + (a.Y * b.Y);
        }

        /// <summary>
        ///   Determines whether the specified <see cref="T:System.Object" /> is equal to the current
        ///   <see cref="T:System.Object" />.
        /// </summary>
        /// <returns>
        ///   true if the specified <see cref="T:System.Object" /> is equal to the current <see cref="T:System.Object" />;
        ///   otherwise, false.
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
            if (obj.GetType() != this.GetType())
            {
                return false;
            }
            return this.Equals((Vector2I)obj);
        }

        /// <summary>
        ///   Determines whether the specified <see cref="Vector2I" /> is equal to the current <see cref="Vector2I" />.
        /// </summary>
        /// <returns>
        ///   true if the specified <see cref="Vector2I" /> is equal to the current <see cref="Vector2I" />; otherwise, false.
        /// </returns>
        /// <param name="other">
        ///   The <see cref="Vector2I" /> to compare with the current <see cref="Vector2I" />.
        /// </param>
        public bool Equals(Vector2I other)
        {
            return this.X == other.X && this.Y == other.Y;
        }

        /// <summary>
        ///   Calculates the distance between this and the passed vector.
        /// </summary>
        /// <param name="vector"> Vector to compute distance to. </param>
        /// <returns> Distance between this and the passed vector. </returns>
        public float GetDistance(Vector2I vector)
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
                return (this.X * 397) ^ this.Y;
            }
        }

        /// <summary>
        ///   Returns a vector which is perpendicular to this vector.
        /// </summary>
        /// <returns> Vector perpendicular to this one. </returns>
        public Vector2I GetPerpendicularVector()
        {
            return new Vector2I(-this.Y, this.X);
        }

        /// <summary>
        ///   Calculates the square distance between this and the passed vector.
        /// </summary>
        /// <param name="vector"> Vector to compute square distance to. </param>
        /// <returns> Square distance between this and the passed vector. </returns>
        public int GetSquareDistance(Vector2I vector)
        {
            return MathUtils.Pow(vector.X - this.X, 2) + MathUtils.Pow(vector.Y - this.Y, 2);
        }

        /// <summary>
        ///   Checks if this vector is parallel or anti-parallel to the passed one.
        /// </summary>
        /// <param name="other"> Vector to check. </param>
        /// <returns> True if both vectors are parallel or anti-parallel, else false. </returns>
        public bool IsParallel(Vector2I other)
        {
            if (this.IsZero || other.IsZero)
            {
                return false;
            }

            return Math.Abs(this.CalculateDotProduct(other) / (this.Magnitude * other.Magnitude)) == 1;
        }

        /// <summary>
        ///   Sum of the absolute values of the differences of the components of both vectors.
        /// </summary>
        /// <param name="a">First vector.</param>
        /// <param name="b">Second vector.</param>
        /// <returns>|a.X - b.X| + |a.Y - b.Y|</returns>
        public static int ManhattanDistance(Vector2I a, Vector2I b)
        {
            return Math.Abs(a.X - b.X) + Math.Abs(a.Y - b.Y);
        }

        /// <summary>
        ///   Creates a new vector out of the two passed vectors and takes the maximum values from these for each component.
        /// </summary>
        /// <param name="value1"> First vector. </param>
        /// <param name="value2"> Second vector. </param>
        /// <returns> Vector which components are the maximum of the respective components of the two passed vectors. </returns>
        public static Vector2I Max(Vector2I value1, Vector2I value2)
        {
            return new Vector2I(MathUtils.Max(value1.X, value2.X), MathUtils.Max(value1.Y, value2.Y));
        }

        /// <summary>
        ///   Creates a new vector out of the two passed vectors and takes the minimum values from these for each component.
        /// </summary>
        /// <param name="value1"> First vector. </param>
        /// <param name="value2"> Second vector. </param>
        /// <returns> Vector which components are the minimum of the respective components of the two passed vectors. </returns>
        public static Vector2I Min(Vector2I value1, Vector2I value2)
        {
            return new Vector2I(MathUtils.Min(value1.X, value2.X), MathUtils.Min(value1.Y, value2.Y));
        }

        /// <summary>
        ///   Sums the components of the passed vectors and returns the resulting vector.
        /// </summary>
        /// <param name="a"> First vector. </param>
        /// <param name="b"> Second vector. </param>
        /// <returns> Vector which components are the sum of the respective components of the two passed vectors. </returns>
        public static Vector2I operator +(Vector2I a, Vector2I b)
        {
            return new Vector2I(a.X + b.X, a.Y + b.Y);
        }

        /// <summary>
        ///   Adds the passed value to each component of the passed vector.
        /// </summary>
        /// <param name="a"> Vector to add the value to. </param>
        /// <param name="b"> Integer value to add. </param>
        /// <returns>
        ///   Vector where each component is the sum of the particular component of the passed vector plus the passed
        ///   value.
        /// </returns>
        public static Vector2I operator +(Vector2I a, int b)
        {
            return new Vector2I(a.X + b, a.Y + b);
        }

        /// <summary>
        ///   Divides each component of the passed vector by the passed value.
        /// </summary>
        /// <param name="a"> Vector to divide by the value. </param>
        /// <param name="b"> Value to divide by. </param>
        /// <returns>
        ///   Vector where each component is the result of the particular component of the passed vector divided by the
        ///   passed value.
        /// </returns>
        public static Vector2I operator /(Vector2I a, int b)
        {
            return new Vector2I(a.X / b, a.Y / b);
        }

        /// <summary>
        ///   Indicates if the two passed vectors are equal.
        /// </summary>
        /// <param name="a"> First vector. </param>
        /// <param name="b"> Second vector. </param>
        /// <returns> True if the two passed vectors are equal; otherwise, false. </returns>
        public static bool operator ==(Vector2I a, Vector2I b)
        {
            return Equals(a, b);
        }

        /// <summary>
        ///   Indicates if the two passed vectors are not equal.
        /// </summary>
        /// <param name="a"> First vector. </param>
        /// <param name="b"> Second vector. </param>
        /// <returns> True if the two passed vectors are not equal; otherwise, false. </returns>
        public static bool operator !=(Vector2I a, Vector2I b)
        {
            return !Equals(a, b);
        }

        /// <summary>
        ///   Multiplies each vector component of the two passed vectors.
        /// </summary>
        /// <param name="a"> First vector. </param>
        /// <param name="b"> Second vector. </param>
        /// <returns> Vector which components are the product of the respective components of the passed vectors. </returns>
        public static Vector2I operator *(Vector2I a, Vector2I b)
        {
            return new Vector2I(a.X * b.X, a.Y * b.Y);
        }

        /// <summary>
        ///   Multiplies each vector component with the specified value.
        /// </summary>
        /// <param name="a"> Vector to multiply. </param>
        /// <param name="b"> Value to multiply by. </param>
        /// <returns> Vector which components are the product of the respective component of the specified vector and the value. </returns>
        public static Vector2I operator *(Vector2I a, int b)
        {
            return new Vector2I(a.X * b, a.Y * b);
        }

        /// <summary>
        ///   Multiplies each vector component with the specified value.
        /// </summary>
        /// <param name="a"> Value to multiply by. </param>
        /// <param name="b"> Vector to multiply. </param>
        /// <returns> Vector which components are the product of the respective component of the specified vector and the value. </returns>
        public static Vector2I operator *(int a, Vector2I b)
        {
            return new Vector2I(a * b.X, a * b.Y);
        }

        /// <summary>
        ///   Multiplies each vector component with the specified value.
        /// </summary>
        /// <param name="a"> Vector to multiply. </param>
        /// <param name="b"> Value to multiply by. </param>
        /// <returns> Vector which components are the product of the respective component of the specified vector and the value. </returns>
        public static Vector2F operator *(Vector2I a, float b)
        {
            return new Vector2F(a.X * b, a.Y * b);
        }

        /// <summary>
        ///   Multiplies each vector component with the specified value.
        /// </summary>
        /// <param name="a"> Value to multiply by. </param>
        /// <param name="b"> Vector to multiply. </param>
        /// <returns> Vector which components are the product of the respective component of the specified vector and the value. </returns>
        public static Vector2F operator *(float a, Vector2I b)
        {
            return new Vector2F(a * b.X, a * b.Y);
        }

        /// <summary>
        ///   Subtracts the passed value from each component of the passed vector.
        /// </summary>
        /// <param name="a"> Vector to subtract the value from. </param>
        /// <param name="b"> Integer value to subtract. </param>
        /// <returns>
        ///   Vector where each component is the sum of the particular component of the passed vector minus the passed
        ///   value.
        /// </returns>
        public static Vector2I operator -(Vector2I a, int b)
        {
            return new Vector2I(a.X - b, a.Y - b);
        }

        /// <summary>
        ///   Subtracts the components of the second passed vector from the first passed.
        /// </summary>
        /// <param name="a"> First vector. </param>
        /// <param name="b"> Second vector. </param>
        /// <returns> Vector which components are the difference of the respective components of the two passed vectors. </returns>
        public static Vector2I operator -(Vector2I a, Vector2I b)
        {
            return new Vector2I(a.X - b.X, a.Y - b.Y);
        }

        /// <summary>
        ///   Negates each component of the passed vector.
        /// </summary>
        /// <param name="a"> Vector to negate. </param>
        /// <returns> Vector which components have the negated value of the respective components of the passed vector. </returns>
        public static Vector2I operator -(Vector2I a)
        {
            return new Vector2I(-a.X, -a.Y);
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
                "({0},{1})",
                this.X.ToString(CultureInfo.InvariantCulture.NumberFormat),
                this.Y.ToString(CultureInfo.InvariantCulture.NumberFormat));
        }

        #endregion
    }
}