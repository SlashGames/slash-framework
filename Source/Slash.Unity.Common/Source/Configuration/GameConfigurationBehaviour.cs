// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GameConfigurationBehaviour.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Unity.Common.Configuration
{
    using System.IO;
    using System.Xml.Serialization;

    using Slash.Collections.AttributeTables;
    using Slash.GameBase.Blueprints;

    using UnityEngine;

    /// <summary>
    ///   Behaviour to edit the game configuration inside Unity.
    /// </summary>
    public class GameConfigurationBehaviour : MonoBehaviour
    {
        #region Fields

        /// <summary>
        ///   Path to blueprint manager asset.
        /// </summary>
        public string BlueprintAssetPath = "Blueprints/Game.blueprints";

        /// <summary>
        ///   Path to configuration file asset.
        /// </summary>
        public string ConfigurationFilePath = "Configuration/GameConfiguration";

        private IAttributeTable configuration;

        #endregion

        #region Public Properties

        /// <summary>
        ///   Load blueprint manager from asset.
        /// </summary>
        public BlueprintManager BlueprintManager { get; private set; }

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
            throw new System.NotImplementedException("Not implemented for Windows Store build target.");
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

        private BlueprintManager LoadBlueprints()
        {
            var blueprintAsset = Resources.Load(this.BlueprintAssetPath) as TextAsset;

            BlueprintManager blueprintManager = null;
            if (blueprintAsset != null)
            {
                // Load blueprints.
                var blueprintManagerSerializer = new XmlSerializer(typeof(BlueprintManager));
                var blueprintStream = new MemoryStream(blueprintAsset.bytes);
                blueprintManager = (BlueprintManager)blueprintManagerSerializer.Deserialize(blueprintStream);

                // Resolve parents.
                BlueprintUtils.ResolveParents(blueprintManager, blueprintManager);
            }
            else
            {
                Debug.LogError(string.Format("Blueprint asset not found: {0}", this.BlueprintAssetPath));
            }

            return blueprintManager;
        }

        private void OnEnable()
        {
            if (this.Configuration == null)
            {
                this.Load();
            }

            if (this.BlueprintManager == null)
            {
                this.LoadBlueprints();
            }
        }

        #endregion
    }
}