// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ValueWithTypeDictionarySerializer.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Serialization.Dictionary
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    ///   Converts value-type-pairs between dictionary representations and back.
    /// </summary>
    public class ValueWithTypeDictionarySerializer : IDictionarySerializer
    {
        #region Constants

        internal const string DataType = "type";

        internal const string DataValue = "value";

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///   Deserializes an object from a dictionary.
        /// </summary>
        /// <param name="context">Serialization parameters, such as custom serializers and version number.</param>
        /// <param name="data">Dictionary which contains the object data.</param>
        /// <returns>Deserialized object.</returns>
        public object Deserialize(DictionarySerializationContext context, Dictionary<string, object> data)
        {
            ValueWithType valueWithType = new ValueWithType();

            object typeFullName;
            object value = data[DataValue];
            if (data.TryGetValue(DataType, out typeFullName))
            {
                valueWithType.TypeFullName = (string)typeFullName;
                Type type = valueWithType.Type;
                valueWithType.Value = context.deserialize(type, value);
            }
            else
            {
                valueWithType.Value = value;
                valueWithType.Type = value.GetType();
            }

            return valueWithType;
        }

        /// <summary>
        ///   Serializes an object to a dictionary.
        /// </summary>
        /// <param name="context">Serialization parameters, such as custom serializers and version number.</param>
        /// <param name="obj">Object to serialize.</param>
        /// <returns>Dictionary which contains object data.</returns>
        public Dictionary<string, object> Serialize(DictionarySerializationContext context, object obj)
        {
            ValueWithType valueWithType = (ValueWithType)obj;

            Dictionary<string, object> data = new Dictionary<string, object>
                {
                    { DataValue, context.serialize(valueWithType.Value) }
                };

            if (!context.isRawSerializationPossible(valueWithType.Type))
            {
                data.Add(DataType, valueWithType.TypeFullName);
            }

            return data;
        }

        #endregion
    }
}