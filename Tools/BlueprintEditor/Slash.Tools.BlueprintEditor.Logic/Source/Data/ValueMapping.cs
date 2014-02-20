// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ValueMapping.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Tools.BlueprintEditor.Logic.Data
{
    using System;

    [Serializable]
    public class ValueMapping
    {
        #region Public Properties

        public string MappingSource { get; set; }

        public string MappingTarget { get; set; }

        #endregion
    }
}