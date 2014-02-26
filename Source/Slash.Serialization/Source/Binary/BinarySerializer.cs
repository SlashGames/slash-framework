// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BinarySerializer.cs" company="Slash Games">
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

    using Slash.Reflection.Utils;
    using Slash.Serialization.Xml;

    public class BinarySerializer
    {
        #region Public Methods and Operators

        public T Deserialize<T>(Stream stream)
        {
            Type type = typeof(T);
            return (T)this.Deserialize(type, stream);
        }

        public object Deserialize(Type type, Stream stream)
        {
            BinaryReader reader = new BinaryReader(stream);
            return this.Deserialize(type, reader);
        }

        public void Serialize(Stream stream, object o)
        {
            BinaryWriter writer = new BinaryWriter(stream);
            this.Serialize(writer, o);
        }

        #endregion

        #region Methods

        private object Deserialize(Type type, BinaryReader reader)
        {
            // Check for primitive type.
            if (type.IsPrimitive)
            {
                return this.DeserializePrimitive(type, reader);
            }

            // Check for string.
            if (type == typeof(string))
            {
                return reader.ReadString();
            }

            // Check for derived types.
            if (type == typeof(ValueWithType))
            {
                return this.DeserializeValueWithType(reader);
            }

            // Check for array.
            if (type.IsArray)
            {
                return this.DeserializeArray(reader);
            }

            // Check for list.
            if (typeof(IList).IsAssignableFrom(type))
            {
                return this.DeserializeList(reader);
            }

            // Check for dictionary.
            if (typeof(IDictionary).IsAssignableFrom(type))
            {
                return this.DeserializeDictionary(reader);
            }

            // Check for enum.
            if (type.IsEnum)
            {
                return Enum.Parse(type, reader.ReadString());
            }

            // Deserialize with reflection.
            try
            {
                return this.DeserializeReflection(type, reader);
            }
            catch (Exception e)
            {
                throw new SerializationException(string.Format("Unsupported type: {0}", type.Name), e);
            }
        }

        private object DeserializeArray(BinaryReader reader)
        {
            int count = reader.ReadInt32();
            string itemTypeString = reader.ReadString();
            Type itemType = ReflectionUtils.FindType(itemTypeString);

            if (itemType == null)
            {
                throw new SerializationException(string.Format("Item type '{0}' not found", itemTypeString));
            }

            Array array = Array.CreateInstance(itemType, count);

            for (int i = 0; i < count; i++)
            {
                if (itemType.IsSealed)
                {
                    array.SetValue(this.Deserialize(itemType, reader), i);
                }
                else
                {
                    ValueWithType valueWithType = this.DeserializeValueWithType(reader);
                    array.SetValue(valueWithType.Value, i);
                }
            }

            return array;
        }

        private IDictionary DeserializeDictionary(BinaryReader reader)
        {
            int count = reader.ReadInt32();

            // Create dictionary.
            string keyTypeName = reader.ReadString();
            string valueTypeName = reader.ReadString();

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
                if (keyType.IsSealed)
                {
                    key = this.Deserialize(keyType, reader);
                }
                else
                {
                    ValueWithType valueWithType = this.DeserializeValueWithType(reader);
                    key = valueWithType.Value;
                }

                // Read value.
                if (valueType.IsSealed)
                {
                    value = this.Deserialize(valueType, reader);
                }
                else
                {
                    ValueWithType valueWithType = this.DeserializeValueWithType(reader);
                    value = valueWithType.Value;
                }

                dictionary.Add(key, value);
            }

            return dictionary;
        }

        private IList DeserializeList(BinaryReader reader)
        {
            int count = reader.ReadInt32();
            string itemTypeString = reader.ReadString();
            Type itemType = ReflectionUtils.FindType(itemTypeString);

            if (itemType == null)
            {
                throw new SerializationException(string.Format("Item type '{0}' not found", itemTypeString));
            }

            IList list = (IList)Activator.CreateInstance(typeof(List<>).MakeGenericType(itemType));

            for (int i = 0; i < count; i++)
            {
                if (itemType.IsSealed)
                {
                    list.Add(this.Deserialize(itemType, reader));
                }
                else
                {
                    ValueWithType valueWithType = this.DeserializeValueWithType(reader);
                    list.Add(valueWithType.Value);
                }
            }

            return list;
        }

        private object DeserializePrimitive(Type type, BinaryReader reader)
        {
            if (type == typeof(bool))
            {
                return reader.ReadBoolean();
            }

            if (type == typeof(byte))
            {
                return reader.ReadByte();
            }

            if (type == typeof(char))
            {
                return reader.ReadChar();
            }

            if (type == typeof(double))
            {
                return reader.ReadDouble();
            }

            if (type == typeof(float))
            {
                return reader.ReadSingle();
            }

            if (type == typeof(int))
            {
                return reader.ReadInt32();
            }

            if (type == typeof(long))
            {
                return reader.ReadInt64();
            }

            if (type == typeof(sbyte))
            {
                return reader.ReadSByte();
            }

            if (type == typeof(short))
            {
                return reader.ReadInt16();
            }

            if (type == typeof(uint))
            {
                return reader.ReadUInt32();
            }

            if (type == typeof(ulong))
            {
                return reader.ReadUInt64();
            }

            if (type == typeof(ushort))
            {
                return reader.ReadUInt16();
            }

            throw new ArgumentException(string.Format("Unsupported primitive type: {0}", type.Name));
        }

        private object DeserializeReflection(Type type, BinaryReader reader)
        {
            // Create object instance.
            object o = Activator.CreateInstance(type);

            // Deserialize fields.
            foreach (FieldInfo field in this.ReflectFields(type))
            {
                try
                {
                    object fieldValue = this.Deserialize(field.FieldType, reader);
                    field.SetValue(o, fieldValue);
                }
                catch (Exception e)
                {
                    throw new SerializationException(
                        string.Format("Unable to deserialize field {0}.{1}.", type.FullName, field.Name), e);
                }
            }

            // Deserialize properties.
            foreach (PropertyInfo property in this.ReflectProperties(type))
            {
                try
                {
                    object propertyValue = this.Deserialize(property.PropertyType, reader);
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

        private ValueWithType DeserializeValueWithType(BinaryReader reader)
        {
            ValueWithType valueWithType = new ValueWithType();

            valueWithType.TypeFullName = reader.ReadString();
            valueWithType.Value = this.Deserialize(valueWithType.Type, reader);

            return valueWithType;
        }

        private IEnumerable<FieldInfo> ReflectFields(Type type)
        {
            FieldInfo[] fields = type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

            // Sort fields by name to prevent re-ordering members from being a breaking change.
            Array.Sort(fields, (first, second) => string.Compare(first.Name, second.Name, StringComparison.Ordinal));

            foreach (FieldInfo field in fields)
            {
                if (Attribute.IsDefined(field, typeof(SerializeMemberAttribute)))
                {
                    yield return field;
                }
            }
        }

        private IEnumerable<PropertyInfo> ReflectProperties(Type type)
        {
            PropertyInfo[] properties =
                type.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

            // Sort properties by name to prevent re-ordering members from being a breaking change.
            Array.Sort(properties, (first, second) => string.Compare(first.Name, second.Name, StringComparison.Ordinal));

            foreach (PropertyInfo property in properties)
            {
                if (Attribute.IsDefined(property, typeof(SerializeMemberAttribute)))
                {
                    yield return property;
                }
            }
        }

        private void Serialize(BinaryWriter writer, object o)
        {
            if (o == null)
            {
                throw new ArgumentNullException("o");
            }

            Type type = o.GetType();
            this.Serialize(writer, o, type);
        }

        private void Serialize(BinaryWriter writer, object o, Type type)
        {
            // Check for primitive types.
            if (type.IsPrimitive)
            {
                this.SerializePrimitive(writer, o);
                return;
            }

            // Check for string.
            if (type == typeof(string))
            {
                string s = (string)o;
                writer.Write(string.IsNullOrEmpty((string)o) ? string.Empty : s);
                return;
            }

            // Check for derived types.
            if (type == typeof(ValueWithType))
            {
                this.SerializeValueWithType(writer, (ValueWithType)o);
                return;
            }

            // Check for list.
            if (typeof(IList).IsAssignableFrom(type))
            {
                this.SerializeList(writer, (IList)o);
                return;
            }

            // Check for dictionary.
            if (typeof(IDictionary).IsAssignableFrom(type))
            {
                this.SerializeDictionary(writer, (IDictionary)o);
                return;
            }

            // Check for enum.
            if (type.IsEnum)
            {
                writer.Write(o.ToString());
                return;
            }

            // Serialize with reflection.
            try
            {
                this.SerializeReflection(writer, o);
            }
            catch (Exception e)
            {
                throw new SerializationException(string.Format("Unsupported type: {0}", type.Name), e);
            }
        }

        private void SerializeDictionary(BinaryWriter writer, IDictionary dictionary)
        {
            Type keyType = dictionary.GetType().GetGenericArguments()[0];
            Type valueType = dictionary.GetType().GetGenericArguments()[1];

            writer.Write(dictionary.Count);
            writer.Write(keyType.FullName);
            writer.Write(valueType.FullName);

            foreach (object key in dictionary.Keys)
            {
                // Write key.
                if (keyType.IsSealed || key == null)
                {
                    this.Serialize(writer, key, keyType);
                }
                else
                {
                    this.SerializeValueWithType(writer, new ValueWithType(key));
                }

                // Write value.
                object value = dictionary[key];

                if (valueType.IsSealed || value == null)
                {
                    this.Serialize(writer, value, valueType);
                }
                else
                {
                    this.SerializeValueWithType(writer, new ValueWithType(value));
                }
            }
        }

        private void SerializeList(BinaryWriter writer, IList list)
        {
            Type listType = list.GetType();
            Type itemType = listType.IsArray ? listType.GetElementType() : listType.GetGenericArguments()[0];

            writer.Write(list.Count);
            writer.Write(itemType.FullName);

            foreach (object item in list)
            {
                if (itemType.IsSealed || item == null)
                {
                    this.Serialize(writer, item, itemType);
                }
                else
                {
                    this.SerializeValueWithType(writer, new ValueWithType(item));
                }
            }
        }

        private void SerializePrimitive(BinaryWriter writer, object o)
        {
            if (o is bool)
            {
                writer.Write((bool)o);
            }
            else if (o is byte)
            {
                writer.Write((byte)o);
            }
            else if (o is char)
            {
                writer.Write((char)o);
            }
            else if (o is double)
            {
                writer.Write((double)o);
            }
            else if (o is float)
            {
                writer.Write((float)o);
            }
            else if (o is int)
            {
                writer.Write((int)o);
            }
            else if (o is long)
            {
                writer.Write((long)o);
            }
            else if (o is sbyte)
            {
                writer.Write((sbyte)o);
            }
            else if (o is short)
            {
                writer.Write((short)o);
            }
            else if (o is uint)
            {
                writer.Write((uint)o);
            }
            else if (o is ulong)
            {
                writer.Write((ulong)o);
            }
            else if (o is ushort)
            {
                writer.Write((ushort)o);
            }
            else
            {
                throw new ArgumentException(string.Format("Unsupported primitive type: {0}", o.GetType().Name));
            }
        }

        private void SerializeReflection(BinaryWriter writer, object o)
        {
            Type type = o.GetType();

            // Serialize fields.
            foreach (FieldInfo field in this.ReflectFields(type))
            {
                object fieldValue = field.GetValue(o);
                this.Serialize(writer, fieldValue, field.FieldType);
            }

            // Serialize properties.
            foreach (PropertyInfo property in this.ReflectProperties(type))
            {
                object propertyValue = property.GetGetMethod().Invoke(o, null);
                this.Serialize(writer, propertyValue, property.PropertyType);
            }
        }

        private void SerializeValueWithType(BinaryWriter writer, ValueWithType valueWithType)
        {
            writer.Write(valueWithType.TypeFullName);
            this.Serialize(writer, valueWithType.Value, valueWithType.Type);
        }

        #endregion
    }
}