// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InspectorTooltipAttribute.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Unity.Common.Utils
{
    using UnityEngine;

    /// <summary>
    ///   Found in Unity forum: http://forum.unity3d.com/threads/182621-Inspector-Tooltips.
    /// </summary>
    public class InspectorTooltipAttribute : PropertyAttribute
    {
        #region Constructors and Destructors

        /// <summary>
        ///   Constructor.
        /// </summary>
        /// <param name="text">Tooltip text.</param>
        public InspectorTooltipAttribute(string text)
        {
            this.Text = text;
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///   Tooltip text.
        /// </summary>
        public string Text { get; private set; }

        #endregion
    }
}