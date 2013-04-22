// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ValueEditorContext.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace SlashGames.Unity.Common.GUI.ValueEditors
{
    /// <summary>
    ///   Implements the basic methods.
    /// </summary>
    public abstract class ValueEditorContext
    {
        #region Public Methods and Operators

        /// <summary>
        ///   Takes the value, tries to cast it to the specified type and returns it.
        /// </summary>
        /// <typeparam name="T"> Expected type of value. </typeparam>
        /// <returns> Value casted to specified type. </returns>
        /// <exception type="InvalidCastException">Thrown if value isn't of expected type.</exception>
        public static T GetValue<T>(object objectValue)
        {
            return (T)objectValue;
        }

        /// <summary>
        ///   Tries to take the value, cast it to the specified type and return it. If value isn't of expected type, false is returned.
        /// </summary>
        /// <typeparam name="T"> Expected type of value. </typeparam>
        /// <param name="objectValue"> Object value. </param>
        /// <param name="value"> Contains the value if it was casted successful; otherwise the default value of the specified type. </param>
        /// <returns> True if the value could be successful casted; otherwise, false. </returns>
        public static bool TryGetValue<T>(object objectValue, out T value)
        {
            if (!(objectValue is T))
            {
                value = default(T);
                return false;
            }

            value = (T)objectValue;
            return true;
        }

        #endregion
    }
}