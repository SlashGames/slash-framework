// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UIFloatingWidgetRoot.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Unity.NGUIExt.Floating
{
    using UnityEngine;

    /// <summary>
    ///   Provides convinience methods for creating floating widgets. Attach to UIRoot to ensure proper positioning.
    /// </summary>
    [RequireComponent(typeof(UIRoot))]
    public class UIFloatingWidgetRoot : MonoBehaviour
    {
        #region Fields

        /// <summary>
        ///   Prefab for floating texts.
        /// </summary>
        public GameObject FloatingTextPrefab;

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///   Creates a floating text at the specified screen position.
        /// </summary>
        /// <param name="text">Text to show.</param>
        /// <param name="screenPosition">Initial screen position of the text to show.</param>
        public void CreateText(string text, Vector2 screenPosition)
        {
            // Spawn text.
            var floatingGameObject = NGUITools.AddChild(this.gameObject, this.FloatingTextPrefab);
            floatingGameObject.transform.localPosition = screenPosition;

            // Set position.
            var label = floatingGameObject.GetComponent<UILabel>();
            label.text = text;
        }

        #endregion
    }
}