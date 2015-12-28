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

        private static HierarchicalBlueprintManager hierarchicalBlueprintManager;

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
            // Build hierarchical blueprint manager for resolving parents.
            hierarchicalBlueprintManager = new HierarchicalBlueprintManager();

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
                        // Right now, only a single blueprint file is supported. This might change in the future.
                        blueprintManager = (BlueprintManager)blueprintManagerSerializer.Deserialize(blueprintStream);
                        blueprintFileName = Application.dataPath.Substring(
                            0, Application.dataPath.Length - "Assets".Length)
                                            + AssetDatabase.GetAssetPath(blueprintTextAsset);

                        hierarchicalBlueprintManager.AddChild(blueprintManager);
                    }
                    catch (XmlException e)
                    {
                        EditorUtility.DisplayDialog(
                            string.Format("Error reading blueprint file {0}", blueprintAsset.name), e.Message, "Close");
                        return;
                    }
                }
            }

            // Resolve parents of all blueprints.
            BlueprintUtils.ResolveParents(hierarchicalBlueprintManager, hierarchicalBlueprintManager);

            // Load components.
            inspectorTypeTable = InspectorTypeTable.FindInspectorTypes(typeof(IEntityComponent));
        }

        private static void SaveBlueprints()
        {
            var blueprintFile = new FileInfo(blueprintFileName);

            // Make sure directory exists.
            if (blueprintFile.DirectoryName != null && 
                !Directory.Exists(blueprintFile.DirectoryName))
            {
                Directory.CreateDirectory(blueprintFile.DirectoryName);
            }

            using (var streamWriter = blueprintFile.CreateText())
            {
                var blueprintManagerSerializer = new XmlSerializer(typeof(BlueprintManager));
                blueprintManagerSerializer.Serialize(streamWriter, blueprintManager);
            }

            AssetDatabase.Refresh();

            EditorUtility.DisplayDialog(
                "Blueprints Saved", string.Format("Blueprints have been written to {0}.", blueprintFileName), "OK");
        }

        [MenuItem("Slash Games/Blueprints/Blueprint Editor")]
        private static void ShowBlueprintEditor()
        {
            blueprintManager = null;
            blueprintFileName = string.Empty;
            inspectorTypeTable = null;

            // Load blueprints.
            LoadBlueprints();

            if (blueprintManager == null)
            {
                // Creating new blueprints file.
                blueprintFileName = "Assets/Resources/" + BlueprintsFolder + "/Blueprints.xml";
                blueprintManager = new BlueprintManager();
                hierarchicalBlueprintManager.AddChild(blueprintManager);
                EditorUtility.DisplayDialog(
                    "No blueprint file found",
                    string.Format("No blueprints could be found in resources folder {0}. Created new one called {1}.", BlueprintsFolder, blueprintFileName),
                    "Ok");
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
                                this.selectedBlueprint =
                                    hierarchicalBlueprintManager.GetBlueprint(this.selectedBlueprintId);
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

                                // Show parent blueprint components, but don't allow to remove them.
                                foreach (var componentType in this.selectedBlueprint.GetAllComponentTypes().Except(this.selectedBlueprint.ComponentTypes))
                                {
                                    GUILayout.Label(componentType.Name);
                                }

                                // Show blueprint components.
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
                                    inspectorTypeTable.Types().Except(this.selectedBlueprint.GetAllComponentTypes()))
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

                        EditorGUIUtils.BlueprintComponentsField(this.selectedBlueprint, this.selectedBlueprint.AttributeTable, inspectorTypeTable, hierarchicalBlueprintManager);
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