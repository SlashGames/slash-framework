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
    ///   Converts stacks between dictionary representations and back.
    /// </summary>
    public class StackDictionarySerializer : IDictionarySerializer
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
            string itemTypeString = (string)data[DataType];
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
                    value = valueWithType.Value;
                }

                pushMethod.Invoke(stack, new[] { value });
            }

            return stack;
        }

        /// <summary>
        ///   Serializes an object to a dictionary.
        /// </summary>
        /// <param name="context">Serialization parameters, such as custom serializers and version number.</param>
        /// <param name="obj">Object to serialize.</param>
        /// <returns>Dictionary which contains object data.</returns>
        public Dictionary<string, object> Serialize(DictionarySerializationContext context, object obj)
        {
            ICollection collection = (ICollection)obj;
            Type itemType = collection.GetType().GetGenericArguments()[0];

            Dictionary<string, object> data = new Dictionary<string, object>
                {
                    { DataCount, collection.Count },
                    { DataType, itemType.FullName }
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
    ///   Converts stacks between dictionary representations and back.
    /// </summary>
    /// <typeparam name="T">Type of the stack elements.</typeparam>
    public class StackDictionarySerializer<T> : IDictionarySerializer
    {
        #region Constants

        private const string DataCount = "Count";

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
                    if (!(valueWithType.Value is T))
                    {
                        throw new SerializationException(
                            string.Format(
                                "Expected type {0}, but value type was of type {1}",
                                typeof(T),
                                valueWithType.Value.GetType()));
                    }
                    value = (T)valueWithType.Value;
                }
                stack.Push(value);
            }

            return stack;
        }

        /// <summary>
        ///   Serializes an object to a dictionary.
        /// </summary>
        /// <param name="context">Serialization parameters, such as custom serializers and version number.</param>
        /// <param name="obj">Object to serialize.</param>
        /// <returns>Dictionary which contains object data.</returns>
        public Dictionary<string, object> Serialize(DictionarySerializationContext context, object obj)
        {
            Stack<T> stack = (Stack<T>)obj;

            Dictionary<string, object> data = new Dictionary<string, object> { { DataCount, stack.Count } };

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