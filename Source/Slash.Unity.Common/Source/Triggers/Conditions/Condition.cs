// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Condition.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Unity.Common.Triggers.Conditions
{
    using System;

    using UnityEngine;

    [Serializable]
    public class Condition : MonoBehaviour, ICondition
    {
        #region Public Events

        public event ConditionFulfilledDelegate Fulfilled;

        #endregion

        #region Methods

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