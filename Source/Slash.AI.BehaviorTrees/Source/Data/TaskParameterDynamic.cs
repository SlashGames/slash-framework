// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TaskParameterDynamic.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.AI.BehaviorTrees.Data
{
    using System;
    using System.Xml.Serialization;

    using Slash.Xml;

    /// <summary>
    ///   Task parameter for dynamic types (e.g. base class of derived types).
    /// </summary>
    /// <typeparam name="T"> </typeparam>
    [Serializable]
    public class TaskParameterDynamic<T> : TaskParameterGeneric<T>
    {
        #region Public Properties

        /// <summary>
        ///   User value xml serialization.
        /// </summary>
        [XmlElement("UserValue")]
        public XmlAnything<T> ConcreteUserValueSerialized
        {
            get
            {
                return new XmlAnything<T>(this.ConcreteUserValue);
            }

            set
            {
                this.ConcreteUserValue = value != null ? value.Value : default(T);
            }
        }

        #endregion
    }
}