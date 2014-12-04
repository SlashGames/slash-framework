// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TypePropertyAttribute.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Unity.Common.PropertyDrawers
{
    using System;

    using UnityEngine;

    public class TypePropertyAttribute : PropertyAttribute
    {
        #region Fields

        /// <summary>
        ///   Base type to choose a type from.
        /// </summary>
        public Type BaseType;

        #endregion
    }
}