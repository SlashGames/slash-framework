namespace Slash.AI.BehaviorTrees.Editor
{
    using System.Collections.Generic;
    using System.Xml;
    using System.Xml.Schema;
    using System.Xml.Serialization;

    using Slash.AI.BehaviorTrees.Interfaces;

    /// <summary>
    ///   TODO: This class performs an important function.
    /// </summary>
    public class XmlWrapperList : IXmlSerializable
    {
        #region Fields

        /// <summary>
        ///   Wrapped list.
        /// </summary>
        private List<ITask> list;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///   Initializes a new instance of the <see cref="XmlWrapperList" /> class.
        /// </summary>
        public XmlWrapperList()
        {
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref="XmlWrapperList" /> class.
        /// </summary>
        /// <param name="list"> The list. </param>
        public XmlWrapperList(List<ITask> list)
        {
            this.list = list;
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///   Gets the list.
        /// </summary>
        public List<ITask> List
        {
            get
            {
                return this.list;
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///   The get schema.
        /// </summary>
        /// <returns> The System.Xml.Schema.XmlSchema. </returns>
        public XmlSchema GetSchema()
        {
            return null;
        }

        /// <summary>
        ///   The read xml.
        /// </summary>
        /// <param name="reader"> The reader. </param>
        public void ReadXml(XmlReader reader)
        {
            this.list = new List<ITask>();
            if (reader.IsEmptyElement)
            {
                reader.Skip();
                return;
            }

            reader.ReadStartElement();
            while (reader.NodeType != XmlNodeType.EndElement)
            {
                if (reader.NodeType == XmlNodeType.Element)
                {
                    if (reader.Name == "Child")
                    {
                        XmlWrapper wrapper = new XmlWrapper();
                        wrapper.ReadXml(reader);
                        this.list.Add(wrapper.Task);
                    }
                    else
                    {
                        reader.Skip();
                    }
                }
                else
                {
                    reader.Skip();
                }
            }

            reader.ReadEndElement();
        }

        /// <summary>
        ///   The write xml.
        /// </summary>
        /// <param name="writer"> The writer. </param>
        public void WriteXml(XmlWriter writer)
        {
            foreach (ITask task in this.list)
            {
                if (task == null)
                {
                    continue;
                }

                // Wrap task.
                XmlWrapper wrapper = new XmlWrapper(task);
                writer.WriteStartElement("Child");
                wrapper.WriteXml(writer);
                writer.WriteEndElement();
            }
        }

        #endregion
    }
}