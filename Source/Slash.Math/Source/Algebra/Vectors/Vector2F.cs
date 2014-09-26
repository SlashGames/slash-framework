// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Vector2F.cs" company="Slash Games">
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
    ///   2-dimensional float vector.
    /// </summary>
#if !WINDOWS_STORE && !WINDOWS_PHONE
    [Serializable]
#endif
    [DictionarySerializable]
    public class Vector2F
    {
        #region Static Fields

        /// <summary>
        ///   Unrotated forward vector.
        /// </summary>
        public static Vector2F Forward = new Vector2F(0, 1);

        /// <summary>
        ///   Both vector components are -1.
        /// </summary>
        public static Vector2F MinusOne = new Vector2F(-1, -1);

        /// <summary>
        ///   Both vector components are 1.
        /// </summary>
        public static Vector2F One = new Vector2F(1, 1);

        /// <summary>
        ///   Unrotated side vector.
        /// </summary>
        public static Vector2F Side = new Vector2F(1, 0);

        /// <summary>
        ///   Angle to transform forward to side vector.
        /// </summary>
        public static float SideAngle = -MathUtils.PiOver2;

        /// <summary>
        ///   X component is 1, Y component is 0.
        /// </summary>
        public static Vector2F UnitX = new Vector2F(1, 0);

        /// <summary>
        ///   X component is 0, Y component is 1.
        /// </summary>
        public static Vector2F UnitY = new Vector2F(0, 1);

        /// <summary>
        ///   Both vector components are 0.
        /// </summary>
        public static Vector2F Zero = new Vector2F(0, 0);

        #endregion

        #region Fields

        /// <summary>
        ///   X component.
        /// </summary>
        [DictionarySerializable]
        public readonly float X;

        /// <summary>
        ///   Y component.
        /// </summary>
        [DictionarySerializable]
        public readonly float Y;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///   Constructs a new zero vector.
        /// </summary>
        public Vector2F()
        {
            this.X = 0;
            this.Y = 0;
        }

        /// <summary>
        ///   Constructor.
        /// </summary>
        /// <param name="vector"> Initial vector. </param>
        public Vector2F(Vector2F vector)
        {
            this.X = vector.X;
            this.Y = vector.Y;
        }

        /// <summary>
        ///   Constructor.
        /// </summary>
        /// <param name="x"> Initial x value. </param>
        /// <param name="y"> Initial y value. </param>
        public Vector2F(float x, float y)
        {
            this.X = x;
            this.Y = y;
        }

        /// <summary>
        ///   Constructor.
        /// </summary>
        /// <param name="values">
        ///   Float array which contains the initial vector value.
        ///   Value at index 0 is taken as the initial x value, value at index 1 is taken as the initial y value.
        /// </param>
        public Vector2F(params float[] values)
        {
            if (values == null)
            {
                throw new ArgumentNullException("values");
            }

            if (values.Length < 2)
            {
                throw new ArgumentException("Expected a float array which size is at least 2.", "values");
            }

            this.X = values[0];
            this.Y = values[1];
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
        public float SquareMagnitude
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
        public static Vector2F Abs(Vector2F vector)
        {
            return new Vector2F(Math.Abs(vector.X), Math.Abs(vector.Y));
        }

        /// <summary>
        ///   Returns a positive number if c is to the left of the line going from a to b.
        /// </summary>
        /// <param name="a">First point of the line.</param>
        /// <param name="b">Second point of the line.</param>
        /// <param name="c">Point to check against the line.</param>
        /// <returns> Positive number if point is left, negative if point is right, and 0 if points are collinear. </returns>
        public static float Area(Vector2F a, Vector2F b, Vector2F c)
        {
            return a.X * (b.Y - c.Y) + b.X * (c.Y - a.Y) + c.X * (a.Y - b.Y);
        }

        /// <summary>
        ///   Calculates the angle between two vectors on a plane.
        /// </summary>
        /// <param name="vector1"> First vector. </param>
        /// <param name="vector2"> Second vector. </param>
        /// <returns> Return the angle between two vectors on a plane. The angle is from vector 1 to vector 2, positive counter-clockwise. The result is between -pi -> pi. </returns>
        public static float CalculateAngle(Vector2F vector1, Vector2F vector2)
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
        ///   Determines if three vectors are collinear (ie. on a straight line).
        /// </summary>
        /// <param name="a"> First vector. </param>
        /// <param name="b"> Second vector. </param>
        /// <param name="c"> Third vector. </param>
        /// <returns> True if the three vectors are collinear; otherwise, false. </returns>
        public static bool Collinear(ref Vector2F a, ref Vector2F b, ref Vector2F c)
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
        public static bool Collinear(ref Vector2F a, ref Vector2F b, ref Vector2F c, float tolerance)
        {
            return MathUtils.FloatInRange(Area(a, b, c), -tolerance, tolerance);
        }

        /// <summary>
        ///   Calculates the cross product of the two passed vectors. See http://en.wikipedia.org/wiki/Cross_product for more details.
        /// </summary>
        /// <param name="a"> First vector. </param>
        /// <param name="b"> Second vector. </param>
        /// <returns> Cross product of the two passed vectors. </returns>
        public static float Cross(Vector2F a, Vector2F b)
        {
            return a.X * b.Y - a.Y * b.X;
        }

        /// <summary>
        ///   Returns the distance between the two passed vectors.
        /// </summary>
        /// <param name="a"> First vector. </param>
        /// <param name="b"> Second vector. </param>
        /// <returns> Distance betwwen the two passed vectors. </returns>
        public static float Distance(Vector2F a, Vector2F b)
        {
            return (a - b).Magnitude;
        }

        /// <summary>
        ///   Calculates the dot product of the two passed vectors. See http://en.wikipedia.org/wiki/Dot_product for more details.
        /// </summary>
        /// <param name="a"> First vector. </param>
        /// <param name="b"> Second vector. </param>
        /// <returns> Dot product of the two passed vectors. </returns>
        public static float Dot(Vector2F a, Vector2F b)
        {
            return (a.X * b.X) + (a.Y * b.Y);
        }

        /// <summary>
        ///   Creates a new vector out of the two passed vectors and takes the maximum values from these for each component.
        /// </summary>
        /// <param name="value1"> First vector. </param>
        /// <param name="value2"> Second vector. </param>
        /// <returns> Vector which components are the maximum of the respective components of the two passed vectors. </returns>
        public static Vector2F Max(Vector2F value1, Vector2F value2)
        {
            return new Vector2F(MathUtils.Max(value1.X, value2.X), MathUtils.Max(value1.Y, value2.Y));
        }

        /// <summary>
        ///   Creates a new vector out of the two passed vectors and takes the minimum values from these for each component.
        /// </summary>
        /// <param name="value1"> First vector. </param>
        /// <param name="value2"> Second vector. </param>
        /// <returns> Vector which components are the minimum of the respective components of the two passed vectors. </returns>
        public static Vector2F Min(Vector2F value1, Vector2F value2)
        {
            return new Vector2F(MathUtils.Min(value1.X, value2.X), MathUtils.Min(value1.Y, value2.Y));
        }

        /// <summary>
        ///   Spherically interpolates between the two passed vectors.
        /// </summary>
        /// <param name="from">First point of the arc.</param>
        /// <param name="to">Last point of the arc.</param>
        /// <param name="step">Interpolation parameter.</param>
        /// <returns>
        ///   Value of <paramref name="step" /> along the path along the line segment in the plane.
        /// </returns>
        public static Vector2F Slerp(Vector2F from, Vector2F to, float step)
        {
            if (step == 0)
            {
                return from;
            }

            if (from == to || step == 1)
            {
                return to;
            }

            float theta = MathUtils.ACos(Dot(from, to));
            if (theta == 0)
            {
                return to;
            }

            float sinTheta = MathUtils.Sin(theta);
            if (sinTheta == 0.0f)
            {
                return to;
            }

            return MathUtils.Sin((1 - step) * theta) / sinTheta * from + MathUtils.Sin(step * theta) / sinTheta * to;
        }

        /// <summary>
        ///   Sums the components of the passed vectors and returns the resulting vector.
        /// </summary>
        /// <param name="a"> First vector. </param>
        /// <param name="b"> Second vector. </param>
        /// <returns> Vector which components are the sum of the respective components of the two passed vectors. </returns>
        public static Vector2F operator +(Vector2F a, Vector2F b)
        {
            if (a == null || b == null)
            {
                return null;
            }

            return new Vector2F(a.X + b.X, a.Y + b.Y);
        }

        /// <summary>
        ///   Adds the passed float value to each component of the passed vector.
        /// </summary>
        /// <param name="a"> Vector to add the float value to. </param>
        /// <param name="b"> Float value to add. </param>
        /// <returns> Vector where each component is the sum of the particular component of the passed vector plus the passed float value. </returns>
        public static Vector2F operator +(Vector2F a, float b)
        {
            return a != null ? new Vector2F(a.X + b, a.Y + b) : null;
        }

        /// <summary>
        ///   Divides each component of the passed vector by the passed value.
        /// </summary>
        /// <param name="a"> Vector to divide by the float value. </param>
        /// <param name="b"> Float value to divide by. </param>
        /// <returns> Vector where each component is the result of the particular component of the passed vector divided by the passed float value. </returns>
        public static Vector2F operator /(Vector2F a, float b)
        {
            return a != null ? new Vector2F(a.X / b, a.Y / b) : null;
        }

        /// <summary>
        ///   Indicates if the two passed vectors are equal.
        /// </summary>
        /// <param name="a"> First vector. </param>
        /// <param name="b"> Second vector. </param>
        /// <returns> True if the two passed vectors are equal; otherwise, false. </returns>
        public static bool operator ==(Vector2F a, Vector2F b)
        {
            return Equals(a, b);
        }

        /// <summary>
        ///   Indicates if the two passed vectors are not equal.
        /// </summary>
        /// <param name="a"> First vector. </param>
        /// <param name="b"> Second vector. </param>
        /// <returns> True if the two passed vectors are not equal; otherwise, false. </returns>
        public static bool operator !=(Vector2F a, Vector2F b)
        {
            return !Equals(a, b);
        }

        /// <summary>
        ///   Multiplies each vector component of the two passed vectors.
        /// </summary>
        /// <param name="a"> First vector. </param>
        /// <param name="b"> Second vector. </param>
        /// <returns> Vector which components are the product of the respective components of the passed vectors. </returns>
        public static Vector2F operator *(Vector2F a, Vector2F b)
        {
            if (a == null || b == null)
            {
                return null;
            }

            return new Vector2F(a.X * b.X, a.Y * b.Y);
        }

        /// <summary>
        ///   Multiplies each vector component with the passed float value.
        /// </summary>
        /// <param name="a"> Vector to multiply. </param>
        /// <param name="b"> Float value to multiply by. </param>
        /// <returns> Vector which components are the product of the respective component of the passed vector and the float value. </returns>
        public static Vector2F operator *(Vector2F a, float b)
        {
            return a != null ? new Vector2F(a.X * b, a.Y * b) : null;
        }

        /// <summary>
        ///   Multiplies each vector component with the passed float value.
        /// </summary>
        /// <param name="a"> Float value to multiply by. </param>
        /// <param name="b"> Vector to multiply. </param>
        /// <returns> Vector which components are the product of the respective component of the passed vector and the float value. </returns>
        public static Vector2F operator *(float a, Vector2F b)
        {
            return b != null ? new Vector2F(a * b.X, a * b.Y) : null;
        }

        /// <summary>
        ///   Subtracts the passed float value from each component of the passed vector.
        /// </summary>
        /// <param name="a"> Vector to subtract the float value from. </param>
        /// <param name="b"> Float value to subtract. </param>
        /// <returns> Vector where each component is the sum of the particular component of the passed vector minus the passed float value. </returns>
        public static Vector2F operator -(Vector2F a, float b)
        {
            return a != null ? new Vector2F(a.X - b, a.Y - b) : null;
        }

        /// <summary>
        ///   Subtracts the components of the second passed vector from the first passed.
        /// </summary>
        /// <param name="a"> First vector. </param>
        /// <param name="b"> Second vector. </param>
        /// <returns> Vector which components are the difference of the respective components of the two passed vectors. </returns>
        public static Vector2F operator -(Vector2F a, Vector2F b)
        {
            if (a == null || b == null)
            {
                return null;
            }

            return new Vector2F(a.X - b.X, a.Y - b.Y);
        }

        /// <summary>
        ///   Negates each component of the passed vector.
        /// </summary>
        /// <param name="a"> Vector to negate. </param>
        /// <returns> Vector which components have the negated value of the respective components of the passed vector. </returns>
        public static Vector2F operator -(Vector2F a)
        {
            return a != null ? new Vector2F(-a.X, -a.Y) : null;
        }

        /// <summary>
        ///   Adds the passed values to the x and y value.
        /// </summary>
        /// <param name="addX"> Value to add to x. </param>
        /// <param name="addY"> Value to add to y. </param>
        /// <returns>Sum of <paramref name="addX"/> and <paramref name="addY"/>.</returns>
        public Vector2F Add(float addX, float addY)
        {
            return new Vector2F(this.X + addX, this.Y + addY);
        }

        /// <summary>
        ///   Calculates the dot product of this and the passed vector. See http://en.wikipedia.org/wiki/Dot_product for more details.
        /// </summary>
        /// <param name="vector"> Vector to calculate dot product with. </param>
        /// <returns> Dot product of this and the passed vector. </returns>
        public float CalculateDotProduct(Vector2F vector)
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
            return this.Equals((Vector2F)obj);
        }

        /// <summary>
        ///   Calculates the distance between this and the passed vector.
        /// </summary>
        /// <param name="vector"> Vector to compute distance to. </param>
        /// <returns> Distance between this and the passed vector. </returns>
        public float GetDistance(Vector2F vector)
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
                return (this.X.GetHashCode() * 397) ^ this.Y.GetHashCode();
            }
        }

        /// <summary>
        ///   Returns the normalized vector.
        /// </summary>
        /// <returns> This vector normalized. </returns>
        public Vector2F GetNormalized()
        {
            float magnitude = this.Magnitude;
            if (magnitude != 0.0f)
            {
                return new Vector2F(this.X / magnitude, this.Y / magnitude);
            }

            return Zero;
        }

        /// <summary>
        ///   Returns a vector which is perpendicular to this vector.
        /// </summary>
        /// <returns> Vector perpendicular to this one. </returns>
        public Vector2F GetPerpendicularVector()
        {
            return new Vector2F(-this.Y, this.X);
        }

        /// <summary>
        ///   Creates a new vector which is reflected by the specified vector.
        /// </summary>
        /// <param name="reflect">Vector to reflect with.</param>
        /// <returns>This vector reflected with the specified vector.</returns>
        public Vector2F GetReflected(Vector2F reflect)
        {
            Vector2F normalizedReflect = reflect.GetNormalized();
            return
                new Vector2F(
                    this.X - 2 * normalizedReflect.X * (normalizedReflect.X * this.X + normalizedReflect.Y * this.Y),
                    this.Y - 2 * normalizedReflect.Y * (normalizedReflect.X * this.X + normalizedReflect.Y * this.Y));
        }

        /// <summary>
        ///   Calculates the square distance between this and the passed vector.
        /// </summary>
        /// <param name="vector"> Vector to compute square distance to. </param>
        /// <returns> Square distance between this and the passed vector. </returns>
        public float GetSquareDistance(Vector2F vector)
        {
            return MathUtils.Pow(vector.X - this.X, 2) + MathUtils.Pow(vector.Y - this.Y, 2);
        }

        /// <summary>
        ///   Checks if this vector is parallel or anti-parallel to the passed one.
        /// </summary>
        /// <param name="other"> Vector to check. </param>
        /// <returns> True if both vectors are parallel or anti-parallel, else false. </returns>
        public bool IsParallel(Vector2F other)
        {
            if (this.IsZero || other.IsZero)
            {
                return false;
            }

            return Math.Abs(this.CalculateDotProduct(other) / (this.Magnitude * other.Magnitude)) == 1;
        }

        /// <summary>
        ///   Creates a new vector which is limited to the specified length.
        ///   If the vector is longer it is truncated, otherwise it stays the same.
        /// </summary>
        /// <param name="limit"> Maximum magnitude. </param>
        /// <returns>Vector with the same orientation as this vector, limited to the specified length.</returns>
        public Vector2F Limit(float limit)
        {
            if (this.SquareMagnitude > limit * limit)
            {
                return this.GetNormalized() * limit;
            }
            return this;
        }

        /// <summary>
        ///   Normalizes this vector.
        /// </summary>
        /// <returns>Vector with the same orientation as this one, and unit length.</returns>
        public Vector2F Normalize()
        {
            float magnitude = this.Magnitude;
            if (magnitude != 0.0f)
            {
                return new Vector2F(this.X / magnitude, this.Y / magnitude);
            }

            return new Vector2F(this.X, this.Y);
        }

        /// <summary>
        ///   Rotates the vector counter-clockwise.
        /// </summary>
        /// <param name="angle"> Angle (in radians). </param>
        /// <returns> Rotated vector. </returns>
        public Vector2F Rotate(float angle)
        {
            float cos = MathUtils.Cos(angle);
            float sin = MathUtils.Sin(angle);
            float newX = this.X * cos - this.Y * sin;
            float newY = this.Y * cos + this.X * sin;
            return new Vector2F(newX, newY);
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

        /// <summary>
        ///   Changes the magnitude of this vector to the passed value.
        /// </summary>
        /// <param name="length"> New magnitude. </param>
        /// <returns>Vector with the same orientation as this one, and the specified length.</returns>
        public Vector2F Truncate(float length)
        {
            float magnitude = this.Magnitude;
            if (magnitude != 0.0f)
            {
                return new Vector2F(this.X / magnitude * length, this.Y / magnitude * length);
            }
            return new Vector2F(this.X, this.Y);
        }

        #endregion

        #region Methods

        /// <summary>
        ///   Determines whether the specified <see cref="Vector2F" /> is equal to the current <see cref="Vector2F" />.
        /// </summary>
        /// <returns>
        ///   true if the specified <see cref="Vector2F" /> is equal to the current <see cref="Vector2F" />; otherwise, false.
        /// </returns>
        /// <param name="other">
        ///   The <see cref="Vector2F" /> to compare with the current <see cref="Vector2F" />.
        /// </param>
        protected bool Equals(Vector2F other)
        {
            return this.X.Equals(other.X) && this.Y.Equals(other.Y);
        }

        #endregion
    }
}