// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BuildSettingsAndroid.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Unity.Editor.Common.MenuItems.Build
{
    using UnityEditor;

    /// <summary>
    ///   Build settings specific for Android build.
    /// </summary>
    public class BuildSettingsAndroid
    {
        #region Constructors and Destructors

        /// <summary>
        ///   Constructor.
        /// </summary>
        public BuildSettingsAndroid()
        {
            this.BundleVersionCode = PlayerSettings.Android.bundleVersionCode;
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///   Bundle version code of android package.
        /// </summary>
        public int BundleVersionCode { get; set; }

        #endregion

        #region Public Methods and Operators

        public override string ToString()
        {
            return string.Format("BundleVersionCode: {0}", this.BundleVersionCode);
        }

        #endregion
    }
}