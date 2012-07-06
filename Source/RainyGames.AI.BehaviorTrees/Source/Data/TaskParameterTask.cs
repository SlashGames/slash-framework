// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TaskParameterTask.cs" company="Rainy Games">
//   Copyright (c) Rainy Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace RainyGames.AI.BehaviorTrees.Data
{
    using System;
    using System.Xml.Serialization;

    using RainyGames.AI.BehaviorTrees.Editor;
    using RainyGames.AI.BehaviorTrees.Interfaces;

    /// <summary>
    ///   Task parameter for tasks.
    /// </summary>
    [Serializable]
    public class TaskParameterTask : TaskParameterGeneric<ITask>
    {
        #region Public Properties

        /// <summary>
        ///   User value xml serialization.
        /// </summary>
        [XmlElement("UserValue")]
        public XmlWrapper ConcreteUserValueSerialized
        {
            get
            {
                return new XmlWrapper(this.ConcreteUserValue);
            }

            set
            {
                this.ConcreteUserValue = value != null ? value.Task : null;
            }
        }

        #endregion
    }
}