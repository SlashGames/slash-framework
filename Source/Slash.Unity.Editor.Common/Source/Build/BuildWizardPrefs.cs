// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BuildWizardPrefs.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Unity.Editor.Common.Build
{
    using UnityEditor;

    public class BuildWizardPrefs
    {
        #region Constants

        private const string ConfigValuePrefix = "BuildWizardPrefs/";

        #endregion

        #region Public Properties

        public static BuildSettings BuildSettings
        {
            get
            {
                return LoadBuildSettings();
            }
            set
            {
                SaveBuildSettings(value);
            }
        }

        #endregion

        #region Properties

        private static BuildTarget BuildTarget
        {
            get
            {
                return
                    (BuildTarget)
                    EditorPrefs.GetInt(ConfigValuePrefix + "BuildTarget", (int)BuildTarget.StandaloneWindows);
            }
            set
            {
                EditorPrefs.SetInt(ConfigValuePrefix + "BuildTarget", (int)value);
            }
        }

        private static BuildType BuildType
        {
            get
            {
                return (BuildType)EditorPrefs.GetInt(ConfigValuePrefix + "BuildType", (int)BuildType.Release);
            }
            set
            {
                EditorPrefs.SetInt(ConfigValuePrefix + "BuildType", (int)value);
            }
        }

        private static string BundleVersion
        {
            get
            {
                return EditorPrefs.GetString(ConfigValuePrefix + "BundleVersion", PlayerSettings.bundleVersion);
            }
            set
            {
                EditorPrefs.SetString(ConfigValuePrefix + "BundleVersion", value);
            }
        }

        #endregion

        #region Methods

        private static BuildSettings LoadBuildSettings()
        {
            BuildSettings buildSettings = new BuildSettings
                {
                    BuildTarget = BuildTarget,
                    BuildType = BuildType,
                    BundleVersion = BundleVersion
                };
            return buildSettings;
        }

        private static void SaveBuildSettings(BuildSettings buildSettings)
        {
            BuildTarget = buildSettings.BuildTarget;
            BuildType = buildSettings.BuildType;
            BundleVersion = buildSettings.BundleVersion;
        }

        #endregion
    }
}