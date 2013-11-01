// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Direction.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Math.Utils
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    ///   Orientations on the tile grid.
    /// </summary>
    [Flags]
    public enum DirectionType
    {
        /// <summary>
        ///   None (No Direction).
        /// </summary>
        None = 0,

        /// <summary>
        ///   North (Up).
        /// </summary>
        North = 1 << 0,

        /// <summary>
        ///   East (Right).
        /// </summary>
        East = 1 << 1,

        /// <summary>
        ///   Down.
        /// </summary>
        South = 1 << 2,

        /// <summary>
        ///   West (Left).
        /// </summary>
        West = 1 << 3,

        /// <summary>
        ///   All (Up,Down,Left and Right).
        /// </summary>
        All = North | East | South | West,

        /// <summary>
        ///   NorthEast (Up, Right).
        /// </summary>
        NorthEast = North | East,

        /// <summary>
        ///   SouthEast (Down,Right).
        /// </summary>
        SouthEast = South | East,

        /// <summary>
        ///   SouthWest (Left,Down).
        /// </summary>
        SouthWest = South | West,

        /// <summary>
        ///   NorthWest (Up,Left).
        /// </summary>
        NorthWest = North | West,

        Horizontal = West | East,

        Vertical = North | South
    }

    public static class Direction
    {
        #region Public Methods and Operators

        /// <summary>
        ///   Calculates opposite directions of given directions.
        /// </summary>
        /// <param name="direction">Direction of which to calculate opposite direction.</param>
        /// <returns>Opposite direction of parameter.</returns>
        public static DirectionType Opposite(DirectionType direction)
        {
            DirectionType ret = direction;
            if ((direction & DirectionType.North) != 0)
            {
                ret = (ret & ~DirectionType.North) | DirectionType.South;
            }
            else if ((direction & DirectionType.South) != 0)
            {
                ret = (ret & ~DirectionType.South) | DirectionType.North;
            }
            if ((direction & DirectionType.West) != 0)
            {
                ret = (ret & ~DirectionType.West) | DirectionType.East;
            }
            else if ((direction & DirectionType.East) != 0)
            {
                ret = (ret & ~DirectionType.East) | DirectionType.West;
            }

            return ret;
        }

        /// <summary>
        ///   Returns an enumeration of cardinal directions (N-E-S-W).
        /// </summary>
        /// <returns>Enumeration of cardinal directions.</returns>
        public static IEnumerable<DirectionType> cardinalDirections()
        {
            yield return DirectionType.North;
            yield return DirectionType.East;
            yield return DirectionType.South;
            yield return DirectionType.West;
        }

        /// <summary>
        ///   Computes the direction which is closest to the specified vector.
        /// </summary>
        /// <param name="vector">Vector to get direction for.</param>
        /// <returns>Direction which is closest to the specified vector.</returns>
        public static DirectionType computeCardinalDirection(Vector2F vector)
        {
            // Compute angle.
            float angle = Vector2F.CalculateAngle(Vector2F.Forward, vector);
            return computeCardinalDirection(angle);
        }

        /// <summary>
        ///   Computes the direction which is closest to the specified angle.
        ///   Angle defines rotation from forward direction (0, 1) positive counter-clockwise.
        /// </summary>
        /// <param name="angle">Angle to get direction for (from forward direction (0, 1) positive counter-clockwise).</param>
        /// <returns>Direction which is closest to the specified angle.</returns>
        public static DirectionType computeCardinalDirection(float angle)
        {
            // Wrap angle.
            angle = MathF.WrapValue(angle, MathF.TwoPi);

            // Compute direction.
            if (angle < MathF.PiOver4)
            {
                return DirectionType.North;
            }
            if (angle < 0.25f * MathF.TwoPi + MathF.PiOver4)
            {
                return DirectionType.West;
            }
            if (angle < 0.5f * MathF.TwoPi + MathF.PiOver4)
            {
                return DirectionType.South;
            }
            if (angle < 0.75f * MathF.TwoPi + MathF.PiOver4)
            {
                return DirectionType.East;
            }

            return DirectionType.North;
        }

        /// <summary>
        ///   Returns the index for the specified cardinal direction to use it, e.g. in a collection.
        /// </summary>
        /// <param name="direction">Cardinal direction to get index for.</param>
        /// <exception cref="ArgumentException">Thrown if specified direction is no cardinal direction.</exception>
        /// <returns>Index between 0 and 4 (exclusive).</returns>
        public static int getCardinalDirectionIndex(DirectionType direction)
        {
            switch (direction)
            {
                case DirectionType.North:
                    return 0;
                case DirectionType.East:
                    return 1;
                case DirectionType.South:
                    return 2;
                case DirectionType.West:
                    return 3;
                default:
                    throw new ArgumentException(string.Format("No cardinal direction: {0}", direction));
            }
        }

        /// <summary>
        ///   Checks if the specified direction is a cardinal direction (i.e. N, E, S or W).
        /// </summary>
        /// <param name="direction">Direction to check.</param>
        /// <returns>True if the specified direction is a cardinal direction; otherwise, false.</returns>
        public static bool isCardinalDirection(DirectionType direction)
        {
            switch (direction)
            {
                case DirectionType.North:
                case DirectionType.East:
                case DirectionType.South:
                case DirectionType.West:
                    return true;
            }

            return false;
        }

        /// <summary>
        ///   Checks if the specified value is a direction and a cardinal direction (i.e. N, E, S or W).
        /// </summary>
        /// <param name="value">Value to check.</param>
        /// <returns>True if the specified value is a direction and a cardinal direction; otherwise, false.</returns>
        public static bool isCardinalDirection(object value)
        {
            return value is DirectionType && isCardinalDirection((DirectionType)value);
        }

        /// <summary>
        ///   Indicates if the specified direction is a horizontal one (W, E) or a vertical one (N, S).
        /// </summary>
        /// <param name="direction">Direction to check.</param>
        /// <returns>True if the specified direction is a horizontal one; otherwise, false.</returns>
        public static bool isHorizontalDirection(DirectionType direction)
        {
            switch (direction)
            {
                case DirectionType.East:
                case DirectionType.West:
                    return true;
            }
            return false;
        }

        /// <summary>
        ///   Moves the specified location by one field into the specified direction.
        /// </summary>
        /// <param name="gridLocation">Location to move.</param>
        /// <param name="direction">Direction to move to.</param>
        /// <returns>New location after moving the specified location into the specified direction.</returns>
        public static Vector2I moveLocation(Vector2I gridLocation, DirectionType direction)
        {
            return gridLocation + toVector2I(direction);
        }

        /// <summary>
        ///   Returns directions of a vector, based on quadrants.
        /// </summary>
        /// <param name="vector">Vector to convert.</param>
        /// <returns> Set of directions.</returns>
        public static DirectionType toDirections(Vector2I vector)
        {
            DirectionType ret = DirectionType.None;
            if (vector.Y == 0 && vector.X == 0)
            {
                return DirectionType.None;
            }
            if (vector.Y != 0)
            {
                ret |= (vector.Y > 0) ? DirectionType.North : DirectionType.South;
            }
            if (vector.X != 0)
            {
                ret |= (vector.X > 0) ? DirectionType.East : DirectionType.West;
            }
            return ret;
        }

        /// <summary>
        ///   Returns directions of a vector, based on quadrants.
        /// </summary>
        /// <param name="vector">Vector to convert.</param>
        /// <returns> Set of directions.</returns>
        public static DirectionType toDirections(Vector2F vector)
        {
            DirectionType ret = DirectionType.None;
            if (vector.Y == 0.0f && vector.X == 0.0f)
            {
                return DirectionType.None;
            }
            if (vector.Y != 0.0f)
            {
                ret |= (vector.Y > 0.0f) ? DirectionType.North : DirectionType.South;
            }
            if (vector.X != 0.0f)
            {
                ret |= (vector.X > 0.0f) ? DirectionType.East : DirectionType.West;
            }
            return ret;
        }

        /// <summary>
        ///   Converts the specified direction into a vector.
        /// </summary>
        /// <param name="direction">Direction to convert.</param>
        /// <returns>Vector pointing in the specified direction.</returns>
        public static Vector2F toVector2F(this DirectionType direction)
        {
            Vector2I directionVector = toVector2I(direction);
            return new Vector2F(directionVector.X, directionVector.Y);
        }

        /// <summary>
        ///   Converts the specified direction into a vector.
        /// </summary>
        /// <param name="direction">Direction to convert.</param>
        /// <returns>Vector pointing in the specified direction.</returns>
        public static Vector2I toVector2I(DirectionType direction)
        {
            return
                new Vector2I(
                    (direction & DirectionType.West) != DirectionType.None
                        ? -1
                        : (direction & DirectionType.East) != DirectionType.None ? +1 : 0,
                    (direction & DirectionType.North) != DirectionType.None
                        ? +1
                        : (direction & DirectionType.South) != DirectionType.None ? -1 : 0);
        }

        #endregion
    }
}