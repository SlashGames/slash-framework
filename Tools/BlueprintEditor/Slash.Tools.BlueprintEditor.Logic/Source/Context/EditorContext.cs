// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EditorContext.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Tools.BlueprintEditor.Logic.Context
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    using Slash.GameBase.Blueprints;

    public sealed class EditorContext
    {
        #region Fields

        private readonly XmlSerializer blueprintManagerSerializer;

        private readonly XmlSerializer projectSettingsSerializer;

        /// <summary>
        ///   Active blueprint manager.
        /// </summary>
        private BlueprintManager blueprintManager;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///   Constructor.
        /// </summary>
        public EditorContext()
        {
            this.ProjectSettings = new ProjectSettings();
            BlueprintManager initialBlueprintManager = new BlueprintManager();
            this.ProjectSettings.BlueprintFiles.Add(new BlueprintFile { BlueprintManager = initialBlueprintManager });
            this.ProjectSettings.EntityComponentTypesChanged += this.OnEntityComponentTypesChanged;
            this.BlueprintManager = initialBlueprintManager;
            this.blueprintManagerSerializer = new XmlSerializer(typeof(BlueprintManager));
            this.projectSettingsSerializer = new XmlSerializer(typeof(ProjectSettings));
        }

        #endregion

        #region Delegates

        public delegate void BlueprintManagerChangedDelegate(
            BlueprintManager newBlueprintManager, BlueprintManager oldBlueprintManager);

        public delegate void EntityComponentTypesChangedDelegate();

        #endregion

        #region Public Events

        /// <summary>
        ///   Raised when the blueprint manager in the context changed.
        /// </summary>
        public event BlueprintManagerChangedDelegate BlueprintManagerChanged;

        public event EntityComponentTypesChangedDelegate EntityComponentTypesChanged;

        #endregion

        #region Public Properties

        public IEnumerable<Type> AvailableComponentTypes
        {
            get
            {
                return this.ProjectSettings.EntityComponentTypes;
            }
        }

        /// <summary>
        ///   Blueprint manager which is edited.
        /// </summary>
        public BlueprintManager BlueprintManager
        {
            get
            {
                return this.blueprintManager;
            }
            private set
            {
                if (ReferenceEquals(value, this.blueprintManager))
                {
                    return;
                }

                BlueprintManager oldBlueprintManager = this.blueprintManager;
                this.blueprintManager = value;

                this.OnBlueprintManagerChanged(this.blueprintManager, oldBlueprintManager);
            }
        }

        public string SerializationPath { get; set; }

        #endregion

        #region Properties

        /// <summary>
        ///   Project to edit.
        /// </summary>
        private ProjectSettings ProjectSettings { get; set; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///   Adds the assembly at the specified path to the project.
        /// </summary>
        /// <param name="assemblyPath">Path to assembly.</param>
        public void AddAssembly(string assemblyPath)
        {
            Assembly assembly = Assembly.LoadFile(assemblyPath);
            this.ProjectSettings.AddAssembly(assembly);
        }

        public void Load(string path)
        {
            // Load project.
            FileStream fileStream = new FileStream(path, FileMode.Open);
            this.ProjectSettings = (ProjectSettings)this.projectSettingsSerializer.Deserialize(fileStream);
            if (this.ProjectSettings == null)
            {
                throw new SerializationException(
                    string.Format("Couldn't deserialize project settings from '{0}'.", path));
            }
            this.ProjectSettings.EntityComponentTypesChanged += this.OnEntityComponentTypesChanged;

            fileStream.Close();
            this.SerializationPath = path;

            // Load blueprint files.
            foreach (var blueprintFile in this.ProjectSettings.BlueprintFiles)
            {
                FileStream blueprintFileStream = new FileStream(blueprintFile.Path, FileMode.Open);
                BlueprintManager newBlueprintManager =
                    (BlueprintManager)this.blueprintManagerSerializer.Deserialize(blueprintFileStream);
                if (newBlueprintManager == null)
                {
                    throw new SerializationException(
                        string.Format("Couldn't deserialize blueprint manager from '{0}'.", path));
                }
                blueprintFile.BlueprintManager = newBlueprintManager;
                blueprintFileStream.Close();
            }

            // Set first blueprint file as active blueprint manager.
            BlueprintFile firstBlueprintFile = this.ProjectSettings.BlueprintFiles.FirstOrDefault();
            this.BlueprintManager = firstBlueprintFile != null ? firstBlueprintFile.BlueprintManager : null;

            // Raise events.
            this.OnEntityComponentTypesChanged();
        }

        public void New()
        {
            this.BlueprintManager = new BlueprintManager();
            this.SerializationPath = null;
        }

        public void Save()
        {
            // Save blueprint files.
            for (int index = 0; index < this.ProjectSettings.BlueprintFiles.Count; index++)
            {
                var blueprintFile = this.ProjectSettings.BlueprintFiles[index];

                // Set generic blueprint file path if not set.
                if (blueprintFile.Path == null)
                {
                    blueprintFile.Path = GenerateBlueprintFilePath(this.SerializationPath, index);
                }

                var blueprintFileStream = new FileStream(blueprintFile.Path, FileMode.Create);
                this.blueprintManagerSerializer.Serialize(blueprintFileStream, blueprintFile.BlueprintManager);
                blueprintFileStream.Close();
            }

            // Save project.
            var fileStream = new FileStream(this.SerializationPath, FileMode.Create);
            this.projectSettingsSerializer.Serialize(fileStream, this.ProjectSettings);
            fileStream.Close();
        }

        #endregion

        #region Methods

        private static string GenerateBlueprintFilePath(string projectPath, int fileIndex)
        {
            return string.Format(
                "{0}_{1}.xml",
                Path.Combine(Path.GetDirectoryName(projectPath), Path.GetFileNameWithoutExtension(projectPath)),
                fileIndex);
        }

        private void OnBlueprintManagerChanged(
            BlueprintManager newBlueprintManager, BlueprintManager oldBlueprintManager)
        {
            BlueprintManagerChangedDelegate handler = this.BlueprintManagerChanged;
            if (handler != null)
            {
                handler(newBlueprintManager, oldBlueprintManager);
            }
        }

        private void OnEntityComponentTypesChanged()
        {
            EntityComponentTypesChangedDelegate handler = this.EntityComponentTypesChanged;
            if (handler != null)
            {
                handler();
            }
        }

        #endregion
    }
}