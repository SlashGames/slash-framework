// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EventCondition.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Unity.Common.Triggers.Conditions
{
    using System;

    using Slash.Unity.Common.ViewModels;

    /// <summary>
    ///   Condition that is fulfilled whenever a specific game event occurs.
    /// </summary>
    [Serializable]
    public sealed class EventCondition : Condition
    {
        #region Fields

        /// <summary>
        ///   Event fulfilling this condition.
        /// </summary>
        public ViewEventDelegate EventDelegate;

        #endregion

        #region Methods

        private void Awake()
        {
            // Register for trigger event.
            if (this.EventDelegate != null)
            {
                this.EventDelegate.Register(this.OnEvent);
            }
        }

        private void OnEvent(object eventArgs)
        {
            this.OnFulfilled();
        }

        #endregion
    }
}