// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SerializableDictionary.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Serialization.Xml
{
    using System.Collections.Generic;
    using System.Xml;
    using System.Xml.Schema;
    using System.Xml.Serialization;

    /// <summary>
    ///   Adds xml serialization to the generic dictionary.
    ///   Doesn't handle polymorphic keys/values.
    ///   From http://weblogs.asp.net/pwelter34/archive/2006/05/03/444961.aspx
    /// </summary>
    /// <typeparam name="TKey">Key type.</typeparam>
    /// <typeparam name="TValue">Value type.</typeparam>
    [XmlType("dictionary")]
    public class SerializableDictionary<TKey, TValue> : Dictionary<TKey, TValue>, IXmlSerializable
    {
        #region Fields

        /// <summary>
        ///   Xml element name for items in dictionary.
        /// </summary>
        private string itemElementName = "item";

        /// <summary>
        ///   Xml element name for key of items.
        /// </summary>
        private string keyElementName = "key";

        /// <summary>
        ///   Xml element name for value of items.
        /// </summary>
        private string valueElementName = "value";

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///   Constructor.
        /// </summary>
        /// <param name="capacity">Initial capacity.</param>
        public SerializableDictionary(int capacity)
            : base(capacity)
        {
        }

        /// <summary>
        ///   Constructor.
        /// </summary>
        public SerializableDictionary()
        {
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///   Xml element name for items in dictionary.
        /// </summary>
        public string ItemElementName
        {
            get
            {
                return this.itemElementName;
            }
            set
            {
                this.itemElementName = value;
            }
        }

        /// <summary>
        ///   Xml element name for key of items.
        /// </summary>
        public string KeyElementName
        {
            get
            {
                return this.keyElementName;
            }
            set
            {
                this.keyElementName = value;
            }
        }

        /// <summary>
        ///   Xml element name for value of items.
        /// </summary>
        public string ValueElementName
        {
            get
            {
                return this.valueElementName;
            }
            set
            {
                this.valueElementName = value;
            }
        }

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
            XmlSerializer keySerializer = new XmlSerializer(typeof(TKey), new XmlRootAttribute(this.KeyElementName));
            XmlSerializer valueSerializer = new XmlSerializer(
                typeof(TValue), new XmlRootAttribute(this.ValueElementName));

            bool wasEmpty = reader.IsEmptyElement;
            reader.Read();

            if (wasEmpty)
            {
                return;
            }

            while (reader.NodeType != XmlNodeType.EndElement)
            {
                reader.ReadStartElement(this.ItemElementName);

                TKey key = (TKey)keySerializer.Deserialize(reader);
                TValue value = (TValue)valueSerializer.Deserialize(reader);

                this.Add(key, value);

                reader.ReadEndElement();
                reader.MoveToContent();
            }
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
            XmlSerializer keySerializer = new XmlSerializer(typeof(TKey), new XmlRootAttribute(this.KeyElementName));
            XmlSerializer valueSerializer = new XmlSerializer(
                typeof(TValue), new XmlRootAttribute(this.ValueElementName));
            var xns = new XmlSerializerNamespaces();
            xns.Add(string.Empty, string.Empty);
            foreach (var keyValuePair in this)
            {
                writer.WriteStartElement(this.ItemElementName);

                keySerializer.Serialize(writer, keyValuePair.Key, xns);
                valueSerializer.Serialize(writer, keyValuePair.Value, xns);

                writer.WriteEndElement();
            }
        }

        #endregion
    }
}