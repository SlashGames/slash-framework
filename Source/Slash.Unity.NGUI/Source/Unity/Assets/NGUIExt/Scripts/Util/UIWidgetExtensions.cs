// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UIWidgetExtensions.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Unity.NGUIExt.Util
{
    using UnityEngine;

    public static class UIWidgetExtensions
    {
        #region Public Methods and Operators

        public static void SetAlpha(this UIWidget widget, float alpha)
        {
            widget.color = new Color(widget.color.r, widget.color.g, widget.color.b, alpha);
        }

        #endregion
    }
}