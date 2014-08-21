// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UIFadeWidget.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Unity.NGUIExt.Colors
{
    using Slash.Unity.NGUIExt.Util;

    using UnityEngine;

    /// <summary>
    ///   Fades the target widget in and out once.
    /// </summary>
    public class UIFadeWidget : MonoBehaviour
    {
        #region Fields

        public float AlphaPerSecond;

        public bool FadeAutomatically;

        public UIWidget Target;

        private FadeState fadeState = FadeState.Hidden;

        #endregion

        #region Enums

        private enum FadeState
        {
            FadingIn,

            FadingOut,

            Hidden
        }

        #endregion

        #region Properties

        private float CurrentWidgetAlpha
        {
            get
            {
                return this.Target.color.a;
            }
        }

        #endregion

        #region Public Methods and Operators

        public void FadeIn(float alphaPerSecond)
        {
            this.AlphaPerSecond = alphaPerSecond;
            this.fadeState = FadeState.FadingIn;
        }

        #endregion

        #region Methods

        private void Start()
        {
            if (this.Target == null)
            {
                this.Target = this.GetComponent<UIWidget>();
            }

            // Hide.
            this.Target.SetAlpha(0f);

            if (this.FadeAutomatically)
            {
                // Start fading in.
                this.FadeIn(this.AlphaPerSecond);
            }
        }

        private void Update()
        {
            if (this.fadeState == FadeState.Hidden)
            {
                return;
            }

            var alphaPerFrame = this.AlphaPerSecond * Time.deltaTime;

            if (this.fadeState == FadeState.FadingIn)
            {
                // Fade in.
                this.Target.SetAlpha(this.CurrentWidgetAlpha + alphaPerFrame);

                if (this.CurrentWidgetAlpha >= 1)
                {
                    this.fadeState = FadeState.FadingOut;
                }
            }
            else
            {
                // Fade out.
                this.Target.SetAlpha(this.CurrentWidgetAlpha - alphaPerFrame);

                if (this.CurrentWidgetAlpha <= 0)
                {
                    this.fadeState = FadeState.Hidden;
                }
            }
        }

        #endregion
    }
}