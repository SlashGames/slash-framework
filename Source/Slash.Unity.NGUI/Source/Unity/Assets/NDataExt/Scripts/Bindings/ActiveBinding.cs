// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ActiveBinding.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Unity.NDataExt.Bindings
{
    using UnityEngine;

    /// <summary>
    ///   Binds whether the target game object is active or inactive.
    /// </summary>
    public class ActiveBinding : NguiBooleanBinding
    {
        #region Fields

        /// <summary>
        ///   Object to set active or inactive.
        /// </summary>
        public GameObject Target;

        #endregion

        #region Methods

        protected override void ApplyNewValue(bool newValue)
        {
            // NOTE(co): Have to check, game object might be already destroyed.
            // ReSharper disable ConditionIsAlwaysTrueOrFalse
            if (this == null)
            // ReSharper restore ConditionIsAlwaysTrueOrFalse
            {
                return;
            }

            var target = this.Target ?? this.gameObject;
            target.SetActive(newValue);
        }

        #endregion
    }
}