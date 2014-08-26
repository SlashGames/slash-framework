// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UnityColorUtils.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Unity.Common.Utils
{
    using UnityEngine;

    /// <summary>
    ///   Utility methods for handling Unity color objects.
    /// </summary>
    public static class UnityColorUtils
    {
        #region Public Methods and Operators

        /// <summary>
        ///   Creates a unity color from integer RGB values.
        ///   The constructor of the Unity Color class takes float values from
        ///   0 to 1 by default.
        /// </summary>
        /// <param name="r">Red value (0-255).</param>
        /// <param name="g">Green value (0-255).</param>
        /// <param name="b">Blue value (0-255).</param>
        /// <returns>Unity color from the specified RGB values.</returns>
        public static Color ColorFromRGB(int r, int g, int b)
        {
            return new Color(r / 255.0f, g / 255.0f, b / 255.0f);
        }

        #endregion
    }
}