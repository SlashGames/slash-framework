// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UnityUtils.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace oxyd.unity.utils
{
    using System.Collections.Generic;
    using System.Linq;

    using UnityEngine;

    public static class UnityUtils
    {
        #region Public Methods and Operators

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

        #endregion
    }
}