// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LocalizationTableNGUISerializer.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Tools.BlueprintEditor.Logic.Localization
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    using Slash.SystemExt.Exceptions;

    public class LocalizationTableNGUISerializer : ILocalizationTableSerializer
    {
        #region Public Methods and Operators

        public ILocalizationTable Deserialize(Stream stream)
        {
            LocalizationTable table = new LocalizationTable();
            List<Exception> errors = new List<Exception>();

            using (TextReader reader = new StreamReader(stream))
            {
                string line;

                while ((line = reader.ReadLine()) != null)
                {
                    var keyValuePair = line.Split('=');

                    var key = keyValuePair[0];
                    var value = keyValuePair[1];

                    if (table.ContainsKey(key))
                    {
                        errors.Add(new ArgumentException(string.Format("Duplicate localization key: {0}", key)));
                    }

                    table[key] = value;
                }
            }

            if (errors.Count > 0)
            {
                throw new AggregateException(errors);
            }

            return table;
        }

        public void Serialize(Stream stream, ILocalizationTable table)
        {
            // Sort localization table entries by key before writing to the file.
            List<KeyValuePair<string, string>> data = table.ToList();
            data.Sort((first, second) => string.Compare(first.Key, second.Key, StringComparison.Ordinal));

            // Write data to file.
            using (TextWriter writer = new StreamWriter(stream))
            {
                foreach (var keyValuePair in data)
                {
                    writer.WriteLine("{0}={1}", keyValuePair.Key, keyValuePair.Value);
                }
            }
        }

        #endregion
    }
}