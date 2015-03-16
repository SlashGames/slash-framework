// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EnumValueEditor.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Unity.Common.GUI.ValueEditors
{
    using System;
    using System.Collections.Generic;

    using Slash.Reflection.Utils;

    using UnityEngine;

    /// <summary>
    ///   Editor to change an enum value.
    /// </summary>
    public class EnumValueEditor : IValueEditor
    {
        #region Fields

        private readonly Dictionary<object, bool> showPopupList =
            new Dictionary<object, bool>(EqualityComparer<object>.Default);

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///   Edits the specified context.
        /// </summary>
        /// <param name="context">Editor context to work with.</param>
        public void Edit(IValueEditorContext context)
        {
            if (!ReflectionUtils.IsEnum(context.Type))
            {
                throw new ArgumentException(string.Format("Type '{0}' is no enum type.", context.Type), "context");
            }

            // Get enum value.
            Array enumValues = Enum.GetValues(context.Type);
            int selectedItem;
            Enum value;
            if (context.Value != null)
            {
                value = (Enum)context.Value;
                selectedItem = Array.IndexOf(enumValues, value);
            }
            else
            {
                // Take first value of enum type.
                value = (Enum)enumValues.GetValue(0);
                selectedItem = 0;
            }

            GUILayout.BeginHorizontal();
            GUILayout.Label(context.Name);
            bool showList;
            this.showPopupList.TryGetValue(context.Key, out showList);
            int newSelectedItem = GUILayoutExt.Popup(selectedItem, Enum.GetNames(context.Type), ref showList);
            this.showPopupList[context.Key] = showList;
            GUILayout.EndHorizontal();

            if (newSelectedItem != selectedItem)
            {
                context.Value = enumValues.GetValue(newSelectedItem);
            }
        }

        #endregion
    }
}