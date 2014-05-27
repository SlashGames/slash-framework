// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GameConfigurationEditor.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Unity.Editor.Common.Inspectors.Configuration
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    using Slash.Collections.AttributeTables;
    using Slash.Collections.Utils;
    using Slash.GameBase.Inspector.Attributes;
    using Slash.GameBase.Inspector.Data;
    using Slash.GameBase.Systems;
    using Slash.Unity.Common.Configuration;
    using Slash.Unity.Editor.Common.Inspectors.Utils;

    using UnityEditor;

    using UnityEngine;

    [CustomEditor(typeof(GameConfigurationBehaviour))]
    public class GameConfigurationEditor : Editor
    {
        #region Fields

        private readonly Dictionary<object, bool> foldoutProperty = new Dictionary<object, bool>();

        private GameConfigurationBehaviour gameConfiguration;

        private InspectorTypeTable inspectorSystemTypes;

        #endregion

        #region Public Methods and Operators

        public override void OnInspectorGUI()
        {
            this.DrawDefaultInspector();

            // Collect system types.
            if (this.inspectorSystemTypes == null)
            {
                this.inspectorSystemTypes = InspectorTypeTable.FindInspectorTypes(typeof(ISystem));
            }

            IAttributeTable configuration = this.gameConfiguration.Configuration;
            foreach (var inspectorType in this.inspectorSystemTypes)
            {
                // Draw inspector type.
                this.DrawInspector(inspectorType, configuration);
            }

            if (GUILayout.Button("Reload"))
            {
                // Reload configuration.
                this.gameConfiguration.Load();
            }

            if (GUILayout.Button("Save"))
            {
                // Save configuration.
                // TODO(co): Save automatically if changed.
                this.gameConfiguration.Save();

                // Refresh assets.
                AssetDatabase.Refresh();
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

                if (inspectorProperty.IsList)
                {
                    // Build array.
                    IList currentList = currentValue as IList;
                    InspectorPropertyAttribute localInspectorProperty = inspectorProperty;
                    IList newList;
                    this.foldoutProperty[inspectorProperty] =
                        EditorGUIUtils.ArrayField(
                            this.foldoutProperty.GetValueOrDefault(inspectorProperty, false),
                            new GUIContent(inspectorProperty.Name),
                            currentList,
                            count =>
                                {
                                    IList list = localInspectorProperty.GetEmptyList();
                                    for (int idx = 0; idx < count; idx++)
                                    {
                                        list.Add(null);
                                    }
                                    return list;
                                },
                            (obj, index) =>
                            this.DrawInspectorProperty(localInspectorProperty, new GUIContent("Item " + index), obj),
                            out newList);

                    // Set new value if changed.
                    if (!Equals(newList, currentList))
                    {
                        configuration.SetValue(inspectorProperty.Name, newList);
                    }
                }
                else
                {
                    // Draw inspector property.
                    object newValue = this.DrawInspectorProperty(inspectorProperty, currentValue);

                    // Set new value if changed.
                    if (!Equals(newValue, currentValue))
                    {
                        configuration.SetValue(inspectorProperty.Name, newValue);
                    }
                }
            }
        }

        private object DrawInspectorProperty(
            InspectorPropertyAttribute inspectorProperty, GUIContent label, object currentValue)
        {
            // Draw inspector control.
            if (inspectorProperty is InspectorBoolAttribute)
            {
                return EditorGUILayout.Toggle(label, Convert.ToBoolean(currentValue));
            }
            if (inspectorProperty is InspectorStringAttribute || inspectorProperty is InspectorBlueprintAttribute)
            {
                return EditorGUILayout.TextField(label, Convert.ToString(currentValue));
            }
            if (inspectorProperty is InspectorFloatAttribute)
            {
                return EditorGUILayout.FloatField(label, Convert.ToSingle(currentValue));
            }
            if (inspectorProperty is InspectorIntAttribute)
            {
                return EditorGUILayout.IntField(label, Convert.ToInt32(currentValue));
            }
            InspectorEnumAttribute enumInspectorProperty = inspectorProperty as InspectorEnumAttribute;
            if (enumInspectorProperty != null)
            {
                return EditorGUILayout.EnumPopup(
                    label, (Enum)Convert.ChangeType(currentValue, enumInspectorProperty.PropertyType));
            }

            EditorGUILayout.HelpBox(
                string.Format("No inspector found for property type '{0}'.", inspectorProperty.GetType().Name),
                MessageType.Warning);
            return currentValue;
        }

        private object DrawInspectorProperty(InspectorPropertyAttribute inspectorProperty, object currentValue)
        {
            return this.DrawInspectorProperty(
                inspectorProperty, new GUIContent(inspectorProperty.Name, inspectorProperty.Description), currentValue);
        }

        private void OnEnable()
        {
            this.gameConfiguration = (GameConfigurationBehaviour)this.target;
        }

        #endregion
    }
}