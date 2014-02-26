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

    public class ListDictionarySerializer : IDictionarySerializer
    {
        #region Constants

        private const string DATA_COUNT = "Count";

        private const string DATA_TYPE = "Type";

        #endregion

        #region Public Methods and Operators

        public object deserialize(DictionarySerializationContext context, Dictionary<string, object> data)
        {
            if (!data.ContainsKey(DATA_COUNT))
            {
                throw new ArgumentException(string.Format("List property not specified: {0}", DATA_COUNT));
            }

            if (!data.ContainsKey(DATA_TYPE))
            {
                throw new ArgumentException(string.Format("List property not specified: {0}", DATA_TYPE));
            }

            int count = Convert.ToInt32(data[DATA_COUNT]);
            string itemTypeString = (string)data[DATA_TYPE];
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
                    value = context.deserialize(itemType, valueData);
                }
                else
                {
                    ValueWithType valueWithType = context.deserialize(typeof(ValueWithType), valueData) as ValueWithType;
                    value = valueWithType.Value;
                }

                list.Add(value);
            }

            return list;
        }

        public Dictionary<string, object> serialize(DictionarySerializationContext context, object obj)
        {
            IList list = (IList)obj;
            Type itemType = list.GetType().GetGenericArguments()[0];

            Dictionary<string, object> data = new Dictionary<string, object>
                {
                    { DATA_COUNT, list.Count },
                    { DATA_TYPE, itemType.FullName }
                };

            bool typeSealed = itemType.IsSealed;
            for (int i = 0; i < list.Count; i++)
            {
                object value = list[i];
                object valueData = typeSealed || value == null ? value : new ValueWithType(value);

                string valueKey = i.ToString(CultureInfo.InvariantCulture);
                data.Add(valueKey, context.serialize(valueData));
            }

            return data;
        }

        #endregion
    }

    /// <summary>
    ///   Plist serializer for arbitrary lists.
    /// </summary>
    /// <typeparam name="T">Type of the list elements.</typeparam>
    public class ListDictionarySerializer<T> : IDictionarySerializer
    {
        #region Constants

        private const string DATA_COUNT = "Count";

        private const string DATA_TYPE = "Type";

        #endregion

        #region Public Methods and Operators

        public object deserialize(DictionarySerializationContext context, Dictionary<string, object> data)
        {
            int count = Convert.ToInt32(data[DATA_COUNT]);
            List<T> list = new List<T>(count);

            for (int i = 0; i < count; i++)
            {
                T value;

                if (context.canSerialize(typeof(T)))
                {
                    value = (T)context.deserialize(typeof(T), data[i.ToString(CultureInfo.InvariantCulture)]);
                }
                else
                {
                    value = (T)data[i.ToString(CultureInfo.InvariantCulture)];
                }

                list.Add(value);
            }

            return list;
        }

        public Dictionary<string, object> serialize(DictionarySerializationContext context, object obj)
        {
            List<T> list = (List<T>)obj;
            Dictionary<string, object> data = new Dictionary<string, object>();
            data.Add(DATA_COUNT, list.Count);
            data.Add(DATA_TYPE, typeof(T).FullName);

            for (int i = 0; i < list.Count; i++)
            {
                if (context.canSerialize(typeof(T)))
                {
                    data.Add(i.ToString(CultureInfo.InvariantCulture), context.serialize(list[i]));
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