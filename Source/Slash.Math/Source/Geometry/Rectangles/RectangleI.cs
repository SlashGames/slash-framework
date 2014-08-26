// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RectangleI.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Math.Geometry.Rectangles
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Slash.Math.Algebra.Vectors;
    using Slash.Math.Utils;

    /// <summary>
    ///   Rectangle with integer position and extent.
    /// </summary>
    public class RectangleI : IEquatable<RectangleI>
    {
        #region Static Fields

        /// <summary>
        ///   Zero rectangle (at origin with size zero).
        /// </summary>
        public static readonly RectangleI Zero = new RectangleI(0, 0, 0, 0);

        #endregion

        #region Fields

        /// <summary>
        ///   Size of this rectangle, its width and height.
        /// </summary>
        private Vector2I size;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///   Constructs a new rectangle with the specified position and size.
        /// </summary>
        /// <param name="x">
        ///   X-component of the rectangle position.
        /// </param>
        /// <param name="y">
        ///   Y-component of the rectangle position.
        /// </param>
        /// <param name="width">
        ///   Rectangle width.
        /// </param>
        /// <param name="height">
        ///   Rectangle height.
        /// </param>
        public RectangleI(int x, int y, int width, int height)
            : this(new Vector2I(x, y), new Vector2I(width, height))
        {
        }

        /// <summary>
        ///   Constructs a new rectangle with the specified position and size.
        /// </summary>
        /// <param name="x">
        ///   X-component of the rectangle position.
        /// </param>
        /// <param name="y">
        ///   Y-component of the rectangle position.
        /// </param>
        /// <param name="size">
        ///   Rectangle size.
        /// </param>
        public RectangleI(int x, int y, Vector2I size)
            : this(new Vector2I(x, y), size)
        {
        }

        /// <summary>
        ///   Constructs a new rectangle with the specified position and size.
        /// </summary>
        /// <param name="position">
        ///   Rectangle position.
        /// </param>
        /// <param name="width">
        ///   Rectangle width.
        /// </param>
        /// <param name="height">
        ///   Rectangle height.
        /// </param>
        public RectangleI(Vector2I position, int width, int height)
            : this(position, new Vector2I(width, height))
        {
        }

        /// <summary>
        ///   Constructs a new rectangle with the specified position and size.
        /// </summary>
        /// <param name="position">
        ///   Rectangle position.
        /// </param>
        /// <param name="size">
        ///   Rectangle size.
        /// </param>
        public RectangleI(Vector2I position, Vector2I size)
        {
            this.Position = position;
            this.Size = size;
        }

        /// <summary>
        ///   Copy contructor.
        /// </summary>
        /// <param name="rectangle">Rectangle to copy.</param>
        public RectangleI(RectangleI rectangle)
        {
            this.Position = rectangle.Position;
            this.size = rectangle.Position;
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///   Gets the area of this rectangle, the product of its width and height.
        /// </summary>
        public int Area
        {
            get
            {
                return this.Size.X * this.Size.Y;
            }
        }

        /// <summary>
        ///   Gets the y-component of the bottom side of this rectangle.
        /// </summary>
        public int Bottom
        {
            get
            {
                return this.Position.Y + this.Size.Y;
            }
        }

        /// <summary>
        ///   Gets the position of the bottom left corner of this rectangle.
        /// </summary>
        public Vector2I BottomLeft
        {
            get
            {
                return this.Position + new Vector2I(0, this.Size.Y);
            }
        }

        /// <summary>
        ///   Gets the position of the bottom right corner of this rectangle.
        /// </summary>
        public Vector2I BottomRight
        {
            get
            {
                return this.Position + this.Size;
            }
        }

        /// <summary>
        ///   Gets the position of the center of this rectangle.
        /// </summary>
        public Vector2I Center
        {
            get
            {
                return this.Position + this.Size / 2;
            }
        }

        /// <summary>
        ///   Gets the exact position of the center of this rectangle.
        /// </summary>
        public Vector2F ExactCenter
        {
            get
            {
                return new Vector2F(this.XMin + this.Width * 0.5f, this.YMin + this.Height * 0.5f);
            }
        }

        /// <summary>
        ///   Gets or sets the height of this rectangle.
        /// </summary>
        public int Height
        {
            get
            {
                return this.Size.Y;
            }

            set
            {
                this.Size = new Vector2I(this.Size.X, value);
            }
        }

        /// <summary>
        ///   Gets the x-component of the left side of this rectangle.
        /// </summary>
        public int Left
        {
            get
            {
                return this.Position.X;
            }
        }

        /// <summary>
        ///   Gets the position of this rectangle.
        /// </summary>
        public Vector2I Position { get; set; }

        /// <summary>
        ///   Gets the x-component of the right side of this rectangle.
        /// </summary>
        public int Right
        {
            get
            {
                return this.Position.X + this.Size.X;
            }
        }

        /// <summary>
        ///   Gets or sets the size of this rectangle, its width and height.
        /// </summary>
        public Vector2I Size
        {
            get
            {
                return this.size;
            }

            set
            {
                if (value.X < 0)
                {
                    throw new ArgumentOutOfRangeException("Width", "Width must be non-negative.");
                }

                if (value.Y < 0)
                {
                    throw new ArgumentOutOfRangeException("Height", "Height must be non-negative.");
                }

                this.size = value;
            }
        }

        /// <summary>
        ///   Gets the y-component of the top side of this rectangle.
        /// </summary>
        public int Top
        {
            get
            {
                return this.Position.Y;
            }
        }

        /// <summary>
        ///   Gets the position of the top left corner of this rectangle.
        /// </summary>
        public Vector2I TopLeft
        {
            get
            {
                return this.Position;
            }
        }

        /// <summary>
        ///   Gets the position of the top right corner of this rectangle.
        /// </summary>
        public Vector2I TopRight
        {
            get
            {
                return this.Position + new Vector2I(this.Size.X, 0);
            }
        }

        /// <summary>
        ///   Gets or sets the width of this rectangle.
        /// </summary>
        public int Width
        {
            get
            {
                return this.Size.X;
            }

            set
            {
                this.Size = new Vector2I(value, this.Size.Y);
            }
        }

        /// <summary>
        ///   Gets or sets the x-component of the position of this rectangle.
        /// </summary>
        public int X
        {
            get
            {
                return this.Position.X;
            }

            set
            {
                this.Position = new Vector2I(value, this.Position.Y);
            }
        }

        /// <summary>
        ///   Maximum x-value of this rectangle.
        /// </summary>
        public int XMax
        {
            get
            {
                return this.Position.X + this.size.X;
            }
        }

        /// <summary>
        ///   Minimum x-value of this rectangle.
        /// </summary>
        public int XMin
        {
            get
            {
                return this.Position.X;
            }
        }

        /// <summary>
        ///   Gets or sets the y-component of the position of this rectangle.
        /// </summary>
        public int Y
        {
            get
            {
                return this.Position.Y;
            }

            set
            {
                this.Position = new Vector2I(this.Position.X, value);
            }
        }

        /// <summary>
        ///   Maximum y-value of this rectangle.
        /// </summary>
        public int YMax
        {
            get
            {
                return this.Position.Y + this.size.Y;
            }
        }

        /// <summary>
        ///   Minimum x-value of this rectangle.
        /// </summary>
        public int YMin
        {
            get
            {
                return this.Position.Y;
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///   Creates a new rectangle from the specified bounds.
        /// </summary>
        /// <param name="xMin">Minimum x value.</param>
        /// <param name="yMin">Minimum y value.</param>
        /// <param name="xMax">Maximum x value.</param>
        /// <param name="yMax">Maximum y value.</param>
        /// <returns>New rectangle with the specified bounds.</returns>
        /// <exception cref="ArgumentException">Thrown if a minimum bound is bigger than the associated maximum bound.</exception>
        public static RectangleI FromBounds(int xMin, int yMin, int xMax, int yMax)
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

            return new RectangleI(xMin, yMin, xMax - xMin, yMax - yMin);
        }

        /// <summary>
        ///   Calculates the rectangle which spans the specified positions.
        /// </summary>
        /// <param name="positions">Positions to span a rectangle around.</param>
        /// <returns>Rectangle which spans the specified positions.</returns>
        public static RectangleI span(IEnumerable<Vector2I> positions)
        {
            if (positions == null || !positions.Any())
            {
                return null;
            }

            int xMin = int.MaxValue;
            int xMax = int.MinValue;
            int yMin = int.MaxValue;
            int yMax = int.MinValue;
            foreach (Vector2I position in positions)
            {
                xMin = MathUtils.Min(xMin, position.X);
                xMax = MathUtils.Max(xMax, position.X);
                yMin = MathUtils.Min(yMin, position.Y);
                yMax = MathUtils.Max(yMax, position.Y);
            }

            return new RectangleI(xMin, yMin, xMax - xMin + 1, yMax - yMin + 1);
        }

        /// <summary>
        ///   Calculates the point closest to the specified position on the border of this rectangle.
        ///   If this rectangle contains the specified position, the position is returned instead.
        /// </summary>
        /// <param name="position">Position to calculate the closest border point for.</param>
        /// <returns>
        ///   Closest point to the specified position on the border of this rectangle,
        ///   if this rectangle doesn't contain that position, and the position otherwise.
        /// </returns>
        public Vector2I CalculateClosestPoint(Vector2I position)
        {
            return new Vector2I(
                MathUtils.Clamp(position.X, this.XMin, this.XMax), MathUtils.Clamp(position.Y, this.YMin, this.YMax));
        }

        /// <summary>
        ///   Calculates the point closest to the specified position on the border of this rectangle
        ///   with width <paramref name="threshold" />.
        ///   If this rectangle contains the specified position, the position is returned instead.
        /// </summary>
        /// <param name="position">Position to calculate the closest border point for.</param>
        /// <param name="threshold">Thickness of the rectangle border.</param>
        /// <returns>
        ///   Closest point to the specified position on the border of this rectangle,
        ///   if this rectangle doesn't contain that position, and the position otherwise.
        /// </returns>
        public Vector2F CalculateClosestPoint(Vector2F position, Vector2F threshold)
        {
            if (threshold.X < 0 || threshold.Y < 0)
            {
                throw new ArgumentException("Threshold value has to be >= 0.", "threshold");
            }
            return new Vector2F(
                MathUtils.Clamp(position.X, this.XMin + threshold.X, this.XMax - threshold.X),
                MathUtils.Clamp(position.Y, this.YMin + threshold.Y, this.YMax - threshold.Y));
        }

        /// <summary>
        ///   Calculates the point closest to the specified position on the border of this rectangle
        ///   with width <paramref name="threshold" />.
        ///   If this rectangle contains the specified position, the position is returned instead.
        /// </summary>
        /// <param name="position">Position to calculate the closest border point for.</param>
        /// <param name="threshold">Thickness of the rectangle border.</param>
        /// <returns>
        ///   Closest point to the specified position on the border of this rectangle,
        ///   if this rectangle doesn't contain that position, and the position otherwise.
        /// </returns>
        public Vector2I CalculateClosestPoint(Vector2I position, Vector2I threshold)
        {
            if (threshold.X < 0 || threshold.Y < 0)
            {
                throw new ArgumentException("Threshold value has to be >= 0.", "threshold");
            }
            return new Vector2I(
                MathUtils.Clamp(position.X, this.XMin + threshold.X, this.XMax - threshold.X - 1),
                MathUtils.Clamp(position.Y, this.YMin + threshold.Y, this.YMax - threshold.Y - 1));
        }

        /// <summary>
        ///   Checks whether this rectangle entirely encompasses the passed other one.
        /// </summary>
        /// <param name="other">
        ///   Rectangle to check.
        /// </param>
        /// <returns>
        ///   <c>true</c>, if this rectangle contains <paramref name="other" />, and <c>false</c> otherwise.
        /// </returns>
        public bool Contains(RectangleI other)
        {
            return (this.Left <= other.Left && this.Right >= other.Right)
                   && (this.Top <= other.Top && this.Bottom >= other.Bottom);
        }

        /// <summary>
        ///   Checks whether this rectangle contains the point denoted by the specified vector.
        /// </summary>
        /// <param name="point">
        ///   Point to check.
        /// </param>
        /// <returns>
        ///   <c>true</c>, if this rectangle contains <paramref name="point" />, and <c>false</c> otherwise.
        /// </returns>
        public bool Contains(Vector2I point)
        {
            return point.X >= this.Left && point.X < this.Right && point.Y >= this.Top && point.Y < this.Bottom;
        }

        /// <summary>
        ///   Compares the passed rectangle to this one for equality.
        /// </summary>
        /// <param name="other">
        ///   Rectangle to compare.
        /// </param>
        /// <returns>
        ///   <c>true</c>, if both rectangles are equal, and <c>false</c> otherwise.
        /// </returns>
        public bool Equals(RectangleI other)
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
        ///   Compares the passed rectangle to this one for equality.
        /// </summary>
        /// <param name="obj">
        ///   Rectangle to compare.
        /// </param>
        /// <returns>
        ///   <c>true</c>, if both rectangles are equal, and <c>false</c> otherwise.
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

            return obj.GetType() == this.GetType() && this.Equals((RectangleI)obj);
        }

        /// <summary>
        ///   Gets the hash code of this rectangle.
        /// </summary>
        /// <returns>
        ///   Hash code of this rectangle.
        /// </returns>
        public override int GetHashCode()
        {
            unchecked
            {
                return (this.Position.GetHashCode() * 397) ^ this.Size.GetHashCode();
            }
        }

        /// <summary>
        ///   Intersects this rectangle with the specified one, creating an intersection rectangle.
        /// </summary>
        /// <param name="rectangle">Rectangle to intersect with.</param>
        /// <param name="intersectionRectangle">Intersection rectangle.</param>
        /// <returns>True if the two rectangles intersect; otherwise, false.</returns>
        public bool Intersect(RectangleI rectangle, out RectangleI intersectionRectangle)
        {
            if (!this.Intersects(rectangle))
            {
                intersectionRectangle = Zero;
                return false;
            }

            intersectionRectangle = FromBounds(
                MathUtils.Max(this.XMin, rectangle.XMin),
                MathUtils.Max(this.YMin, rectangle.YMin),
                MathUtils.Min(this.XMax, rectangle.XMax),
                MathUtils.Min(this.YMax, rectangle.YMax));
            return true;
        }

        /// <summary>
        ///   Checks whether this rectangle at least partially intersects the passed other one.
        /// </summary>
        /// <param name="other">
        ///   Rectangle to check.
        /// </param>
        /// <returns>
        ///   <c>true</c>, if this rectangle intersects <paramref name="other" />, and <c>false</c> otherwise.
        /// </returns>
        public bool Intersects(RectangleI other)
        {
            return (this.Right > other.Left && this.Left < other.Right)
                   && (this.Bottom > other.Top && this.Top < other.Bottom);
        }

        /// <summary>
        ///   Returns a <see cref="string" /> representation of this rectangle.
        /// </summary>
        /// <returns>
        ///   This rectangle as <see cref="string" />.
        /// </returns>
        public override string ToString()
        {
            return string.Format("Position: {0}, Size: {1}", this.Position, this.Size);
        }

        /// <summary>
        ///   Creates the union of this rectangle and the specified other one.
        /// </summary>
        /// <param name="other">
        ///   Rectangle to unite with.
        /// </param>
        /// <returns>Union rectangle of this and the specified rectangle.</returns>
        public RectangleI Union(RectangleI other)
        {
            int maxX = Math.Max(this.XMax, other.XMax);
            int maxY = Math.Max(this.YMax, other.YMax);

            int x = Math.Min(this.X, other.X);
            int y = Math.Min(this.Y, other.Y);

            return new RectangleI(x, y, maxX - x, maxY - y);
        }

        #endregion
    }
}