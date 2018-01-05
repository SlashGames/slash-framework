// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LogicToVisualMappingAttribute.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Unity.Common.ECS
{
    using System;

    [AttributeUsage(AttributeTargets.Class)]
    public class LogicToVisualMappingAttribute : Attribute
    {
        #region Public Properties

        /// <summary>
        ///   Type of logic component the mono behaviour visualizes.
        /// </summary>
        public Type LogicType { get; set; }

        #endregion
    }
}