// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UIChangeColorPeriodically.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Unity.NGUIExt.Colors
{
    using UnityEngine;

    /// <summary>
    ///   Changes the target widget color in periodic intervals.
    /// </summary>
    public class UIChangeColorPeriodically : MonoBehaviour
    {
        #region Fields

        /// <summary>
        ///   Time to show each color, in seconds.
        /// </summary>
        public float ColorDuration = 0.5f;

        /// <summary>
        ///   Colors to cycle.
        /// </summary>
        public Color[] Colors;

        /// <summary>
        ///   Widget to change the color of.
        /// </summary>
        public UIWidget Target;

        /// <summary>
        ///   Index of the current widget color.
        /// </summary>
        private int currentColor;

        /// <summary>
        ///   How long to still keep the current color, in seconds.
        /// </summary>
        private float currentColorDurationRemaining;

        #endregion

        #region Methods

        private void OnDisable()
        {
            this.currentColor = 0;
            this.UpdateWidgetColor();
        }

        private void OnEnable()
        {
            if (this.Target == null)
            {
                this.Target = this.GetComponent<UIWidget>();
            }

            this.currentColor = 1;
            this.UpdateWidgetColor();
        }

        private void Update()
        {
            this.currentColorDurationRemaining -= Time.deltaTime;

            if (this.currentColorDurationRemaining <= 0)
            {
                // Next color.
                this.currentColor = (this.currentColor + 1) % this.Colors.Length;
                this.UpdateWidgetColor();
            }
        }

        private void UpdateWidgetColor()
        {
            this.Target.color = this.Colors[this.currentColor];
            this.currentColorDurationRemaining = this.ColorDuration;
        }

        #endregion
    }
}