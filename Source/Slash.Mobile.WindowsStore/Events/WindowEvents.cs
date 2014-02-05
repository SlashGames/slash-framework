// --------------------------------------------------------------------------------------------------------------------
// <copyright file="WindowEvents.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Mobile.Events
{
    public static class WindowEvents
    {
        #region Static Fields

        public static VisibilityChangedDelegate VisibilityChanged;

        public static SizeChangedDelegate SizeChanged;

        #endregion

        #region Delegates

        public delegate void VisibilityChangedDelegate(bool visible);

        public delegate void SizeChangedDelegate(double width, double height);
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

        public static void OnSizeChanged(double width, double height)
        {
            var handler = SizeChanged;
            if (handler != null)
            {
                handler(width, height);
            }
        }

        #endregion
    }
}