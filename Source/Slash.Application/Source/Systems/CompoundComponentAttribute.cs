// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CompoundComponentAttribute.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Application.Systems
{
    using System;

    public class CompoundComponentAttribute : Attribute
    {
        #region Properties

        /// <summary>
        ///   Indicates if this component is optional.
        /// </summary>
        public bool IsOptional { get; set; }

        #endregion
    }
}