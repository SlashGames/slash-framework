// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SerializationTestUtils.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Tests.Utils
{
    using System.IO;
    using System.Xml.Serialization;

    using NUnit.Framework;

    public static class SerializationTestUtils
    {
        #region Public Methods and Operators

        /// <summary>
        ///   Utility method to test xml serialization. Serializes the specified object and deserializes it afterwards.
        ///   At the end the original and deserialized object are compared for equality (Assert.AreEqual). The test
        ///   fails if they are not equal.
        /// </summary>
        /// <typeparam name="T">Type of object to test xml serialization for.</typeparam>
        /// <param name="obj">Object to serialize/deserialize.</param>
        /// <returns>Deserialized object to perform further test with it if required.</returns>
        public static T TestXmlSerialization<T>(T obj)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));

            StringWriter stringWriter = new StringWriter();
            serializer.Serialize(stringWriter, obj);
            string text = stringWriter.ToString();
            stringWriter.Close();

            StringReader stringReader = new StringReader(text);
            T deserialized = (T)serializer.Deserialize(stringReader);
            stringReader.Close();
            Assert.AreEqual(obj, deserialized);

            return deserialized;
        }

        #endregion
    }
}