// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RandomUtils.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Math.Utils
{
    using System;

    /// <summary>
    ///   Utility methods which have to do with randomness.
    /// </summary>
    public static class RandomUtils
    {
        #region Static Fields

        /// <summary>
        ///   Random number generator.
        /// </summary>
        private static Random random;

        #endregion

        #region Public Properties

        /// <summary>
        ///   Random number generator singleton.
        /// </summary>
        public static Random Random
        {
            get
            {
                return random ?? (random = new Random());
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///   Generates a random sign.
        /// </summary>
        /// <returns>-1 or 1.</returns>
        public static int RandomSign()
        {
            return Random.NextDouble() < .5 ? 1 : -1;
        }

        /// <summary>
        ///   Returns a random double number between the specified minimum and maximum (exclusive).
        /// </summary>
        /// <param name="min">Minimum value.</param>
        /// <param name="max">Maximum value (exclusive).</param>
        /// <returns>Random double number between the specified minimum and maximum (exclusive).</returns>
        public static double RangeDouble(double min, double max)
        {
            if (min > max)
            {
                throw new ArgumentException(
                    string.Format("Minium value ({0}) has to be less or equal maximum value ({1})", min, max), "min");
            }

            double range = max - min;
            return min + Random.NextDouble() * range;
        }

        #endregion
    }
}