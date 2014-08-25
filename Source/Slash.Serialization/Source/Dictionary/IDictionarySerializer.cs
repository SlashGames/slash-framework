// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IDictionarySerializer.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Serialization.Dictionary
{
    using System.Collections.Generic;

    /// <summary>
    ///   Converts objects between dictionary representations and back.
    /// </summary>
    public interface IDictionarySerializer
    {
        #region Public Methods and Operators

        /// <summary>
        ///   Deserializes an object from a dictionary.
        /// </summary>
        /// <param name="context">Serialization parameters, such as custom serializers and version number.</param>
        /// <param name="data">Dictionary which contains the object data.</param>
        /// <returns>Deserialized object.</returns>
        object Deserialize(DictionarySerializationContext context, Dictionary<string, object> data);

        /// <summary>
        ///   Serializes an object to a dictionary.
        /// </summary>
        /// <param name="context">Serialization parameters, such as custom serializers and version number.</param>
        /// <param name="obj">Object to serialize.</param>
        /// <returns>Dictionary which contains object data.</returns>
        Dictionary<string, object> Serialize(DictionarySerializationContext context, object obj);

        #endregion
    }
}