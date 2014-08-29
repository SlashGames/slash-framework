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

    using Slash.Reflection.Extensions;
    using Slash.Reflection.Utils;

    /// <summary>
    ///   Dictionary serializer for dictionaries with string keys.
    /// </summary>
    public class DictionaryDictionarySerializer : IDictionarySerializer
    {
        #region Constants

        private const string DataPairs = "Pairs";

        private const string DataType = "Type";

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///   Reads the object from the specified dictionary.
        /// </summary>
        /// <param name="context">Serialization parameters, such as custom serializers and version number.</param>
        /// <param name="data">Object to read.</param>
        /// <returns>Read object.</returns>
        public object Deserialize(DictionarySerializationContext context, Dictionary<string, object> data)
        {
            string itemTypeString = (string)data[DataType];
            Type itemType = ReflectionUtils.FindType(itemTypeString);
            if (itemType == null)
            {
                throw new SerializationException(string.Format("Item type '{0}' not found", itemTypeString));
            }

            Type genericType = typeof(Dictionary<,>).MakeGenericType(typeof(string), itemType);
            IDictionary dictionary = (IDictionary)Activator.CreateInstance(genericType);

            bool typeSealed = itemType.IsSealed();
            Dictionary<string, object> pairs = (Dictionary<string, object>)data[DataPairs];
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
                    value = context.Deserialize(itemType, valueData);
                }
                else
                {
                    ValueWithType valueWithType = context.Deserialize(typeof(ValueWithType), valueData) as ValueWithType;
                    value = valueWithType.Value;
                }

                dictionary.Add(pair.Key, value);
            }

            return dictionary;
        }

        /// <summary>
        ///   Converts the specified object into a dictionary.
        /// </summary>
        /// <param name="context">Serialization parameters, such as custom serializers and version number.</param>
        /// <param name="obj">Object to convert.</param>
        /// <returns>Dictionary representation of the specified object.</returns>
        public Dictionary<string, object> Serialize(DictionarySerializationContext context, object obj)
        {
            IDictionary dictionary = (IDictionary)obj;
            Type keyType = dictionary.GetType().GetGenericArguments()[0];
            if (keyType != typeof(string))
            {
                throw new SerializationException("Only dictionaries with string keys can be serialized right now");
            }

            Type valueType = dictionary.GetType().GetGenericArguments()[1];

            Dictionary<string, object> data = new Dictionary<string, object> { { DataType, valueType.FullName } };

            bool typeSealed = valueType.IsSealed();

            Dictionary<string, object> keyValuePairs = new Dictionary<string, object>();
            foreach (string key in dictionary.Keys)
            {
                object value = dictionary[key];
                object valueData = typeSealed || value == null ? value : new ValueWithType(value);
                keyValuePairs.Add(key, context.Serialize(valueData));
            }
            data.Add(DataPairs, keyValuePairs);

            return data;
        }

        #endregion
    }
}