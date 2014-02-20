// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LocalizationTableNGUISerializer.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Tools.BlueprintEditor.Logic.Localization
{
    using System.IO;

    public class LocalizationTableNGUISerializer : ILocalizationTableSerializer
    {
        #region Public Methods and Operators

        public ILocalizationTable Deserialize(Stream stream)
        {
            LocalizationTable table = new LocalizationTable();

            using (TextReader reader = new StreamReader(stream))
            {
                string line;

                while ((line = reader.ReadLine()) != null)
                {
                    var keyValuePair = line.Split('=');

                    var key = keyValuePair[0];
                    var value = keyValuePair[1];

                    table[key] = value;
                }
            }

            return table;
        }

        public void Serialize(Stream stream, ILocalizationTable table)
        {
            using (TextWriter writer = new StreamWriter(stream))
            {
                foreach (var keyValuePair in table)
                {
                    writer.WriteLine(string.Format("{0}={1}", keyValuePair.Key, keyValuePair.Value));
                }
            }
        }

        #endregion
    }
}