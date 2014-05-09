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
        #region Static Fields

        public static string DefaultDelimiter = ";";

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///   Constructor.
        /// </summary>
        public CsvImportData()
        {
            this.Delimiter = DefaultDelimiter;
        }

        #endregion

        #region Public Properties

        public string BlueprintIdColumn { get; set; }

        public string BlueprintParentId { get; set; }

        /// <summary>
        ///   Field delimiter in CSV file.
        /// </summary>
        public string Delimiter { get; set; }

        public string IgnoredBlueprintId { get; set; }

        public List<ValueMapping> Mappings { get; set; }

        #endregion
    }
}