// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Vector2F.cs" company="Rainy Games">
//   Copyright (c) Rainy Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace RainyGames.Math.Algebra.Vectors
{
    using System;
    using System.Globalization;

    using RainyGames.Math.Utils;

    /// <summary>
    ///   Struct which represents a 2 dimensional float vector.
    /// </summary>
    [Serializable]
    public struct Vector2F
    {
        #region Static Fields

        /// <summary>
        ///   Unrotated forward vector.
        /// </summary>
        public static Vector2F Forward = new Vector2F(0, 1);

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
        public static float SideAngle = -MathF.PiOver2;

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
        public float X;

        /// <summary>
        ///   Y component.
        /// </summary>
        public float Y;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///   Constructor.
        /// </summary>
        /// <param name="vector"> Initial vector. </param>
        public Vector2F(Vector2F vector)
            : this()
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
            : this()
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
            : this()
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
                return MathF.Sqrt(this.SquareMagnitude);
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

        public static Vector2F Abs(Vector2F vector)
        {
            return new Vector2F(Math.Abs(vector.X), Math.Abs(vector.Y));
        }

        /// <summary>
        ///   Returns a positive number if c is to the left of the line going from a to b.
        /// </summary>
        /// <returns> Positive number if point is left, negative if point is right, and 0 if points are collinear. </returns>
        public static float Area(Vector2F a, Vector2F b, Vector2F c)
        {
            return Area(ref a, ref b, ref c);
        }

        /// <summary>
        ///   Returns a positive number if c is to the left of the line going from a to b.
        /// </summary>
        /// <returns> Positive number if point is left, negative if point is right, and 0 if points are collinear. </returns>
        public static float Area(ref Vector2F a, ref Vector2F b, ref Vector2F c)
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
            float theta1 = MathF.Atan2(vector1.Y, vector1.X);
            float theta2 = MathF.Atan2(vector2.Y, vector2.X);
            float dtheta = theta2 - theta1;
            while (dtheta > MathF.Pi)
            {
                dtheta -= (2 * MathF.Pi);
            }
            while (dtheta < -MathF.Pi)
            {
                dtheta += (2 * MathF.Pi);
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
            return MathF.FloatInRange(Area(ref a, ref b, ref c), -tolerance, tolerance);
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

            float theta = MathF.ACos(Dot(from, to));
            if (theta == 0)
            {
                return to;
            }

            float sinTheta = MathF.Sin(theta);
            if (sinTheta == 0.0f)
            {
                return to;
            }

            return MathF.Sin((1 - step) * theta) / sinTheta * from + MathF.Sin(step * theta) / sinTheta * to;
        }

        /// <summary>
        ///   Sums the components of the passed vectors and returns the resulting vector.
        /// </summary>
        /// <param name="a"> First vector. </param>
        /// <param name="b"> Second vector. </param>
        /// <returns> Vector which components are the sum of the respective components of the two passed vectors. </returns>
        public static Vector2F operator +(Vector2F a, Vector2F b)
        {
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
            return new Vector2F(a.X + b, a.Y + b);
        }

        /// <summary>
        ///   Divides each component of the passed vector by the passed value.
        /// </summary>
        /// <param name="a"> Vector to divide by the float value. </param>
        /// <param name="b"> Float value to divide by. </param>
        /// <returns> Vector where each component is the result of the particular component of the passed vector divided by the passed float value. </returns>
        public static Vector2F operator /(Vector2F a, float b)
        {
            return new Vector2F(a.X / b, a.Y / b);
        }

        /// <summary>
        ///   Indicates if the two passed vectors are equal.
        /// </summary>
        /// <param name="a"> First vector. </param>
        /// <param name="b"> Second vector. </param>
        /// <returns> True if the two passed vectors are equal; otherwise, false. </returns>
        public static bool operator ==(Vector2F a, Vector2F b)
        {
            return a.Equals(b);
        }

        /// <summary>
        ///   Indicates if the two passed vectors are not equal.
        /// </summary>
        /// <param name="a"> First vector. </param>
        /// <param name="b"> Second vector. </param>
        /// <returns> True if the two passed vectors are not equal; otherwise, false. </returns>
        public static bool operator !=(Vector2F a, Vector2F b)
        {
            return a.Equals(b) == false;
        }

        /// <summary>
        ///   Multiplies each vector component of the two passed vectors.
        /// </summary>
        /// <param name="a"> First vector. </param>
        /// <param name="b"> Second vector. </param>
        /// <returns> Vector which components are the product of the respective components of the passed vectors. </returns>
        public static Vector2F operator *(Vector2F a, Vector2F b)
        {
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
            return new Vector2F(a.X * b, a.Y * b);
        }

        /// <summary>
        ///   Multiplies each vector component with the passed float value.
        /// </summary>
        /// <param name="a"> Float value to multiply by. </param>
        /// <param name="b"> Vector to multiply. </param>
        /// <returns> Vector which components are the product of the respective component of the passed vector and the float value. </returns>
        public static Vector2F operator *(float a, Vector2F b)
        {
            return new Vector2F(a * b.X, a * b.Y);
        }

        /// <summary>
        ///   Subtracts the passed float value from each component of the passed vector.
        /// </summary>
        /// <param name="a"> Vector to subtract the float value from. </param>
        /// <param name="b"> Float value to subtract. </param>
        /// <returns> Vector where each component is the sum of the particular component of the passed vector minus the passed float value. </returns>
        public static Vector2F operator -(Vector2F a, float b)
        {
            return new Vector2F(a.X - b, a.Y - b);
        }

        /// <summary>
        ///   Subtracts the components of the second passed vector from the first passed.
        /// </summary>
        /// <param name="a"> First vector. </param>
        /// <param name="b"> Second vector. </param>
        /// <returns> Vector which components are the difference of the respective components of the two passed vectors. </returns>
        public static Vector2F operator -(Vector2F a, Vector2F b)
        {
            return new Vector2F(a.X - b.X, a.Y - b.Y);
        }

        /// <summary>
        ///   Negates each component of the passed vector.
        /// </summary>
        /// <param name="a"> Vector to negate. </param>
        /// <returns> Vector which components have the negated value of the respective components of the passed vector. </returns>
        public static Vector2F operator -(Vector2F a)
        {
            return new Vector2F(-a.X, -a.Y);
        }

        /// <summary>
        ///   Adds the passed values to the x and y value.
        /// </summary>
        /// <param name="addX"> Value to add to x. </param>
        /// <param name="addY"> Value to add to y. </param>
        public void Add(float addX, float addY)
        {
            this.X += addX;
            this.Y += addY;
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

        public override bool Equals(object obj)
        {
            if (!(obj is Vector2F))
            {
                return false;
            }

            return this.Equals((Vector2F)obj);
        }

        public bool Equals(Vector2F v)
        {
            return this.X == v.X && this.Y == v.Y;
        }

        /// <summary>
        ///   Calculates the distance between this and the passed vector.
        /// </summary>
        /// <param name="vector"> Vector to compute distance to. </param>
        /// <returns> Distance between this and the passed vector. </returns>
        public float GetDistance(Vector2F vector)
        {
            return MathF.Sqrt(this.GetSquareDistance(vector));
        }

        public override int GetHashCode()
        {
            return this.X.GetHashCode() ^ this.Y.GetHashCode();
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
            else
            {
                return Zero;
            }
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
        ///   Calculates the counter-clockwise rotated vector.
        /// </summary>
        /// <param name="angle"> Angle (in radians). </param>
        /// <returns> Rotated vector. </returns>
        public Vector2F GetRotated(float angle)
        {
            Vector2F rotatedVector = new Vector2F(this);
            rotatedVector.Rotate(angle);
            return rotatedVector;
        }

        /// <summary>
        ///   Calculates the square distance between this and the passed vector.
        /// </summary>
        /// <param name="vector"> Vector to compute square distance to. </param>
        /// <returns> Square distance between this and the passed vector. </returns>
        public float GetSquareDistance(Vector2F vector)
        {
            return MathF.Pow(vector.X - this.X, 2) + MathF.Pow(vector.Y - this.Y, 2);
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
        ///   Normalizes this vector.
        /// </summary>
        public void Normalize()
        {
            float magnitude = this.Magnitude;
            if (magnitude != 0.0f)
            {
                this.X /= magnitude;
                this.Y /= magnitude;
            }
        }

        /// <summary>
        ///   Rotates the vector counter-clockwise.
        /// </summary>
        /// <param name="angle"> Angle (in radians). </param>
        public void Rotate(float angle)
        {
            float cos = MathF.Cos(angle);
            float sin = MathF.Sin(angle);
            float newX = this.X * cos - this.Y * sin;
            float newY = this.Y * cos + this.X * sin;
            this.X = newX;
            this.Y = newY;
        }

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
        public void Truncate(float length)
        {
            float magnitude = this.Magnitude;
            if (magnitude != 0.0f)
            {
                this.X = this.X / magnitude * length;
                this.Y = this.Y / magnitude * length;
            }
        }

        #endregion
    }
}