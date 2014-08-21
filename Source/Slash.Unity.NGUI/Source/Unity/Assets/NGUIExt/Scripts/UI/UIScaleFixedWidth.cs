// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UIScaleFixedWidth.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Unity.NGUIExt.Scripts.UI
{
    using UnityEngine;

    /// <summary>
    ///   Scales the UI by fixed width instead of fixed height.
    /// </summary>
    [ExecuteInEditMode]
    public class UIScaleFixedWidth : MonoBehaviour
    {
        #region Fields

        public UIRoot Root;

        public int TargetWidth = 1920;

        private bool adjustManualHeight;

        #endregion

        #region Methods

        private void AdjustManualHeight()
        {
            float scale = (float)Screen.width / this.TargetWidth;
            this.Root.manualHeight = Mathf.RoundToInt(Screen.height / scale);
        }

        private void Awake()
        {
            if (this.Root == null)
            {
                this.Root = this.GetComponent<UIRoot>();
            }
        }

        [ContextMenu("Adjust Manual Height")]
        private void DoAdjustManualHeight()
        {
            this.adjustManualHeight = true;
        }

        private void Start()
        {
            this.AdjustManualHeight();
        }

        private void Update()
        {
            if (this.adjustManualHeight)
            {
                this.AdjustManualHeight();
                this.adjustManualHeight = false;
            }
        }

        #endregion
    }
}