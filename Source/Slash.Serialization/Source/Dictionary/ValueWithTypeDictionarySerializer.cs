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
    ///   Plist serializer for a value-type-pair.
    /// </summary>
    public class ValueWithTypeDictionarySerializer : IDictionarySerializer
    {
        #region Constants

        internal const string DATA_TYPE = "type";

        internal const string DATA_VALUE = "value";

        #endregion

        #region Public Methods and Operators

        public object deserialize(DictionarySerializationContext context, Dictionary<string, object> data)
        {
            ValueWithType valueWithType = new ValueWithType();

            object typeFullName;
            object value = data[DATA_VALUE];
            if (data.TryGetValue(DATA_TYPE, out typeFullName))
            {
                valueWithType.TypeFullName = (string)typeFullName;
                Type type = valueWithType.Type;
                valueWithType.value = context.deserialize(type, value);
            }
            else
            {
                valueWithType.value = value;
                valueWithType.Type = value.GetType();
            }

            return valueWithType;
        }

        public Dictionary<string, object> serialize(DictionarySerializationContext context, object obj)
        {
            ValueWithType valueWithType = (ValueWithType)obj;

            Dictionary<string, object> data = new Dictionary<string, object>
                {
                    { DATA_VALUE, context.serialize(valueWithType.value) }
                };

            if (!context.isRawSerializationPossible(valueWithType.Type))
            {
                data.Add(DATA_TYPE, valueWithType.TypeFullName);
            }

            return data;
        }

        #endregion
    }
}