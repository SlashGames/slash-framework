// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EntityConfigurationEditor.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Unity.Editor.Common.Inspectors.ECS
{
    using System;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Xml.Serialization;

    using Slash.Collections.AttributeTables;
    using Slash.ECS.Blueprints;
    using Slash.ECS.Components;
    using Slash.ECS.Inspector.Data;
    using Slash.Unity.Common.ECS;
    using Slash.Unity.Editor.Common.Inspectors.Utils;

    using UnityEditor;

    using UnityEngine;

    /// <summary>
    ///   Custom inspector for entity configurations.
    /// </summary>
    [CustomEditor(typeof(EntityConfigurationBehaviour))]
    public class EntityConfigurationEditor : Editor
    {
        #region Constants

        /// <summary>
        ///   Extension of the files containing game blueprints.
        /// </summary>
        private const string BlueprintFileExtension = ".blueprints.xml";

        #endregion

        #region Static Fields

        /// <summary>
        ///   Ids of all available blueprints.
        /// </summary>
        private static string[] blueprintIds;

        /// <summary>
        ///   Blueprint manager holding all available blueprints.
        /// </summary>
        private static HierarchicalBlueprintManager hierarchicalBlueprintManager;

        /// <summary>
        ///   Inspector data of all entity components.
        /// </summary>
        private static InspectorTypeTable inspectorComponentTypes;

        #endregion

        #region Fields

        /// <summary>
        ///   Target entity configuration.
        /// </summary>
        private EntityConfigurationBehaviour entityConfigurationBehaviour;

        /// <summary>
        ///   Current blueprint of the target entity configuration.
        /// </summary>
        private Blueprint selectedBlueprint;

        /// <summary>
        ///   Index of the current blueprint of the target entity configuration.
        /// </summary>
        private int selectedBlueprintIndex;

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///   Draws the entity configuration inspector.
        /// </summary>
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            this.entityConfigurationBehaviour = (EntityConfigurationBehaviour)this.target;

            if (EditorApplication.isPlaying)
            {
                // Show entity data.
                EditorGUILayout.LabelField(
                    new GUIContent("Entity Id"),
                    new GUIContent(this.entityConfigurationBehaviour.EntityId.ToString(CultureInfo.InvariantCulture)));
                EditorGUILayout.LabelField(
                    new GUIContent("Blueprint Id"), new GUIContent(this.entityConfigurationBehaviour.BlueprintId));
            }
            else
            {
                if (blueprintIds == null)
                {
                    // Load project blueprint data.
                    this.LoadBlueprints();

                    // Get the index of the blueprint of the target entity.
                    this.selectedBlueprintIndex = Array.IndexOf(
                        blueprintIds, this.entityConfigurationBehaviour.BlueprintId);

                    // Find the blueprint of the target entity.
                    this.OnSelectedBlueprintChanged();
                }

                // Show blueprint dropdown.
                var oldSelectedBlueprintIndex = Array.IndexOf(
                    blueprintIds, this.entityConfigurationBehaviour.BlueprintId);
                this.selectedBlueprintIndex = EditorGUILayout.Popup(
                    "Blueprint", oldSelectedBlueprintIndex, blueprintIds);
                if (this.selectedBlueprintIndex != oldSelectedBlueprintIndex)
                {
                    this.OnSelectedBlueprintChanged();
                }

                if (this.entityConfigurationBehaviour.Configuration == null)
                {
                    // Create initial configuration.
                    this.entityConfigurationBehaviour.Configuration = new AttributeTable();
                }

                if (this.selectedBlueprint == null)
                {
                    // Find the blueprint of the target entity.
                    this.OnSelectedBlueprintChanged();
                }

                foreach (var componentType in this.selectedBlueprint.ComponentTypes)
                {
                    var inspectorType = inspectorComponentTypes[componentType];

                    // Draw inspector.
                    this.DrawInspector(inspectorType, this.entityConfigurationBehaviour.Configuration);
                }

                if (GUILayout.Button("Reload Blueprints"))
                {
                    this.LoadBlueprints();
                }
            }
        }

        #endregion

        #region Methods

        private void DrawInspector(InspectorType inspectorType, IAttributeTable configuration)
        {
            foreach (var inspectorProperty in inspectorType.Properties)
            {
                // Get current value.
                object currentValue = configuration.GetValueOrDefault(inspectorProperty.Name, inspectorProperty.Default);

                // Draw inspector property.
                object newValue = EditorGUIUtils.LogicInspectorPropertyField(inspectorProperty, currentValue);

                // Set new value if changed.
                if (!Equals(newValue, currentValue))
                {
                    configuration.SetValue(inspectorProperty.Name, newValue);
                }
            }
        }

        private void LoadBlueprints()
        {
            inspectorComponentTypes = InspectorTypeTable.FindInspectorTypes(typeof(IEntityComponent));

            // Search all directories for blueprint files.
            DirectoryInfo directory = new DirectoryInfo(Application.dataPath);
            FileInfo[] blueprintFiles = directory.GetFiles("*" + BlueprintFileExtension, SearchOption.AllDirectories);

            if (blueprintFiles.Length == 0)
            {
                Debug.LogError(string.Format("No blueprint file ({0}) found!", BlueprintFileExtension));
            }
            else
            {
                // Create new hiearchical blueprint manager.
                hierarchicalBlueprintManager = new HierarchicalBlueprintManager();
                var blueprintManagerSerializer = new XmlSerializer(typeof(BlueprintManager));
                var filesProcessed = 0;

                foreach (var blueprintFile in blueprintFiles)
                {
                    // Create new blueprint manager.
                    using (var blueprintFileStream = blueprintFile.OpenRead())
                    {
                        try
                        {
                            // Try to read the blueprint data.
                            var blueprintManager =
                                (BlueprintManager)blueprintManagerSerializer.Deserialize(blueprintFileStream);

                            if (blueprintManager != null)
                            {
                                hierarchicalBlueprintManager.AddChild(blueprintManager);
                                ++filesProcessed;
                            }
                        }
                        catch (InvalidOperationException)
                        {
                            Debug.LogError(blueprintFile.Name + " is no blueprint file.");
                        }
                    }
                }

                // Store all blueprint ids for access from a pulldown menu.
                blueprintIds = hierarchicalBlueprintManager.Blueprints.Select(blueprint => blueprint.Key).ToArray();

                Debug.Log(
                    string.Format(
                        "Loaded {0} blueprint(s) from {1} blueprint file(s).",
                        hierarchicalBlueprintManager.Blueprints.Count(),
                        filesProcessed));
            }
        }

        private void OnSelectedBlueprintChanged()
        {
            // Update selected blueprint of the target entity.
            var selectedBlueprintName = blueprintIds[this.selectedBlueprintIndex >= 0 ? this.selectedBlueprintIndex : 0];
            this.entityConfigurationBehaviour.BlueprintId = selectedBlueprintName;
            this.selectedBlueprint = hierarchicalBlueprintManager.GetBlueprint(selectedBlueprintName);
        }

        #endregion
    }
}