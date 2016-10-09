// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BinaryDeserializer.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Serialization.Binary
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.IO;
    using System.Reflection;
    using System.Runtime.Serialization;

    using Slash.Reflection.Extensions;
    using Slash.Reflection.Utils;
    using Slash.Serialization.Xml;

    /// <summary>
    ///   Reads objects from their binary representations.
    /// </summary>
    public class BinaryDeserializer
    {
        #region Fields

        private readonly BinaryReader reader;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///   Creates a new binary deserializer for reading objects from the specified stream.
        /// </summary>
        /// <param name="stream">Stream to read objects from.</param>
        public BinaryDeserializer(Stream stream)
        {
            this.reader = new BinaryReader(stream);
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///   Reads and returns an object of the specified type from the current stream.
        /// </summary>
        /// <typeparam name="T">Type of the object to read.</typeparam>
        /// <returns>Object of the specified type read from the current stream.</returns>
        public T Deserialize<T>()
        {
            Type type = typeof(T);
            return (T)this.Deserialize(type);
        }

        /// <summary>
        ///   Reads and returns an object of the specified type from the current stream.
        /// </summary>
        /// <param name="type">Type of the object to read.</param>
        /// <returns>Object of the specified type read from the current stream.</returns>
        public object Deserialize(Type type)
        {
            // Check if null (or default) value.
            if (!this.reader.ReadBoolean())
            {
                if (type.IsValueType)
                {
                    return Activator.CreateInstance(type);
                }

                return null;
            }

            // Check for primitive type.
            if (type.IsPrimitive())
            {
                return this.DeserializePrimitive(type);
            }

            // Check for string.
            if (type == typeof(string))
            {
                return this.reader.ReadString();
            }

            // Check for derived types.
            if (type == typeof(ValueWithType))
            {
                return this.DeserializeValueWithType();
            }

            // Check for array.
            if (type.IsArray)
            {
                return this.DeserializeArray();
            }

            // Check for list.
            if (typeof(IList).IsAssignableFrom(type))
            {
                return this.DeserializeList();
            }

            // Check for dictionary.
            if (typeof(IDictionary).IsAssignableFrom(type))
            {
                return this.DeserializeDictionary();
            }

            // Check for enum.
            if (type.IsEnum())
            {
                return Enum.Parse(type, this.reader.ReadString());
            }

            // Check for custom implementation.
            if (typeof(IBinarySerializable).IsAssignableFrom(type))
            {
                IBinarySerializable binarySerializable = (IBinarySerializable)Activator.CreateInstance(type);
                binarySerializable.Deserialize(this);
                return binarySerializable;
            }

            // Check if unspecified type.
            if (type == typeof(object))
            {
               var objectTypeFullName = this.reader.ReadString();
               var objectType = ReflectionUtils.FindType(objectTypeFullName);
               return this.Deserialize(objectType);
            }

            // Deserialize with reflection.
            try
            {
                return this.DeserializeReflection(type);
            }
            catch (Exception e)
            {
                throw new SerializationException(string.Format("Unsupported type: {0}", type.Name), e);
            }
        }

        #endregion

        #region Methods

        private object DeserializeArray()
        {
            int count = this.reader.ReadInt32();
            string itemTypeString = this.reader.ReadString();
            Type itemType = ReflectionUtils.FindType(itemTypeString);

            if (itemType == null)
            {
                throw new SerializationException(string.Format("Item type '{0}' not found", itemTypeString));
            }

            Array array = Array.CreateInstance(itemType, count);

            for (int i = 0; i < count; i++)
            {
                if (itemType.IsSealed())
                {
                    array.SetValue(this.Deserialize(itemType), i);
                }
                else
                {
                    ValueWithType valueWithType = this.DeserializeValueWithType();
                    array.SetValue(valueWithType.Value, i);
                }
            }

            return array;
        }

        private IDictionary DeserializeDictionary()
        {
            int count = this.reader.ReadInt32();

            // Create dictionary.
            string keyTypeName = this.reader.ReadString();
            string valueTypeName = this.reader.ReadString();

            Type keyType = ReflectionUtils.FindType(keyTypeName);
            Type valueType = ReflectionUtils.FindType(valueTypeName);

            // TODO(np): Deserialize arbitrary dictionary types.
            Type dictionaryType = typeof(SerializableDictionary<,>).MakeGenericType(keyType, valueType);
            IDictionary dictionary = (IDictionary)Activator.CreateInstance(dictionaryType);

            // Read data.
            for (int i = 0; i < count; i++)
            {
                object key;
                object value;

                // Read key.
                if (keyType.IsSealed())
                {
                    key = this.Deserialize(keyType);
                }
                else
                {
                    ValueWithType valueWithType = this.DeserializeValueWithType();
                    key = valueWithType.Value;
                }

                // Read value.
                if (valueType.IsSealed())
                {
                    value = this.Deserialize(valueType);
                }
                else
                {
                    ValueWithType valueWithType = this.DeserializeValueWithType();
                    value = valueWithType.Value;
                }

                dictionary.Add(key, value);
            }

            return dictionary;
        }

        private IList DeserializeList()
        {
            int count = this.reader.ReadInt32();
            string itemTypeString = this.reader.ReadString();
            Type itemType = ReflectionUtils.FindType(itemTypeString);

            if (itemType == null)
            {
                throw new SerializationException(string.Format("Item type '{0}' not found", itemTypeString));
            }

            IList list = (IList)Activator.CreateInstance(typeof(List<>).MakeGenericType(itemType));

            for (int i = 0; i < count; i++)
            {
                if (itemType.IsSealed())
                {
                    list.Add(this.Deserialize(itemType));
                }
                else
                {
                    ValueWithType valueWithType = this.DeserializeValueWithType();
                    list.Add(valueWithType.Value);
                }
            }

            return list;
        }

        private object DeserializePrimitive(Type type)
        {
            if (type == typeof(bool))
            {
                return this.reader.ReadBoolean();
            }

            if (type == typeof(byte))
            {
                return this.reader.ReadByte();
            }

            if (type == typeof(char))
            {
                return this.reader.ReadChar();
            }

            if (type == typeof(double))
            {
                return this.reader.ReadDouble();
            }

            if (type == typeof(float))
            {
                return this.reader.ReadSingle();
            }

            if (type == typeof(int))
            {
                return this.reader.ReadInt32();
            }

            if (type == typeof(long))
            {
                return this.reader.ReadInt64();
            }

            if (type == typeof(sbyte))
            {
                return this.reader.ReadSByte();
            }

            if (type == typeof(short))
            {
                return this.reader.ReadInt16();
            }

            if (type == typeof(uint))
            {
                return this.reader.ReadUInt32();
            }

            if (type == typeof(ulong))
            {
                return this.reader.ReadUInt64();
            }

            if (type == typeof(ushort))
            {
                return this.reader.ReadUInt16();
            }

            throw new ArgumentException(string.Format("Unsupported primitive type: {0}", type.Name));
        }

        private object DeserializeReflection(Type type)
        {
            // Create object instance.
            object o = Activator.CreateInstance(type);

            // Deserialize fields.
            foreach (FieldInfo field in BinarySerializationReflectionUtils.ReflectFields(type))
            {
                try
                {
                    object fieldValue = this.Deserialize(field.FieldType);
                    field.SetValue(o, fieldValue);
                }
                catch (Exception e)
                {
                    throw new SerializationException(
                        string.Format("Unable to deserialize field {0}.{1}.", type.FullName, field.Name), e);
                }
            }

            // Deserialize properties.
            foreach (PropertyInfo property in BinarySerializationReflectionUtils.ReflectProperties(type))
            {
                try
                {
                    object propertyValue = this.Deserialize(property.PropertyType);
                    property.SetValue(o, propertyValue, null);
                }
                catch (Exception e)
                {
                    throw new SerializationException(
                        string.Format("Unable to deserialize property {0}.{1}.", type.FullName, property.Name), e);
                }
            }

            return o;
        }

        private ValueWithType DeserializeValueWithType()
        {
            ValueWithType valueWithType = new ValueWithType();

            valueWithType.TypeFullName = this.reader.ReadString();
            valueWithType.Value = this.Deserialize(valueWithType.Type);

            return valueWithType;
        }

        #endregion
    }
}