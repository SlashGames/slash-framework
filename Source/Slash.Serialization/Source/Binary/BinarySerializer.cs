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

            if (type.IsGenericType)
            {
                Type genericTypeDefinition = type.GetGenericTypeDefinition();

                // Check for list.
                if (genericTypeDefinition == typeof(List<>))
                {
                    return this.DeserializeList(reader);
                }

                // Check for dictionary.
                if (genericTypeDefinition == typeof(Dictionary<,>))
                {
                    return this.DeserializeDictionary(reader);
                }
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

        private IDictionary DeserializeDictionary(BinaryReader reader)
        {
            int count = reader.ReadInt32();

            // Create dictionary.
            string keyTypeName = reader.ReadString();
            string valueTypeName = reader.ReadString();

            Type keyType = ReflectionUtils.FindType(keyTypeName);
            Type valueType = ReflectionUtils.FindType(valueTypeName);

            Type dictionaryType = typeof(Dictionary<,>).MakeGenericType(keyType, valueType);
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
            FieldInfo[] fields = this.ReflectFields(type);

            foreach (FieldInfo field in fields)
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
            PropertyInfo[] properties = this.ReflectProperties(type);

            foreach (PropertyInfo property in properties)
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

        private FieldInfo[] ReflectFields(Type type)
        {
            FieldInfo[] fields = type.GetFields(BindingFlags.Public | BindingFlags.Instance);
            Array.Sort(fields, (first, second) => string.Compare(first.Name, second.Name, StringComparison.Ordinal));
            return fields;
        }

        private PropertyInfo[] ReflectProperties(Type type)
        {
            PropertyInfo[] properties = type.GetProperties();
            Array.Sort(properties, (first, second) => string.Compare(first.Name, second.Name, StringComparison.Ordinal));
            return properties;
        }

        private void Serialize(BinaryWriter writer, object o)
        {
            Type type = o.GetType();

            // Check for primitive types.
            if (type.IsPrimitive)
            {
                this.SerializePrimitive(writer, o);
                return;
            }

            // Check for string.
            string s = o as string;

            if (s != null)
            {
                writer.Write(s);
                return;
            }

            // Check for derived types.
            if (o is ValueWithType)
            {
                this.SerializeValueWithType(writer, o);
                return;
            }

            if (type.IsGenericType)
            {
                Type genericTypeDefinition = type.GetGenericTypeDefinition();

                // Check for list.
                if (genericTypeDefinition == typeof(List<>))
                {
                    this.SerializeList(writer, (IList)o);
                    return;
                }

                // Check for dictionary.
                if (genericTypeDefinition == typeof(Dictionary<,>))
                {
                    this.SerializeDictionary(writer, (IDictionary)o);
                    return;
                }
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
                object keyData = keyType.IsSealed || key == null ? key : new ValueWithType(key);
                this.Serialize(writer, keyData);

                // Write value.
                object value = dictionary[key];
                object valueData = valueType.IsSealed || value == null ? value : new ValueWithType(value);
                this.Serialize(writer, valueData);
            }
        }

        private void SerializeList(BinaryWriter writer, IList list)
        {
            Type itemType = list.GetType().GetGenericArguments()[0];

            writer.Write(list.Count);
            writer.Write(itemType.FullName);

            foreach (object item in list)
            {
                object itemData = itemType.IsSealed || item == null ? item : new ValueWithType(item);
                this.Serialize(writer, itemData);
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
            FieldInfo[] fields = this.ReflectFields(type);

            foreach (FieldInfo field in fields)
            {
                object fieldValue = field.GetValue(o);
                this.Serialize(writer, fieldValue);
            }

            // Serialize properties.
            PropertyInfo[] properties = this.ReflectProperties(type);

            foreach (PropertyInfo property in properties)
            {
                object propertyValue = property.GetGetMethod().Invoke(o, null);
                this.Serialize(writer, propertyValue);
            }
        }

        private void SerializeValueWithType(BinaryWriter writer, object o)
        {
            ValueWithType valueWithType = (ValueWithType)o;

            writer.Write(valueWithType.TypeFullName);
            this.Serialize(writer, valueWithType.Value);
        }

        #endregion
    }
}