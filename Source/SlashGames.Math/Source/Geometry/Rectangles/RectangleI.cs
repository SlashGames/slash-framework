// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RectangleI.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace SlashGames.Math.Geometry.Rectangles
{
    using SlashGames.Math.Algebra.Vectors;

    /// <summary>
    ///   In Euclidean plane geometry, a rectangle is any quadrilateral with four right angles.
    ///   The word rectangle comes from the Latin rectangulus, which is a combination of rectus (right) and angulus (angle).
    /// </summary>
    public class RectangleI
    {
        #region Constructors and Destructors

        /// <summary>
        ///   Constructor.
        /// </summary>
        public RectangleI()
        {
        }

        /// <summary>
        ///   Constructs a new rectangle with the specified position and extent.
        /// </summary>
        /// <param name="x"> The x-coordinate of the position of the new rectangle. </param>
        /// <param name="y"> The y-coordinate of the position of the new rectangle. </param>
        /// <param name="width"> The width of the new rectangle. </param>
        /// <param name="height"> The height of the new rectangle. </param>
        public RectangleI(int x, int y, int width, int height)
        {
            this.X = x;
            this.Y = y;

            this.Width = width;
            this.Height = height;
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///   Calculates the center position.
        /// </summary>
        public Vector2I Center
        {
            get
            {
                return new Vector2I(this.X + this.Width / 2, this.Y + this.Height / 2);
            }
        }

        /// <summary>
        ///   Gets or sets the height of this rectangle.
        /// </summary>
        public int Height { get; set; }

        /// <summary>
        ///   Calculates the top center position.
        /// </summary>
        public Vector2I TopCenter
        {
            get
            {
                return new Vector2I(this.X + this.Width / 2, this.Y);
            }
        }

        /// <summary>
        ///   Gets or sets the width of this rectangle.
        /// </summary>
        public int Width { get; set; }

        /// <summary>
        ///   Gets or sets the x-coordinate of the position of this rectangle.
        /// </summary>
        public int X { get; set; }

        /// <summary>
        ///   Gets or sets the y-coordinate of the position of this rectangle.
        /// </summary>
        public int Y { get; set; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///   Checks whether this rectangle entirely encompasses the passed one.
        /// </summary>
        /// <param name="otherRectangle"> The rectangle to check. </param>
        /// <returns> <c>true</c> , if this rectangle contains <c>otherRectangle</c> , and <c>false</c> otherwise. </returns>
        public bool Contains(RectangleI otherRectangle)
        {
            return (this.X <= otherRectangle.X && this.X + this.Width >= otherRectangle.X + otherRectangle.Width)
                   && (this.Y <= otherRectangle.Y && this.Y + this.Height >= otherRectangle.Y + otherRectangle.Height);
        }

        /// <summary>
        ///   Checks whether this rectangle contains the specified point.
        /// </summary>
        /// <param name="point"> Point to check. </param>
        /// <returns> True if the rectangle contains the specified point; otherwise, false. </returns>
        public bool Contains(Vector2I point)
        {
            return this.X <= point.X && this.X + this.Width >= point.X && this.Y <= point.Y
                   && this.Y + this.Height >= point.Y;
        }

        /// <summary>
        ///   Checks whether this rectangle at least partially intersects the passed one.
        /// </summary>
        /// <param name="otherRectangle"> The rectangle to check. </param>
        /// <returns> <c>true</c> , if this rectangle intersetcts <c>otherRectangle</c> , and <c>false</c> otherwise. </returns>
        public bool IntersectsWith(RectangleI otherRectangle)
        {
            return (this.X + this.Width > otherRectangle.X && this.X < otherRectangle.X + otherRectangle.Width)
                   && (this.Y + this.Height > otherRectangle.Y && this.Y < otherRectangle.Y + otherRectangle.Height);
        }

        #endregion
    }
}