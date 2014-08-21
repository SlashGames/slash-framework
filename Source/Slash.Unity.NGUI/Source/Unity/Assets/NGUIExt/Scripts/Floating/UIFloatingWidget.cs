// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UIFloatingWidget.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Unity.NGUIExt.Floating
{
    using Slash.Unity.NGUIExt.Util;

    using UnityEngine;

    public class UIFloatingWidget : MonoBehaviour
    {
        #region Fields

        /// <summary>
        ///   Fade alpha per second.
        /// </summary>
        public float FadeSpeed = 0.33f;

        /// <summary>
        ///   Position change, in units per second.
        /// </summary>
        public float FloatingSpeed = 1.0f;

        /// <summary>
        ///   Time before the widget is removed, in seconds.
        /// </summary>
        public float Lifetime;

        /// <summary>
        ///   Widget to float.
        /// </summary>
        public UIWidget Widget;

        private float lifetimeRemaining;

        #endregion

        #region Methods

        private void Awake()
        {
            if (this.Widget == null)
            {
                this.Widget = this.GetComponent<UIWidget>();
            }
        }

        private void OnEnable()
        {
            this.lifetimeRemaining = this.Lifetime;
        }

        private void Update()
        {
            this.lifetimeRemaining -= Time.deltaTime;

            if (this.lifetimeRemaining <= 0)
            {
                Destroy(this.gameObject);
            }
            else
            {
                // Float.
                this.transform.localPosition += new Vector3(0.0f, this.FloatingSpeed * Time.deltaTime, 0.0f);

                // Fade.
                this.Widget.SetAlpha(this.Widget.color.a - this.FadeSpeed * Time.deltaTime);
            }
        }

        #endregion
    }
}