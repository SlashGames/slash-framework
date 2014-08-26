// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UnityGameObjectUtils.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Unity.Common.Utils
{
    using System.Collections.Generic;
    using System.Linq;

    using UnityEngine;

    using Object = UnityEngine.Object;

    /// <summary>
    ///   Utility methods for handling Unity game object hierarchies.
    /// </summary>
    public static class UnityGameObjectUtils
    {
        #region Public Methods and Operators

        /// <summary>
        ///   Instantiates a new game object from the specified prefab and adds it
        ///   to the game object.
        ///   Makes sure the position/rotation/scale are initialized correctly.
        /// </summary>
        /// <param name="parent">Game object to add child to.</param>
        /// <param name="prefab">Prefab to instantiate new child from.</param>
        /// <returns>Instantiated new child.</returns>
        public static GameObject AddChild(this GameObject parent, GameObject prefab)
        {
            GameObject go = Object.Instantiate(prefab) as GameObject;
            if (go != null && parent != null)
            {
                Transform t = go.transform;
                t.parent = parent.transform;
                t.localPosition = Vector3.zero;
                t.localRotation = Quaternion.identity;
                t.localScale = Vector3.one;
                go.layer = parent.layer;
            }
            return go;
        }

        /// <summary>
        ///   Destroys all children from the specified game object.
        /// </summary>
        /// <param name="gameObject">Game object to destroy children.</param>
        public static void DestroyChildren(this GameObject gameObject)
        {
            List<GameObject> children = gameObject.GetChildren().ToList();
            foreach (GameObject child in children)
            {
                // Set inactive to hide immediatly. The destruction is just performed after the next update.
                child.SetActive(false);
                Object.Destroy(child);
            }
        }

        /// <summary>
        ///   Collects all children from the specified game object.
        /// </summary>
        /// <param name="gameObject">Game object to collect children from.</param>
        /// <returns>Enumeration of all children of the specified game object.</returns>
        public static IEnumerable<GameObject> GetChildren(this GameObject gameObject)
        {
            return (from Transform child in gameObject.transform select child.gameObject);
        }

        /// <summary>
        ///   Returns all children of the specified game object, ordered by name.
        /// </summary>
        /// <param name="gameObject">Game object to get the children of.</param>
        /// <returns>Enumeration of all children of the specified game object.</returns>
        public static IEnumerable<GameObject> GetOrderedChildren(this GameObject gameObject)
        {
            return gameObject.GetChildren().OrderBy(go => go.name);
        }

        /// <summary>
        ///   Returns the full path (i.e. names of all ancestors and self) to the game object.
        /// </summary>
        /// <param name="gameObject">Game object to get path for.</param>
        /// <returns>Full path of the specified game object.</returns>
        public static string GetPath(this GameObject gameObject)
        {
            string path = string.Empty;
            if (gameObject.transform.parent != null)
            {
                path = gameObject.transform.parent.gameObject.GetPath() + "/";
            }
            path += gameObject.name;
            return path;
        }

        #endregion
    }
}