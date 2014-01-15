// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InspectorTypeAttribute.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.GameBase.Inspector.Attributes
{
    using System;

    /// <summary>
    ///   Exposes the type to the inspector.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class InspectorTypeAttribute : Attribute
    {
        #region Public Properties

        /// <summary>
        ///   A user-friendly description of the type.
        /// </summary>
        public string Description { get; set; }

        #endregion
    }
}