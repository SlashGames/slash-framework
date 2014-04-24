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

        public static bool ArrayField<T>(bool foldout, GUIContent content, T[] array) where T : Object
        {
            foldout = EditorGUILayout.Foldout(foldout, content);
            if (foldout)
            {
                int size = EditorGUILayout.IntField("Size", array.Length);
                if (array.Length != size)
                {
                    T[] newArray = new T[size];
                    for (int x = 0; x < size; x++)
                    {
                        if (x < array.Length)
                        {
                            newArray[x] = array[x];
                        }
                    }
                    array = newArray;
                }
                for (int x = 0; x < array.Length; x++)
                {
                    array[x] = (T)EditorGUILayout.ObjectField("Element " + x, array[x], typeof(T), false);
                }
            }

            return foldout;
        }

        #endregion
    }
}