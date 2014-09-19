// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HighlightColorBinding.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Unity.NDataExt.Bindings
{
    using UnityEngine;

    /// <summary>
    ///   Changes the target widget's color for a short time whenever the bound text changes.
    /// </summary>
    public class HighlightColorBinding : NguiBooleanBinding
    {
        #region Fields

        /// <summary>
        ///   Color to change to when the binding is true.
        /// </summary>
        public Color HighlightColor;

        /// <summary>
        ///   Widget to change the color of.
        /// </summary>
        public UIWidget[] Targets;

        /// <summary>
        ///   Initial color of the widget.
        /// </summary>
        private Color[] initialColors;

        #endregion

        #region Public Methods and Operators

        public override void Awake()
        {
            base.Awake();

            if (this.Targets == null || this.Targets.Length == 0)
            {
                this.Targets = this.GetComponentsInChildren<UIWidget>();
            }

            if (this.Targets != null && this.Targets.Length > 0)
            {
                this.initialColors = new Color[this.Targets.Length];
                for (int index = 0; index < this.Targets.Length; index++)
                {
                    this.initialColors[index] = this.Targets[index].color;
                }
            }
        }

        #endregion

        #region Methods

        protected override void ApplyNewValue(bool newValue)
        {
            for (int index = 0; index < this.Targets.Length; index++)
            {
                var widget = this.Targets[index];
                widget.color = newValue ? this.HighlightColor : this.initialColors[index];
            }
        }

        #endregion
    }
}