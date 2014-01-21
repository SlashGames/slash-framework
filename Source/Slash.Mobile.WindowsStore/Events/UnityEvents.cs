// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UnityEvents.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Mobile.Events
{
    public static class UnityEvents
    {
        #region Static Fields

        /// <summary>
        ///   Called from Unity when the app is responsive and ready for play.
        /// </summary>
        public static UnityLoadedDelegate UnityLoaded;

        #endregion

        #region Delegates

        public delegate void UnityLoadedDelegate();

        #endregion

        #region Public Methods and Operators

        public static void OnUnityLoaded()
        {
            UnityLoadedDelegate handler = UnityLoaded;
            if (handler != null)
            {
                handler();
            }
        }

        #endregion
    }
}