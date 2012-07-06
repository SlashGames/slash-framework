// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GUILayoutExt.cs" company="Rainy Games">
//   Copyright (c) Rainy Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace RainyGames.Unity.Common.GUI
{
    using UnityEngine;

    /// <summary>
    ///   Extends the Unity.GUILayout class.
    /// </summary>
    public static class GUILayoutExt
    {
        #region Public Methods and Operators

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