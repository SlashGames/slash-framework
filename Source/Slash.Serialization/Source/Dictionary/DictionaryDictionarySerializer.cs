// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DictionaryDictionarySerializer.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Serialization.Dictionary
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    using Slash.Reflection.Utils;

    /// <summary>
    ///   Dictionary serializer for dictionaries with string keys.
    /// </summary>
    public class DictionaryDictionarySerializer : IDictionarySerializer
    {
        #region Constants

        private const string DATA_PAIRS = "Pairs";

        private const string DATA_TYPE = "Type";

        #endregion

        #region Public Methods and Operators

        public object deserialize(DictionarySerializationContext context, Dictionary<string, object> data)
        {
            string itemTypeString = (string)data[DATA_TYPE];
            Type itemType = ReflectionUtils.FindType(itemTypeString);
            if (itemType == null)
            {
                throw new SerializationException(string.Format("Item type '{0}' not found", itemTypeString));
            }

            Type genericType = typeof(Dictionary<,>).MakeGenericType(typeof(string), itemType);
            IDictionary dictionary = (IDictionary)Activator.CreateInstance(genericType);

            bool typeSealed = itemType.IsSealed;
            Dictionary<string, object> pairs = (Dictionary<string, object>)data[DATA_PAIRS];
            foreach (KeyValuePair<string, object> pair in pairs)
            {
                object valueData = pair.Value;
                object value;
                if (valueData == null)
                {
                    value = null;
                }
                else if (typeSealed)
                {
                    value = context.deserialize(itemType, valueData);
                }
                else
                {
                    ValueWithType valueWithType = context.deserialize(typeof(ValueWithType), valueData) as ValueWithType;
                    value = valueWithType.value;
                }

                dictionary.Add(pair.Key, value);
            }

            return dictionary;
        }

        public Dictionary<string, object> serialize(DictionarySerializationContext context, object obj)
        {
            IDictionary dictionary = (IDictionary)obj;
            Type keyType = dictionary.GetType().GetGenericArguments()[0];
            if (keyType != typeof(string))
            {
                throw new SerializationException("Only dictionaries with string keys can be serialized right now");
            }

            Type valueType = dictionary.GetType().GetGenericArguments()[1];

            Dictionary<string, object> data = new Dictionary<string, object> { { DATA_TYPE, valueType.FullName } };

            bool typeSealed = valueType.IsSealed;

            Dictionary<string, object> keyValuePairs = new Dictionary<string, object>();
            foreach (string key in dictionary.Keys)
            {
                object value = dictionary[key];
                object valueData = typeSealed || value == null ? value : new ValueWithType(value);
                keyValuePairs.Add(key, context.serialize(valueData));
            }
            data.Add(DATA_PAIRS, keyValuePairs);

            return data;
        }

        #endregion
    }
}