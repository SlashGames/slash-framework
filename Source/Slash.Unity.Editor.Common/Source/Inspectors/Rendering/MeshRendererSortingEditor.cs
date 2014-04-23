// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MeshRendererSortingEditor.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Unity.Editor.Common.Inspectors.Rendering
{
    using UnityEditor;

    using UnityEngine;

    /// <summary>
    ///   Adds sorting layer and order to mesh renderer to adjust it to work correctly with sprite rendering.
    /// </summary>
    [CustomEditor(typeof(MeshRenderer))]
    public class MeshRendererSortingEditor : Editor
    {
        #region Public Methods and Operators

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            MeshRenderer renderer = (MeshRenderer)this.target;
            if (renderer == null)
            {
                EditorGUILayout.HelpBox(
                    "No mesh renderer found. Can't adjust sorting layer and order.", MessageType.Warning);
                return;
            }

            EditorGUILayout.BeginHorizontal();

            EditorGUI.BeginChangeCheck();

            string sortingLayerName = EditorGUILayout.TextField("Sorting Layer Name", renderer.sortingLayerName);

            if (EditorGUI.EndChangeCheck())
            {
                renderer.sortingLayerName = sortingLayerName;
            }

            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();

            EditorGUI.BeginChangeCheck();

            int order = EditorGUILayout.IntField("Sorting Order", renderer.sortingOrder);

            if (EditorGUI.EndChangeCheck())
            {
                renderer.sortingOrder = order;
            }

            EditorGUILayout.EndHorizontal();
        }

        #endregion
    }
}