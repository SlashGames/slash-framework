// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FontSizeBinding.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Unity.NDataExt.Bindings
{
    using System;

    using UnityEngine;

    [Serializable]
    [AddComponentMenu("NGUI/NData/Font Size Binding")]
    public class FontSizeBinding : NguiNumericBinding
    {
        #region Fields

        public UILabel Target;

        #endregion

        #region Public Methods and Operators

        public override void Awake()
        {
            base.Awake();

            if (this.Target == null)
            {
                this.Target = this.GetComponent<UILabel>();
            }
        }

        #endregion

        #region Methods

        protected override void ApplyNewValue(double newValue)
        {
            if (this.Target != null)
            {
                this.Target.fontSize = (int)newValue;
            }
        }

        #endregion
    }
}