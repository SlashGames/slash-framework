// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EditorGUIUtils.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Unity.Editor.Common.Inspectors.Utils
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    using Slash.Collections.AttributeTables;
    using Slash.ECS.Blueprints;
    using Slash.ECS.Configurations;
    using Slash.ECS.Inspector.Attributes;
    using Slash.ECS.Inspector.Data;
    using Slash.Math.Algebra.Vectors;

    using UnityEditor;

    using UnityEngine;

    using Object = UnityEngine.Object;

    /// <summary>
    ///   Additional EditorGUILayout kind methods.
    /// </summary>
    public static class EditorGUIUtils
    {
        #region Public Methods and Operators

        /// <summary>
        ///   Draws an inspector for modifying the specified array.
        /// </summary>
        /// <typeparam name="T">Type of the array to draw an inspector for.</typeparam>
        /// <param name="foldout">Whether to show all array entries, or not.</param>
        /// <param name="foldoutText">Text to show next to the array editor.</param>
        /// <param name="array">Array to draw the inspector for.</param>
        /// <returns>Whether to show all array entries now, or not.</returns>
        public static bool ArrayField<T>(bool foldout, GUIContent foldoutText, ref T[] array) where T : Object
        {
            IList newArray;
            bool newFoldout = ListField(foldout, foldoutText, array, i => new T[i], typeof(T), out newArray);
            array = (T[])newArray;
            return newFoldout;
        }

        /// <summary>
        ///   Draws an inspector for the passed attribute table.
        /// </summary>
        /// <param name="inspectorType">Type to draw inspector controls for.</param>
        /// <param name="attributeTable">Attribute to draw inspector for.</param>
        /// <param name="inspectorTypeTable"></param>
        /// <param name="blueprintManager"></param>
        public static void AttributeTableField(
            InspectorType inspectorType,
            IAttributeTable attributeTable,
            InspectorTypeTable inspectorTypeTable,
            IBlueprintManager blueprintManager)
        {
            foreach (var inspectorProperty in inspectorType.Properties)
            {
                // Get current value.
                object currentValue = attributeTable.GetValueOrDefault(
                    inspectorProperty.Name,
                    inspectorProperty.Default);

                // Draw inspector property.
                object newValue = LogicInspectorPropertyField(
                    inspectorProperty,
                    currentValue,
                    inspectorTypeTable,
                    blueprintManager);

                // Set new value if changed.
                if (!Equals(newValue, currentValue))
                {
                    attributeTable.SetValue(inspectorProperty.Name, newValue);
                }
            }
        }

        public static void BlueprintComponentsField(
            Blueprint blueprint,
            IAttributeTable configuration,
            InspectorTypeTable inspectorTypeTable,
            IBlueprintManager blueprintManager)
        {
            foreach (var componentType in blueprint.GetAllComponentTypes())
            {
                var inspectorType = inspectorTypeTable[componentType];

                // Draw inspector.
                AttributeTableField(inspectorType, configuration, inspectorTypeTable, blueprintManager);
            }
        }

        public static string BlueprintIdSelection(
            GUIContent label,
            string selectedBlueprintId,
            InspectorTypeTable inspectorComponentTypes,
            IBlueprintManager blueprintManager)
        {
            // Store all blueprint ids for access from a pulldown menu.
            var blueprintIds = blueprintManager.Blueprints.Select(blueprint => blueprint.Key).ToArray();

            // Show blueprint dropdown.
            var oldSelectedBlueprintIndex = Array.IndexOf(blueprintIds, selectedBlueprintId);
            var selectedBlueprintIndex = EditorGUILayout.Popup(
                label,
                oldSelectedBlueprintIndex,
                blueprintIds.Select(blueprintId => new GUIContent(blueprintId)).ToArray());
            if (selectedBlueprintIndex != oldSelectedBlueprintIndex)
            {
                // Update selected blueprint of the target entity.
                return blueprintIds[selectedBlueprintIndex >= 0 ? selectedBlueprintIndex : 0];
            }
            else
            {
                return selectedBlueprintId;
            }
        }

        /// <summary>
        ///   Draws an inspector for modifying the specified list.
        /// </summary>
        /// <param name="foldout">Whether to show all list entries, or not.</param>
        /// <param name="foldoutText">Text to show next to the list editor.</param>
        /// <param name="list">List to draw the inspector for.</param>
        /// <param name="createList">Method for creating a new list if the size should be changed.</param>
        /// <param name="objectType">Unity object type of the list items.</param>
        /// <param name="newList">Modified list.</param>
        /// <returns>Whether to show all list entries now, or not.</returns>
        public static bool ListField(
            bool foldout,
            GUIContent foldoutText,
            IList list,
            Func<int, IList> createList,
            Type objectType,
            out IList newList)
        {
            return ListField(
                foldout,
                foldoutText,
                list,
                createList,
                (obj, index) => EditorGUILayout.ObjectField("Element " + index, (Object)obj, objectType, false),
                out newList);
        }

        /// <summary>
        ///   Draws an inspector for modifying the specified list.
        /// </summary>
        /// <param name="foldout">Whether to show all list entries, or not.</param>
        /// <param name="foldoutText">Text to show next to the list editor.</param>
        /// <param name="list">List to draw the inspector for.</param>
        /// <param name="createList">Method for creating a new list if the size should be changed.</param>
        /// <param name="editItem">Method for changing a specific list item.</param>
        /// <param name="newList">Modified list.</param>
        /// <returns>Whether to show all list entries now, or not.</returns>
        public static bool ListField(
            bool foldout,
            GUIContent foldoutText,
            IList list,
            Func<int, IList> createList,
            Func<object, int, object> editItem,
            out IList newList)
        {
            foldout = EditorGUILayout.Foldout(foldout, foldoutText);
            if (foldout)
            {
                EditorGUI.indentLevel++;

                int currentSize = list != null ? list.Count : 0;
                int newSize = EditorGUILayout.IntField("Size", currentSize);
                if (currentSize != newSize)
                {
                    newList = createList(newSize);
                    for (int x = 0; x < currentSize && x < newSize; x++)
                    {
                        newList[x] = list[x];
                    }
                }
                else
                {
                    newList = list;
                    list = newList;
                }

                for (int x = 0; x < currentSize; x++)
                {
                    list[x] = editItem(list[x], x);
                }

                EditorGUI.indentLevel--;
            }
            else
            {
                newList = list;
            }

            return foldout;
        }

        /// <summary>
        ///   Draws an inspector for modifying the specified list.
        /// </summary>
        /// <typeparam name="T">Type of the list items.</typeparam>
        /// <param name="foldout">Whether to show all list entries, or not.</param>
        /// <param name="foldoutText">Text to show next to the list editor.</param>
        /// <param name="list">List to draw the inspector for.</param>
        /// <param name="createList">Method for creating a new list if the size should be changed.</param>
        /// <returns>Whether to show all list entries now, or not.</returns>
        public static bool ListField<T>(
            bool foldout,
            GUIContent foldoutText,
            ref IList list,
            Func<int, IList> createList) where T : Object
        {
            IList newArray;
            bool newFoldout = ListField(foldout, foldoutText, list, createList, typeof(T), out newArray);
            list = newArray;
            return newFoldout;
        }

        /// <summary>
        ///   Draws an inspector for the specified logic property.
        /// </summary>
        /// <param name="inspectorProperty">Logic property to draw the inspector for.</param>
        /// <param name="currentValue">Current logic property value.</param>
        /// <param name="label">Text to show next to the property editor.</param>
        /// <param name="inspectorTypeTable"></param>
        /// <param name="blueprintManager"></param>
        /// <returns>New logic property value.</returns>
        public static object LogicInspectorPropertyField(
            InspectorPropertyAttribute inspectorProperty,
            object currentValue,
            GUIContent label,
            InspectorTypeTable inspectorTypeTable,
            IBlueprintManager blueprintManager)
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
                object currentEnumValue = (currentValue != null)
                    ? Convert.ChangeType(currentValue, enumInspectorProperty.PropertyType)
                    : Enum.GetValues(enumInspectorProperty.PropertyType).GetValue(0);
                return EditorGUILayout.EnumPopup(label, (Enum)currentEnumValue);
            }
            InspectorVectorAttribute vectorInspectorproperty = inspectorProperty as InspectorVectorAttribute;
            if (vectorInspectorproperty != null)
            {
                if (vectorInspectorproperty.PropertyType == typeof(Vector2I)
                    || vectorInspectorproperty.PropertyType == typeof(List<Vector2I>))
                {
                    Vector2I currentVector2IValue = (currentValue != null) ? (Vector2I)currentValue : Vector2I.Zero;
                    return Vector2IField(label, currentVector2IValue);
                }
                if (vectorInspectorproperty.PropertyType == typeof(Vector2F)
                    || vectorInspectorproperty.PropertyType == typeof(List<Vector2F>))
                {
                    Vector2F currentVector2FValue = (currentValue != null) ? (Vector2F)currentValue : Vector2F.Zero;
                    return Vector2FField(label, currentVector2FValue);
                }
            }
            InspectorEntityAttribute entityInspector = inspectorProperty as InspectorEntityAttribute;
            if (entityInspector != null)
            {
                EntityConfiguration entityConfiguration = currentValue as EntityConfiguration;
                if (entityConfiguration == null)
                {
                    entityConfiguration = new EntityConfiguration();
                }

                entityConfiguration.BlueprintId = BlueprintIdSelection(
                    label,
                    entityConfiguration.BlueprintId,
                    inspectorTypeTable,
                    blueprintManager);

                if (!string.IsNullOrEmpty(entityConfiguration.BlueprintId))
                {
                    Blueprint blueprint = blueprintManager.GetBlueprint(entityConfiguration.BlueprintId);
                    if (blueprint != null)
                    {
                        if (entityConfiguration.Configuration == null)
                        {
                            entityConfiguration.Configuration = new AttributeTable();
                        }

                        ++EditorGUI.indentLevel;
                        BlueprintComponentsField(
                            blueprint,
                            entityConfiguration.Configuration,
                            inspectorTypeTable,
                            blueprintManager);
                        --EditorGUI.indentLevel;
                    }
                }

                return entityConfiguration;
            }

            EditorGUILayout.HelpBox(
                string.Format(
                    "No inspector found for property {0} of type {1}.",
                    inspectorProperty.Name,
                    inspectorProperty.PropertyType),
                MessageType.Warning);
            return currentValue;
        }

        /// <summary>
        ///   Draws an inspector for the specified logic property.
        /// </summary>
        /// <param name="inspectorProperty">Logic property to draw the inspector for.</param>
        /// <param name="currentValue">Current logic property value.</param>
        /// <param name="inspectorTypeTable"></param>
        /// <param name="blueprintManager"></param>
        /// <returns>New logic property value.</returns>
        public static object LogicInspectorPropertyField(
            InspectorPropertyAttribute inspectorProperty,
            object currentValue,
            InspectorTypeTable inspectorTypeTable,
            IBlueprintManager blueprintManager)
        {
            if (inspectorProperty.IsList)
            {
                // Build array.
                IList currentList = currentValue as IList;
                InspectorPropertyAttribute localInspectorProperty = inspectorProperty;
                IList newList;

                ListField(
                    true,
                    new GUIContent(inspectorProperty.Name),
                    currentList,
                    count =>
                    {
                        IList list = localInspectorProperty.GetEmptyList();
                        for (int idx = 0; idx < count; idx++)
                        {
                            list.Add(localInspectorProperty.DefaultListItem);
                        }
                        return list;
                    },
                    (obj, index) =>
                        LogicInspectorPropertyField(
                            localInspectorProperty,
                            obj,
                            new GUIContent("Item " + index),
                            inspectorTypeTable,
                            blueprintManager),
                    out newList);

                return newList;
            }

            // Draw inspector property.
            return LogicInspectorPropertyField(
                inspectorProperty,
                currentValue,
                new GUIContent(inspectorProperty.Name, inspectorProperty.Description),
                inspectorTypeTable,
                blueprintManager);
        }

        /// <summary>
        ///   Draws an inspector for the specified shader.
        /// </summary>
        /// <param name="shaderContext">Shader to draw the inspector for.</param>
        /// <param name="label">Text to show next to the shader editor.</param>
        /// <returns>New selected shader name.</returns>
        public static string ShaderField(ShaderContext shaderContext, string label)
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PrefixLabel(label);
            string selectedShaderName = shaderContext.SelectedShader;
            if (GUILayout.Button(selectedShaderName))
            {
                shaderContext.SelectedShader = selectedShaderName;
                shaderContext.DisplayShaderContext(GUILayoutUtility.GetRect(GUIContent.none, EditorStyles.popup));
            }
            EditorGUILayout.EndHorizontal();

            return selectedShaderName;
        }

        public static Vector2F Vector2FField(GUIContent label, Vector2F v)
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PrefixLabel(label);
            var x = EditorGUILayout.FloatField("X", v.X);
            var y = EditorGUILayout.FloatField("Y", v.Y);
            EditorGUILayout.EndHorizontal();

            return new Vector2F(x, y);
        }

        public static Vector2I Vector2IField(GUIContent label, Vector2I v)
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PrefixLabel(label);
            var x = EditorGUILayout.IntField("X", v.X);
            var y = EditorGUILayout.IntField("Y", v.Y);
            EditorGUILayout.EndHorizontal();

            return new Vector2I(x, y);
        }

        #endregion
    }
}