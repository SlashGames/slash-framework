// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SerializationUtils.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Serialization.Utils
{
    using System.IO;
    using System.Runtime.Serialization.Formatters.Binary;

    public static class SerializationUtils
    {
        #region Public Methods and Operators

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

        #endregion
    }
}