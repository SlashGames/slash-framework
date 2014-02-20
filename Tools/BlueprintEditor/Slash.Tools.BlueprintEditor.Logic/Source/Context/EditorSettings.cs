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
        #region Public Properties

        /// <summary>
        ///   Normalized BCP-47 language tag for the language to show all localized strings in. <c>null</c> shows raw localization keys.
        /// </summary>
        public string ProjectLanguage { get; set; }

        #endregion
    }
}