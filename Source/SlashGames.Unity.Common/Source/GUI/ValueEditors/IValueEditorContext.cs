// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IValueEditorContext.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace SlashGames.Unity.Common.GUI.ValueEditors
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
        ///   Unique key, so the value editors can store meta data for the value over multiple frames.
        /// </summary>
        object Key { get; }

        /// <summary>
        ///   Name of the attribute.
        /// </summary>
        string Name { get; }

        /// <summary>
        ///   Value type.
        /// </summary>
        Type Type { get; }

        /// <summary>
        ///   Value to edit.
        /// </summary>
        object Value { get; set; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///   Takes the value, tries to cast it to the specified type and returns it.
        /// </summary>
        /// <typeparam name="T"> Expected type of value. </typeparam>
        /// <returns> Value casted to specified type. </returns>
        /// <exception type="InvalidCastException">Thrown if value isn't of expected type.</exception>
        T GetValue<T>();

        /// <summary>
        ///   Tries to take the value, cast it to the specified type and return it. If value isn't of expected type, false is returned.
        /// </summary>
        /// <typeparam name="T"> Expected type of value. </typeparam>
        /// <param name="value"> Contains the value if it was casted successful; otherwise the default value of the specified type. </param>
        /// <returns> True if the value could be successful casted; otherwise, false. </returns>
        bool TryGetValue<T>(out T value);

        #endregion
    }
}