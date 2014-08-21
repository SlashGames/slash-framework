// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ActiveMultiBinding.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Unity.NDataExt.Bindings
{
    using System.Collections.Generic;
    using System.Linq;

    using UnityEngine;

    /// <summary>
    ///   Binds whether the target game object is active to ALL boolean values being true.
    /// </summary>
    public class ActiveMultiBinding : BooleanMultiBinding
    {
        #region Fields

        public GameObject Target;

        #endregion

        #region Methods

        protected override void ApplyNewValue(List<bool> values)
        {
            bool newValue = values.All(value => value);
            this.ApplyNewValue(newValue);
        }

        private void ApplyNewValue(bool newValue)
        {
            GameObject target = this.Target != null ? this.Target : this.gameObject;
            if (target.activeSelf == newValue)
            {
                return;
            }

            target.SetActive(newValue);
        }

        #endregion
    }
}