// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UISliderLabel.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Unity.NGUIExt.Interaction
{
    using UnityEngine;

    /// <summary>
    ///   Connects a slider to a UI label to show the current slider value.
    /// </summary>
    public class UISliderLabel : MonoBehaviour
    {
        #region Fields

        /// <summary>
        ///   Formatting of slider value.
        /// </summary>
        public string Format = "0.00";

        /// <summary>
        ///   Label to set slider value to.
        /// </summary>
        public UILabel Label;

        /// <summary>
        ///   Range maximum.
        /// </summary>
        public float Max = 1.0f;

        /// <summary>
        ///   Range minimum.
        /// </summary>
        public float Min = 0.0f;

        /// <summary>
        ///   Slider to get value from.
        /// </summary>
        public UISlider Slider;

        #endregion

        #region Methods

        private void OnDisable()
        {
            if (this.Slider != null)
            {
                EventDelegate.Remove(this.Slider.onChange, this.OnSliderValueChanged);
            }
        }

        private void OnEnable()
        {
            if (this.Slider != null)
            {
                EventDelegate.Add(this.Slider.onChange, this.OnSliderValueChanged);
            }
        }

        private void OnSliderValueChanged()
        {
            if (this.Label == null)
            {
                return;
            }

            var value = UIProgressBar.current.value;
            float outputValue = value * (this.Max - this.Min) + this.Min;
            this.Label.text = outputValue.ToString(this.Format);
        }

        #endregion
    }
}