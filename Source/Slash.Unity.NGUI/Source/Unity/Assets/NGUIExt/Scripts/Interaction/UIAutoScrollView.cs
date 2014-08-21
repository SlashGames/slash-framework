// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UIAutoScrollView.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Unity.NGUIExt.Interaction
{
    using UnityEngine;

    /// <summary>
    ///   Automatically scrolls the target ScrollView (think Credits).
    /// </summary>
    public class UIAutoScrollView : MonoBehaviour
    {
        #region Fields

        /// <summary>
        ///   Auto scroll view will reset if this local y position is reached.
        /// </summary>
        public float RestartAtYValue;

        /// <summary>
        ///   Scroll speed, in pixels per second.
        /// </summary>
        public float ScrollSpeed;

        public UIScrollView Target;

        #endregion

        #region Methods

        private void Awake()
        {
            if (this.Target == null)
            {
                this.Target = this.GetComponent<UIScrollView>();
            }
        }

        private void Update()
        {
            this.Target.MoveRelative(new Vector3(0, this.ScrollSpeed * Time.deltaTime, 0));

            if ((this.RestartAtYValue < 0 || this.RestartAtYValue > 0)
                && this.transform.localPosition.y > this.RestartAtYValue)
            {
                this.Target.MoveRelative(new Vector3(0, -this.transform.localPosition.y, 0));
            }
        }

        #endregion
    }
}