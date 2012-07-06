// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IValueEditorContext.cs" company="Rainy Games">
//   Copyright (c) Rainy Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace RainyGames.Unity.Common.GUI.ValueEditors
{
    using System;

    public interface IValueEditorContext
    {
        #region Public Properties

        /// <summary>
        ///   Description of the attribute.
        /// </summary>
        string Description { get; }

        /// <summary>
        ///   Name of the attribute.
        /// </summary>
        string Name { get; }

        /// <summary>
        ///   Value to edit.
        /// </summary>
        object Value { get; set; }

        /// <summary>
        ///   Unique key, so the value editors can store meta data for the value over multiple frames.
        /// </summary>
        object Key { get; }

        /// <summary>
        ///   Value type.
        /// </summary>
        Type Type { get; }

        #endregion
    }
}