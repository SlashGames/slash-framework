// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InputUtils.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Unity.Common.Input
{
    using UnityEngine;

    /// <summary>
    ///   Utility methods for handling Unity input and Unity GUI.
    /// </summary>
    public static class InputUtils
    {
        #region Public Properties

        /// <summary>
        ///   Returns the current mouse position on the GUI.
        ///   Considers that the coordinate system of the world starts in the top-left corner,
        ///   but the coordinate system of the GUI starts in the bottom-left corner,
        ///   so the y coordinate is inverted.
        /// </summary>
        public static Vector2 GUIMousePosition
        {
            get
            {
                Vector3 mousePosition = Input.mousePosition;
                return new Vector2(mousePosition.x, Screen.height - mousePosition.y);
            }
        }

        #endregion
    }
}