// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InspectorComponent.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.GameBase.Inspector.Data
{
    using System;
    using System.Collections.Generic;

    using Slash.GameBase.Inspector.Attributes;

    /// <summary>
    ///   Component accessible to the user in the landscape designer.
    /// </summary>
    public class InspectorType
    {
        #region Public Properties

        /// <summary>
        ///   Name of type.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///   Description of type.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        ///   Raw attribute.
        /// </summary>
        public InspectorTypeAttribute Attribute { get; set; }

        /// <summary>
        ///   Properties exposed in the inspector.
        /// </summary>
        public List<InspectorPropertyAttribute> Properties { get; set; }

        /// <summary>
        ///   C# type of the component.
        /// </summary>
        public Type Type { get; set; }

        #endregion
    }
}