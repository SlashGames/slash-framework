// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ILocalizationTable.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Tools.BlueprintEditor.Logic.Localization
{
    using System.Collections.Generic;

    public interface ILocalizationTable : IEnumerable<KeyValuePair<string, string>>
    {
        #region Public Indexers

        string this[string key] { get; set; }

        #endregion
    }
}