// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LoggerExtensions.cs" company="Rainy Games GmbH">
//   Copyright (c) Rainy Games GmbH. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace RainyGames.Unity.Common.Logging
{
    using RainyGames.Diagnostics.Logging;

    using UnityEngine;

    public static class LoggerExtensions
    {
        #region Public Methods and Operators

        /// <summary>
        ///   Call this method to initialize the logger to use it with Unity.
        /// </summary>
        public static void InitLogger()
        {
            if (Application.platform == RuntimePlatform.OSXWebPlayer
                || Application.platform == RuntimePlatform.WindowsWebPlayer)
            {
                // logging won't make a lot of sense in a Web player...
                return;
            }

            string configFile = Application.dataPath + "/Configurations/log4net.xml";
            if (Application.platform == RuntimePlatform.WindowsPlayer)
            {
                configFile = Application.dataPath + "\\Configurations\\log4net.xml";
            }

            Debug.Log(string.Format("Init logger with configuration {0}", configFile));

            Logger.Configure(configFile);
        }

        #endregion
    }
}