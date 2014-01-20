// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GameConfigurationBehaviour.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Unity.Common.Configuration
{
    using System;
    using System.IO;
    using System.Xml.Serialization;

    using Slash.Collections.AttributeTables;

    using UnityEngine;

    /// <summary>
    ///   Behaviour to edit the game configuration inside Unity.
    /// </summary>
    public class GameConfigurationBehaviour : MonoBehaviour
    {
        #region Fields

        public string ConfigurationFilePath = "Configuration/GameConfiguration";

        private IAttributeTable configuration;

        #endregion

        #region Public Properties

        public IAttributeTable Configuration
        {
            get
            {
                if (this.configuration == null)
                {
                    this.Load();
                }
                return this.configuration;
            }
            set
            {
                this.configuration = value;
            }
        }

        #endregion

        #region Public Methods and Operators

        public void Load()
        {
            Debug.Log("Loading game configuration from resources at " + this.ConfigurationFilePath);

            TextAsset configurationFile = (TextAsset)Resources.Load(this.ConfigurationFilePath);
            if (configurationFile == null)
            {
                Debug.LogWarning("No configuration file at " + this.ConfigurationFilePath);
                this.Configuration = new AttributeTable();
                return;
            }

            XmlSerializer xmlSerializer = new XmlSerializer(typeof(AttributeTable));
            this.Configuration = (IAttributeTable)xmlSerializer.Deserialize(new StringReader(configurationFile.text));
        }

        public void Save()
        {
#if WINDOWS_STORE
            throw new NotImplementedException("Not implemented for Windows Store build target.");
#else
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(AttributeTable));
            string filePath = "Assets/Resources/" + this.ConfigurationFilePath + ".xml";
            // Make sure directory exists.
            Directory.CreateDirectory(Path.GetDirectoryName(filePath));

            Debug.Log("Save to " + filePath);
            StreamWriter writer = new StreamWriter(filePath);
            xmlSerializer.Serialize(writer, this.Configuration);
            writer.Close();
#endif
        }

        #endregion

        #region Methods

        private void OnEnable()
        {
            if (this.Configuration == null)
            {
                this.Load();
            }
        }

        #endregion
    }
}