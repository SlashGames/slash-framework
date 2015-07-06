// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UnityGameObjectUtils.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Unity.Common.Utils
{
    using System;
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
        ///   Instantiates a new game object with the specified name and adds it
        ///   to the game object.
        ///   Makes sure the position/rotation/scale are initialized correctly.
        /// </summary>
        /// <param name="parent">Game object to add child to.</param>
        /// <param name="name">Name of the new child.</param>
        /// <returns>Instantiated new child.</returns>
        public static GameObject AddChild(this GameObject parent, string name)
        {
            GameObject go = AddChild(parent, (GameObject)null);
            go.name = name;
            return go;
        }

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
            GameObject go = prefab != null ? Object.Instantiate(prefab) as GameObject : new GameObject();
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
                if (Application.isEditor && !Application.isPlaying)
                {
                    Object.DestroyImmediate(child);
                }
                else
                {
                    Object.Destroy(child);
                }
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

        public static IEnumerable<GameObject> GetDescendants(this GameObject gameObject)
        {
            foreach (Transform child in gameObject.transform)
            {
                yield return child.gameObject;

                // Depth-first.
                foreach (var descendant in child.gameObject.GetDescendants())
                {
                    yield return descendant;
                }
            }
        }

        public static IEnumerable<GameObject> GetDescendantsAndSelf(this GameObject gameObject)
        {
            yield return gameObject;

            foreach (var descendant in gameObject.GetDescendants())
            {
                yield return descendant;
            }
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
            string path = String.Empty;
            if (gameObject.transform.parent != null)
            {
                path = gameObject.transform.parent.gameObject.GetPath() + "/";
            }
            path += gameObject.name;
            return path;
        }

        /// <summary>
        ///   Returns the absolute scale of the specified transform relative to the specified ancestor.
        /// </summary>
        /// <param name="transform">Transform to get scale for.</param>
        /// <param name="ancestor">Ancestor to which the scale should be relative. Null if global.</param>
        /// <returns>Absolute scale of the specified transform relative to the specified ancestor.</returns>
        public static Vector3 GetScale(this Transform transform, Transform ancestor)
        {
            if (transform == null || transform == ancestor)
            {
                return Vector3.one;
            }
            Vector3 parentScale = GetScale(transform.parent, ancestor);
            return new Vector3(
                transform.localScale.x * parentScale.x,
                transform.localScale.y * parentScale.y,
                transform.localScale.z * parentScale.z);
        }

        /// <summary>
        ///   Sets the absolute scale of the specified transform relative to the specified ancestor.
        /// </summary>
        /// <param name="transform">Transform to set scale for.</param>
        /// <param name="ancestor">Ancestor to which the scale should be relative. Null if global.</param>
        /// <param name="scale">Absolute scale.</param>
        public static void SetScale(this Transform transform, Transform ancestor, Vector3 scale)
        {
            Vector3 parentScale = transform.parent != null ? GetScale(transform.parent, ancestor) : Vector3.one;
            if (parentScale != Vector3.zero)
            {
                Vector3 localScale = new Vector3(
                    scale.x / parentScale.x, scale.y / parentScale.y, scale.z / parentScale.z);
                transform.localScale = localScale;
            }
        }

        #endregion
    }
}