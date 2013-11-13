// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LineDrawing.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Math.Geometry.Lines
{
    using System;

    using Slash.Math.Algebra.Vectors;
    using Slash.Math.Utils;

    public static class LineDrawing
    {
        #region Delegates

        /// <summary>
        ///   The plot function delegate
        /// </summary>
        /// <param name="x">The x co-ord being plotted</param>
        /// <param name="y">The y co-ord being plotted</param>
        /// <returns>True to continue, false to stop the algorithm</returns>
        public delegate bool PlotFunction(int x, int y);

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///   Plot the line from the specified start to the specified end location by using the Bresenham algorithm.
        ///   See http://en.wikipedia.org/wiki/Bresenham%27s_line_algorithm for details.
        /// </summary>
        /// <param name="start">The start location.</param>
        /// <param name="end">The end location.</param>
        /// <param name="plot">The plotting function (if this returns false, the algorithm stops early)</param>
        public static void Bresenham(Vector2I start, Vector2I end, PlotFunction plot)
        {
            Bresenham(start.X, start.Y, end.X, end.Y, plot);
        }

        /// <summary>
        ///   Plot the line from (x0, y0) to (x1, y1) by using the Bresenham algorithm.
        ///   See http://en.wikipedia.org/wiki/Bresenham%27s_line_algorithm for details.
        /// </summary>
        /// <param name="x0">The start x</param>
        /// <param name="y0">The start y</param>
        /// <param name="x1">The end x</param>
        /// <param name="y1">The end y</param>
        /// <param name="plot">The plotting function (if this returns false, the algorithm stops early)</param>
        public static void Bresenham(int x0, int y0, int x1, int y1, PlotFunction plot)
        {
            bool steep = Math.Abs(y1 - y0) > Math.Abs(x1 - x0);
            if (steep)
            {
                Swap(ref x0, ref y0);
                Swap(ref x1, ref y1);
            }
            if (x0 > x1)
            {
                Swap(ref x0, ref x1);
                Swap(ref y0, ref y1);
            }
            int dX = (x1 - x0);
            int dY = Math.Abs(y1 - y0);
            int err = (dX / 2);
            int ystep = (y0 < y1 ? 1 : -1);
            int y = y0;

            for (int x = x0; x <= x1; ++x)
            {
                if (!(steep ? plot(y, x) : plot(x, y)))
                {
                    return;
                }
                err = err - dY;
                if (err < 0)
                {
                    y += ystep;
                    err += dX;
                }
            }
        }

        /// <summary>
        ///   Plot the line from (x0, y0) to (x1, y1) by using the Bresenham algorithm.
        ///   See http://en.wikipedia.org/wiki/Bresenham%27s_line_algorithm for details.
        /// </summary>
        /// <param name="x0">The start x</param>
        /// <param name="y0">The start y</param>
        /// <param name="x1">The end x</param>
        /// <param name="y1">The end y</param>
        /// <param name="plot">The plotting function (if this returns false, the algorithm stops early)</param>
        public static void Bresenham(float x0, float y0, float x1, float y1, PlotFunction plot)
        {
            Bresenham(
                MathUtils.FloorToInt(x0),
                MathUtils.FloorToInt(y0),
                MathUtils.FloorToInt(x1),
                MathUtils.FloorToInt(y1),
                plot);
        }

        /// <summary>
        ///   Checks if the specified location lies on the bresenham line spanned by the start and end location.
        /// </summary>
        /// <param name="startLocation">Start location of bresenham line.</param>
        /// <param name="endLocation">End location of bresenham line.</param>
        /// <param name="location">Location to check.</param>
        /// <returns>True if the specified location lies on the bresenham line spanned by the start and end location; otherwise, false.</returns>
        public static bool BresenhamContainsPoint(Vector2I startLocation, Vector2I endLocation, Vector2I location)
        {
            bool hitsLocation = false;
            Bresenham(
                startLocation.X,
                startLocation.Y,
                endLocation.X,
                endLocation.Y,
                (x, y) =>
                    {
                        if (location.X == x && location.Y == y)
                        {
                            hitsLocation = true;
                            return false;
                        }
                        return true;
                    });
            return hitsLocation;
        }

        #endregion

        #region Methods

        private static void Swap<T>(ref T lhs, ref T rhs)
        {
            T temp = lhs;
            lhs = rhs;
            rhs = temp;
        }

        #endregion
    }
}