// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LocalizationBehaviour.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Unity.NGUIExt.Globalization
{
    using System.IO;

    using UnityEngine;

    public class LocalizationBehaviour : MonoBehaviour
    {
        #region Public Methods and Operators

        /// <summary>
        ///   Utility method to convert a Unity system language to the associated language code.
        ///   TODO(co): Complete list.
        /// </summary>
        /// <param name="language">Language to get code for.</param>
        /// <returns>Language code for the specified system language.</returns>
        public static string SystemLanguageToLanguageCode(SystemLanguage language)
        {
            switch (language)
            {
                case SystemLanguage.German:
                    return "de";
                case SystemLanguage.English:
                    return "en";
                case SystemLanguage.Spanish:
                    return "es";
                case SystemLanguage.French:
                    return "fr";
                case SystemLanguage.Italian:
                    return "it";
                default:
                    return null;
            }
        }

        #endregion

        #region Methods

        private void Start()
        {
#if !UNITY_METRO
            // Check for overridden localization data.
            var localizationFilePath = Application.persistentDataPath + "/Localization.txt";
            FileInfo localizationFile = new FileInfo(localizationFilePath);

            if (localizationFile.Exists)
            {
                Debug.Log("Found localization file at " + localizationFilePath);

                using (var fileStream = localizationFile.OpenRead())
                {
                    using (var binaryReader = new BinaryReader(fileStream))
                    {
                        var bytes = binaryReader.ReadBytes((int)localizationFile.Length);
                        var nguiByteReader = new ByteReader(bytes);
                        var localizationData = nguiByteReader.ReadDictionary();
                        Localization.Set("TestLanguage", localizationData);
                        return;
                    }
                }
            }
#endif

            // Get current system language.
            SystemLanguage systemLanguage = Application.systemLanguage;
            Debug.Log(string.Format("System Language: {0}", systemLanguage));

            // Check if available, otherwise fallback to English.
            TextAsset textAsset = Resources.Load(systemLanguage.ToString(), typeof(TextAsset)) as TextAsset;
            if (textAsset == null)
            {
                systemLanguage = SystemLanguage.English;
                Debug.Log(string.Format("Using fallback language: {0}", systemLanguage));
            }

            Localization.language = systemLanguage.ToString();
        }

        #endregion
    }
}