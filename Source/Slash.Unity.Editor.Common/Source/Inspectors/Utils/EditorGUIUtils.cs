// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EditorGUIUtils.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Unity.Editor.Common.Inspectors.Utils
{
    using System;
    using System.Collections;

    using Slash.GameBase.Inspector.Attributes;

    using UnityEditor;

    using UnityEditorInternal;

    using UnityEngine;

    using Object = UnityEngine.Object;

    public static class EditorGUIUtils
    {
        #region Public Methods and Operators

        public static bool ArrayField<T>(bool foldout, GUIContent content, ref T[] array) where T : Object
        {
            IList newArray;
            bool newFoldout = ArrayField(foldout, content, array, CreateArray<T>, typeof(T), out newArray);
            array = (T[])newArray;
            return newFoldout;
        }

        public static bool ArrayField(
            bool foldout,
            GUIContent content,
            IList array,
            Func<int, IList> createArray,
            Type objectType,
            out IList newArray)
        {
            return ArrayField(
                foldout,
                content,
                array,
                createArray,
                (obj, index) => EditorGUILayout.ObjectField("Element " + index, (Object)obj, objectType, false),
                out newArray);
        }

        public static bool ArrayField(
            bool foldout,
            GUIContent content,
            IList array,
            Func<int, IList> createArray,
            Func<object, int, object> editItem,
            out IList newArray)
        {
            foldout = EditorGUILayout.Foldout(foldout, content);
            if (foldout)
            {
                EditorGUI.indentLevel++;

                int currentSize = array != null ? array.Count : 0;
                int newSize = EditorGUILayout.IntField("Size", currentSize);
                if (currentSize != newSize)
                {
                    newArray = createArray(newSize);
                    for (int x = 0; x < newSize; x++)
                    {
                        if (x < currentSize)
                        {
                            newArray[x] = array != null ? array[x] : null;
                        }
                    }
                }
                else
                {
                    newArray = array;
                    array = newArray;
                }

                for (int x = 0; x < currentSize; x++)
                {
                    array[x] = editItem(array[x], x);
                }

                EditorGUI.indentLevel--;
            }
            else
            {
                newArray = array;
            }

            return foldout;
        }

        public static bool ArrayField<T>(
            bool foldout, GUIContent content, ref IList array, Func<int, IList> createArray) where T : Object
        {
            IList newArray;
            bool newFoldout = ArrayField(foldout, content, array, createArray, typeof(T), out newArray);
            array = newArray;
            return newFoldout;
        }

        public static object LogicInspectorPropertyField(
            InspectorPropertyAttribute inspectorProperty, object currentValue, GUIContent label)
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

        public static object LogicInspectorPropertyField(
            InspectorPropertyAttribute inspectorProperty, object currentValue)
        {
            return LogicInspectorPropertyField(
                inspectorProperty, currentValue, new GUIContent(inspectorProperty.Name, inspectorProperty.Description));
        }

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

        #endregion

        #region Methods

        private static IList CreateArray<T>(int i)
        {
            return new T[i];
        }

        #endregion
    }

    public class ShaderContext : ScriptableObject
    {
        #region Fields

        public string SelectedShader;

        private readonly Material dummyMaterial;

        private MenuCommand mc;

        #endregion

        #region Constructors and Destructors

        public ShaderContext()
        {
            // Create dummy material to make it not highlight any shaders inside:
            const string TmpStr = "Shader \"Hidden/tmp_shdr\"{SubShader{Pass{}}}";
            this.dummyMaterial = new Material(TmpStr);
        }

        #endregion

        #region Public Methods and Operators

        public void DisplayShaderContext(Rect r)
        {
            if (this.mc == null)
            {
                this.mc = new MenuCommand(this, 0);
            }

            Shader shader = string.IsNullOrEmpty(this.SelectedShader) ? null : Shader.Find(this.SelectedShader);
            Material temp = shader != null ? new Material(shader) : this.dummyMaterial;

            // Rebuild shader menu:
            InternalEditorUtility.SetupShaderMenu(temp);

            // Destroy temporary material.
            if (shader != null)
            {
                DestroyImmediate(temp, true);
            }

            // Display shader popup:
            EditorUtility.DisplayPopupMenu(r, "CONTEXT/ShaderPopup", this.mc);
        }

        #endregion

        #region Methods

        private void OnSelectedShaderPopup(string command, Object shader)
        {
            this.SelectedShader = shader != null ? shader.name : null;
        }

        #endregion
    }
}