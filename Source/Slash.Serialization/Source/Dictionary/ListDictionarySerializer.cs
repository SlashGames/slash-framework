// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ListDictionarySerializer.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Serialization.Dictionary
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Runtime.Serialization;

    using Slash.Reflection.Utils;

    /// <summary>
    ///   Converts lists between dictionary representations and back.
    /// </summary>
    public class ListDictionarySerializer : IDictionarySerializer
    {
        #region Constants

        private const string DataCount = "Count";

        private const string DataType = "Type";

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
            if (!data.ContainsKey(DataCount))
            {
                throw new ArgumentException(string.Format("List property not specified: {0}", DataCount));
            }

            if (!data.ContainsKey(DataType))
            {
                throw new ArgumentException(string.Format("List property not specified: {0}", DataType));
            }

            int count = Convert.ToInt32(data[DataCount]);
            string itemTypeString = (string)data[DataType];
            Type itemType = ReflectionUtils.FindType(itemTypeString);
            if (itemType == null)
            {
                throw new SerializationException(string.Format("Item type '{0}' not found", itemTypeString));
            }

            IList list = (IList)Activator.CreateInstance(typeof(List<>).MakeGenericType(itemType));

            bool typeSealed = itemType.IsSealed;
            for (int i = 0; i < count; i++)
            {
                object valueData = data[i.ToString(CultureInfo.InvariantCulture)];

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

                list.Add(value);
            }

            return list;
        }

        /// <summary>
        ///   Serializes an object to a dictionary.
        /// </summary>
        /// <param name="context">Serialization parameters, such as custom serializers and version number.</param>
        /// <param name="obj">Object to serialize.</param>
        /// <returns>Dictionary which contains object data.</returns>
        public Dictionary<string, object> Serialize(DictionarySerializationContext context, object obj)
        {
            IList list = (IList)obj;
            Type itemType = list.GetType().GetGenericArguments()[0];

            Dictionary<string, object> data = new Dictionary<string, object>
                {
                    { DataCount, list.Count },
                    { DataType, itemType.FullName }
                };

            bool typeSealed = itemType.IsSealed;
            for (int i = 0; i < list.Count; i++)
            {
                object value = list[i];
                object valueData = typeSealed || value == null ? value : new ValueWithType(value);

                string valueKey = i.ToString(CultureInfo.InvariantCulture);
                data.Add(valueKey, context.Serialize(valueData));
            }

            return data;
        }

        #endregion
    }

    /// <summary>
    ///   Converts lists between dictionary representations and back.
    /// </summary>
    /// <typeparam name="T">Type of the list elements.</typeparam>
    public class ListDictionarySerializer<T> : IDictionarySerializer
    {
        #region Constants

        private const string DataCount = "Count";

        private const string DataType = "Type";

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
            int count = Convert.ToInt32(data[DataCount]);
            List<T> list = new List<T>(count);

            for (int i = 0; i < count; i++)
            {
                T value;

                if (context.CanSerialize(typeof(T)))
                {
                    value = (T)context.Deserialize(typeof(T), data[i.ToString(CultureInfo.InvariantCulture)]);
                }
                else
                {
                    value = (T)data[i.ToString(CultureInfo.InvariantCulture)];
                }

                list.Add(value);
            }

            return list;
        }

        /// <summary>
        ///   Serializes an object to a dictionary.
        /// </summary>
        /// <param name="context">Serialization parameters, such as custom serializers and version number.</param>
        /// <param name="obj">Object to serialize.</param>
        /// <returns>Dictionary which contains object data.</returns>
        public Dictionary<string, object> Serialize(DictionarySerializationContext context, object obj)
        {
            List<T> list = (List<T>)obj;
            Dictionary<string, object> data = new Dictionary<string, object>
                {
                    { DataCount, list.Count },
                    { DataType, typeof(T).FullName }
                };

            for (int i = 0; i < list.Count; i++)
            {
                if (context.CanSerialize(typeof(T)))
                {
                    data.Add(i.ToString(CultureInfo.InvariantCulture), context.Serialize(list[i]));
                }
                else
                {
                    data.Add(i.ToString(CultureInfo.InvariantCulture), list[i]);
                }
            }

            return data;
        }

        #endregion
    }
}