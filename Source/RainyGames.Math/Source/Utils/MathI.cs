// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Mathf.cs" company="Rainy Games">
//   Copyright (c) Rainy Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace RainyGames.Math.Utils
{
    using System;
    using RainyGames.Math.Algebra.Vectors;

    /// <summary>
    ///   Mathematical functions for integers.
    /// </summary>
    public static class MathI
    {
        public static int Pow(int x, uint pow)
        {
            int ret = 1;
            while (pow != 0)
            {
                if ((pow & 1) == 1)
                    ret *= x;
                x *= x;
                pow >>= 1;
            }
            return ret;

        }
    }
}