// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GLUtils.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using UnityEngine;

public static class GLUtils
{
    #region Public Methods and Operators

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

    public static void GLDrawGridOutline(Vector3 outlineOrigin, Vector2 outlineSize)
    {
        Vector3 botLeft = new Vector3(outlineOrigin.x, outlineOrigin.y, outlineOrigin.z);
        Vector3 botRight = new Vector3(outlineOrigin.x + outlineSize.x, outlineOrigin.y, outlineOrigin.z);
        Vector3 topLeft = new Vector3(outlineOrigin.x, outlineOrigin.y + outlineSize.y, outlineOrigin.z);
        Vector3 topRight = new Vector3(
            outlineOrigin.x + outlineSize.x, outlineOrigin.y + outlineSize.y, outlineOrigin.z);

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

    public static void GLDrawOutline(Vector3 outlineOrigin, Vector2 outlineSize, float thickness)
    {
        Vector3 botLeft = new Vector3(outlineOrigin.x, outlineOrigin.y, outlineOrigin.z);
        Vector3 botRight = new Vector3(outlineOrigin.x + outlineSize.x, outlineOrigin.y, outlineOrigin.z);
        Vector3 topLeft = new Vector3(outlineOrigin.x, outlineOrigin.y + outlineSize.y, outlineOrigin.z);
        Vector3 topRight = new Vector3(
            outlineOrigin.x + outlineSize.x, outlineOrigin.y + outlineSize.y, outlineOrigin.z);

        Vector3 botLeftInner = new Vector3(botLeft.x + thickness, botLeft.y + thickness, botLeft.z);
        Vector3 botRightInner = new Vector3(botRight.x - thickness, botRight.y + thickness, botRight.z);
        Vector3 topLeftInner = new Vector3(topLeft.x + thickness, topLeft.y - thickness, topLeft.z);
        Vector3 topRightInner = new Vector3(topRight.x - thickness, topRight.y - thickness, topRight.z);

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