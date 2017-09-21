// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TypePropertyAttribute.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Unity.InspectorExt.PropertyDrawers
{
    using System;
    using UnityEngine;

    public class TypePropertyAttribute : PropertyAttribute
    {
        /// <summary>
        ///   Base type to choose a type from.
        /// </summary>
        public Type BaseType;

        /// <summary>
        ///   Indicates if the fully qualified name should be shown in the popup.
        /// </summary>
        public bool UseFullName;
    }
}