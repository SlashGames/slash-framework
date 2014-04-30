// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EditorGUIExt.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Unity.Editor.Common.Inspectors.Utils
{
    using UnityEditor;

    using UnityEngine;

    public static class EditorGUIUtils
    {
        #region Public Methods and Operators

        public static bool ArrayField<T>(bool foldout, GUIContent content, ref T[] array) where T : Object
        {
            foldout = EditorGUILayout.Foldout(foldout, content);
            if (foldout)
            {
                int currentSize = array != null ? array.Length : 0;
                int newSize = EditorGUILayout.IntField("Size", currentSize);
                if (currentSize != newSize)
                {
                    T[] newArray = new T[newSize];
                    for (int x = 0; x < newSize; x++)
                    {
                        if (x < currentSize)
                        {
                            newArray[x] = array != null ? array[x] : null;
                        }
                    }
                    array = newArray;
                    currentSize = newSize;
                }
                for (int x = 0; x < currentSize; x++)
                {
                    array[x] = (T)EditorGUILayout.ObjectField("Element " + x, array[x], typeof(T), false);
                }
            }

            return foldout;
        }

        #endregion
    }
}