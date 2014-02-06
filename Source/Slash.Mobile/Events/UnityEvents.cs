// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UnityEvents.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Mobile.Events
{
    public static class UnityEvents
    {
        #region Delegates

        public delegate void UnityLoadedDelegate();

        public delegate void UnityShutDownDelegate();

        #endregion

        #region Public Events

        /// <summary>
        ///   Called from Unity when the app is responsive and ready for play.
        /// </summary>
        public static event UnityLoadedDelegate UnityLoaded;

        /// <summary>
        ///   Called from Unity when shutting down.
        /// </summary>
        public static event UnityShutDownDelegate UnityShutDown;

        #endregion

        #region Public Methods and Operators

        public static void OnUnityLoaded()
        {
            var handler = UnityLoaded;
            if (handler != null)
            {
                handler();
            }
        }

        public static void OnUnityShutDown()
        {
            var handler = UnityShutDown;
            if (handler != null)
            {
                handler();
            }
        }

        #endregion
    }
}