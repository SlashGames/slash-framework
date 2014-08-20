// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HighlightSpriteAlpha.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Unity.Common.Visual
{
    using UnityEngine;

    /// <summary>
    ///   Highlights a Unity 2D sprite by fading it in and out.
    /// </summary>
    public class HighlightSpriteAlpha : MonoBehaviour
    {
        #region Fields

        /// <summary>
        ///   Alpha increase or decrease per second.
        /// </summary>
        public float AlphaPerSecond = 1.0f;

        /// <summary>
        ///   Minimum alpha value.
        /// </summary>
        public float MinAlpha = 0.5f;

        /// <summary>
        ///   Sprite to change the alpha value of.
        /// </summary>
        public SpriteRenderer Target;

        /// <summary>
        ///   Whether the sprite is currently fading in or out.
        /// </summary>
        private FadeState fadeState;

        #endregion

        #region Enums

        private enum FadeState
        {
            None,

            FadeOut,

            FadeIn,

            FadeInAndStop
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///   Starts highlighting the target sprite by fading it in and out.
        /// </summary>
        public void StartHighlight()
        {
            if (this.fadeState == FadeState.FadeIn || this.fadeState == FadeState.FadeOut)
            {
                return;
            }

            this.fadeState = FadeState.FadeOut;
        }

        /// <summary>
        ///   Stops highlighting the target sprite by fading it in and stopping fading.
        /// </summary>
        public void StopHighlight()
        {
            if (this.fadeState == FadeState.None || this.fadeState == FadeState.FadeInAndStop)
            {
                return;
            }

            this.fadeState = FadeState.FadeInAndStop;
        }

        #endregion

        #region Methods

        private void Awake()
        {
            if (this.Target == null)
            {
                this.Target = this.GetComponent<SpriteRenderer>();
            }
        }

        private void Update()
        {
            Color oldColor = this.Target.color;
            float newAlpha = oldColor.a;

            switch (this.fadeState)
            {
                case FadeState.FadeOut:
                    newAlpha = oldColor.a - this.AlphaPerSecond * Time.deltaTime;

                    if (newAlpha <= this.MinAlpha)
                    {
                        this.fadeState = FadeState.FadeIn;
                    }
                    break;

                case FadeState.FadeIn:
                    newAlpha = oldColor.a + this.AlphaPerSecond * Time.deltaTime;

                    if (newAlpha >= 1.0f)
                    {
                        this.fadeState = FadeState.FadeOut;
                    }
                    break;

                case FadeState.FadeInAndStop:
                    newAlpha = oldColor.a + this.AlphaPerSecond * Time.deltaTime;

                    if (newAlpha >= 1.0f)
                    {
                        this.fadeState = FadeState.None;
                    }
                    break;
            }

            this.Target.color = new Color(oldColor.r, oldColor.g, oldColor.b, newAlpha);
        }

        #endregion
    }
}