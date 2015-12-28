// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BuildOptionSet.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Unity.Editor.Common.Build
{
    using System;

    using NDesk.Options;

    using UnityEditor;

    /// <summary>
    ///   Capsules the command line options from which the build settings can be adjusted.
    /// </summary>
    public static class BuildOptionSet
    {
        #region Static Fields

        private static readonly OptionSet OptionSet = new OptionSet
            {
                {
                    "buildpath|buildPath=", "Path to store the build.", path =>
                        {
                            if (path == null)
                            {
                                throw new OptionException(
                                    "Please provide a build path by providing a command line argument 'buildpath=path/to/build'.",
                                    "buildpath");
                            }
                            optionBuildPath = path;
                        }
                },
                {
                    "buildtarget|buildTarget=", "Platform to build.", buildTarget =>
                        {
                            if (buildTarget == null)
                            {
                                throw new OptionException(
                                    "Please provide a build target (see UnityEditor.BuildTarget enum for valid values).",
                                    "buildtarget");
                            }
                            try
                            {
                                optionBuildTarget = (BuildTarget)Enum.Parse(typeof(BuildTarget), buildTarget);
                            }
                            catch (ArgumentException e)
                            {
                                throw new OptionException(
                                    String.Format(
                                        "'{0}' is no valid build target (see UnityEditor.BuildTarget enum for valid values).",
                                        buildTarget),
                                    "buildtarget",
                                    e);
                            }
                        }
                },
                {
                    "buildtype|buildType:", "Configuration to build (Debug|Release). Default: Release.", buildType =>
                        {
                            try
                            {
                                optionBuildType = (BuildType)Enum.Parse(typeof(BuildType), buildType);
                            }
                            catch (ArgumentException e)
                            {
                                throw new OptionException(
                                    String.Format("'{0}' is no valid build type (Debug|Release).", buildType), "buildtype", e);
                            }
                        }
                },
                {
                    "android.bundleversioncode|android.bundleVersionCode:", "Unique build version code (1-999999)",
                    (int bundleVersionCode) => optionAndroidBundleVersionCode = bundleVersionCode
                },
                {
                    "bundleversion|bundleVersion:", "Version number which is shown to the user.",
                    bundleVersion => optionBundleVersion = bundleVersion
                }
            };

        /// <summary>
        ///   Unique bundle version code (1-999999).
        /// </summary>
        private static int? optionAndroidBundleVersionCode;

        private static string optionBuildPath;

        private static BuildTarget optionBuildTarget;

        /// <summary>
        ///   Type of configuration to build.
        /// </summary>
        private static BuildType optionBuildType = BuildType.Release;

        /// <summary>
        ///   Version number which is shown to the user.
        /// </summary>
        private static string optionBundleVersion;

        #endregion

        #region Public Methods and Operators

        public static BuildSettings Parse(string[] args)
        {
            OptionSet.Parse(args);

            BuildSettings buildSettings = new BuildSettings
                {
                    BuildTarget = optionBuildTarget,
                    BuildType = optionBuildType,
                    BuildPath = optionBuildPath
                };
            if (optionAndroidBundleVersionCode.HasValue)
            {
                buildSettings.Android.BundleVersionCode = optionAndroidBundleVersionCode.Value;
            }
            if (optionBundleVersion != null)
            {
                buildSettings.BundleVersion = optionBundleVersion;
            }

            return buildSettings;
        }

        #endregion
    }
}