// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CreateEmptyInCenter.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Unity.Editor.Common.MenuItems.Util
{
    using System.Collections.Generic;
    using System.Linq;

    using UnityEditor;

    using UnityEngine;

    /// <summary>
    ///   Creates a new, empty game object at the center of the current selection,
    ///   and reparents all selected game objects to the new one.
    /// </summary>
    public class CreateEmptyInCenterEditor : Editor
    {
        #region Public Methods and Operators

        [MenuItem("GameObject/Create Empty In Center %&n")]
        public static void CreateEmptyInCenter()
        {
            var oldSelection = new List<Transform>(Selection.transforms);

            // Compute center.
            var center = oldSelection.Aggregate(Vector3.zero, (current, selected) => current + selected.position);
            center /= oldSelection.Count;

            // Create empty.
            var empty = new GameObject();
            empty.transform.position = center;

            Undo.RegisterCreatedObjectUndo(empty, "Created centered empty");

            // Reparent objects.
            foreach (var selected in oldSelection)
            {
                Undo.SetTransformParent(selected.transform, empty.transform, "Reparented selected objects");
            }

            // Select empty.
            Selection.activeGameObject = empty;
        }

        #endregion
    }
}