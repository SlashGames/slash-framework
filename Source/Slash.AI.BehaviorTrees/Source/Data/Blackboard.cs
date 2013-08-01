// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Blackboard.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.AI.BehaviorTrees.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Slash.Collections.AttributeTables;
    using Slash.Collections.Utils;

    /// <summary>
    ///   Class which can be used to exchange data between behaviors.
    /// </summary>
    [Serializable]
    public class Blackboard : AttributeTable
    {
        #region Constructors and Destructors

        /// <summary>
        ///   Initializes a new instance of the <see cref="Blackboard" /> class. Constructor.
        /// </summary>
        /// <param name="original"> Blackboard to copy attributes from. </param>
        public Blackboard(Blackboard original)
            : base(original)
        {
            this.Parents = new List<Blackboard>(original.Parents);
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref="Blackboard" /> class. Constructor.
        /// </summary>
        public Blackboard()
        {
            this.Parents = new List<Blackboard>();
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///   Parents of the blackboard. If an attribute isn't found on the blackboard, the parent list is searched for the attribute from first parent to last one until the attribute is found.
        /// </summary>
        public List<Blackboard> Parents { get; set; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///   Checks if an attribute with the passed id exists.
        /// </summary>
        /// <param name="id"> Id of attribute to check. </param>
        /// <returns> True if an attribute with the passed id exists, else false. </returns>
        public override bool Contains(object id)
        {
            return base.Contains(id) || this.Parents != null && this.Parents.Any(parent => parent.Contains(id));
        }

        /// <summary>
        ///   The equals.
        /// </summary>
        /// <param name="other"> The other. </param>
        /// <returns> The System.Boolean. </returns>
        public bool Equals(Blackboard other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return base.Equals(other) && CollectionUtils.SequenceEqual(other.Parents, this.Parents);
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

            return this.Equals(obj as Blackboard);
        }

        /// <summary>
        ///   The get hash code.
        /// </summary>
        /// <returns> The System.Int32. </returns>
        public override int GetHashCode()
        {
            unchecked
            {
                return (base.GetHashCode() * 397) ^ (this.Parents != null ? this.Parents.GetHashCode() : 0);
            }
        }

        /// <summary>
        ///   Returns the attribute with the passed id. If not found the passed default value is returned.
        /// </summary>
        /// <typeparam name="T"> Type of attribute. </typeparam>
        /// <param name="id"> Id of attribute. </param>
        /// <param name="defaultValue"> Default value to return if attribute is not found. </param>
        /// <returns> Value of attribute. Returns the default value if no object with the passed id was found. </returns>
        public T GetValue<T>(object id, T defaultValue)
        {
            T attribute;
            return this.TryGetValue(id, out attribute) ? attribute : defaultValue;
        }

        /// <summary>
        ///   Tries to find the attribute with the passed id.
        /// </summary>
        /// <typeparam name="T"> Type of attribute. </typeparam>
        /// <param name="id"> Id of attribute. </param>
        /// <param name="value"> Contains value if id was found. </param>
        /// <returns> True if the attribute was found, else false. </returns>
        public override bool TryGetValue<T>(object id, out T value)
        {
            object objectValue;
            if (base.TryGetValue(id, out objectValue) && objectValue is T)
            {
                value = (T)objectValue;
                return true;
            }

            if (this.Parents != null)
            {
                foreach (Blackboard parent in this.Parents)
                {
                    if (parent.TryGetValue(id, out value))
                    {
                        return true;
                    }
                }
            }

            value = default(T);
            return false;
        }

        #endregion

        /*
        /// <summary>
        ///   Called when a value can't be read from xml because its type wasn't found. Can be used to read the xml node e.g. into a temporary object.
        /// </summary>
        /// <param name="reader"> Xml reader. </param>
        /// <param name="value"> Holds the read value. </param>
        /// <param name="name"> Node name. </param>
        protected override void ReadXmlUnknownValue(XmlReader reader, out object value, string name)
        {
            // Try to read task.
            ITask task = null;
            if (XmlWrapper.OnXmlReadUnknownTask != null)
            {
                XmlWrapper.OnXmlReadUnknownTask(reader, out task);
            }

            value = task;
        }

        /// <summary>
        ///   Writes a value to xml.
        /// </summary>
        /// <param name="writer"> Xml writer. </param>
        /// <param name="value"> Value to write to xml. </param>
        protected override void WriteXmlValue(XmlWriter writer, object value)
        {
            if (value is ITask)
            {
                XmlWrapper wrapper = new XmlWrapper((ITask)value);
                wrapper.WriteXml(writer);
            }
            else
            {
                base.WriteXmlValue(writer, value, name);
            }
        }*/
    }
}