// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SimpleValueEditorContext.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Unity.Common.GUI.ValueEditors
{
    using System;

    /// <summary>
    ///   Value editor context where every property can and has to be set.
    /// </summary>
    public class SimpleValueEditorContext : IValueEditorContext
    {
        #region Public Properties

        /// <summary>
        ///   Description of the attribute.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        ///   Unique key, so the value editors can store meta data for the value over multiple frames.
        /// </summary>
        public object Key { get; set; }

        /// <summary>
        ///   Name of the attribute.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///   Value type.
        /// </summary>
        public Type Type { get; set; }

        /// <summary>
        ///   Value to edit.
        /// </summary>
        public object Value { get; set; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///   Takes the value, tries to cast it to the specified type and returns it.
        /// </summary>
        /// <typeparam name="T"> Expected type of value. </typeparam>
        /// <returns> Value casted to specified type. </returns>
        /// <exception type="InvalidCastException">Thrown if value isn't of expected type.</exception>
        public T GetValue<T>()
        {
            return ValueEditorContext.GetValue<T>(this.Value);
        }

        /// <summary>
        ///   Tries to take the value, cast it to the specified type and return it. If value isn't of expected type, false is returned.
        /// </summary>
        /// <typeparam name="T"> Expected type of value. </typeparam>
        /// <param name="value"> Contains the value if it was casted successful; otherwise the default value of the specified type. </param>
        /// <returns> True if the value could be successful casted; otherwise, false. </returns>
        public bool TryGetValue<T>(out T value)
        {
            return ValueEditorContext.TryGetValue(this.Value, out value);
        }

        #endregion
    }
}