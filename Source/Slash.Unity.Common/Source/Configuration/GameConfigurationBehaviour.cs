// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GameConfigurationBehaviour.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Unity.Common.Configuration
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Xml;
    using System.Xml.Serialization;

    using Slash.Collections.AttributeTables;
    using Slash.GameBase.Blueprints;
    using Slash.Serialization.Binary;

    using UnityEngine;

    using Object = UnityEngine.Object;

    /// <summary>
    ///   Behaviour to edit the game configuration inside Unity.
    /// </summary>
    public class GameConfigurationBehaviour : MonoBehaviour
    {
        #region Fields

        /// <summary>
        ///   Folder to blueprint manager assets.
        /// </summary>
        public string BlueprintAssetsFolder;

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

        private BlueprintManager LoadBlueprints()
        {
            var blueprintManager = new BlueprintManager();
            XmlSerializer blueprintManagerSerializer = null;

            Object[] blueprintAssets = Resources.LoadAll(this.BlueprintAssetsFolder);

            foreach (var blueprintObjectAsset in blueprintAssets)
            {
                var blueprintAsset = blueprintObjectAsset as TextAsset;

                if (blueprintAsset != null)
                {
                    var blueprintStream = new MemoryStream(blueprintAsset.bytes);

                    // Load blueprints.
                    BlueprintManager subBlueprintManager = null;

                    if (Application.isEditor)
                    {
                        if (blueprintManagerSerializer == null)
                        {
                            blueprintManagerSerializer = new XmlSerializer(typeof(BlueprintManager));
                        }
                        try
                        {
                            subBlueprintManager = (BlueprintManager)blueprintManagerSerializer.Deserialize(blueprintStream);
                        }
                        catch (XmlException e)
                        {
                            Debug.LogError(
                                string.Format("Exception deserializing blueprint xml '{0}': {1}", blueprintAsset.name, e.Message));
                        }
                    }
                    else
                    {
                        BinaryDeserializer binaryDeserializer = new BinaryDeserializer(blueprintStream);
                        subBlueprintManager = binaryDeserializer.Deserialize<BlueprintManager>();
                    }

                    if (subBlueprintManager != null)
                    {
                        blueprintManager.AddBlueprints(subBlueprintManager);
                    }
                }
                else
                {
                    Debug.LogError(string.Format("Blueprint asset is no text asset: {0}", blueprintObjectAsset.name));
                }
            }

            // Resolve parents.
            BlueprintUtils.ResolveParents(blueprintManager, blueprintManager);

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
                this.BlueprintManager = this.LoadBlueprints();

                Debug.Log(string.Format("{0} blueprints loaded.", this.BlueprintManager.Blueprints.Count()));
            }
        }

        #endregion
    }
}