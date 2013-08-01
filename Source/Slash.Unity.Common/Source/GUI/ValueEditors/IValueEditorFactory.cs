// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IValueEditorFactory.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Unity.Common.GUI.ValueEditors
{
    using System;

    public interface IValueEditorFactory
    {
        #region Public Methods and Operators

        /// <summary>
        ///   Returns the value editor for a value of the specified type.
        /// </summary>
        /// <typeparam name="T">Value type to get editor for.</typeparam>
        /// <returns>Value editor for specified type.</returns>
        IValueEditor GetEditor<T>();

        /// <summary>
        ///   Returns the value editor for a value of the specified type.
        /// </summary>
        /// <param name="type">Value type to get editor for.</param>
        /// <returns>Value editor for specified type.</returns>
        IValueEditor GetEditor(Type type);

        #endregion
    }
}