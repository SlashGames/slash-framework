// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EventCondition.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Unity.HOTweenExt
{
    using System;

    using Slash.Unity.Common.Triggers.Conditions;
    using Slash.Unity.Common.ViewModels;

    [Serializable]
    public sealed class EventCondition : Condition
    {
        #region Fields

        public ViewEventDelegate EventDelegate;

        #endregion

        #region Public Events
        
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