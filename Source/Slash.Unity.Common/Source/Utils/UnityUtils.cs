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
        ///   Destroys all children from the specified game object.
        /// </summary>
        /// <param name="gameObject">Game object to destroy children.</param>
        public static void DestroyChildren(this GameObject gameObject)
        {
            List<GameObject> children = (from Transform child in gameObject.transform select child.gameObject).ToList();
            foreach (GameObject child in children)
            {
                // Set inactive to hide immediatly. The destruction is just performed after the next update.
                child.SetActive(false);
                Object.Destroy(child);
            }
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