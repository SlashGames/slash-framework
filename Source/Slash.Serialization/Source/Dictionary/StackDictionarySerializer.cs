// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StackDictionarySerializer.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Serialization.Dictionary
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Reflection;
    using System.Runtime.Serialization;

    using Slash.Reflection.Utils;

    /// <summary>
    ///   Dictionary serializer for arbitrary stacks.
    /// </summary>
    public class StackDictionarySerializer : IDictionarySerializer
    {
        #region Constants

        private const string DATA_COUNT = "Count";

        private const string DATA_TYPE = "Type";

        #endregion

        #region Public Methods and Operators

        public object deserialize(DictionarySerializationContext context, Dictionary<string, object> data)
        {
            int count = Convert.ToInt32(data[DATA_COUNT]);
            string itemTypeString = (string)data[DATA_TYPE];
            Type itemType = ReflectionUtils.FindType(itemTypeString);
            if (itemType == null)
            {
                throw new SerializationException(string.Format("Item type '{0}' not found", itemTypeString));
            }

            Type genericType = typeof(Stack<>).MakeGenericType(itemType);
            object stack = Activator.CreateInstance(genericType);

            bool typeSealed = itemType.IsSealed;
            MethodInfo pushMethod = genericType.GetMethod("Push");
            for (int i = count - 1; i >= 0; --i)
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
                    value = valueWithType.value;
                }

                pushMethod.Invoke(stack, new[] { value });
            }

            return stack;
        }

        public Dictionary<string, object> serialize(DictionarySerializationContext context, object obj)
        {
            ICollection collection = (ICollection)obj;
            Type itemType = collection.GetType().GetGenericArguments()[0];

            Dictionary<string, object> data = new Dictionary<string, object>
                {
                    { DATA_COUNT, collection.Count },
                    { DATA_TYPE, itemType.FullName }
                };

            bool typeSealed = itemType.IsSealed;

            int index = 0;
            foreach (object value in collection)
            {
                object valueData = typeSealed || value == null ? value : new ValueWithType(value);
                data.Add(index.ToString(CultureInfo.InvariantCulture), context.serialize(valueData));
                ++index;
            }

            return data;
        }

        #endregion
    }

    /// <summary>
    ///   Dictionary serializer for arbitrary stacks.
    /// </summary>
    /// <typeparam name="T">Type of the stack elements.</typeparam>
    public class StackDictionarySerializer<T> : IDictionarySerializer
    {
        #region Constants

        private const string DATA_COUNT = "Count";

        #endregion

        #region Public Methods and Operators

        public object deserialize(DictionarySerializationContext context, Dictionary<string, object> data)
        {
            int count = Convert.ToInt32(data[DATA_COUNT]);
            Stack<T> stack = new Stack<T>(count);

            bool typeSealed = typeof(T).IsSealed;
            for (int i = count - 1; i >= 0; --i)
            {
                object valueData = data[i.ToString(CultureInfo.InvariantCulture)];
                T value;
                if (typeSealed)
                {
                    value = (T)context.deserialize(typeof(T), valueData);
                }
                else
                {
                    ValueWithType valueWithType = context.deserialize(typeof(ValueWithType), valueData) as ValueWithType;
                    if (!(valueWithType.value is T))
                    {
                        throw new SerializationException(
                            string.Format(
                                "Expected type {0}, but value type was of type {1}",
                                typeof(T),
                                valueWithType.value.GetType()));
                    }
                    value = (T)valueWithType.value;
                }
                stack.Push(value);
            }

            return stack;
        }

        public Dictionary<string, object> serialize(DictionarySerializationContext context, object obj)
        {
            Stack<T> stack = (Stack<T>)obj;

            Dictionary<string, object> data = new Dictionary<string, object> { { DATA_COUNT, stack.Count } };

            bool typeSealed = typeof(T).IsSealed;

            int index = 0;
            foreach (T value in stack)
            {
                object valueData = typeSealed || value == null ? (object)value : new ValueWithType(value);
                data.Add(index.ToString(CultureInfo.InvariantCulture), context.serialize(valueData));
                ++index;
            }

            return data;
        }

        #endregion
    }
}