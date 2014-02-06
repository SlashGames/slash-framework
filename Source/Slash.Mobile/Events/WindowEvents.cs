// --------------------------------------------------------------------------------------------------------------------
// <copyright file="WindowEvents.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Mobile.Events
{
    public static class WindowEvents
    {
        #region Delegates

        public delegate void SizeChangedDelegate(double width, double height);

        public delegate void VisibilityChangedDelegate(bool visible);

        #endregion

        #region Public Events

        public static event SizeChangedDelegate SizeChanged;

        public static event VisibilityChangedDelegate VisibilityChanged;

        #endregion

        #region Public Methods and Operators

        public static void OnSizeChanged(double width, double height)
        {
            var handler = SizeChanged;
            if (handler != null)
            {
                handler(width, height);
            }
        }

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