// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XmlAnything.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Serialization.Xml
{
    using System;
    using System.Runtime.Serialization;
    using System.Xml;
    using System.Xml.Schema;
    using System.Xml.Serialization;

    /// <summary>
    ///   Wrapper for serializing an object along with its type to and from XML.
    /// </summary>
    /// <typeparam name="T">Type of the object to serialize.</typeparam>
    public sealed class XmlAnything<T> : IXmlSerializable
    {
        #region Constructors and Destructors

        /// <summary>
        ///   Constructs an empty wrapper.
        /// </summary>
        public XmlAnything()
        {
        }

        /// <summary>
        ///   Constructs a wrapper for serializing the passed object along with its type to and from XML.
        /// </summary>
        /// <param name="t">Object to wrap.</param>
        public XmlAnything(T t)
        {
            this.Value = t;
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///   Wrapped object that can be serialized to and from XML.
        /// </summary>
        public T Value { get; set; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///   This method is reserved and should not be used. When implementing the IXmlSerializable interface, you should return null (Nothing in Visual Basic) from this method, and instead, if specifying a custom schema is required, apply the
        ///   <see
        ///     cref="T:System.Xml.Serialization.XmlSchemaProviderAttribute" />
        ///   to the class.
        /// </summary>
        /// <returns>
        ///   An <see cref="T:System.Xml.Schema.XmlSchema" /> that describes the XML representation of the object that is produced by the
        ///   <see
        ///     cref="M:System.Xml.Serialization.IXmlSerializable.WriteXml(System.Xml.XmlWriter)" />
        ///   method and consumed by the
        ///   <see
        ///     cref="M:System.Xml.Serialization.IXmlSerializable.ReadXml(System.Xml.XmlReader)" />
        ///   method.
        /// </returns>
        public XmlSchema GetSchema()
        {
            return null;
        }

        /// <summary>
        ///   Generates an object from its XML representation.
        /// </summary>
        /// <param name="reader">
        ///   The <see cref="T:System.Xml.XmlReader" /> stream from which the object is deserialized.
        /// </param>
        public void ReadXml(XmlReader reader)
        {
            if (!reader.HasAttributes)
            {
                throw new FormatException("expected a type attribute!");
            }
            string typeString = reader.GetAttribute("type");
            reader.Read(); // consume the value
            if (typeString == null || typeString == "null")
            {
                return; // leave T at default value
            }

            Type type = Type.GetType(typeString);
            if (type == null)
            {
                throw new SerializationException(string.Format("Type '{0}' not found.", typeString));
            }

            XmlSerializer serializer = new XmlSerializer(type);
            this.Value = (T)serializer.Deserialize(reader);
            reader.ReadEndElement();
        }

        /// <summary>
        ///   Converts an object into its XML representation.
        /// </summary>
        /// <param name="writer">
        ///   The <see cref="T:System.Xml.XmlWriter" /> stream to which the object is serialized.
        /// </param>
        public void WriteXml(XmlWriter writer)
        {
            if (this.Value == null)
            {
                writer.WriteAttributeString("type", "null");
                return;
            }
            Type type = this.Value.GetType();
            XmlSerializer serializer = new XmlSerializer(type);
            writer.WriteAttributeString("type", type.AssemblyQualifiedName);
            serializer.Serialize(writer, this.Value);
        }

        #endregion
    }
}