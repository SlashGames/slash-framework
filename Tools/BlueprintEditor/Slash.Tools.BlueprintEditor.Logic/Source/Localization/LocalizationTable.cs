// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LocalizationTable.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Tools.BlueprintEditor.Logic.Localization
{
    using System.Collections;
    using System.Collections.Generic;

    public class LocalizationTable : ILocalizationTable
    {
        #region Fields

        private readonly Dictionary<string, string> localizationTable = new Dictionary<string, string>();

        #endregion

        #region Public Indexers

        public string this[string key]
        {
            get
            {
                string localizedValue;

                if (this.localizationTable.TryGetValue(key, out localizedValue))
                {
                    return localizedValue;
                }

                // Return localization key for easier debugging.
                return key;
            }

            set
            {
                this.localizationTable[key] = value;
            }
        }

        #endregion

        #region Public Methods and Operators

        public IEnumerator<KeyValuePair<string, string>> GetEnumerator()
        {
            return this.localizationTable.GetEnumerator();
        }

        #endregion

        #region Explicit Interface Methods

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        #endregion
    }
}