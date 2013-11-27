// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InspectorComponentAttribute.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.GameBase.Inspector.Attributes
{
    using System;

    /// <summary>
    ///   Exposes the component to the inspector.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class InspectorComponentAttribute : Attribute
    {
        #region Constructors and Destructors

        /// <summary>
        ///   Constructor.
        /// </summary>
        public InspectorComponentAttribute()
        {
            this.CanBeRemoved = true;
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///   Whether this component can be removed from a blueprint in the inspector, or not.
        ///   Default: true.
        /// </summary>
        public bool CanBeRemoved { get; set; }

        /// <summary>
        ///   Component priority. Specifies the position the component appears within lists.
        ///   A lower value indicates a higher priority.
        /// </summary>
        public int Priority { get; set; }

        #endregion
    }
}