// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Language.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Globalization
{
    using System.Globalization;

    public class Language
    {
        #region Public Properties

        /// <summary>
        ///   Localized string that is suitable for display to the user for identifying the language.
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        ///   Normalized BCP-47 language tag for this language (languagecode2-country/regioncode2).
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///   Name of the language in the language itself.
        /// </summary>
        public string NativeName { get; set; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///   Gets the language the application is currently using.
        /// </summary>
        /// <returns>Name and BCP-47 language tag of the language the application is currently using.</returns>
        public static Language GetCurrentLanguage()
        {
            // Get the locale the application is currently using.
            // http://stackoverflow.com/questions/5710127/get-operating-system-language-in-c-sharp
            CultureInfo currentCulture = CultureInfo.CurrentCulture;

            Language language = new Language
                {
                    DisplayName = currentCulture.DisplayName,
                    Name = currentCulture.Name,
                    NativeName = currentCulture.NativeName
                };

            return language;
        }

        public override string ToString()
        {
            return string.Format(
                "Display Name: {0}, Name: {1}, Native Name: {2}", this.DisplayName, this.Name, this.NativeName);
        }

        #endregion
    }
}