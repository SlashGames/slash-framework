// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BuildManager.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Unity.Editor.Common.Build
{
    using System;
    using System.IO;
    using System.Linq;

    using NDesk.Options;

    using UnityEditor;

    using UnityEngine;

    public class BuildManager
    {
        #region Fields

        /// <summary>
        ///   Original editor build settings, stored to restore the settings after a build and not change
        ///   the settings assets.
        /// </summary>
        public readonly BuildSettings EditorBuildSettings = new BuildSettings();

        #endregion

        #region Delegates

        public delegate void PostBuildDelegate();

        public delegate void PreBuildDelegate(BuildSettings buildSettings);

        #endregion

        #region Public Events

        public event PostBuildDelegate PostBuild;

        public event PreBuildDelegate PreBuild;

        #endregion

        #region Public Properties

        public string AndroidKeyaliasPass { get; set; }

        public string AndroidKeystorePass { get; set; }

        public BuildConfiguration BuildConfigurationDebug { get; set; }

        public BuildConfiguration BuildConfigurationEditor { get; set; }

        public BuildConfiguration BuildConfigurationRelease { get; set; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///   Performs the build from the command line.
        ///   Command line options:
        ///   <para>
        ///     buildTarget     Platform to build.
        ///   </para>
        ///   <para>
        ///     buildPath       Path to store the build.
        ///   </para>
        ///   <para>
        ///     buildType       Type of configuration to build.
        ///   </para>
        /// </summary>
        public void PerformBuild()
        {
            string[] args = Environment.GetCommandLineArgs();
            BuildSettings buildSettings = null;
            try
            {
                buildSettings = BuildOptionSet.Parse(args);
            }
            catch (OptionException e)
            {
                Debug.LogError("BuildPlayer.PerformBuild: " + e.Message);
                EditorApplication.Exit(1);
            }

            bool success = this.PerformBuild(buildSettings);
            if (!success)
            {
                EditorApplication.Exit(1);
            }
        }

        /// <summary>
        ///   Performs the build of the specified target to the default file path for the specified build target.
        /// </summary>
        /// <param name="buildTarget">Target to build.</param>
        /// <param name="buildType">Configuration to build.</param>
        /// <returns>True if the build was successful; otherwise, false.</returns>
        public bool PerformBuild(BuildTarget buildTarget, BuildType buildType = BuildType.Release)
        {
            return
                this.PerformBuild(
                    new BuildSettings
                        {
                            BuildTarget = buildTarget,
                            BuildType = buildType,
                            BuildPath = this.EditorBuildSettings.GetDefaultBuildPath(buildTarget)
                        });
        }

        /// <summary>
        ///   Performs the build of the specified target to the specified file path.
        /// </summary>
        /// <returns>True if the build was successful; otherwise, false.</returns>
        public bool PerformBuild(BuildSettings buildSettings)
        {
            Debug.Log(string.Format("Building with settings: {0}", buildSettings));

            this.PrepareBuild(buildSettings);

            // Collect scenes.
            var scenes = UnityEditor.EditorBuildSettings.scenes.Select(scene => scene.path);

            foreach (var scene in scenes)
            {
                Debug.Log("Adding scene to build: " + scene);
            }

            // Make sure build directory exists.
            if (string.IsNullOrEmpty(buildSettings.BuildPath))
            {
                buildSettings.BuildPath = this.EditorBuildSettings.GetDefaultBuildPath(buildSettings.BuildTarget);
            }

            string buildDirectory = Path.GetDirectoryName(buildSettings.BuildPath);
            if (buildDirectory != null)
            {
                Debug.Log(string.Format("Creating directory {0}", buildDirectory));
                Directory.CreateDirectory(buildDirectory);
            }

            // Determine build options.
            BuildOptions buildOptions = BuildOptions.None;
            switch (buildSettings.BuildType)
            {
                case BuildType.Debug:
                    {
                        buildOptions = BuildOptions.Development | BuildOptions.AllowDebugging;
                    }
                    break;
            }

            // Build player.
            string errorMessage = BuildPipeline.BuildPlayer(
                scenes.ToArray(), buildSettings.BuildPath, buildSettings.BuildTarget, buildOptions);

            this.RestoreEditor();

            return string.IsNullOrEmpty(errorMessage);
        }

        public void PrepareBuild(BuildSettings buildSettings)
        {
            this.OnPreBuild(buildSettings);
            this.SetBuildConfiguration(buildSettings);
        }

        /// <summary>
        ///   Restores everything after a build to work again in the editor.
        /// </summary>
        public void RestoreEditor(BuildSettings buildSettings = null)
        {
            if (this.BuildConfigurationEditor != null)
            {
                // Use build configuration of editor.
                PlayerSettings.productName = this.BuildConfigurationEditor.ProductName;
#if UNITY_5_6_OR_NEWER
                PlayerSettings.applicationIdentifier = this.BuildConfigurationEditor.AndroidBundleIdentifier;
#else                
                PlayerSettings.bundleIdentifier = this.BuildConfigurationEditor.AndroidBundleIdentifier;
#endif
            }

            this.SetBuildConfiguration(buildSettings ?? this.EditorBuildSettings);

            this.OnPostBuild();
        }

        #endregion

        #region Methods

        private void OnPostBuild()
        {
            var handler = this.PostBuild;
            if (handler != null)
            {
                handler();
            }
        }

        private void OnPreBuild(BuildSettings buildSettings)
        {
            var handler = this.PreBuild;
            if (handler != null)
            {
                handler(buildSettings);
            }
        }

        private void SetBuildConfiguration(BuildSettings buildSettings)
        {
            BuildConfiguration buildConfiguration = null;
            switch (buildSettings.BuildType)
            {
                case BuildType.Debug:
                    {
                        buildConfiguration = this.BuildConfigurationDebug;
                    }
                    break;
                case BuildType.Release:
                    {
                        buildConfiguration = this.BuildConfigurationRelease;
                    }
                    break;
            }

            if (buildConfiguration != null)
            {
                // Setup build configuration.
                PlayerSettings.productName = buildConfiguration.ProductName;
                PlayerSettings.applicationIdentifier = buildConfiguration.AndroidBundleIdentifier;
            }

            // Setup special settings.
            PlayerSettings.keystorePass = this.AndroidKeystorePass;
            PlayerSettings.keyaliasPass = this.AndroidKeyaliasPass;
            PlayerSettings.bundleVersion = buildSettings.BundleVersion;
            PlayerSettings.Android.bundleVersionCode = buildSettings.Android.BundleVersionCode;
            EditorUserBuildSettings.development = buildSettings.BuildType == BuildType.Debug;

            switch (buildSettings.BuildTarget)
            {
                case BuildTarget.WSAPlayer:
                    {
                        PlayerSettings.productName = this.BuildConfigurationRelease.ProductName;
                    }
                    break;
            }
        }

        #endregion
    }
}