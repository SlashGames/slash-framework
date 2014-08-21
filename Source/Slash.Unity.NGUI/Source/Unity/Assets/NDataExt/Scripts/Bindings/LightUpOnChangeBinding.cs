// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LightUpOnChangeBinding.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Unity.NDataExt.Bindings
{
    using System;
    using System.Collections;

    using UnityEngine;

    public class LightUpOnChangeBinding : NguiNumericBinding
    {
        #region Fields

        /// <summary>
        ///   Factor to multiply sprite color by.
        /// </summary>
        public float ColorFactor = 0.2f;

        public UISprite Target;

        /// <summary>
        ///   Duration of tween animation (in s).
        /// </summary>
        public float TweenDuration = 0.2f;

        private Color originalColor;

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///   Given HSL in range of 0-1, returns RGB in range of 0-255.
        /// </summary>
        public static void HSL2RGB(double h, double s, double l, out double r, out double g, out double b)
        {
            // Default to gray.
            r = l;
            g = l;
            b = l;

            double v = (l <= 0.5) ? (l * (1.0 + s)) : (l + s - l * s);
            if (v <= 0)
            {
                return;
            }

            double m = l + l - v;
            double sv = (v - m) / v;
            h *= 6.0;
            int sextant = (int)h;
            double fract = h - sextant;
            double vsf = v * sv * fract;
            double mid1 = m + vsf;
            double mid2 = v - vsf;
            switch (sextant % 6)
            {
                case 0:
                    r = v;
                    g = mid1;
                    b = m;
                    break;
                case 1:
                    r = mid2;
                    g = v;
                    b = m;
                    break;
                case 2:
                    r = m;
                    g = v;
                    b = mid1;
                    break;
                case 3:
                    r = m;
                    g = mid2;
                    b = v;
                    break;
                case 4:
                    r = mid1;
                    g = m;
                    b = v;
                    break;
                case 5:
                    r = v;
                    g = m;
                    b = mid2;
                    break;
            }
        }

        public static void RGB2HSL(float r, float g, float b, out double h, out double s, out double l)
        {
            // Default to black.
            h = 0;
            s = 0;
            l = 0;

            double v = Math.Max(r, g);
            v = Math.Max(v, b);
            double m = Math.Min(r, g);
            m = Math.Min(m, b);
            l = (m + v) / 2.0;
            if (l <= 0.0)
            {
                return;
            }
            double vm = v - m;
            s = vm;
            if (s > 0.0)
            {
                s /= (l <= 0.5) ? (v + m) : (2.0 - v - m);
            }
            else
            {
                return;
            }
            double r2 = (v - r) / vm;
            double g2 = (v - g) / vm;
            double b2 = (v - b) / vm;
            if (r == v)
            {
                h = (g == m ? 5.0 + b2 : 1.0 - g2);
            }
            else if (g == v)
            {
                h = (b == m ? 1.0 + r2 : 3.0 - b2);
            }
            else
            {
                h = (r == m ? 3.0 + g2 : 5.0 - r2);
            }
            h /= 6.0;
        }

        public override void Awake()
        {
            base.Awake();

            if (this.Target == null)
            {
                this.Target = this.GetComponent<UISprite>();
            }
        }

        public override void Start()
        {
            base.Start();

            this.originalColor = this.Target.color;
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
            if (this.Target == null)
            {
                yield break;
            }

            float halfDuration = this.TweenDuration * 0.5f;

            double h, s, l;
            RGB2HSL(this.originalColor.r, this.originalColor.g, this.originalColor.b, out h, out s, out l);
            l = Math.Min(l + this.ColorFactor, 1.0f);
            double r, g, b;
            HSL2RGB(h, s, l, out r, out g, out b);
            Color lightUpColor = new Color((float)r, (float)g, (float)b);

            // Light up.
            TweenColor lightUpTween = UITweener.Begin<TweenColor>(this.gameObject, halfDuration);
            lightUpTween.from = this.originalColor;
            lightUpTween.to = lightUpColor;

            // Wait till scaled up.
            yield return new WaitForSeconds(halfDuration);

            // Light down.
            TweenColor lightDownTween = UITweener.Begin<TweenColor>(this.gameObject, halfDuration);
            lightDownTween.from = lightUpColor;
            lightDownTween.to = this.originalColor;
        }

        #endregion
    }
}