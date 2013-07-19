// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TaskParameterAttribute.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.AI.BehaviorTrees.Attributes
{
    using System;

    using Slash.AI.BehaviorTrees.Editor;

    /// <summary>
    ///   Attribute to flag a property in a task to be a task parameter. Can take some meta data about the parameter like the name which is shown in the editor.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class TaskParameterAttribute : Attribute
    {
        #region Public Properties

        /// <summary>
        ///   Default value.
        /// </summary>
        public object Default { get; set; }

        /// <summary>
        ///   Description.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        ///   Meta type (used for parameters which are initialized by sub configurations).
        /// </summary>
        public Type MetaType { get; set; }

        /// <summary>
        ///   Readable name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///   Indicates how the parameter should be visualized.
        /// </summary>
        public VisualizationType VisualizationType { get; set; }

        #endregion
    }
}