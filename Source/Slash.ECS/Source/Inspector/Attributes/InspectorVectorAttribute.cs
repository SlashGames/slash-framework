﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InspectorVectorAttribute.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.ECS.Inspector.Attributes
{
    using System;

    /// <summary>
    ///   Exposes the property to the inspector.
    /// </summary>
    [Serializable]
    public class InspectorVectorAttribute : InspectorPropertyAttribute
    {
        #region Constructors and Destructors

        /// <summary>
        ///   Constructor.
        /// </summary>
        /// <param name="name">Property name to be shown in the inspector.</param>
        public InspectorVectorAttribute(string name)
        {
            this.Name = name;
        }

        /// <summary>
        ///   Constructor.
        /// </summary>
        public InspectorVectorAttribute()
        {
        }

        #endregion
    }
}