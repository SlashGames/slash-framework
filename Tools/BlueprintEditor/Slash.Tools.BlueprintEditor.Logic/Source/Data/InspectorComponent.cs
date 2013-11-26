// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InspectorComponent.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Tools.BlueprintEditor.Logic.Data
{
    using System;
    using System.Collections.Generic;

    using Slash.GameBase.Attributes;

    /// <summary>
    ///   Component accessible to the user in the landscape designer.
    /// </summary>
    public class InspectorComponent
    {
        #region Public Properties

        /// <summary>
        ///   Inspector metadata for the component.
        /// </summary>
        public InspectorComponentAttribute Component { get; set; }

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