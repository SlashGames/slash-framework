// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TooltipDrawer.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Unity.Editor.Common.Inspectors.Utils
{
    using System;
    using System.Reflection;

    using Slash.Unity.Common.Utils;

    using UnityEditor;

    using UnityEngine;

    using Object = UnityEngine.Object;

    /// <summary>
    ///   Found in Unity forum: http://forum.unity3d.com/threads/182621-Inspector-Tooltips.
    /// </summary>
    [CustomPropertyDrawer(typeof(Tooltip))]
    public class TooltipDrawer : PropertyDrawer
    {
        #region Static Fields

        private static Type editorType;

        private static MethodInfo layerMaskFieldMethod;

        #endregion

        #region Fields

        private Type fieldType;

        private GUIContent label;

        private GUIContent oldlabel;

        #endregion

        //private SerializedProperty currentProperty = null;

        #region Properties

        private static Type EditorType
        {
            get
            {
                if (editorType == null)
                {
                    Assembly assembly = Assembly.GetAssembly(typeof(EditorGUI));

                    editorType = assembly.GetType("UnityEditor.EditorGUI");

                    if (editorType == null)
                    {
                        Debug.LogWarning("TooltipDrawer: Failed to open source file of EditorGUI");
                    }
                }

                return editorType;
            }
        }

        private static MethodInfo LayerMaskFieldMethod
        {
            get
            {
                if (layerMaskFieldMethod == null)
                {
                    Type[] typeDecleration = new[] { typeof(Rect), typeof(SerializedProperty), typeof(GUIContent) };

                    layerMaskFieldMethod = EditorType.GetMethod(
                        "LayerMaskField",
                        BindingFlags.NonPublic | BindingFlags.Static,
                        Type.DefaultBinder,
                        typeDecleration,
                        null);

                    if (layerMaskFieldMethod == null)
                    {
                        Debug.LogError("TooltipDrawer: Failed to locate the internal LayerMaskField method.");
                    }
                }

                return layerMaskFieldMethod;
            }
        }

        private GUIContent Label
        {
            get
            {
                if (this.label == null)
                {
                    Tooltip labelAttribute = (Tooltip)this.attribute;

                    this.label = new GUIContent(this.oldlabel.text, labelAttribute.Text);
                }

                return this.label;
            }
        }

        #endregion

        #region Public Methods and Operators

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent oldLabel)
        {
            this.oldlabel = oldLabel;

            EditorGUI.BeginProperty(position, this.Label, property);

            EditorGUI.BeginChangeCheck();

            switch (property.propertyType)
            {
                case SerializedPropertyType.AnimationCurve:

                    AnimationCurve newAnimationCurveValue = EditorGUI.CurveField(
                        position, this.Label, property.animationCurveValue);

                    if (EditorGUI.EndChangeCheck())
                    {
                        property.animationCurveValue = newAnimationCurveValue;
                    }

                    break;

                case SerializedPropertyType.Boolean:

                    bool newBoolValue = EditorGUI.Toggle(position, this.Label, property.boolValue);

                    if (EditorGUI.EndChangeCheck())
                    {
                        property.boolValue = newBoolValue;
                    }

                    break;

                case SerializedPropertyType.Bounds:

                    Bounds newBoundsValue = EditorGUI.BoundsField(position, this.Label, property.boundsValue);

                    if (EditorGUI.EndChangeCheck())
                    {
                        property.boundsValue = newBoundsValue;
                    }

                    break;

                case SerializedPropertyType.Color:

                    Color newColorValue = EditorGUI.ColorField(position, this.Label, property.colorValue);

                    if (EditorGUI.EndChangeCheck())
                    {
                        property.colorValue = newColorValue;
                    }

                    break;

                case SerializedPropertyType.Enum:

                    int newEnumValueIndex =
                        (int)
                        (object)
                        EditorGUI.EnumPopup(
                            position,
                            this.Label,
                            Enum.Parse(this.GetFieldType(property), property.enumNames[property.enumValueIndex]) as Enum);

                    if (EditorGUI.EndChangeCheck())
                    {
                        property.enumValueIndex = newEnumValueIndex;
                    }

                    break;

                case SerializedPropertyType.Float:

                    float newFloatValue = EditorGUI.FloatField(position, this.Label, property.floatValue);

                    if (EditorGUI.EndChangeCheck())
                    {
                        property.floatValue = newFloatValue;
                    }

                    break;

                case SerializedPropertyType.Integer:

                    int newIntValue = EditorGUI.IntField(position, this.Label, property.intValue);

                    if (EditorGUI.EndChangeCheck())
                    {
                        property.intValue = newIntValue;
                    }

                    break;

                case SerializedPropertyType.LayerMask:

                    LayerMaskFieldMethod.Invoke(property.intValue, new object[] { position, property, this.Label });

                    break;

                case SerializedPropertyType.ObjectReference:

                    Object newObjectReferenceValue = EditorGUI.ObjectField(
                        position, this.Label, property.objectReferenceValue, this.GetFieldType(property), true);

                    if (EditorGUI.EndChangeCheck())
                    {
                        property.objectReferenceValue = newObjectReferenceValue;
                    }

                    break;

                case SerializedPropertyType.Rect:

                    Rect newRectValue = EditorGUI.RectField(position, this.Label, property.rectValue);

                    if (EditorGUI.EndChangeCheck())
                    {
                        property.rectValue = newRectValue;
                    }

                    break;

                case SerializedPropertyType.String:

                    string newStringValue = EditorGUI.TextField(position, this.Label, property.stringValue);

                    if (EditorGUI.EndChangeCheck())
                    {
                        property.stringValue = newStringValue;
                    }

                    break;

                default:

                    Debug.LogWarning("TooltipDrawer: found an un-handled type: " + property.propertyType);

                    break;
            }

            EditorGUI.EndProperty();
        }

        #endregion

        #region Methods

        private Type GetFieldType(SerializedProperty property)
        {
            if (this.fieldType == null)
            {
                Type parentClassType = property.serializedObject.targetObject.GetType();

                FieldInfo propertyFieldInfo = parentClassType.GetField(
                    property.name, BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public);

                if (propertyFieldInfo == null)
                {
                    Debug.LogError("TooltipDrawer: Could not locate the object in the parent class");

                    return null;
                }

                this.fieldType = propertyFieldInfo.FieldType;
            }

            return this.fieldType;
        }

        #endregion
    }
}