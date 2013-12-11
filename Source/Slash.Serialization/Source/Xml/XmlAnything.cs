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

    public sealed class XmlAnything<T> : IXmlSerializable
    {
        #region Constructors and Destructors

        public XmlAnything()
        {
        }

        public XmlAnything(T t)
        {
            this.Value = t;
        }

        #endregion

        #region Public Properties

        public T Value { get; set; }

        #endregion

        #region Public Methods and Operators

        public XmlSchema GetSchema()
        {
            return null;
        }

        public void ReadXml(XmlReader reader)
        {
            if (!reader.HasAttributes)
            {
                throw new FormatException("expected a type attribute!");
            }
            string typeString = reader.GetAttribute("type");
            reader.Read(); // consume the value
            if ( typeString == null || typeString == "null")
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