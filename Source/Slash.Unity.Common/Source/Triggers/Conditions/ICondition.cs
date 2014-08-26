// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ICondition.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Unity.Common.Triggers.Conditions
{
    /// <summary>
    ///   Condition has been fulfilled.
    /// </summary>
    public delegate void ConditionFulfilledDelegate();

    /// <summary>
    ///   Generic condition that has to be fulfilled before a set of actions is triggered.
    /// </summary>
    public interface ICondition
    {
        #region Public Events

        /// <summary>
        ///   Condition has been fulfilled.
        /// </summary>
        event ConditionFulfilledDelegate Fulfilled;

        #endregion
    }
}