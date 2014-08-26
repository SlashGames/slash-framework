// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GUILayoutExt.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Unity.Common.GUI
{
    using UnityEngine;

    /// <summary>
    ///   Extends the Unity.GUILayout class.
    /// </summary>
    public static class GUILayoutExt
    {
        #region Public Methods and Operators

        /// <summary>
        ///   Shows either the currently selected option, or the whole list of options.
        /// </summary>
        /// <param name="selectedIndex">Index of the currently selected option.</param>
        /// <param name="displayedOptions">List of options.</param>
        /// <param name="showList">Whether to show the whole list, or not.</param>
        /// <returns>Index of the new selected option.</returns>
        public static int Popup(int selectedIndex, string[] displayedOptions, ref bool showList)
        {
            string selectedItem = displayedOptions[selectedIndex];

            if (GUILayout.Button(selectedItem))
            {
                Debug.Log("Pushed button");
                showList = !showList;
            }

            if (showList)
            {
                int newSelectedIndex = GUILayout.SelectionGrid(selectedIndex, displayedOptions, 1);
                if (newSelectedIndex != selectedIndex)
                {
                    showList = false;
                }
                return newSelectedIndex;
            }
            else
            {
                return selectedIndex;
            }
        }

        #endregion
    }
}