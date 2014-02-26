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

            // Check for list.
            if (type.IsGenericType)
            {
                Type genericTypeDefinition = type.GetGenericTypeDefinition();

                if (genericTypeDefinition == typeof(List<>))
                {
                    return this.DeserializeList(reader);
                }
            }

            throw new SerializationException(string.Format("Unsupported type: {0}", type.Name));
        }

        private object DeserializeList(BinaryReader reader)
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
                    ValueWithType valueWithType = (ValueWithType)this.Deserialize(typeof(ValueWithType), reader);
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

        private object DeserializeValueWithType(BinaryReader reader)
        {
            ValueWithType valueWithType = new ValueWithType();

            valueWithType.TypeFullName = reader.ReadString();
            valueWithType.Value = this.Deserialize(valueWithType.Type, reader);

            return valueWithType;
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

            // Check for list.
            if (type.IsGenericType)
            {
                Type genericTypeDefinition = type.GetGenericTypeDefinition();

                if (genericTypeDefinition == typeof(List<>))
                {
                    this.SerializeList(writer, (IList)o);
                    return;
                }
            }

            throw new SerializationException(string.Format("Unsupported type: {0}", type.Name));
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

        private void SerializeValueWithType(BinaryWriter writer, object o)
        {
            ValueWithType valueWithType = (ValueWithType)o;

            writer.Write(valueWithType.TypeFullName);
            this.Serialize(writer, valueWithType.Value);
        }

        #endregion
    }
}