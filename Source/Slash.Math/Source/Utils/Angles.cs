// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Angles.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Math.Utils
{
    using System;

    using Slash.Math.Algebra.Vectors;

    /// <summary>
    ///   Contains utility methods to work with angles. All parameter values use the unit radian for angles.
    /// </summary>
    public static class Angles
    {
        #region Constants

        /// <summary>
        ///   Factor to convert a degree angle to a radian angle.
        /// </summary>
        public const float Deg2RadFactor = MathUtils.Pi / 180.0f;

        /// <summary>
        ///   Factor to convert a radian angle to a degree angle.
        /// </summary>
        public const float Rad2DegFactor = 180.0f / MathUtils.Pi;

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///   Computes the smallest difference from first to second angle. Takes into account cases like AngleDiff( -Pi, Pi ) == 0. Returned radian angle is between (-Pi, Pi).
        /// </summary>
        /// <param name="angleA"> First angle (in radians). </param>
        /// <param name="angleB"> Second angle (in radians). </param>
        /// <returns> Smallest difference between the two angles (in radians) between (-Pi, Pi). </returns>
        public static float AngleDiff(float angleA, float angleB)
        {
            angleA = ClampAngle(angleA);
            angleB = ClampAngle(angleB);
            float diff = angleB - angleA;
            float diff2 = diff < 0 ? diff + MathUtils.TwoPi : diff - MathUtils.TwoPi;
            return ClampAngle(Math.Min(diff2, diff));
        }

        /// <summary>
        ///   Computes the smallest difference from first to second angle. Takes into account cases like AngleDiff( -Pi, Pi ) == 0. Returned radian angle is between (0, 2*Pi).
        /// </summary>
        /// <param name="angleA"> First angle (in radians). </param>
        /// <param name="angleB"> Second angle (in radians). </param>
        /// <returns> Smallest difference between the two angles (in radians) between (0, 2*Pi). </returns>
        public static float AngleDiffPositive(float angleA, float angleB)
        {
            angleA = ClampAngle(angleA);
            angleB = ClampAngle(angleB);
            float diff = angleB - angleA;
            float diff2 = diff < 0 ? diff + MathUtils.TwoPi : diff - MathUtils.TwoPi;
            return ClampAnglePositive(Math.Min(diff2, diff));
        }

        /// <summary>
        ///   Clamps the angle to -Pi and Pi.
        /// </summary>
        /// <param name="angle"> Angle (in radians). </param>
        /// <returns> Clamped angle (in radians). </returns>
        public static float ClampAngle(float angle)
        {
            angle = angle % MathUtils.TwoPi;
            while (angle < -MathUtils.Pi)
            {
                angle += MathUtils.TwoPi;
            }
            while (angle > MathUtils.Pi)
            {
                angle -= MathUtils.TwoPi;
            }
            return angle;
        }

        /// <summary>
        ///   Clamps the angle to 0 and 2*PI.
        /// </summary>
        /// <param name="angle"> Angle (in radians). </param>
        /// <returns> Clamped angle (in radians). </returns>
        public static float ClampAnglePositive(float angle)
        {
            angle = angle % MathUtils.TwoPi;
            while (angle < 0)
            {
                angle += MathUtils.TwoPi;
            }
            while (angle > MathUtils.TwoPi)
            {
                angle -= MathUtils.TwoPi;
            }
            return angle;
        }

        /// <summary>
        ///   Interpolates an angle from a start value to an end value.
        /// </summary>
        /// <param name="from"> Start angle (in radians). </param>
        /// <param name="to"> End angle (in radians). </param>
        /// <param name="step"> Step value (0...1). </param>
        /// <returns> Returns the start angle if step value is smaller or equal to zero, the end angle if step value is bigger or equal to 1 and an interpolated angle (in radians) if the step value is between 0 and 1. </returns>
        public static float CurveAngle(float from, float to, float step)
        {
            if (step <= 0)
            {
                return from;
            }
            if (from == to || step >= 1)
            {
                return to;
            }

            Vector2F fromVector = new Vector2F(MathUtils.Cos(from), MathUtils.Sin(from));
            Vector2F toVector = new Vector2F(MathUtils.Cos(to), MathUtils.Sin(to));

            Vector2F currentVector = Vector2F.Slerp(fromVector, toVector, step);

            return MathUtils.Atan2(currentVector.Y, currentVector.X);
        }

        /// <summary>
        ///   Converts a degree angle to a radian angle.
        /// </summary>
        /// <param name="deg"> Angle to convert (in degree). </param>
        /// <returns> Converted angle (in radians). </returns>
        public static float Deg2Rad(float deg)
        {
            return (deg * MathUtils.Pi) / 180.0f;
        }

        /// <summary>
        ///   Converts a radian angle to a degree angle.
        /// </summary>
        /// <param name="rad"> Angle to convert (in radians). </param>
        /// <returns> Converted angle (in degree). </returns>
        public static float Rad2Deg(float rad)
        {
            return (rad * 180.0f) / MathUtils.Pi;
        }

        /// <summary>
        ///   Spherical interpolation of a value from a start angle to an end value for the passed step.
        /// </summary>
        /// <param name="from"> Start angle (in radians). </param>
        /// <param name="to"> End angle (in radians). </param>
        /// <param name="step"> Current step value (0...1). </param>
        /// <returns> Current interpolation value (in radians). </returns>
        public static float Slerp(float from, float to, float step)
        {
            if (step <= 0)
            {
                return from;
            }
            if (from == to || step >= 1)
            {
                return to;
            }

            from = ClampAngle(from);
            to = ClampAngle(to);

            // Compute difference
            float angleDiff = AngleDiff(from, to);
            if (angleDiff == 0)
            {
                return to;
            }

            float sinTheta = MathUtils.Sin(angleDiff);
            return MathUtils.Sin((1 - step) * angleDiff) / sinTheta * from + MathUtils.Sin(step * angleDiff) / sinTheta * to;
        }

        #endregion
    }
}