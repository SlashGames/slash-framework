// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SmoothSliderBinding.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Unity.NDataExt.Bindings
{
    using UnityEngine;

    /// <summary>
    ///   Extends NguiSliderBinding by smoothly applying all changes.
    /// </summary>
    public class SmoothSliderBinding : NguiSliderBinding
    {
        #region Fields

        /// <summary>
        ///   Speed factor for applying changes.
        /// </summary>
        public float Step = 1f;

        /// <summary>
        ///   Current delta applied each frame.
        /// </summary>
        private float delta;

        /// <summary>
        ///   Target slider value.
        /// </summary>
        private float targetValue;

        #endregion

        #region Public Methods and Operators

        public override void Start()
        {
            base.Start();

            this.targetValue = (float)this.GetValue();
        }

        #endregion

        #region Methods

        protected override void SetValue(double val)
        {
            this.targetValue = (float)val;
            this.delta = (float)(this.targetValue - this.GetValue()) * this.Step;
        }

        private void Update()
        {
            var currentValue = (float)this.GetValue();

            if (currentValue != this.targetValue)
            {
                float newValue = currentValue + this.delta * Time.deltaTime;

                if (this.delta < 0)
                {
                    newValue = Mathf.Max(newValue, this.targetValue);
                }
                else
                {
                    newValue = Mathf.Min(newValue, this.targetValue);
                }

                base.SetValue(newValue);
            }
        }

        #endregion
    }
}