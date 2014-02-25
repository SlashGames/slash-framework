// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LocalizationContext.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace BlueprintEditor.ViewModels
{
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.IO;

    using Slash.Tools.BlueprintEditor.Logic.Context;
    using Slash.Tools.BlueprintEditor.Logic.Localization;

    public class LocalizationContext
    {
        #region Fields

        private readonly EditorContext context;

        private readonly Dictionary<string, ILocalizationTable> languages = new Dictionary<string, ILocalizationTable>();

        private readonly ILocalizationTableSerializer localizationTableSerializer =
            new LocalizationTableNGUISerializer();

        #endregion

        #region Constructors and Destructors

        public LocalizationContext(EditorContext context)
        {
            this.context = context;

            this.context.PropertyChanged += this.OnContextPropertyChanged;
        }

        #endregion

        #region Delegates

        public delegate void ProjectLanguageChangedDelegate(string newLanguage);

        #endregion

        #region Public Events

        public event ProjectLanguageChangedDelegate ProjectLanguageChanged;

        #endregion

        #region Public Properties

        public string ProjectLanguage
        {
            get
            {
                return this.context.ProjectLanguage;
            }
        }

        public bool RawLocalizationKeys
        {
            get
            {
                return this.ProjectLanguage == EditorSettings.LanguageTagRawLocalizationKeys;
            }
        }

        #endregion

        #region Properties

        private string CurrentBlueprintId
        {
            get
            {
                return this.context.SelectedBlueprint.BlueprintId;
            }
        }

        #endregion

        #region Indexers

        private string this[string localizationKey]
        {
            get
            {
                if (this.ProjectLanguage == EditorSettings.LanguageTagRawLocalizationKeys)
                {
                    return localizationKey;
                }

                return this.languages[this.ProjectLanguage][localizationKey];
            }

            set
            {
                if (this.RawLocalizationKeys)
                {
                    return;
                }

                this.languages[this.ProjectLanguage][localizationKey] = value;
            }
        }

        #endregion

        #region Public Methods and Operators

        public string GetLocalizedStringForBlueprint(string blueprintId, string key)
        {
            var localizationKey = this.GetLocalizationKey(blueprintId, key);
            return this[localizationKey];
        }

        public string GetLocalizedStringForCurrentBlueprint(string key)
        {
            var localizationKey = this.GetLocalizationKey(this.CurrentBlueprintId, key);
            var localizedValue = this[localizationKey];
            return localizedValue;
        }

        public void LoadLanguages()
        {
            this.languages.Clear();

            foreach (var languageFile in this.context.ProjectSettings.LanguageFiles)
            {
                var fileInfo = new FileInfo(languageFile.Path);

                using (var stream = fileInfo.OpenRead())
                {
                    var languageTag = Path.GetFileNameWithoutExtension(languageFile.Path);
                    var localizationTable = this.localizationTableSerializer.Deserialize(stream);

                    this.languages.Add(languageTag, localizationTable);
                }
            }
        }

        public void SaveLanguages()
        {
            foreach (var languageFile in this.context.ProjectSettings.LanguageFiles)
            {
                var fileInfo = new FileInfo(languageFile.Path);

                using (var stream = fileInfo.Create())
                {
                    var languageTag = Path.GetFileNameWithoutExtension(languageFile.Path);
                    var localizationTable = this.languages[languageTag];

                    this.localizationTableSerializer.Serialize(stream, localizationTable);
                }
            }
        }

        public void SetLocalizedStringForBlueprint(string blueprintId, string key, string value)
        {
            var localizationKey = this.GetLocalizationKey(blueprintId, key);
            this[localizationKey] = value;
        }

        public void SetLocalizedStringForCurrentBlueprint(string key, string value)
        {
            var localizationKey = this.GetLocalizationKey(this.CurrentBlueprintId, key);
            this[localizationKey] = value;
        }

        #endregion

        #region Methods

        private string GetLocalizationKey(string blueprintId, string key)
        {
            return string.Format("{0}.{1}", blueprintId, key);
        }

        private void OnContextPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "ProjectLanguage")
            {
                this.OnProjectLanguageChanged(this.ProjectLanguage);
            }
        }

        private void OnProjectLanguageChanged(string newLanguage)
        {
            var handler = this.ProjectLanguageChanged;
            if (handler != null)
            {
                handler(newLanguage);
            }
        }

        #endregion
    }
}