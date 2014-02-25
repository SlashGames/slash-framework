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
    using Slash.Serialization.Dictionary;

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
            if (type.IsPrimitive)
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

            // Check for string.
            if (type == typeof(string))
            {
                return reader.ReadString();
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
                object itemData;
                object item;

                if (itemType.IsSealed)
                {
                    list.Add(this.Deserialize(itemType, reader));
                }
                else
                {
                    ValueWithType valueWithType = (ValueWithType)this.Deserialize(typeof(ValueWithType), reader);
                    list.Add(valueWithType.value);
                }
            }

            return list;
        }

        private void Serialize(BinaryWriter writer, object o)
        {
            Type type = o.GetType();

            // Check for primitive types.
            if (type.IsPrimitive)
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
                    throw new ArgumentException(string.Format("Unsupported primitive type: {0}", type.Name));
                }

                return;
            }

            // Check for string.
            string s = o as string;

            if (s != null)
            {
                writer.Write(s);
                return;
            }

            // Check for list.
            if (type.IsGenericType)
            {
                Type genericTypeDefinition = type.GetGenericTypeDefinition();

                if (genericTypeDefinition == typeof(List<>))
                {
                    this.Serialize(writer, (IList)o);
                    return;
                }
            }

            throw new SerializationException(string.Format("Unsupported type: {0}", type.Name));
        }

        private void Serialize(BinaryWriter writer, IList list)
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

        #endregion
    }
}