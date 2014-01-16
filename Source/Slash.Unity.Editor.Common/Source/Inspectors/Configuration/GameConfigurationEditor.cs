// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GameConfigurationEditor.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Unity.Editor.Common.Inspectors.Configuration
{
    using System;
    using System.Collections.Generic;

    using Slash.Collections.AttributeTables;
    using Slash.GameBase.Inspector.Attributes;
    using Slash.GameBase.Inspector.Utils;
    using Slash.GameBase.Systems;
    using Slash.Reflection.Utils;
    using Slash.Unity.Common.Configuration;

    using UnityEditor;

    using UnityEngine;

    [CustomEditor(typeof(GameConfigurationBehaviour))]
    public class GameConfigurationEditor : Editor
    {
        #region Fields

        private GameConfigurationBehaviour gameConfiguration;

        private IEnumerable<Type> inspectorTypes;

        #endregion

        #region Public Methods and Operators

        public override void OnInspectorGUI()
        {
            // Text asset which contains configuration.
            this.gameConfiguration.ConfigurationFilePath = EditorGUILayout.TextField(
                "Configuration File", this.gameConfiguration.ConfigurationFilePath);

            // Collect system types.
            if (this.inspectorTypes == null)
            {
                this.inspectorTypes = ReflectionUtils.FindTypesWithAttribute<InspectorTypeAttribute>();
            }
            Type systemType = typeof(ISystem);
            foreach (var inspectorType in this.inspectorTypes)
            {
                if (!systemType.IsAssignableFrom(inspectorType))
                {
                    continue;
                }

                // Draw inspector type.
                this.DrawInspector(inspectorType);
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
            }
        }

        #endregion

        #region Methods

        private void DrawInspector(Type inspectorType)
        {
            // Get inspector properties.
            Dictionary<InspectorPropertyAttribute, InspectorConditionalPropertyAttribute> conditionalInspectors =
                new Dictionary<InspectorPropertyAttribute, InspectorConditionalPropertyAttribute>();
            List<InspectorPropertyAttribute> inspectorProperties =
                InspectorUtils.CollectInspectorProperties(inspectorType, ref conditionalInspectors);

            IAttributeTable configuration = this.gameConfiguration.Configuration;

            foreach (var inspectorProperty in inspectorProperties)
            {
                // Get current value.
                object currentValue = configuration.GetValueOrDefault(inspectorProperty.Name, inspectorProperty.Default);

                // Draw inspector property.
                object newValue = this.DrawInspectorProperty(inspectorProperty, currentValue);

                // Set new value if changed.
                if (!Equals(newValue, currentValue))
                {
                    configuration.SetValue(inspectorProperty.Name, newValue);
                }
            }
        }

        private object DrawInspectorProperty(InspectorPropertyAttribute inspectorProperty, object currentValue)
        {
            // Draw inspector control.
            GUIContent label = new GUIContent(inspectorProperty.Name, inspectorProperty.Description);
            if (inspectorProperty is InspectorBoolAttribute)
            {
                return EditorGUILayout.Toggle(label, Convert.ToBoolean(currentValue));
            }
            if (inspectorProperty is InspectorStringAttribute)
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
                    label, (Enum)Convert.ChangeType(currentValue, enumInspectorProperty.EnumType));
            }

            EditorGUILayout.HelpBox(
                string.Format("No inspector found for property type '{0}'.", inspectorProperty.GetType().Name),
                MessageType.Warning);
            return currentValue;
        }

        private void OnEnable()
        {
            this.gameConfiguration = (GameConfigurationBehaviour)this.target;
        }

        #endregion
    }
}