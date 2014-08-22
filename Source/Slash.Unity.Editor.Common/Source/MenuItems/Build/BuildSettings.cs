// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BuildSettings.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Unity.Editor.Common.Build
{
    using UnityEditor;

    /// <summary>
    ///   General build settings.
    /// </summary>
    public class BuildSettings
    {
        #region Constructors and Destructors

        /// <summary>
        ///   Constructor.
        /// </summary>
        public BuildSettings()
        {
            this.Android = new BuildSettingsAndroid();
            this.BundleVersion = PlayerSettings.bundleVersion;
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///   Android specific build settings.
        /// </summary>
        public BuildSettingsAndroid Android { get; private set; }

        /// <summary>
        ///   Path to package/executable/...
        /// </summary>
        public string BuildPath { get; set; }

        /// <summary>
        ///   Target to build.
        /// </summary>
        public BuildTarget BuildTarget { get; set; }

        /// <summary>
        ///   Configuration to build.
        /// </summary>
        public BuildType BuildType { get; set; }

        /// <summary>
        ///   Bundle version.
        ///   Version number of application which is shown to the user.
        /// </summary>
        public string BundleVersion { get; set; }

        #endregion

        #region Public Methods and Operators

        public override string ToString()
        {
            return string.Format(
                "Android: {0}, BuildPath: {1}, BuildTarget: {2}, BuildType: {3}, BundleVersion: {4}",
                this.Android,
                this.BuildPath,
                this.BuildTarget,
                this.BuildType,
                this.BundleVersion);
        }

        #endregion
    }
}