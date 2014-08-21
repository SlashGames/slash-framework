// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PercentageBinding.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Unity.NDataExt.Bindings
{
    public class PercentageBinding : NguiCustomBoundsNumericBinding
    {
        #region Fields

        public string Format = "{0:F0}";

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

        protected override void ApplyNewCustomBoundsValue(double val)
        {
            if (this.Target == null)
            {
                return;
            }

            this.Target.text = string.Format(this.Format, val * 100);
        }

        #endregion
    }
}