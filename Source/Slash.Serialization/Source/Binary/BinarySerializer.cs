// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BinarySerializer.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Serialization.Binary
{
    using System;
    using System.Collections;
    using System.IO;
    using System.Reflection;
    using System.Runtime.Serialization;

    using Slash.Reflection.Extensions;
    using Slash.SystemExt.Utils;

    /// <summary>
    ///   Converts objects to their binary representations.
    /// </summary>
    public class BinarySerializer
    {
        #region Fields

        private readonly BinaryWriter writer;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///   Creates a new binary serializer for writing objects to the specified stream.
        /// </summary>
        /// <param name="stream">Stream to write objects to.</param>
        public BinarySerializer(Stream stream)
        {
            this.writer = new BinaryWriter(stream);
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///   Writes the specified object to the current stream.
        /// </summary>
        /// <param name="o">Object to serialize.</param>
        public void Serialize(object o)
        {
            if (o == null)
            {
                throw new ArgumentNullException("o");
            }

            Type type = o.GetType();
            this.Serialize(o, type);
        }

        #endregion

        #region Methods

        private void Serialize(object o, Type type)
        {
            // Check for primitive types.
            if (type.IsPrimitive())
            {
                this.SerializePrimitive(o);
                return;
            }

            // Check for string.
            if (type == typeof(string))
            {
                string s = (string)o;
                this.writer.Write(string.IsNullOrEmpty((string)o) ? string.Empty : s);
                return;
            }

            // Check for derived types.
            if (type == typeof(ValueWithType))
            {
                this.SerializeValueWithType((ValueWithType)o);
                return;
            }

            // Check for list.
            if (typeof(IList).IsAssignableFrom(type))
            {
                this.SerializeList((IList)o);
                return;
            }

            // Check for dictionary.
            if (typeof(IDictionary).IsAssignableFrom(type))
            {
                this.SerializeDictionary((IDictionary)o);
                return;
            }

            // Check for enum.
            if (type.IsEnum())
            {
                this.writer.Write(o.ToString());
                return;
            }

            // Check for custom implementation.
            if (typeof(IBinarySerializable).IsAssignableFrom(type))
            {
                IBinarySerializable binarySerializable = (IBinarySerializable)o;
                binarySerializable.Serialize(this);
                return;
            }

            // Serialize with reflection.
            try
            {
                this.SerializeReflection(o);
            }
            catch (Exception e)
            {
                throw new SerializationException(string.Format("Unsupported type: {0}", type.Name), e);
            }
        }

        private void SerializeDictionary(IDictionary dictionary)
        {
            Type keyType = dictionary.GetType().GetGenericArguments()[0];
            Type valueType = dictionary.GetType().GetGenericArguments()[1];

            this.writer.Write(dictionary.Count);
            this.writer.Write(keyType.FullName);
            this.writer.Write(valueType.FullName);

            foreach (object key in dictionary.Keys)
            {
                // Write key.
                if (keyType.IsSealed() || key == null)
                {
                    this.Serialize(key, keyType);
                }
                else
                {
                    this.SerializeValueWithType(new ValueWithType(key));
                }

                // Write value.
                object value = dictionary[key];

                if (valueType.IsSealed() || value == null)
                {
                    this.Serialize(value, valueType);
                }
                else
                {
                    this.SerializeValueWithType(new ValueWithType(value));
                }
            }
        }

        private void SerializeList(IList list)
        {
            Type listType = list.GetType();
            Type itemType = listType.IsArray ? listType.GetElementType() : listType.GetGenericArguments()[0];

            this.writer.Write(list.Count);
            this.writer.Write(itemType.FullName);

            foreach (object item in list)
            {
                if (itemType.IsSealed() || item == null)
                {
                    this.Serialize(item, itemType);
                }
                else
                {
                    this.SerializeValueWithType(new ValueWithType(item));
                }
            }
        }

        private void SerializePrimitive(object o)
        {
            if (o is bool)
            {
                this.writer.Write((bool)o);
            }
            else if (o is byte)
            {
                this.writer.Write((byte)o);
            }
            else if (o is char)
            {
                this.writer.Write((char)o);
            }
            else if (o is double)
            {
                this.writer.Write((double)o);
            }
            else if (o is float)
            {
                this.writer.Write((float)o);
            }
            else if (o is int)
            {
                this.writer.Write((int)o);
            }
            else if (o is long)
            {
                this.writer.Write((long)o);
            }
            else if (o is sbyte)
            {
                this.writer.Write((sbyte)o);
            }
            else if (o is short)
            {
                this.writer.Write((short)o);
            }
            else if (o is uint)
            {
                this.writer.Write((uint)o);
            }
            else if (o is ulong)
            {
                this.writer.Write((ulong)o);
            }
            else if (o is ushort)
            {
                this.writer.Write((ushort)o);
            }
            else
            {
                throw new ArgumentException(string.Format("Unsupported primitive type: {0}", o.GetType().Name));
            }
        }

        private void SerializeReflection(object o)
        {
            Type type = o.GetType();

            // Serialize fields.
            foreach (FieldInfo field in BinarySerializationReflectionUtils.ReflectFields(type))
            {
                object fieldValue = field.GetValue(o);
                this.Serialize(fieldValue, field.FieldType);
            }

            // Serialize properties.
            foreach (PropertyInfo property in BinarySerializationReflectionUtils.ReflectProperties(type))
            {
                object propertyValue = property.GetGetMethod().Invoke(o, null);
                this.Serialize(propertyValue, property.PropertyType);
            }
        }

        private void SerializeValueWithType(ValueWithType valueWithType)
        {
            string typeName = SystemExtensions.RemoveAssemblyInfo(valueWithType.TypeFullName);
            this.writer.Write(typeName);
            this.Serialize(valueWithType.Value, valueWithType.Type);
        }

        #endregion
    }
}