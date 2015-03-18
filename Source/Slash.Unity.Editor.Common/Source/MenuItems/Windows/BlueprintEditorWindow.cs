// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BlueprintEditorWindow.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Unity.Editor.Common.MenuItems.Windows
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Xml;
    using System.Xml.Serialization;

    using Slash.ECS.Blueprints;
    using Slash.ECS.Components;
    using Slash.ECS.Inspector.Data;
    using Slash.Unity.Editor.Common.Inspectors.Utils;

    using UnityEditor;

    using UnityEngine;

    /// <summary>
    ///   Blueprint editor for creating blueprints, adding components and modifying
    ///   attributes.
    /// </summary>
    public class BlueprintEditorWindow : EditorWindow
    {
        #region Constants

        private const string BlueprintsFolder = "Blueprints";

        #endregion

        #region Static Fields

        private static string blueprintFileName;

        private static BlueprintManager blueprintManager;

        private static InspectorTypeTable inspectorTypeTable;

        #endregion

        #region Fields

        private Vector2 blueprintDetailsScrollPosition = Vector2.zero;

        private Vector2 blueprintListScrollPosition = Vector2.zero;

        private string newBlueprintId = string.Empty;

        private Blueprint selectedBlueprint;

        private string selectedBlueprintId = string.Empty;

        #endregion

        #region Methods

        private static void LoadBlueprints()
        {
            var blueprintAssets = Resources.LoadAll(BlueprintsFolder, typeof(TextAsset));

            foreach (var blueprintAsset in blueprintAssets)
            {
                var blueprintTextAsset = blueprintAsset as TextAsset;

                if (blueprintTextAsset != null)
                {
                    var blueprintStream = new MemoryStream(blueprintTextAsset.bytes);

                    // Load blueprints.
                    var blueprintManagerSerializer = new XmlSerializer(typeof(BlueprintManager));

                    try
                    {
                        blueprintManager = (BlueprintManager)blueprintManagerSerializer.Deserialize(blueprintStream);
                        blueprintFileName = Application.dataPath.Substring(
                            0, Application.dataPath.Length - "Assets".Length)
                                            + AssetDatabase.GetAssetPath(blueprintTextAsset);
                    }
                    catch (XmlException e)
                    {
                        EditorUtility.DisplayDialog(
                            string.Format("Error reading blueprint file {0}", blueprintAsset.name), e.Message, "Close");
                        return;
                    }

                    // Load components.
                    inspectorTypeTable = InspectorTypeTable.FindInspectorTypes(typeof(IEntityComponent));
                }
            }
        }

        private static void SaveBlueprints()
        {
            var blueprintFile = new FileInfo(blueprintFileName);

            using (var fileStream = blueprintFile.Open(FileMode.Create, FileAccess.ReadWrite))
            {
                var blueprintManagerSerializer = new XmlSerializer(typeof(BlueprintManager));
                blueprintManagerSerializer.Serialize(fileStream, blueprintManager);
            }

            AssetDatabase.Refresh();

            EditorUtility.DisplayDialog(
                "Blueprints Saved", string.Format("Blueprints have been written to {0}.", blueprintFileName), "OK");
        }

        [MenuItem("Slash Games/Windows/Blueprint Editor")]
        private static void ShowBlueprintEditor()
        {
            blueprintManager = null;
            blueprintFileName = string.Empty;
            inspectorTypeTable = null;

            // Load blueprints.
            LoadBlueprints();

            if (blueprintManager == null)
            {
                EditorUtility.DisplayDialog(
                    "No blueprint file found",
                    string.Format("No blueprints could be found in resources folder {0}.", BlueprintsFolder),
                    "Close");
                return;
            }

            GetWindow(typeof(BlueprintEditorWindow), true, "Blueprint Editor");
        }

        private bool GUILayoutButtonAdd()
        {
            return GUILayout.Button("+", GUILayout.Width(30f));
        }

        private bool GUILayoutButtonRemove()
        {
            return GUILayout.Button("-", GUILayout.Width(30f));
        }

        private void OnDestroy()
        {
            if (EditorUtility.DisplayDialog("Close Blueprint Editor", "Save Changes?", "Yes", "No"))
            {
                SaveBlueprints();
            }
        }

        private void OnGUI()
        {
            // Reload blueprints if missing.
            if (blueprintManager == null)
            {
                // Load blueprints.
                LoadBlueprints();
            }

            string removedBlueprintId = null;
            Type removedComponentType = null;

            // Summary.
            GUILayout.Label("Summary", EditorStyles.boldLabel);
            EditorGUILayout.LabelField("Blueprint file:", blueprintFileName);
            EditorGUILayout.LabelField("Selected blueprint:", this.selectedBlueprintId);

            if (GUILayout.Button("Save Blueprints"))
            {
                SaveBlueprints();
            }

            EditorGUILayout.BeginHorizontal();
            {
                this.blueprintListScrollPosition = EditorGUILayout.BeginScrollView(
                    this.blueprintListScrollPosition, false, true);
                EditorGUILayout.BeginVertical(GUILayout.Width(200f));
                {
                    // List blueprints.
                    GUILayout.Label("Blueprints", EditorStyles.boldLabel);

                    foreach (var namedBlueprint in blueprintManager.Blueprints)
                    {
                        EditorGUILayout.BeginHorizontal();
                        {
                            // Remove blueprint button.
                            if (this.GUILayoutButtonRemove())
                            {
                                removedBlueprintId = namedBlueprint.Key;
                            }

                            // Select blueprint button.
                            if (GUILayout.Button(namedBlueprint.Key))
                            {
                                this.selectedBlueprintId = namedBlueprint.Key;
                                this.selectedBlueprint = namedBlueprint.Value;
                            }
                        }
                        EditorGUILayout.EndHorizontal();
                    }

                    // Add blueprint button.
                    EditorGUILayout.BeginHorizontal();
                    {
                        this.newBlueprintId = EditorGUILayout.TextField("Add blueprint:", this.newBlueprintId);

                        if (this.GUILayoutButtonAdd())
                        {
                            try
                            {
                                var blueprint = new Blueprint();
                                blueprintManager.AddBlueprint(this.newBlueprintId, blueprint);
                            }
                            catch (ArgumentException e)
                            {
                                EditorUtility.DisplayDialog("Error", e.Message, "Close");
                            }
                        }
                    }
                    EditorGUILayout.EndHorizontal();
                }
                EditorGUILayout.EndVertical();
                EditorGUILayout.EndScrollView();

                this.blueprintDetailsScrollPosition =
                    EditorGUILayout.BeginScrollView(this.blueprintDetailsScrollPosition, false, true);
                EditorGUILayout.BeginVertical();
                {
                    if (this.selectedBlueprint != null)
                    {
                        // Components.
                        GUILayout.Label("Components", EditorStyles.boldLabel);

                        EditorGUILayout.BeginHorizontal();
                        {
                            // Current components.
                            EditorGUILayout.BeginVertical();
                            {
                                GUILayout.Label("Current", EditorStyles.boldLabel);

                                foreach (var componentType in this.selectedBlueprint.ComponentTypes)
                                {
                                    EditorGUILayout.BeginHorizontal();
                                    {
                                        // Remove component button.
                                        if (this.GUILayoutButtonRemove())
                                        {
                                            removedComponentType = componentType;
                                        }

                                        GUILayout.Label(componentType.Name);
                                    }
                                    EditorGUILayout.EndHorizontal();
                                }
                            }
                            EditorGUILayout.EndVertical();

                            // New components.
                            EditorGUILayout.BeginVertical();
                            {
                                GUILayout.Label("Other", EditorStyles.boldLabel);

                                foreach (var componentType in
                                    inspectorTypeTable.Types().Except(this.selectedBlueprint.ComponentTypes))
                                {
                                    EditorGUILayout.BeginHorizontal();
                                    {
                                        // Add component button.
                                        if (this.GUILayoutButtonAdd())
                                        {
                                            this.selectedBlueprint.ComponentTypes.Add(componentType);
                                        }

                                        GUILayout.Label(componentType.Name);
                                    }
                                    EditorGUILayout.EndHorizontal();
                                }
                            }
                            EditorGUILayout.EndVertical();
                        }
                        EditorGUILayout.EndHorizontal();

                        // Attributes.
                        GUILayout.Label("Attributes", EditorStyles.boldLabel);

                        EditorGUIUtils.BlueprintComponentsField(this.selectedBlueprint, this.selectedBlueprint.AttributeTable, inspectorTypeTable, blueprintManager);
                    }
                }
                EditorGUILayout.EndVertical();
                EditorGUILayout.EndScrollView();
            }
            EditorGUILayout.EndHorizontal();

            // Remove data, if user wants to.
            if (removedBlueprintId != null)
            {
                blueprintManager.RemoveBlueprint(removedBlueprintId);

                this.selectedBlueprint = null;
                this.selectedBlueprintId = string.Empty;
            }

            if (this.selectedBlueprint != null && removedComponentType != null)
            {
                this.selectedBlueprint.ComponentTypes.Remove(removedComponentType);
            }
        }

        #endregion
    }
}