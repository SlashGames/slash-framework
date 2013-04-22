namespace SlashGames.AI.BehaviorTrees.Editor
{
    using System;
    using System.Xml;
    using System.Xml.Schema;
    using System.Xml.Serialization;

    using SlashGames.AI.BehaviorTrees.Implementations;
    using SlashGames.AI.BehaviorTrees.Interfaces;
    using SlashGames.Xml;

    /// <summary>
    ///   Called when an unknown task should be read from a xml document.
    /// </summary>
    /// <param name="reader"> Xml reader. </param>
    /// <param name="task"> Task which is read. </param>
    public delegate void XmlSerializationReadDelegate(XmlReader reader, out ITask task);

    /// <summary>
    ///   Xml wrapper to serialize a decider, even when it's unknown.
    /// </summary>
    public class XmlWrapper : IXmlSerializable
    {
        #region Static Fields

        /// <summary>
        ///   Called when an unknown task is read from a xml document.
        /// </summary>
        public static XmlSerializationReadDelegate OnXmlReadUnknownTask;

        #endregion

        #region Fields

        /// <summary>
        ///   Wrapped task.
        /// </summary>
        private ITask task;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///   Initializes a new instance of the <see cref="XmlWrapper" /> class. Constructor.
        /// </summary>
        public XmlWrapper()
        {
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref="XmlWrapper" /> class. Constructor.
        /// </summary>
        /// <param name="task"> Task to wrap. </param>
        public XmlWrapper(ITask task)
        {
            this.task = task;
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///   Wrapped task.
        /// </summary>
        public ITask Task
        {
            get
            {
                return this.task;
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///   The equals.
        /// </summary>
        /// <param name="other"> The other. </param>
        /// <returns> The System.Boolean. </returns>
        public bool Equals(XmlWrapper other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return Equals(other.task, this.task);
        }

        /// <summary>
        ///   The equals.
        /// </summary>
        /// <param name="obj"> The obj. </param>
        /// <returns> The System.Boolean. </returns>
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

            if (obj.GetType() != typeof(XmlWrapper))
            {
                return false;
            }

            return this.Equals((XmlWrapper)obj);
        }

        /// <summary>
        ///   The get hash code.
        /// </summary>
        /// <returns> The System.Int32. </returns>
        public override int GetHashCode()
        {
            return this.task != null ? this.task.GetHashCode() : 0;
        }

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
            try
            {
                XmlAnything<ITask> xmlWrapper = new XmlAnything<ITask>();
                xmlWrapper.ReadXml(reader);
                this.task = xmlWrapper.Value;
            }
            catch (InvalidCastException)
            {
                if (OnXmlReadUnknownTask != null)
                {
                    OnXmlReadUnknownTask(reader, out this.task);
                }
                else
                {
                    throw;
                }
            }
        }

        /// <summary>
        ///   The write xml.
        /// </summary>
        /// <param name="writer"> The writer. </param>
        public void WriteXml(XmlWriter writer)
        {
            // Special case for reference decider.
            // TODO(co): Would be nice to capsule this inside the ReferenceTask class, but the xml node is written
            // before the ReferenceTask object gets the control.
            ReferenceTask referenceTask = this.task as ReferenceTask;
            if (referenceTask != null)
            {
                string typeString = referenceTask.TaskDescription != null
                                        ? referenceTask.TaskDescription.TypeName
                                        : typeof(ReferenceTask).AssemblyQualifiedName;
                writer.WriteAttributeString("type", typeString);
                string elementName = referenceTask.TaskDescription != null
                                         ? referenceTask.TaskDescription.ClassName
                                         : typeof(ReferenceTask).Name;

                XmlSerializer serializer = new XmlSerializer(typeof(ReferenceTask), new XmlRootAttribute(elementName));
                serializer.Serialize(writer, referenceTask);
            }
            else
            {
                XmlAnything<ITask> xmlWrapper = new XmlAnything<ITask>(this.task);
                xmlWrapper.WriteXml(writer);
            }
        }

        #endregion
    }
}