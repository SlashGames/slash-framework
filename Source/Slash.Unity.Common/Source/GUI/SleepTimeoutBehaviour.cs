// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SleepTimeoutBehaviour.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Unity.Common.GUI
{
    using UnityEngine;

    /// <summary>
    ///   Sets the Screen.sleepTimeout setting.
    /// </summary>
    public class SleepTimeoutBehaviour : MonoBehaviour
    {
        #region Fields

        /// <summary>
        ///   Manual defined timeout (in s).
        ///   Only used if Setting is set to ManualDefined.
        /// </summary>
        public int DefinedTimeout;

        /// <summary>
        ///   Setting of sleep timeout.
        /// </summary>
        public AvailableSetting Setting;

        #endregion

        #region Enums

        public enum AvailableSetting
        {
            SystemSetting,

            NeverSleep,

            ManualDefined
        }

        #endregion

        #region Methods

        private void Awake()
        {
            // Set sleep timeout.
            int sleepTimeout;
            switch (this.Setting)
            {
                case AvailableSetting.ManualDefined:
                    sleepTimeout = this.DefinedTimeout;
                    break;
                case AvailableSetting.NeverSleep:
                    sleepTimeout = SleepTimeout.NeverSleep;
                    break;
                default:
                    sleepTimeout = SleepTimeout.SystemSetting;
                    break;
            }

            Screen.sleepTimeout = sleepTimeout;
        }
        
        #endregion
    }
}