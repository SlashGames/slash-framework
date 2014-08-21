// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UIFlashingWidget.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Unity.NGUIExt.Colors
{
    using UnityEngine;

    /// <summary>
    ///   Makes the target widget flash once per second.
    /// </summary>
    public class UIFlashingWidget : MonoBehaviour
    {
        #region Fields

        public UIWidget Target;

        #endregion

        #region Methods

        private void Awake()
        {
            if (this.Target == null)
            {
                this.Target = this.GetComponent<UIWidget>();
            }
        }

        /// <summary>
        ///   Per frame update.
        /// </summary>
        private void Update()
        {
            // Ensure all widget are flashing the same time.
            this.Target.enabled = Time.realtimeSinceStartup - (int)Time.realtimeSinceStartup < 0.5f;
        }

        #endregion
    }
}