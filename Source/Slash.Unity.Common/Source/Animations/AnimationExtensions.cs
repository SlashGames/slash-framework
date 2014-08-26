// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AnimationExtensions.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Unity.Common.Animations
{
    using System.Collections;

    using UnityEngine;

    /// <summary>
    ///   Extension methods for Unity animation objects.
    /// </summary>
    public static class AnimationExtensions
    {
        #region Public Methods and Operators

        /// <summary>
        ///   Keeps yielding while the animation is playing.
        /// </summary>
        /// <param name="animation">Animation to check.</param>
        /// <returns>Enumerator that keeps yielding while the animation is playing.</returns>
        public static IEnumerator WhilePlaying(this Animation animation)
        {
            do
            {
                yield return 0;
            }
            while (animation.isPlaying);
        }

        /// <summary>
        ///   Keeps yielding while the specified animation is playing.
        /// </summary>
        /// <param name="animation">Animation to check.</param>
        /// <param name="animationName">Name of the animation to check.</param>
        /// <returns>Enumerator that keeps yielding while the specified animation is playing.</returns>
        public static IEnumerator WhilePlaying(this Animation animation, string animationName)
        {
            while (animation.IsPlaying(animationName))
            {
                yield return 0;
            }
        }

        #endregion
    }
}