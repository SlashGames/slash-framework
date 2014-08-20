// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HighlightColorTextBinding.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Unity.NDataExt.Bindings
{
    using System.Collections.Generic;

    using UnityEngine;

    /// <summary>
    ///   Changes the target widget's color for a short time whenever the bound text changes.
    /// </summary>
    public class HighlightColorTextBinding : NguiTextBinding
    {
        #region Fields

        /// <summary>
        ///   New values that shouldn't change the widget color.
        /// </summary>
        public List<string> IgnoredValues;

        /// <summary>
        ///   Color to change to when the bound text changes.
        /// </summary>
        public Color NewColor;

        /// <summary>
        ///   How long to use <see cref="NewColor" /> after the bound text changes, in seconds.
        /// </summary>
        public float NewColorDuration;

        /// <summary>
        ///   Widget to change the color of.
        /// </summary>
        public UIWidget Target;

        /// <summary>
        ///   Initial color of the widget.
        /// </summary>
        private Color initialColor;

        /// <summary>
        ///   How long to still keep the new color, in seconds.
        /// </summary>
        private float newColorDurationRemaining;

        #endregion

        #region Public Methods and Operators

        public override void Awake()
        {
            base.Awake();

            if (this.Target == null)
            {
                this.Target = this.GetComponent<UIWidget>();
            }

            this.initialColor = this.Target.color;
        }

        #endregion

        #region Methods

        protected override void ApplyNewValue(string newValue)
        {
            if (this.IgnoredValues.Contains(newValue))
            {
                return;
            }

            this.newColorDurationRemaining = this.NewColorDuration;
            this.Target.color = this.NewColor;
        }

        private void Update()
        {
            if (this.newColorDurationRemaining < 0)
            {
                return;
            }

            this.newColorDurationRemaining -= Time.deltaTime;

            if (this.newColorDurationRemaining <= 0)
            {
                this.Target.color = this.initialColor;
            }
        }

        #endregion
    }
}