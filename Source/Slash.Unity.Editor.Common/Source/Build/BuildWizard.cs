// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BuildWizard.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Unity.Editor.Common.Build
{
    using UnityEditor;

    using UnityEditorInternal;

    using UnityEngine;

    public class BuildWizard : EditorWindow
    {
        #region Static Fields

        /// <summary>
        ///   Build settings before the editor was prepared for a build.
        /// </summary>
        private static BuildSettings previousBuildSettings;

        #endregion

        #region Fields

        private readonly BuildSettings buildSettings = BuildWizardPrefs.BuildSettings;

        #endregion

        #region Public Methods and Operators

        [MenuItem("Slash Games/Build/Wizard")]
        public static void OpenBuildWizard()
        {
            GetWindow<BuildWizard>("Build Wizard");
        }

        public void OnGUI()
        {
            BuildManager buildManager = new BuildManager();

            EditorGUI.BeginChangeCheck();

            GUILayout.Label("Settings", EditorStyles.boldLabel);

            this.buildSettings.BuildTarget =
                (BuildTarget)EditorGUILayout.EnumPopup("Build Target", this.buildSettings.BuildTarget);
            this.buildSettings.BuildType =
                (BuildType)EditorGUILayout.EnumPopup("Build Type", this.buildSettings.BuildType);
            this.buildSettings.BundleVersion =
                EditorGUILayout.TextField(
                    new GUIContent("Bundle Version", "Version number which is displayed to user."),
                    this.buildSettings.BundleVersion);

            // Additional settings depending on build target.
            if (this.buildSettings.BuildTarget == BuildTarget.Android)
            {
                EditorGUILayout.Separator();

                GUILayout.Label("Android");
                EditorGUI.indentLevel++;

                this.buildSettings.Android.BundleVersionCode = EditorGUILayout.IntField(
                    "Bundle Version Code", this.buildSettings.Android.BundleVersionCode);

                EditorGUI.indentLevel--;
            }

            if (EditorGUI.EndChangeCheck())
            {
                // Save build settings.
                BuildWizardPrefs.BuildSettings = this.buildSettings;
            }

            EditorGUILayout.Separator();

            GUILayout.Label("Build", EditorStyles.boldLabel);

            if (InternalEditorUtility.HasPro())
            {
                if (GUILayout.Button("Build"))
                {
                    buildManager.PerformBuild(this.buildSettings);
                }

                GUILayout.Label("Manual Build", EditorStyles.boldLabel);
            }

            if (GUILayout.Button(new GUIContent("Prepare", "Prepares everything for the build.")))
            {
                // Store build settings for restore.
                if (previousBuildSettings == null)
                {
                    previousBuildSettings = new BuildSettings();
                }

                // Prepare build.
                EditorUserBuildSettings.SwitchActiveBuildTarget(this.buildSettings.BuildTarget);
                buildManager.PrepareBuild(this.buildSettings);

                Debug.Log("Build prepared for settings: " + this.buildSettings);
            }

            EditorGUILayout.HelpBox(
                "After 'Prepare' you have to perform the build by yourself via File/Build. Use 'Restore' afterwards.",
                MessageType.Info);

            if (
                GUILayout.Button(
                    new GUIContent("Restore", "Restores everything after a build to work again in the editor.")))
            {
                // Restore editor.
                buildManager.RestoreEditor(previousBuildSettings ?? new BuildSettings());

                Debug.Log("Build restored for editor use.");
            }
        }

        #endregion
    }
}