// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ScaleOnChangeBinding.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Unity.NDataExt.Bindings
{
    using System.Collections;

    using UnityEngine;

    public class ScaleOnChangeBinding : NguiNumericBinding
    {
        #region Fields

        /// <summary>
        ///   Factor to scale by.
        /// </summary>
        public float ScaleFactor = 2.0f;

        /// <summary>
        ///   Duration of tween animation (in s).
        /// </summary>
        public float TweenDuration = 1.0f;

        private Vector3 originalScale;

        #endregion

        #region Public Methods and Operators

        public override void Start()
        {
            base.Start();

            this.originalScale = this.gameObject.transform.localScale;
        }

        #endregion

        #region Methods

        protected override void ApplyNewValue(double val)
        {
            if (!this.gameObject.activeInHierarchy)
            {
                return;
            }

            this.StartCoroutine(this.StartTween());
        }

        private IEnumerator StartTween()
        {
            float halfDuration = this.TweenDuration * 0.5f;

            // Scale up.
            TweenScale scaleUpTween = TweenScale.Begin(
                this.gameObject, halfDuration, this.originalScale * this.ScaleFactor);
            scaleUpTween.from = this.originalScale;
            scaleUpTween.to = this.originalScale * this.ScaleFactor;

            // Wait till scaled up.
            yield return new WaitForSeconds(halfDuration);

            // Scale down.
            TweenScale scaleDownTween = TweenScale.Begin(this.gameObject, halfDuration, this.originalScale);
            scaleDownTween.from = this.originalScale * this.ScaleFactor;
            scaleDownTween.to = this.originalScale;
        }

        #endregion
    }
}