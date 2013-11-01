// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DictionarySerializationContext.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Serialization.Dictionary
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using System.Runtime.Serialization;

    /// <summary>
    ///   Context for serializing and deserializing arbitrary types as dictionaries to Plists.
    /// </summary>
    public class DictionarySerializationContext
    {
        #region Constants

        /// <summary>
        ///   Serialization version.
        /// </summary>
        private const string DATA_VERSION = "version";

        #endregion

        #region Fields

        /// <summary>
        ///   Maps generic types to serializers for these generic types.
        /// </summary>
        private readonly Dictionary<Type, IDictionarySerializer> genericSerializerMap =
            new Dictionary<Type, IDictionarySerializer>();

        /// <summary>
        ///   Maps types to custom serializers for these types.
        /// </summary>
        private readonly Dictionary<Type, IDictionarySerializer> serializerMap =
            new Dictionary<Type, IDictionarySerializer>();

        /// <summary>
        ///   Settings for serialization/deserialization.
        /// </summary>
        private readonly Dictionary<string, object> settings = new Dictionary<string, object>();

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///   Constructor.
        /// </summary>
        public DictionarySerializationContext()
        {
            // Set some default serializers.
            this.setSerializer(typeof(ValueWithType), new ValueWithTypeDictionarySerializer());

            this.genericSerializerMap[typeof(List<>)] = new ListDictionarySerializer();
            this.genericSerializerMap[typeof(Stack<>)] = new StackDictionarySerializer();
            this.genericSerializerMap[typeof(Dictionary<,>)] = new DictionaryDictionarySerializer();
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///   Settings for serialization/deserialization.
        /// </summary>
        public Dictionary<string, object> Settings
        {
            get
            {
                return this.settings;
            }
        }

        /// <summary>
        ///   Version number of serialization. Used to be compatible with older versions.
        /// </summary>
        public int Version { get; set; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///   Checks whether this context is able to serialize the specified type.
        /// </summary>
        /// <param name="type">Type to check.</param>
        /// <returns>
        ///   <c>true</c>, if this context can serialize the specified type with custom serializers or reflection, and <c>false</c> otherwise.
        /// </returns>
        public bool canSerialize(Type type)
        {
            IDictionarySerializer serializer = this.getSerializer(type);
            return serializer != null || Attribute.IsDefined(type, typeof(DictionarySerializableAttribute));
        }

        /// <summary>
        ///   Deserializes the object from the passed data dictionary.
        /// </summary>
        /// <typeparam name="T">Type of the object to deserialize.</typeparam>
        /// <param name="data">Data dictionary containing the values of the object to deserialize.</param>
        /// <returns>Object deserialized from the passed data dictionary.</returns>
        public T deserialize<T>(object data)
        {
            return (T)this.deserialize(typeof(T), data);
        }

        /// <summary>
        ///   Deserializes the object from the passed data dictionary.
        /// </summary>
        /// <param name="type">Type of the object to deserialize.</param>
        /// <param name="data">Data dictionary containing the values of the object to deserialize.</param>
        /// <returns>Object deserialized from the passed data dictionary.</returns>
        public object deserialize(Type type, object data)
        {
            if (data == null)
            {
                return null;
            }

            // Check if raw serialization possible.
            if (this.isRawSerializationPossible(type))
            {
                return Convert.ChangeType(data, type);
            }

            // Get serializer for object.
            IDictionarySerializer serializer = this.getSerializer(type);

            if (serializer == null)
            {
                // Check if nullable.
                Type nullableType = Nullable.GetUnderlyingType(type);
                if (nullableType != null)
                {
                    return this.deserialize(nullableType, data);
                }

                // Check if generic type.
                if (type.IsGenericType)
                {
                    Type genericType = type.GetGenericTypeDefinition();
                    if (genericType != null && this.genericSerializerMap.TryGetValue(genericType, out serializer))
                    {
                        return serializer.deserialize(this, (Dictionary<string, object>)data);
                    }
                }

                // Check if enum type.
                if (type.IsEnum)
                {
                    // Avoid recursive deserialization of ValueWithType, whose value is an enum in this case.
                    return Enum.Parse(type, (string)data);
                }

                // No custom serializer found - try reflection.
                if (Attribute.IsDefined(type, typeof(DictionarySerializableAttribute)))
                {
                    return this.deserializeReflection(type, (Dictionary<string, object>)data);
                }

                throw new ArgumentException(string.Format("Unsupported type for dictionary serialization: {0}", type));
            }

            return serializer.deserialize(this, (Dictionary<string, object>)data);
        }

        /// <summary>
        ///   Deserializes this context from the passed data dictionary.
        /// </summary>
        /// <param name="obj">Data dictionary containing the values of the object to deserialize.</param>
        public void deserializeContext(object obj)
        {
            Dictionary<string, object> data = (Dictionary<string, object>)obj;
            this.Version = Convert.ToInt32(data[DATA_VERSION]);
        }

        /// <summary>
        ///   Gets the custom serializer of this context for the specified type.
        /// </summary>
        /// <param name="type">Type to get the custom serializer of.</param>
        /// <returns>Custom serializer for the specified type.</returns>
        public IDictionarySerializer getSerializer(Type type)
        {
            IDictionarySerializer serializer;
            this.serializerMap.TryGetValue(type, out serializer);
            return serializer;
        }

        /// <summary>
        ///   Checks if a value of the specified type can be serialized directly or has to converted to a dictionary first.
        ///   If it can be serialized directly, no type information has to be stored to deserialize the value.
        /// </summary>
        /// <param name="type">Type to check.</param>
        /// <returns>True if the type can be serialized directly; otherwise, false.</returns>
        public bool isRawSerializationPossible(Type type)
        {
            // Check if primitive type or string.
            if (type.IsPrimitive || type == typeof(string))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        ///   Serializes the passed object to a data dictionary that can be
        ///   written to a Plist.
        /// </summary>
        /// <param name="obj">Object to serialize.</param>
        /// <returns>Object serialized as data dictionary.</returns>
        public object serialize(object obj)
        {
            if (obj == null)
            {
                return null;
            }

            Type type = obj.GetType();
            return this.serialize(type, obj);
        }

        /// <summary>
        ///   Serializes this context to a data dictionary that can be
        ///   written to a Plist.
        /// </summary>
        /// <returns>This context serialized as data dictionary.</returns>
        public object serializeContext()
        {
            Dictionary<string, object> data = new Dictionary<string, object>();
            data[DATA_VERSION] = this.Version;
            return data;
        }

        /// <summary>
        ///   Sets the custom serializer of this context for the specified type.
        /// </summary>
        /// <param name="type">Type to set the custom serializer of.</param>
        /// <param name="serializer">Custom serializer for the specified type.</param>
        public void setSerializer(Type type, IDictionarySerializer serializer)
        {
            this.serializerMap[type] = serializer;
        }

        /// <summary>
        ///   Tries to deserialize the value with the specified key in the specified data dictionary.
        ///   If the key is not found, the default value of the specified type is returned.
        /// </summary>
        /// <typeparam name="T">Type of value to deserialize.</typeparam>
        /// <param name="data">Data dictionary which contains value.</param>
        /// <param name="key">Key of value in data dictionary.</param>
        /// <returns>Deserialized value if found; otherwise, default value of specified type.</returns>
        public T tryDeserialize<T>(Dictionary<string, object> data, string key)
        {
            return this.tryDeserialize(data, key, default(T));
        }

        /// <summary>
        ///   Tries to deserialize the value with the specified key in the specified data dictionary.
        ///   If the key is not found, the specified default value is returned.
        /// </summary>
        /// <typeparam name="T">Type of value to deserialize.</typeparam>
        /// <param name="data">Data dictionary which contains value.</param>
        /// <param name="key">Key of value in data dictionary.</param>
        /// <param name="defaultValue">Default value to use if key was not found.</param>
        /// <returns>Deserialized value if found; otherwise, specified default value.</returns>
        public T tryDeserialize<T>(Dictionary<string, object> data, string key, T defaultValue)
        {
            if (data == null)
            {
                return defaultValue;
            }

            object valueData;
            if (data.TryGetValue(key, out valueData))
            {
                return this.deserialize<T>(valueData);
            }
            return defaultValue;
        }

        #endregion

        #region Methods

        /// <summary>
        ///   Deserializes the object from the passed data dictionary by
        ///   reflecting its type.
        /// </summary>
        /// <param name="type">Type of the object to deserialize.</param>
        /// <param name="data">Data dictionary containing the values of the object to deserialize.</param>
        /// <returns>Object deserialized from the passed data dictionary.</returns>
        private object deserializeReflection(Type type, Dictionary<string, object> data)
        {
            // Create object instance.
            object obj = Activator.CreateInstance(type);

            // Reflect object fields.
            FieldInfo[] fields = type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

            foreach (FieldInfo field in fields)
            {
                try
                {
                    if (Attribute.IsDefined(field, typeof(DictionarySerializableAttribute)))
                    {
                        // Check how the field value has to be deserialized.
                        Type fieldType = field.FieldType;

                        object serializedValue;

                        if (data.TryGetValue(field.Name, out serializedValue))
                        {
                            object fieldValue = this.deserialize(fieldType, serializedValue);

                            // Set property value.
                            field.SetValue(obj, fieldValue);
                        }
                    }
                }
                catch (ArgumentException e)
                {
                    throw new SerializationException(
                        string.Format("Unable to deserialize field {0}.{1}.", type.FullName, field.Name), e);
                }
            }

            // Reflect object properties.
            PropertyInfo[] properties = type.GetProperties();

            foreach (PropertyInfo property in properties)
            {
                try
                {
                    if (Attribute.IsDefined(property, typeof(DictionarySerializableAttribute)))
                    {
                        // Check how the property value has to be deserialized.
                        Type propertyType = property.PropertyType;

                        object serializedValue;

                        if (data.TryGetValue(property.Name, out serializedValue))
                        {
                            object propertyValue = this.deserialize(propertyType, serializedValue);

                            // Set property value.
                            property.SetValue(obj, propertyValue, null);
                        }
                    }
                }
                catch (Exception e)
                {
                    throw new SerializationException(
                        string.Format("Unable to deserialize property {0}.{1}.", type.FullName, property.Name), e);
                }
            }

            return obj;
        }

        private object serialize(Type type, object obj)
        {
            // Check if raw serialization is possible.
            if (this.isRawSerializationPossible(type))
            {
                return obj;
            }

            // Get serializer for object.
            IDictionarySerializer serializer = this.getSerializer(type);

            if (serializer == null)
            {
                // Check if nullable.
                Type nullableType = Nullable.GetUnderlyingType(type);
                if (nullableType != null)
                {
                    return this.serialize(nullableType, obj);
                }

                // Check if generic type.
                if (type.IsGenericType)
                {
                    Type genericType = type.GetGenericTypeDefinition();
                    if (genericType != null && this.genericSerializerMap.TryGetValue(genericType, out serializer))
                    {
                        return serializer.serialize(this, obj);
                    }
                }

                // Check if enum type.
                if (type.IsEnum)
                {
                    return obj.ToString();
                }

                // No custom serializer found - try reflection.
                if (Attribute.IsDefined(type, typeof(DictionarySerializableAttribute)))
                {
                    return this.serializeReflection(obj);
                }

                throw new ArgumentException(
                    string.Format("Unsupported type for dictionary serialization: {0}", type), "obj");
            }

            return serializer.serialize(this, obj);
        }

        /// <summary>
        ///   Serializes the passed object to a data dictionary that can be
        ///   written to a Plist by reflecting its type.
        /// </summary>
        /// <param name="obj">Object to serialize.</param>
        /// <returns>Object serialized as data dictionary</returns>
        private Dictionary<string, object> serializeReflection(object obj)
        {
            // Create data dictionary.
            Dictionary<string, object> data = new Dictionary<string, object>();

            // Reflect object fields.
            Type type = obj.GetType();
            FieldInfo[] fields = type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

            foreach (FieldInfo field in fields)
            {
                if (Attribute.IsDefined(field, typeof(DictionarySerializableAttribute)))
                {
                    // Check how the field value has to be serialized.
                    object fieldValue = field.GetValue(obj);
                    data.Add(field.Name, this.serialize(fieldValue));
                }
            }

            // Reflect object properties.
            PropertyInfo[] properties = type.GetProperties();

            foreach (PropertyInfo property in properties)
            {
                if (Attribute.IsDefined(property, typeof(DictionarySerializableAttribute)))
                {
                    // Check how the property value has to be serialized.
                    object propertyValue = property.GetGetMethod().Invoke(obj, null);
                    data.Add(property.Name, this.serialize(propertyValue));
                }
            }

            return data;
        }

        #endregion
    }
}