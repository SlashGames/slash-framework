// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GuiExt.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Unity.Common.GUI
{
    using UnityEngine;

    /// <summary>
    ///   Extends the Unity.GUI class.
    /// </summary>
    public static class GUIExt
    {
        #region Static Fields

        private static readonly int popupListHash = "PopupList".GetHashCode();

        #endregion

        #region Delegates

        public delegate void PopupCallback();

        #endregion

        #region Public Methods and Operators

        public static bool Popup(
            Rect position,
            ref bool showList,
            ref int listEntry,
            GUIContent buttonContent,
            object[] list,
            GUIStyle listStyle,
            PopupCallback callback)
        {
            return Popup(
                position, ref showList, ref listEntry, buttonContent, list, "button", "box", listStyle, callback);
        }

        public static bool Popup(
            Rect position,
            ref bool showList,
            ref int listEntry,
            GUIContent buttonContent,
            object[] list,
            GUIStyle buttonStyle,
            GUIStyle boxStyle,
            GUIStyle listStyle,
            PopupCallback callback)
        {
            int controlID = GUIUtility.GetControlID(popupListHash, FocusType.Passive);
            bool done = false;
            switch (Event.current.GetTypeForControl(controlID))
            {
                case EventType.MouseDown:
                    if (position.Contains(Event.current.mousePosition))
                    {
                        GUIUtility.hotControl = controlID;
                        showList = true;
                    }
                    break;
                case EventType.MouseUp:
                    if (showList)
                    {
                        done = true;
                        // Call our delegate method
                        callback();
                    }
                    break;
            }

            GUI.Label(position, buttonContent, buttonStyle);
            if (showList)
            {
                // Get our list of strings
                string[] text = new string[list.Length];
                // convert to string
                for (int i = 0; i < list.Length; i++)
                {
                    text[i] = list[i].ToString();
                }

                Rect listRect = new Rect(position.x, position.y, position.width, list.Length * 20);
                GUI.Box(listRect, "", boxStyle);
                listEntry = GUI.SelectionGrid(listRect, listEntry, text, 1, listStyle);
            }
            if (done)
            {
                showList = false;
            }
            return done;
        }

        #endregion
    }
}