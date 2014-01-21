// --------------------------------------------------------------------------------------------------------------------
// <copyright file="WindowEvents.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Mobile.Events
{
    public class WindowEvents
    {
        #region Static Fields

        public static VisibilityChangedDelegate VisibilityChanged;

        #endregion

        #region Delegates

        public delegate void VisibilityChangedDelegate(bool visible);

        #endregion

        #region Public Methods and Operators

        public static void OnVisibilityChanged(bool visible)
        {
            var handler = VisibilityChanged;
            if (handler != null)
            {
                handler(visible);
            }
        }

        #endregion
    }
}