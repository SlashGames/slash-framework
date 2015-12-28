// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FinishAnimationOnTapBehaviour.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Unity.FingerGesturesExt.Animations
{
    using UnityEngine;

    public class FinishAnimationOnTapBehaviour : GestureRecognizerDelegate
    {
        #region Fields

        public string AnimationName;

        /// <summary>
        ///   Factor to speed up animation to finish it. 0 means that the animation is immediately finished.
        /// </summary>
        [Tooltip("Factor to speed up animation to finish it. 0 means that the animation is immediately finished.")]
        public float FinishSpeed;

        /// <summary>
        ///   Threshold to avoid to finish animation by accident (in sec).
        /// </summary>
        [Tooltip("Threshold to avoid to finish animation by accident (in sec).")]
        public float Threshold;

        #endregion

        #region Public Methods and Operators

        public override bool CanBegin(Gesture gesture, FingerGestures.IFingerList touches)
        {
            //Debug.Log(
            //    UnityUtils.WithTimestamp("Check if tap for " + this.AnimationName + " can begin: " + gesture + ", touches: " + touches.Count), this);

            if (this.animation == null || !this.animation.IsPlaying(this.AnimationName))
            {
                return false;
            }

            // Just allow tap when touch just began.
            if (touches[0].Phase != FingerGestures.FingerPhase.Begin)
            {
                return false;
            }

            // Check if tap started after threshold.
            AnimationState animationState = this.animation[this.AnimationName];
            if (animationState.time < this.Threshold)
            {
                return false;
            }

            return true;
        }

        #endregion

        #region Methods

        private void OnTap()
        {
            if (this.animation == null || !this.animation.IsPlaying(this.AnimationName))
            {
                Debug.Log("Animation " + this.AnimationName + " not playing");
                return;
            }

            //Debug.Log(
            //    UnityUtils.WithTimestamp("Finishing animation " + this.AnimationName + " because received tap."), this);

            AnimationState animationState = this.animation[this.AnimationName];
            if (animationState == null)
            {
                return;
            }

            // Check method to finish animation.
            if (this.FinishSpeed > 0.0f)
            {
                animationState.speed = this.FinishSpeed;
            }
            else
            {
                animationState.normalizedTime = 1.0f;
            }
        }

        #endregion
    }
}