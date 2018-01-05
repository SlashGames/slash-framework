// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EventManagerLogBehaviourEditor.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Unity.Editor.Common.Inspectors.Logging
{
    using System;
    using System.Collections.Generic;

    using Slash.Unity.Common.Logging;

    using UnityEditor;

    using UnityEngine;

    [CustomEditor(typeof(EventManagerLogBehaviour))]
    public abstract class EventManagerLogBehaviourEditor : Editor
    {
        #region Fields

        /// <summary>
        ///   Current inspected behaviour.
        /// </summary>
        private EventManagerLogBehaviour instance;

        #endregion

        #region Properties

        /// <summary>
        ///   Event types which can be configured.
        /// </summary>
        protected abstract IEnumerable<Type> EventTypes { get; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///   Called when the inspector gets disabled.
        /// </summary>
        public void OnDisable()
        {
            this.instance = null;
        }

        /// <summary>
        ///   Called when the inspector gets active.
        /// </summary>
        public void OnEnable()
        {
            this.instance = this.target as EventManagerLogBehaviour;
        }

        /// <summary>
        ///   Called when the inspector GUI should be drawn.
        /// </summary>
        public override void OnInspectorGUI()
        {
            if (this.instance == null)
            {
                return;
            }

            // Draw default inspector.
            this.DrawDefaultInspector();

            // Disable/Enable specific events.
            if (this.instance.Disabled)
            {
                return;
            }

            foreach (var eventType in this.EventTypes)
            {
                EditorGUILayout.Separator();
                this.EnumToggles(eventType, "Disable " + ObjectNames.NicifyVariableName(eventType.Name));
            }
        }

        #endregion

        #region Methods

        private void EnumToggles(Type enumType, string label)
        {
            GUILayout.Label(label, EditorStyles.boldLabel);

            GUILayout.BeginHorizontal();
            GUILayout.Label("All:");
            if (GUILayout.Button("Disable"))
            {
                foreach (var value in Enum.GetValues(enumType))
                {
                    this.instance.SetDisabled(value, true);
                }
            }
            if (GUILayout.Button("Enable"))
            {
                foreach (var value in Enum.GetValues(enumType))
                {
                    this.instance.SetDisabled(value, false);
                }
            }
            GUILayout.EndHorizontal();

            foreach (var value in Enum.GetValues(enumType))
            {
                var isDisabled = this.instance.IsDisabled(value);
                var newValue = EditorGUILayout.Toggle(value.ToString(), isDisabled);
                if (newValue != isDisabled)
                {
                    this.instance.SetDisabled(value, newValue);
                }
            }
        }

        #endregion
    }
}