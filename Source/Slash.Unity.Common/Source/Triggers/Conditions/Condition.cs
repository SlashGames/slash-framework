// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Condition.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Unity.Common.Triggers.Conditions
{
    using System;

    using UnityEngine;

    /// <summary>
    ///   Generic condition that has to be fulfilled before a set of actions is triggered.
    /// </summary>
    [Serializable]
    public class Condition : MonoBehaviour, ICondition
    {
        #region Public Events

        /// <summary>
        ///   Condition has been fulfilled.
        /// </summary>
        public event ConditionFulfilledDelegate Fulfilled;

        #endregion

        #region Methods

        /// <summary>
        ///   Condition has been fulfilled.
        /// </summary>
        protected virtual void OnFulfilled()
        {
            var handler = this.Fulfilled;
            if (handler != null)
            {
                handler();
            }
        }

        #endregion
    }
}