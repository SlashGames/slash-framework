// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ViewEvent.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Unity.Common.ViewModels
{
    using System;

    [Serializable]
    public class ViewEvent
    {
        #region Delegates

        public delegate void EventDelegate(object eventArgs);

        #endregion

        #region Public Events

        public event EventDelegate Event;

        #endregion

        #region Public Methods and Operators

        public void OnEvent(object eventArgs)
        {
            EventDelegate handler = this.Event;
            if (handler != null)
            {
                handler(eventArgs);
            }
        }

        #endregion
    }
}