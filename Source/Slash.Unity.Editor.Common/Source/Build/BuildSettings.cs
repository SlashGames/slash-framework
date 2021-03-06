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
        /// <summary>
        ///   Constructor.
        /// </summary>
        public BuildSettings()
        {
            this.Android = new BuildSettingsAndroid();
            this.BundleVersion = PlayerSettings.bundleVersion;
            this.ProjectName = PlayerSettings.productName;
            this.DefaultBuildFolder = "../../Build";
        }

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
        ///   Target group to build.
        /// </summary>
        public BuildTargetGroup BuildTargetGroup { get; set; }

        /// <summary>
        ///   Configuration to build.
        /// </summary>
        public BuildType BuildType { get; set; }

        /// <summary>
        ///   Bundle version.
        ///   Version number of application which is shown to the user.
        /// </summary>
        public string BundleVersion { get; set; }

        /// <summary>
        ///   Default folder to store build at.
        /// </summary>
        public string DefaultBuildFolder { get; set; }

        /// <summary>
        ///   Project name.
        /// </summary>
        public string ProjectName { get; set; }

        /// <summary>
        ///   Returns the default build path for the specified build target.
        /// </summary>
        /// <param name="buildTarget">Build target to get path for.</param>
        /// <returns>Path to build package/executable/...</returns>
        public string GetDefaultBuildPath(BuildTarget buildTarget)
        {
            var fileExtension = "";
            switch (buildTarget)
            {
                case BuildTarget.Android:
                {
                    fileExtension = "apk";
                }
                    break;
                case BuildTarget.StandaloneWindows:
                {
                    fileExtension = "exe";
                }
                    break;
            }

            return string.Format(
                "{0}/{1}/{2}.{3}",
                this.DefaultBuildFolder,
                buildTarget,
                this.ProjectName,
                fileExtension);
        }

        public override string ToString()
        {
            return
                string.Format(
                    "Android: {0}, BuildPath: {1}, BuildTarget: {2}, BuildTargetGroup: {3}, BuildType: {4}, BundleVersion: {5}, DefaultBuildFolder: {6}, ProjectName: {7}",
                    this.Android,
                    this.BuildPath,
                    this.BuildTarget,
                    this.BuildTargetGroup,
                    this.BuildType,
                    this.BundleVersion,
                    this.DefaultBuildFolder,
                    this.ProjectName);
        }
    }
}