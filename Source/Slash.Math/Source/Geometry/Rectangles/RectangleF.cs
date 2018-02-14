// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RectangleF.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Math.Geometry.Rectangles
{
    using System;
    using Slash.Math.Algebra.Vectors;

    /// <summary>
    ///     Rectangle with floating point position and extent.
    /// </summary>
    public class RectangleF : IEquatable<RectangleF>
    {
        #region Fields

        /// <summary>
        ///     Size of this rectangle, its width and height.
        /// </summary>
        private Vector2F size;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///     Constructs a new rectangle with the specified position and size.
        /// </summary>
        /// <param name="x">
        ///     X-component of the rectangle position.
        /// </param>
        /// <param name="y">
        ///     Y-component of the rectangle position.
        /// </param>
        /// <param name="width">
        ///     Rectangle width.
        /// </param>
        /// <param name="height">
        ///     Rectangle height.
        /// </param>
        public RectangleF(float x, float y, float width, float height)
            : this(new Vector2F(x, y), new Vector2F(width, height))
        {
        }

        /// <summary>
        ///     Constructs a new rectangle with the specified position and size.
        /// </summary>
        /// <param name="x">
        ///     X-component of the rectangle position.
        /// </param>
        /// <param name="y">
        ///     Y-component of the rectangle position.
        /// </param>
        /// <param name="size">
        ///     Rectangle size.
        /// </param>
        public RectangleF(float x, float y, Vector2F size)
            : this(new Vector2F(x, y), size)
        {
        }

        /// <summary>
        ///     Constructs a new rectangle with the specified position and size.
        /// </summary>
        /// <param name="position">
        ///     Rectangle position.
        /// </param>
        /// <param name="width">
        ///     Rectangle width.
        /// </param>
        /// <param name="height">
        ///     Rectangle height.
        /// </param>
        public RectangleF(Vector2F position, float width, float height)
            : this(position, new Vector2F(width, height))
        {
        }

        /// <summary>
        ///     Constructs a new rectangle with the specified position and size.
        /// </summary>
        /// <param name="position">
        ///     Rectangle position.
        /// </param>
        /// <param name="size">
        ///     Rectangle size.
        /// </param>
        public RectangleF(Vector2F position, Vector2F size)
        {
            this.Position = position;
            this.Size = size;
        }

        /// <summary>
        ///     Constructs a new rectangle without position or size.
        /// </summary>
        public RectangleF()
            : this(0f, 0f, 0f, 0f)
        {
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets the area of this rectangle, the product of its width and height.
        /// </summary>
        public float Area
        {
            get { return this.Size.X * this.Size.Y; }
        }

        /// <summary>
        ///     Gets the position of the center of this rectangle.
        /// </summary>
        public Vector2F Center
        {
            get { return this.Position + this.Size * 0.5f; }
        }

        /// <summary>
        ///     Gets or sets the height of this rectangle.
        /// </summary>
        public float Height
        {
            get { return this.Size.Y; }

            set { this.Size = new Vector2F(this.Size.X, value); }
        }

        /// <summary>
        ///     Gets the position of this rectangle.
        /// </summary>
        public Vector2F Position { get; set; }

        /// <summary>
        ///     Gets or sets the size of this rectangle, its width and height.
        /// </summary>
        public Vector2F Size
        {
            get { return this.size; }

            set
            {
                if (value.X < 0)
                {
                    throw new ArgumentOutOfRangeException("value", "Width must be non-negative.");
                }

                if (value.Y < 0)
                {
                    throw new ArgumentOutOfRangeException("value", "Height must be non-negative.");
                }

                this.size = value;
            }
        }

        /// <summary>
        ///     Gets or sets the width of this rectangle.
        /// </summary>
        public float Width
        {
            get { return this.Size.X; }

            set { this.Size = new Vector2F(value, this.Size.Y); }
        }

        /// <summary>
        ///     Gets or sets the x-component of the position of this rectangle.
        /// </summary>
        public float X
        {
            get { return this.Position.X; }

            set { this.Position = new Vector2F(value, this.Position.Y); }
        }

        /// <summary>
        ///     Maximum x-value of this rectangle.
        /// </summary>
        public float XMax
        {
            get { return this.Position.X + this.size.X; }
        }

        /// <summary>
        ///     Minimum x-value of this rectangle.
        /// </summary>
        public float XMin
        {
            get { return this.Position.X; }
        }

        /// <summary>
        ///     Gets or sets the y-component of the position of this rectangle.
        /// </summary>
        public float Y
        {
            get { return this.Position.Y; }

            set { this.Position = new Vector2F(this.Position.X, value); }
        }

        /// <summary>
        ///     Maximum y-value of this rectangle.
        /// </summary>
        public float YMax
        {
            get { return this.Position.Y + this.size.Y; }
        }

        /// <summary>
        ///     Minimum x-value of this rectangle.
        /// </summary>
        public float YMin
        {
            get { return this.Position.Y; }
        }

        /// <summary>
        ///     Maximum values of this rectangle.
        /// </summary>
        public Vector2F Max
        {
            get { return this.Position + this.Size; }
        }

        /// <summary>
        ///     Minimum values of this rectangle.
        /// </summary>
        public Vector2F Min
        {
            get { return this.Position; }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     Creates a new rectangle from the specified bounds.
        /// </summary>
        /// <param name="xMin">Minimum x value.</param>
        /// <param name="yMin">Minimum y value.</param>
        /// <param name="xMax">Maximum x value.</param>
        /// <param name="yMax">Maximum y value.</param>
        /// <returns>New rectangle with the specified bounds.</returns>
        /// <exception cref="ArgumentException">Thrown if a minimum bound is bigger than the associated maximum bound.</exception>
        public static RectangleF FromBounds(float xMin, float yMin, float xMax, float yMax)
        {
            if (xMin > xMax)
            {
                throw new ArgumentException(
                    string.Format("Minimum x bound '{0}' is bigger than maximum bound '{1}'.", xMin, xMax), "xMin");
            }

            if (yMin > yMax)
            {
                throw new ArgumentException(
                    string.Format("Minimum x bound '{0}' is bigger than maximum bound '{1}'.", xMin, xMax), "xMin");
            }

            return new RectangleF(xMin, yMin, xMax - xMin, yMax - yMin);
        }

        /// <summary>
        ///     Checks whether this rectangle entirely encompasses the passed other one.
        /// </summary>
        /// <param name="other">
        ///     Rectangle to check.
        /// </param>
        /// <returns>
        ///     <c>true</c>, if this rectangle contains <paramref name="other" />, and <c>false</c> otherwise.
        /// </returns>
        public bool Contains(RectangleF other)
        {
            return this.XMin <= other.XMin && this.XMax >= other.XMax && this.YMin <= other.YMin &&
                   this.YMax >= other.YMax;
        }

        /// <summary>
        ///     Checks whether this rectangle contains the point denoted by the specified vector.
        /// </summary>
        /// <param name="point">
        ///     Point to check.
        /// </param>
        /// <returns>
        ///     <c>true</c>, if this rectangle contains <paramref name="point" />, and <c>false</c> otherwise.
        /// </returns>
        public bool Contains(Vector2F point)
        {
            return point.X >= this.XMin && point.X < this.XMax && point.Y >= this.YMin && point.Y < this.YMax;
        }

        /// <summary>
        ///     Compares the passed rectangle to this one for equality.
        /// </summary>
        /// <param name="other">
        ///     Rectangle to compare.
        /// </param>
        /// <returns>
        ///     <c>true</c>, if both rectangles are equal, and <c>false</c> otherwise.
        /// </returns>
        public bool Equals(RectangleF other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return this.Position.Equals(other.Position) && this.Size.Equals(other.Size);
        }

        /// <summary>
        ///     Compares the passed rectangle to this one for equality.
        /// </summary>
        /// <param name="obj">
        ///     Rectangle to compare.
        /// </param>
        /// <returns>
        ///     <c>true</c>, if both rectangles are equal, and <c>false</c> otherwise.
        /// </returns>
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

            return obj.GetType() == this.GetType() && this.Equals((RectangleF) obj);
        }

        /// <summary>
        ///     Gets the hash code of this rectangle.
        /// </summary>
        /// <returns>
        ///     Hash code of this rectangle.
        /// </returns>
        public override int GetHashCode()
        {
            unchecked
            {
                return (this.Position.GetHashCode() * 397) ^ this.Size.GetHashCode();
            }
        }

        /// <summary>
        ///     Checks whether this rectangle at least partially intersects the passed other one.
        /// </summary>
        /// <param name="other">
        ///     Rectangle to check.
        /// </param>
        /// <returns>
        ///     <c>true</c>, if this rectangle intersects <paramref name="other" />, and <c>false</c> otherwise.
        /// </returns>
        public bool Intersects(RectangleF other)
        {
            return this.XMax > other.XMin && this.XMin < other.XMax && this.YMax > other.YMin && this.YMin < other.YMax;
        }

        /// <summary>
        ///     Returns a <see cref="string" /> representation of this rectangle.
        /// </summary>
        /// <returns>
        ///     This rectangle as <see cref="string" />.
        /// </returns>
        public override string ToString()
        {
            return string.Format("Position: {0}, Size: {1}", this.Position, this.Size);
        }

        /// <summary>
        ///     Creates the union of this rectangle and the specified other one.
        /// </summary>
        /// <param name="other">
        ///     Rectangle to unite with.
        /// </param>
        /// <returns>Union rectangle of this and the specified rectangle.</returns>
        public RectangleF Union(RectangleF other)
        {
            var maxX = Math.Max(this.XMax, other.XMax);
            var maxY = Math.Max(this.YMax, other.YMax);

            var x = Math.Min(this.X, other.X);
            var y = Math.Min(this.Y, other.Y);

            return new RectangleF(x, y, maxX - x, maxY - y);
        }

        #endregion
    }
}