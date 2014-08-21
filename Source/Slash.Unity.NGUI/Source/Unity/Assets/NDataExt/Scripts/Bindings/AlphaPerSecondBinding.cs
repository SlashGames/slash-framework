// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AlphaPerSecondBinding.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Unity.NDataExt.Bindings
{
    using Slash.Unity.NGUIExt.Colors;

    public class AlphaPerSecondBinding : NguiNumericBinding
    {
        #region Fields

        public UIFadeWidget Target;

        #endregion

        #region Public Methods and Operators

        public override void Awake()
        {
            base.Awake();

            if (this.Target == null)
            {
                this.Target = this.GetComponent<UIFadeWidget>();
            }
        }

        #endregion

        #region Methods

        protected override void ApplyNewValue(double val)
        {
            this.Target.AlphaPerSecond = (float)val;
        }

        #endregion
    }
}