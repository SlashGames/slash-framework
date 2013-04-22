// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TaskParameterTask.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace SlashGames.AI.BehaviorTrees.Data
{
    using System;
    using System.Xml.Serialization;

    using SlashGames.AI.BehaviorTrees.Editor;
    using SlashGames.AI.BehaviorTrees.Interfaces;

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