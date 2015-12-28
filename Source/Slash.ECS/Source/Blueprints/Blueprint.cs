// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Blueprint.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.ECS.Blueprints
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Xml;
    using System.Xml.Schema;
    using System.Xml.Serialization;

    using Slash.Collections.AttributeTables;
    using Slash.Collections.Utils;
    using Slash.Reflection.Utils;
    using Slash.Serialization;
    using Slash.Serialization.Binary;

    /// <summary>
    ///   Blueprint for creating an entity with a specific set of components
    ///   and initial attribute values.
    /// </summary>
    [Serializable]
    public sealed class Blueprint : IXmlSerializable, IBinarySerializable
    {
        #region Constants

        private const string AttributeTableElementName = "AttributeTable";

        private const string ComponentTypeElementName = "ComponentType";

        private const string ComponentTypesElementName = "ComponentTypes";

        private const string ParentIdElementName = "ParentId";

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///   Constructs a new blueprint without any components or data.
        /// </summary>
        public Blueprint()
        {
            this.AttributeTable = new AttributeTable();
            this.ComponentTypes = new List<Type>();
        }

        /// <summary>
        ///   Creates a deep copy of the specified blueprint.
        /// </summary>
        /// <param name="original">Original blueprint to copy.</param>
        public Blueprint(Blueprint original)
        {
            this.AttributeTable = new AttributeTable(original.AttributeTable);
            this.ComponentTypes = new List<Type>(original.ComponentTypes);
            this.Parent = original.Parent;
            this.ParentId = original.ParentId;
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///   Data for initializing the components of entities created with this
        ///   blueprint.
        /// </summary>
        [XmlIgnore]
        public IAttributeTable AttributeTable { get; set; }

        /// <summary>
        ///   Wrapper for AttributeTable property for xml serialization.
        /// </summary>
        [XmlElement("AttributeTable")]
#if !WINDOWS_STORE
        [Browsable(false)]
#endif
        [EditorBrowsable(EditorBrowsableState.Never)]
        [SerializeMember]
        public AttributeTable AttributeTableSerialized
        {
            get
            {
                return new AttributeTable(this.AttributeTable);
            }
            set
            {
                this.AttributeTable = value;
            }
        }

        /// <summary>
        ///   Collection of types of components to add to entities created with
        ///   this blueprint.
        /// </summary>
        [XmlIgnore]
        public List<Type> ComponentTypes { get; set; }

        /// <summary>
        ///   Wrapper for ComponentTypes property for xml serialization.
        /// </summary>
        [XmlArray("ComponentTypes")]
        [XmlArrayItem("ComponentType")]
#if !WINDOWS_STORE
        [Browsable(false)]
#endif
        [EditorBrowsable(EditorBrowsableState.Never)]
        [SerializeMember]
        public string[] ComponentTypesSerialized
        {
            get
            {
                return this.ComponentTypes.Select<Type, string>(componentType => componentType.FullName).ToArray();
            }
            set
            {
                this.ComponentTypes = value.Select<string, Type>(ReflectionUtils.FindType).ToList();
            }
        }

        /// <summary>
        ///   Parent blueprint of this one. All components and attributes of the parent are also
        ///   available for this one. Attributes can be overwritten though.
        /// </summary>
        [XmlIgnore]
        public Blueprint Parent { get; set; }

        /// <summary>
        ///   Id of parent blueprint. Used for serialization/deserialization.
        /// </summary>
        [SerializeMember]
        public string ParentId { get; set; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///   Reads this object from its binary representation.
        /// </summary>
        /// <param name="deserializer">Deserializer to read the object with.</param>
        public void Deserialize(BinaryDeserializer deserializer)
        {
            this.AttributeTableSerialized = deserializer.Deserialize<AttributeTable>();
            this.ComponentTypesSerialized = deserializer.Deserialize<string[]>();
            this.ParentId = deserializer.Deserialize<string>();
        }

        /// <summary>
        ///   Determines whether the specified <see cref="T:System.Object" /> is equal to the current <see cref="T:System.Object" />.
        /// </summary>
        /// <returns>
        ///   true if the specified <see cref="T:System.Object" /> is equal to the current <see cref="T:System.Object" />; otherwise, false.
        /// </returns>
        /// <param name="obj">
        ///   The <see cref="T:System.Object" /> to compare with the current <see cref="T:System.Object" />.
        /// </param>
        /// <filterpriority>2</filterpriority>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            if (obj.GetType() != this.GetType())
            {
                return false;
            }

            return this.Equals((Blueprint)obj);
        }

        /// <summary>
        ///   Returns an enumeration of all component types of this and all ancestor blueprints.
        /// </summary>
        /// <returns>Enumeration of component types of this and all ancestor blueprints.</returns>
        public IEnumerable<Type> GetAllComponentTypes()
        {
            return this.Parent == null
                       ? this.ComponentTypes
                       : this.ComponentTypes.Union(this.Parent.GetAllComponentTypes());
        }

        /// <summary>
        ///   Returns the final attribute table to use for entity creation.
        ///   Considers the attribute tables of the ancestors of this blueprint if there are any.
        /// </summary>
        /// <returns>Final attribute table to use for entity creation.</returns>
        public IAttributeTable GetAttributeTable()
        {
            if (this.Parent == null)
            {
                return this.AttributeTable;
            }

            HierarchicalAttributeTable attributeTable = new HierarchicalAttributeTable(this.AttributeTable);
            Blueprint ancestor = this.Parent;
            while (ancestor != null)
            {
                if (ancestor.AttributeTable != null)
                {
                    attributeTable.AddParent(ancestor.AttributeTable);
                }
                ancestor = ancestor.Parent;
            }
            return attributeTable;
        }

        /// <summary>
        ///   Serves as a hash function for a particular type.
        /// </summary>
        /// <returns>
        ///   A hash code for the current <see cref="T:System.Object" />.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        public override int GetHashCode()
        {
            unchecked
            {
                return ((this.AttributeTable != null ? this.AttributeTable.GetHashCode() : 0) * 397)
                       ^ (this.ComponentTypes != null ? this.ComponentTypes.GetHashCode() : 0);
            }
        }

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
            reader.MoveToContent();
            bool isEmpty = reader.IsEmptyElement;
            reader.ReadStartElement();
            if (isEmpty)
            {
                return;
            }

            if (reader.Name == AttributeTableElementName)
            {
                AttributeTable attributeTable = new AttributeTable();
                attributeTable.ReadXml(reader);
                this.AttributeTableSerialized = attributeTable;
            }

            if (reader.Name == ComponentTypesElementName)
            {
                List<string> componentTypes = new List<string>();
                reader.MoveToContent();
                isEmpty = reader.IsEmptyElement;
                reader.ReadStartElement();

                if (!isEmpty)
                {
                    while (reader.NodeType != XmlNodeType.EndElement)
                    {
                        if (reader.NodeType == XmlNodeType.Element)
                        {
                            string elementName = reader.Name;
                            if (elementName == ComponentTypeElementName)
                            {
                                string componentType = reader.ReadElementContentAsString();
                                componentTypes.Add(componentType);
                            }
                            else
                            {
                                reader.ReadStartElement();
                            }
                        }
                        else
                        {
                            reader.Read();
                        }
                    }
                }
                reader.ReadEndElement();

                this.ComponentTypesSerialized = componentTypes.ToArray();
            }

            if (reader.Name == ParentIdElementName)
            {
                this.ParentId = reader.ReadElementContentAsString();
            }

            reader.ReadEndElement();
        }

        /// <summary>
        ///   Converts this object to its binary representation.
        /// </summary>
        /// <param name="serializer">Serializer to writer this object with.</param>
        public void Serialize(BinarySerializer serializer)
        {
            serializer.Serialize(this.AttributeTableSerialized);
            serializer.Serialize(this.ComponentTypesSerialized);
            serializer.Serialize(string.IsNullOrEmpty(this.ParentId) ? string.Empty : this.ParentId);
        }

        /// <summary>
        ///   Indicates if the AttributeTableSerialized property should be serialized via Xml.
        /// </summary>
        /// <returns>True if it should be serialized; otherwise, false.</returns>
        public bool ShouldSerializeAttributeTableSerialized()
        {
            return this.AttributeTable != null && this.AttributeTable.Count > 0;
        }

        /// <summary>
        ///   Indicates if the ComponentTypesSerialized property should be serialized via Xml.
        /// </summary>
        /// <returns>True if it should be serialized; otherwise, false.</returns>
        public bool ShouldSerializeComponentTypesSerialized()
        {
            return this.ComponentTypes != null && this.ComponentTypes.Count > 0;
        }

        /// <summary>
        ///   Returns a string that represents the current object.
        /// </summary>
        /// <returns>
        ///   A string that represents the current object.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        public override string ToString()
        {
            string componentTypesString = this.ComponentTypes.Aggregate(
                string.Empty, (current, componentType) => current + string.Format("{0}, ", componentType.FullName));
            return string.Format("AttributeTable: {0}, ComponentTypes: {1}", this.AttributeTable, componentTypesString);
        }

        /// <summary>
        ///   Tries to retrieve the value the specified key is mapped to within this
        ///   blueprint. Searches for the key in a parent blueprint if existent.
        /// </summary>
        /// <param name="key"> Key to retrieve the value of. </param>
        /// <param name="value"> Retrieved value. </param>
        /// <returns> True if a value was found; otherwise, false. </returns>
        public bool TryGetValue(object key, out object value)
        {
            if (this.AttributeTable.TryGetValue(key, out value))
            {
                return true;
            }

            if (this.Parent != null)
            {
                return this.Parent.TryGetValue(key, out value);
            }

            return false;
        }

        /// <summary>
        ///   Converts an object into its XML representation.
        /// </summary>
        /// <param name="writer">
        ///   The <see cref="T:System.Xml.XmlWriter" /> stream to which the object is serialized.
        /// </param>
        public void WriteXml(XmlWriter writer)
        {
            if (this.AttributeTable.Count > 0)
            {
                writer.WriteStartElement(AttributeTableElementName);
                this.AttributeTableSerialized.WriteXml(writer);
                writer.WriteEndElement();
            }

            if (this.ComponentTypes.Count > 0)
            {
                writer.WriteStartElement(ComponentTypesElementName);
                foreach (string componentType in this.ComponentTypesSerialized)
                {
                    writer.WriteElementString(ComponentTypeElementName, componentType);
                }
                writer.WriteEndElement();
            }

            if (!string.IsNullOrEmpty(this.ParentId))
            {
                writer.WriteElementString(ParentIdElementName, this.ParentId);
            }
        }

        #endregion

        #region Methods

        private bool Equals(Blueprint other)
        {
            return this.AttributeTable.Equals(other.AttributeTable)
                   && CollectionUtils.SequenceEqual(this.ComponentTypes, other.ComponentTypes);
        }

        #endregion
    }
}