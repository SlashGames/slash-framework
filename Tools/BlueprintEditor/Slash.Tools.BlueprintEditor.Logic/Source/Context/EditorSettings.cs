// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EditorSettings.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Tools.BlueprintEditor.Logic.Context
{
    using System;

    [Serializable]
    public class EditorSettings
    {
        #region Constants

        /// <summary>
        ///   Language tag that shows raw localization keys.
        /// </summary>
        public const string LanguageTagRawLocalizationKeys = "RAW";

        #endregion

        #region Public Properties

        /// <summary>
        ///   Normalized BCP-47 language tag for the language to show all localized strings in.
        /// </summary>
        public string ProjectLanguage { get; set; }

        #endregion
    }
}