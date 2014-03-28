// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AnimationExtensions.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Unity.Common.Animations
{
    using System.Collections;

    using UnityEngine;

    public static class AnimationExtensions
    {
        #region Public Methods and Operators

        public static IEnumerator WhilePlaying(this Animation animation)
        {
            do
            {
                yield return 0;
            }
            while (animation.isPlaying);
        }

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