// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EnumFlagAttribute.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Unity.Common.Utils
{
    using UnityEngine;

    public class EnumFlagAttribute : PropertyAttribute
    {
        #region Constructors and Destructors

        public EnumFlagAttribute()
        {
        }

        public EnumFlagAttribute(string name)
        {
            this.Name = name;
        }

        #endregion

        #region Properties

        public string Name { get; private set; }

        #endregion
    }
}