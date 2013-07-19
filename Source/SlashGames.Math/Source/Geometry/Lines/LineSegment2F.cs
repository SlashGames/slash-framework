// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LineSegment2F.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Math.Geometry.Lines
{
    using Slash.Math.Algebra.Vectors;
    using Slash.Math.Utils;

    /// <summary>
    ///   The class encapsulates a 2D line segment and provides some tool methods related to line segments.
    /// </summary>
    public struct LineSegment2F
    {
        #region Fields

        /// <summary>
        ///   First point of the line segment
        /// </summary>
        public Vector2F PointA;

        /// <summary>
        ///   Second point of the line segment
        /// </summary>
        public Vector2F PointB;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///   Initializes a new instance of the <see cref="LineSegment2F" /> class.
        /// </summary>
        /// <param name="pointA"> First point of line segment </param>
        /// <param name="pointB"> Second point of line segment </param>
        public LineSegment2F(Vector2F pointA, Vector2F pointB)
        {
            this.PointA = pointA;
            this.PointB = pointB;
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///   Returns the direction of the line segment, assuming that PointA is the start point.
        /// </summary>
        public Vector2F Direction
        {
            get
            {
                return this.PointB - this.PointA;
            }
        }

        /// <summary>
        ///   Returns the normalized direction of the line segment, assuming that PointA is the start point.
        /// </summary>
        public Vector2F DirectionNormalized
        {
            get
            {
                return (this.PointB - this.PointA).GetNormalized();
            }
        }

        /// <summary>
        ///   Length of the line segment.
        /// </summary>
        public float Length
        {
            get
            {
                return (this.PointB - this.PointA).Magnitude;
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///   This method detects if two line segments (or lines) intersect,
        ///   and, if so, the point of intersection. Use the <paramref name="firstIsSegment" /> and
        ///   <paramref name="secondIsSegment" /> parameters to set whether the intersection point
        ///   must be on the first and second line segments. Setting these
        ///   both to true means you are doing a line-segment to line-segment
        ///   intersection. Setting one of them to true means you are doing a
        ///   line to line-segment intersection test, and so on.
        ///   Note: If two line segments are coincident, then 
        ///   no intersection is detected (there are actually
        ///   infinite intersection points).
        ///   Author: Jeremy Bell
        /// </summary>
        /// <param name="point1"> The first point of the first line segment. </param>
        /// <param name="point2"> The second point of the first line segment. </param>
        /// <param name="point3"> The first point of the second line segment. </param>
        /// <param name="point4"> The second point of the second line segment. </param>
        /// <param name="point"> This is set to the intersection point if an intersection is detected. </param>
        /// <param name="firstIsSegment"> Set this to true to require that the intersection point be on the first line segment. </param>
        /// <param name="secondIsSegment"> Set this to true to require that the intersection point be on the second line segment. </param>
        /// <returns> True if an intersection is detected, false otherwise. </returns>
        public static bool Intersect(
            ref Vector2F point1,
            ref Vector2F point2,
            ref Vector2F point3,
            ref Vector2F point4,
            bool firstIsSegment,
            bool secondIsSegment,
            out Vector2F point)
        {
            point = new Vector2F();

            // these are reused later.
            // each lettered sub-calculation is used twice, except
            // for b and d, which are used 3 times
            float a = point4.Y - point3.Y;
            float b = point2.X - point1.X;
            float c = point4.X - point3.X;
            float d = point2.Y - point1.Y;

            // denominator to solution of linear system
            float denom = (a * b) - (c * d);

            // if denominator is 0, then lines are parallel
            if (!(denom >= -float.Epsilon && denom <= float.Epsilon))
            {
                float e = point1.Y - point3.Y;
                float f = point1.X - point3.X;
                float oneOverDenom = 1.0f / denom;

                // numerator of first equation
                float ua = (c * e) - (a * f);
                ua *= oneOverDenom;

                // check if intersection point of the two lines is on line segment 1
                if (!firstIsSegment || ua >= 0.0f && ua <= 1.0f)
                {
                    // numerator of second equation
                    float ub = (b * e) - (d * f);
                    ub *= oneOverDenom;

                    // check if intersection point of the two lines is on line segment 2
                    // means the line segments intersect, since we know it is on
                    // segment 1 as well.
                    if (!secondIsSegment || ub >= 0.0f && ub <= 1.0f)
                    {
                        // check if they are coincident (no collision in this case)
                        if (ua != 0f || ub != 0f)
                        {
                            // There is an intersection
                            point.X = point1.X + (ua * b);
                            point.Y = point1.Y + (ua * d);
                            return true;
                        }
                    }
                }
            }

            return false;
        }

        /// <summary>
        ///   This method detects if two line segments (or lines) intersect,
        ///   and, if so, the point of intersection. Use the <paramref name="firstIsSegment" /> and
        ///   <paramref name="secondIsSegment" /> parameters to set whether the intersection point
        ///   must be on the first and second line segments. Setting these
        ///   both to true means you are doing a line-segment to line-segment
        ///   intersection. Setting one of them to true means you are doing a
        ///   line to line-segment intersection test, and so on.
        ///   Note: If two line segments are coincident, then 
        ///   no intersection is detected (there are actually
        ///   infinite intersection points).
        ///   Author: Jeremy Bell
        /// </summary>
        /// <param name="point1"> The first point of the first line segment. </param>
        /// <param name="point2"> The second point of the first line segment. </param>
        /// <param name="point3"> The first point of the second line segment. </param>
        /// <param name="point4"> The second point of the second line segment. </param>
        /// <param name="intersectionPoint"> This is set to the intersection point if an intersection is detected. </param>
        /// <param name="firstIsSegment"> Set this to true to require that the intersection point be on the first line segment. </param>
        /// <param name="secondIsSegment"> Set this to true to require that the intersection point be on the second line segment. </param>
        /// <returns> True if an intersection is detected, false otherwise. </returns>
        public static bool Intersect(
            Vector2F point1,
            Vector2F point2,
            Vector2F point3,
            Vector2F point4,
            bool firstIsSegment,
            bool secondIsSegment,
            out Vector2F intersectionPoint)
        {
            return Intersect(
                ref point1, ref point2, ref point3, ref point4, firstIsSegment, secondIsSegment, out intersectionPoint);
        }

        /// <summary>
        ///   This method detects if two line segments intersect,
        ///   and, if so, the point of intersection. 
        ///   Note: If two line segments are coincident, then 
        ///   no intersection is detected (there are actually
        ///   infinite intersection points).
        /// </summary>
        /// <param name="point1"> The first point of the first line segment. </param>
        /// <param name="point2"> The second point of the first line segment. </param>
        /// <param name="point3"> The first point of the second line segment. </param>
        /// <param name="point4"> The second point of the second line segment. </param>
        /// <param name="intersectionPoint"> This is set to the intersection point if an intersection is detected. </param>
        /// <returns> True if an intersection is detected, false otherwise. </returns>
        public static bool Intersect(
            ref Vector2F point1,
            ref Vector2F point2,
            ref Vector2F point3,
            ref Vector2F point4,
            out Vector2F intersectionPoint)
        {
            return Intersect(ref point1, ref point2, ref point3, ref point4, true, true, out intersectionPoint);
        }

        /// <summary>
        ///   This method detects if two line segments intersect,
        ///   and, if so, the point of intersection. 
        ///   Note: If two line segments are coincident, then 
        ///   no intersection is detected (there are actually
        ///   infinite intersection points).
        /// </summary>
        /// <param name="point1"> The first point of the first line segment. </param>
        /// <param name="point2"> The second point of the first line segment. </param>
        /// <param name="point3"> The first point of the second line segment. </param>
        /// <param name="point4"> The second point of the second line segment. </param>
        /// <param name="intersectionPoint"> This is set to the intersection point if an intersection is detected. </param>
        /// <returns> True if an intersection is detected, false otherwise. </returns>
        public static bool Intersect(
            Vector2F point1, Vector2F point2, Vector2F point3, Vector2F point4, out Vector2F intersectionPoint)
        {
            return Intersect(ref point1, ref point2, ref point3, ref point4, true, true, out intersectionPoint);
        }

        /// <summary>
        ///   Checks if the passed line segments intersect and computes the intersection point if they do
        /// </summary>
        /// <param name="lineSegmentA"> </param>
        /// <param name="lineSegmentB"> </param>
        /// <param name="intersectionPoint"> Intersection point between the two passed line segments </param>
        /// <returns> Returns true if line segments intersect, otherwise false </returns>
        public static bool Intersect(
            LineSegment2F lineSegmentA, LineSegment2F lineSegmentB, out Vector2F intersectionPoint)
        {
            return Intersect(
                lineSegmentA.PointA,
                lineSegmentA.PointB,
                lineSegmentB.PointA,
                lineSegmentB.PointB,
                out intersectionPoint);
        }

        /// <summary>
        ///   Checks if the passed line segments intersect
        /// </summary>
        /// <param name="lineSegmentA"> </param>
        /// <param name="lineSegmentB"> </param>
        /// <returns> Returns true if line segments intersect, otherwise false </returns>
        public static bool Intersect(LineSegment2F lineSegmentA, LineSegment2F lineSegmentB)
        {
            // TODO: This can be made faster because we don't need to compute the intersection point
            Vector2F intersectionPoint;
            return Intersect(
                lineSegmentA.PointA,
                lineSegmentA.PointB,
                lineSegmentB.PointA,
                lineSegmentB.PointB,
                out intersectionPoint);
        }

        /// <summary>
        ///   Computes on which side of the line segment the passed point lies
        /// </summary>
        /// <param name="point"> </param>
        /// <returns> </returns>
        public Side ComputeSide(Vector2F point)
        {
            Vector2F perpendicularDirection = this.Direction.GetPerpendicularVector();
            float dotProduct = perpendicularDirection.CalculateDotProduct(point - this.PointA);
            return dotProduct == 0.0f ? Side.On : (dotProduct < 0.0f ? Side.Left : Side.Right);
        }

        /// <summary>
        ///   Computes the closest point on the line to the passed point
        /// </summary>
        /// <param name="point"> Point to find the closest point on the line segment for </param>
        /// <returns> Closest point on the line segment to the passed point </returns>
        public Vector2F GetClosestPoint(Vector2F point)
        {
            Vector2F segmentDirection = this.PointB - this.PointA;
            float projectionValue = Vector2F.Dot(point - this.PointA, segmentDirection);
            if (projectionValue <= 0.0f)
            {
                return this.PointA;
            }

            float denominator = Vector2F.Dot(segmentDirection, segmentDirection);
            if (projectionValue >= denominator)
            {
                return this.PointB;
            }

            projectionValue /= denominator;
            return this.PointA + (segmentDirection * projectionValue);
        }

        /// <summary>
        ///   Returns the squared distance of this segment to the passed point
        /// </summary>
        /// <param name="point"> Point which distance to the segment should be checked </param>
        /// <returns> Squared distance to the passed point </returns>
        public float GetSquareDistance(Vector2F point)
        {
            // Computing the closest point to get the distance is possible,
            // but not required
            // return (this.GetClosestPoint(point) - point).GetSquareMagnitude();
            Vector2F segmentVector = this.PointB - this.PointA;
            Vector2F startToPoint = point - this.PointA;

            // Check if start point closest point
            float e = Vector2F.Dot(startToPoint, segmentVector);
            if (e <= 0.0f)
            {
                return Vector2F.Dot(startToPoint, startToPoint);
            }

            // Check if end point closest point
            float squareLength = Vector2F.Dot(segmentVector, segmentVector);
            if (e >= squareLength)
            {
                Vector2F endToPoint = point - this.PointB;
                return Vector2F.Dot(endToPoint, endToPoint);
            }

            // Handle cases where point projects onto segment
            return Vector2F.Dot(startToPoint, startToPoint) - (e * e / squareLength);
        }

        #endregion
    }
}

// end namespace