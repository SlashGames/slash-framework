// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ViewEvent.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Unity.Common.ViewModels
{
    using System;

    /// <summary>
    ///   View event interested listeners can register for.
    /// </summary>
    [Serializable]
    public class ViewEvent
    {
        #region Delegates

        /// <summary>
        ///   Event has occurred.
        /// </summary>
        /// <param name="eventArgs">Event parameters.</param>
        public delegate void EventDelegate(object eventArgs);

        #endregion

        #region Public Events

        /// <summary>
        ///   Event has occurred.
        /// </summary>
        public event EventDelegate Event;

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///   Event has occurred.
        /// </summary>
        /// <param name="eventArgs">Event parameters.</param>
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