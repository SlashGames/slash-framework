// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MathI.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Math.Utils
{
    /// <summary>
    ///   Mathematical functions for integers.
    /// </summary>
    public static class MathI
    {
        #region Public Methods and Operators

        public static int Pow(int x, uint pow)
        {
            int ret = 1;
            while (pow != 0)
            {
                if ((pow & 1) == 1)
                {
                    ret *= x;
                }
                x *= x;
                pow >>= 1;
            }
            return ret;
        }

        #endregion
    }
}