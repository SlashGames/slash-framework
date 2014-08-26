// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SerializationUtils.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Serialization.Utils
{
    using System;
    using System.IO;
    using System.Runtime.Serialization.Formatters.Binary;
    using System.Xml;
    using System.Xml.Serialization;

    /// <summary>
    ///   Utility methods for serializing and deserializing objects.
    /// </summary>
    public static class SerializationUtils
    {
        #region Public Methods and Operators

#if !WINDOWS_STORE
        /// <summary>
        ///   Makes a deep copy of the specified object by serializing and deserializing the specified object.
        /// </summary>
        /// <typeparam name="T"> Type to copy. </typeparam>
        /// <param name="obj"> Object to copy. </param>
        /// <returns> A deep copy of the specified object. </returns>
        public static T DeepCopy<T>(T obj)
        {
            // To binary and back.
            return FromMemoryStream<T>(ToMemoryStream(obj));
        }

        /// <summary>
        ///   Deserializes an object of the specified type from the specified memory stream.
        /// </summary>
        /// <typeparam name="T"> Expected type in memory stream. </typeparam>
        /// <param name="memoryStream"> Memory stream to get the object from. </param>
        /// <returns> Deserialized object. </returns>
        public static T FromMemoryStream<T>(MemoryStream memoryStream)
        {
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            return (T)binaryFormatter.Deserialize(memoryStream);
        }
#endif

        /// <summary>
        ///   Deserializes an object of the specified type from the specified XML reader.
        /// </summary>
        /// <param name="reader">XML reader to read object from.</param>
        /// <param name="type">Type of object to deserialize.</param>
        /// <param name="elementName">Element node name the object is serialized under.</param>
        /// <returns>Deserialized object.</returns>
        public static object ReadXml(XmlReader reader, Type type, string elementName)
        {
            // Deserialize object.
            XmlSerializer xmlSerializer = new XmlSerializer(type, new XmlRootAttribute(elementName));
            return xmlSerializer.Deserialize(reader);
        }

#if !WINDOWS_STORE
        /// <summary>
        ///   Serializes the specified object to a binary stream.
        /// </summary>
        /// <param name="obj"> Object to serialize. </param>
        /// <returns> Memory stream which contains the serialized object. </returns>
        public static MemoryStream ToMemoryStream(object obj)
        {
            MemoryStream memoryStream = new MemoryStream();
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            binaryFormatter.Serialize(memoryStream, obj);
            memoryStream.Close();
            return memoryStream;
        }
#endif

        /// <summary>
        ///   Serializes the specified object to xml and adds it to the specified xml writer.
        /// </summary>
        /// <param name="writer">Xml writer to add object to.</param>
        /// <param name="obj">Object to serialize.</param>
        /// <param name="elementName">Element node name to serialize object as.</param>
        public static void WriteXml(XmlWriter writer, object obj, string elementName)
        {
            // Serialize object.
            Type type = obj.GetType();
            XmlSerializer xmlSerializer = new XmlSerializer(type, new XmlRootAttribute(elementName));
            xmlSerializer.Serialize(writer, obj);
        }

        #endregion
    }
}