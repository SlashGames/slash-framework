// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CsvImportData.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Tools.BlueprintEditor.Logic.Data
{
    using System;
    using System.Collections.Generic;

    [Serializable]
    public class CsvImportData
    {
        #region Public Properties

        public string BlueprintIdColumn { get; set; }

        public string BlueprintParentId { get; set; }

        public string IgnoredBlueprintId { get; set; }

        public List<ValueMapping> Mappings { get; set; }

        #endregion
    }
}