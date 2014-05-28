// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UnityUtils.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Unity.Common.Utils
{
    using System.Collections.Generic;
    using System.Linq;

    using UnityEngine;

    public static class UnityUtils
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
        ///   Creates a unity color from integer RGB values.
        ///   The constructor of the Unity Color class takes float values from
        ///   0 to 1 by default.
        /// </summary>
        /// <param name="r">Red value (0-255).</param>
        /// <param name="g">Green value (0-255).</param>
        /// <param name="b">Blue value (0-255).</param>
        /// <returns>Unity color from the specified RGB values.</returns>
        public static Color ColorFromRGB(int r, int g, int b)
        {
            return new Color(r / 255.0f, g / 255.0f, b / 255.0f);
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

        /// <summary>
        ///   Adds a timestamp to the specified string.
        /// </summary>
        /// <param name="message">String to add a timestamp to.</param>
        /// <returns>Timestamped message.</returns>
        public static string WithTimestamp(string message)
        {
            return string.Format("[{0:000.000}] {1}", Time.realtimeSinceStartup, message);
        }

        #endregion
    }
}