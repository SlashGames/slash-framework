// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ExposePropertyAttribute.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Unity.Common.Attributes
{
    using System;

    /// <summary>
    ///   Exposes the property in the Unity inspector. Use in conjunction with
    ///   <c>Slash.Unity.Editor.Common.Inspectors.ExposeProperties</c>.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class ExposePropertyAttribute : Attribute
    {
    }
}