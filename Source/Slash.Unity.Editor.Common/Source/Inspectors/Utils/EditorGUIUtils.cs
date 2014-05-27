// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EditorGUIUtils.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Unity.Editor.Common.Inspectors.Utils
{
    using System;
    using System.Collections;

    using UnityEditor;

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

        #endregion

        #region Methods

        private static IList CreateArray<T>(int i)
        {
            return new T[i];
        }

        #endregion
    }
}