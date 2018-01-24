namespace Slash.Unity.Common.UI.Layouting
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    public static class AvoidOverlapping
    {
        public static void DoVerticalLayout(List<LayoutRectangle> layoutRectangles, float space)
        {
            // Sort by Y position.
            layoutRectangles.Sort(LayoutRectangle.SortByPositionYDesc);

            for (var index = 0; index < layoutRectangles.Count - 1; index++)
            {
                var layoutRectangle = layoutRectangles[index];
                var nextLayoutRectangle = layoutRectangles[index + 1];

                // Check if overlaps with next rectangle.
                var overlapDistance = nextLayoutRectangle.Top - layoutRectangle.Bottom + space;
                if (overlapDistance > 0)
                {
                    // Move rectangle up as far as possible.
                    var rectangleMove =
                        index == 0 ? overlapDistance : Math.Min(layoutRectangle.FreeSpace, overlapDistance);
                    layoutRectangle.FreeSpace -= rectangleMove;
                    layoutRectangle.Move(0, rectangleMove);

                    // Move next rectangle by remaining distance.
                    var remainingMove = overlapDistance - rectangleMove;
                    if (remainingMove > 0)
                    {
                        nextLayoutRectangle.Move(0, -remainingMove);
                    }
                }
                else
                {
                    // Use distance as buffer for next rectangle to move.
                    nextLayoutRectangle.FreeSpace = -overlapDistance;
                }
            }
        }

        /// <summary>
        ///     A rectangle to do layout with.
        /// </summary>
        public class LayoutRectangle
        {
            /// <summary>
            ///     Free space that's available for this layout rectangle.
            /// </summary>
            public float FreeSpace;

            /// <summary>
            ///   Custom user data.
            /// </summary>
            public object Data;

            public float Bottom
            {
                get { return this.Position.y - this.Size.y; }
            }

            public Vector2 Center
            {
                get { return this.Position; }
            }

            public float Top
            {
                get { return this.Position.y; }
            }

            public Vector2 Position;

            public Vector2 Size;

            public void Move(float moveX, float moveY)
            {
                this.Position.x += moveX;
                this.Position.y += moveY;
            }

            public static int SortByPositionYDesc(LayoutRectangle rectangleA, LayoutRectangle rectangleB)
            {
                return rectangleB.Top.CompareTo(rectangleA.Top);
            }
        }
    }
}