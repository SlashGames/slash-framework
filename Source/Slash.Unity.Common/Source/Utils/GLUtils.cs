// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GLUtils.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Unity.Common.Utils
{
    using UnityEngine;

    /// <summary>
    ///   Utility methods extending Unity low-level OpenGL support.
    /// </summary>
    public static class GLUtils
    {
        #region Public Methods and Operators

        /// <summary>
        ///   Draws a two-dimensional unit grid with the specified size.
        /// </summary>
        /// <param name="gridOrigin">Corner of the grid.</param>
        /// <param name="gridSize">Number of rows and columns of the grid.</param>
        public static void GLDrawGrid(Vector3 gridOrigin, Vector2 gridSize)
        {
            // Draw X axis lines.
            for (float y = 0; y <= gridSize.y; y++)
            {
                // Draw X axis lines.
                GL.Vertex3(gridOrigin.x, y + gridOrigin.y, gridOrigin.z);
                GL.Vertex3(gridOrigin.x + gridSize.x, y + gridOrigin.y, gridOrigin.z);
            }

            // Draw Y axis lines.
            for (float x = 0; x <= gridSize.x; x++)
            {
                GL.Vertex3(gridOrigin.x + x, gridOrigin.y, gridOrigin.z);
                GL.Vertex3(gridOrigin.x + x, gridOrigin.y + gridSize.y, gridOrigin.z);
            }
        }

        /// <summary>
        ///   Draws a rectangle with the specified dimensions.
        /// </summary>
        /// <param name="origin">Corner of the rectangle.</param>
        /// <param name="size">Width and height of the rectangle.</param>
        public static void GLDrawRect(Vector3 origin, Vector2 size)
        {
            Vector3 botLeft = new Vector3(origin.x, origin.y, origin.z);
            Vector3 botRight = new Vector3(origin.x + size.x, origin.y, origin.z);
            Vector3 topLeft = new Vector3(origin.x, origin.y + size.y, origin.z);
            Vector3 topRight = new Vector3(origin.x + size.x, origin.y + size.y, origin.z);

            // Draw X axis lines.
            GL.Vertex(botLeft);
            GL.Vertex(topLeft);

            GL.Vertex(botRight);
            GL.Vertex(topRight);

            // Draw Y axis lines.
            GL.Vertex(botLeft);
            GL.Vertex(botRight);

            GL.Vertex(topLeft);
            GL.Vertex(topRight);
        }

        /// <summary>
        ///   Draws a rectangle with the specified dimensions.
        /// </summary>
        /// <param name="origin">Corner of the rectangle.</param>
        /// <param name="size">Width and height of the rectangle.</param>
        /// <param name="lineThickness">Extents of the outlines.</param>
        public static void GLDrawRect(Vector3 origin, Vector2 size, float lineThickness)
        {
            Vector3 botLeft = new Vector3(origin.x, origin.y, origin.z);
            Vector3 botRight = new Vector3(origin.x + size.x, origin.y, origin.z);
            Vector3 topLeft = new Vector3(origin.x, origin.y + size.y, origin.z);
            Vector3 topRight = new Vector3(origin.x + size.x, origin.y + size.y, origin.z);

            Vector3 botLeftInner = new Vector3(botLeft.x + lineThickness, botLeft.y + lineThickness, botLeft.z);
            Vector3 botRightInner = new Vector3(botRight.x - lineThickness, botRight.y + lineThickness, botRight.z);
            Vector3 topLeftInner = new Vector3(topLeft.x + lineThickness, topLeft.y - lineThickness, topLeft.z);
            Vector3 topRightInner = new Vector3(topRight.x - lineThickness, topRight.y - lineThickness, topRight.z);

            // Draw X axis lines.
            GL.Vertex(botLeft);
            GL.Vertex(botRight);
            GL.Vertex(botRightInner);
            GL.Vertex(botLeftInner);

            GL.Vertex(topLeft);
            GL.Vertex(topRight);
            GL.Vertex(topRightInner);
            GL.Vertex(topLeftInner);

            // Draw Y axis lines.
            GL.Vertex(botLeft);
            GL.Vertex(topLeft);
            GL.Vertex(topLeftInner);
            GL.Vertex(botLeftInner);

            GL.Vertex(botRight);
            GL.Vertex(topRight);
            GL.Vertex(topRightInner);
            GL.Vertex(botRightInner);
        }

        #endregion
    }
}