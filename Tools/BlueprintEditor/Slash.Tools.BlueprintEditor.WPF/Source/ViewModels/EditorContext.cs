// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EditorContext.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace BlueprintEditor.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.IO;
    using System.Linq;
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    using MonitoredUndo;

    using Slash.GameBase.Blueprints;
    using Slash.Tools.BlueprintEditor.Logic.Annotations;
    using Slash.Tools.BlueprintEditor.Logic.Context;

    public sealed class EditorContext : INotifyPropertyChanged
    {
        #region Static Fields

        /// <summary>
        ///   Default blueprint file extension.
        /// </summary>
        public static string ProjectBlueprintExtension = "blueprints";

        /// <summary>
        ///   Default project file extension.
        /// </summary>
        public static string ProjectExtension = "bep";

        #endregion

        #region Fields

        private readonly XmlSerializer blueprintManagerSerializer;

        private readonly XmlSerializer projectSettingsSerializer;

        /// <summary>
        ///   Active blueprint manager.
        /// </summary>
        private BlueprintManager blueprintManager;

        private BlueprintManagerViewModel blueprintManagerViewModel;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///   Constructor.
        /// </summary>
        public EditorContext()
        {
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

        public event EntityComponentTypesChangedDelegate EntityComponentTypesChanged;

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Public Properties

        public IEnumerable<Type> AvailableComponentTypes
        {
            get
            {
                return this.ProjectSettings != null ? this.ProjectSettings.EntityComponentTypes : null;
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

                this.blueprintManager = value;

                // Raise event.
                this.OnPropertyChanged("BlueprintManager");

                this.BlueprintManagerViewModel = new BlueprintManagerViewModel(this.blueprintManager);
            }
        }

        public BlueprintManagerViewModel BlueprintManagerViewModel
        {
            get
            {
                return this.blueprintManagerViewModel;
            }
            set
            {
                if (value == this.blueprintManagerViewModel)
                {
                    return;
                }

                this.blueprintManagerViewModel = value;

                this.OnPropertyChanged("BlueprintManagerViewModel");
            }
        }

        /// <summary>
        ///   Project to edit.
        /// </summary>
        public ProjectSettings ProjectSettings { get; set; }

        /// <summary>
        ///   File path to store project xml at.
        /// </summary>
        public string SerializationPath { get; set; }

        #endregion

        #region Public Methods and Operators

        public bool CanExecuteRedo()
        {
            return UndoService.Current[this.BlueprintManagerViewModel].CanRedo;
        }

        public bool CanExecuteUndo()
        {
            return UndoService.Current[this.BlueprintManagerViewModel].CanUndo;
        }

        public void Load(string path)
        {
            // Load project.
            FileStream fileStream = new FileStream(path, FileMode.Open);
            ProjectSettings newProjectSettings = (ProjectSettings)this.projectSettingsSerializer.Deserialize(fileStream);
            if (newProjectSettings == null)
            {
                throw new SerializationException(
                    string.Format("Couldn't deserialize project settings from '{0}'.", path));
            }

            fileStream.Close();

            // Load blueprint files.
            foreach (var blueprintFile in newProjectSettings.BlueprintFiles)
            {
                FileStream blueprintFileStream = new FileStream(blueprintFile.Path, FileMode.Open);
                BlueprintManager newBlueprintManager;
                try
                {
                    newBlueprintManager =
                        (BlueprintManager)this.blueprintManagerSerializer.Deserialize(blueprintFileStream);
                }
                catch (Exception e)
                {
                    throw new SerializationException(
                        string.Format(
                            "Couldn't deserialize blueprint manager from '{0}': {1}.",
                            path,
                            e.GetBaseException().Message),
                        e);
                }

                if (newBlueprintManager == null)
                {
                    throw new SerializationException(
                        string.Format("Couldn't deserialize blueprint manager from '{0}'.", path));
                }
                blueprintFile.BlueprintManager = newBlueprintManager;
                blueprintFileStream.Close();
            }

            // Set new project.
            this.SetProject(newProjectSettings, path);
        }

        public void New()
        {
            ProjectSettings newProjectSettings = new ProjectSettings();
            newProjectSettings.BlueprintFiles.Add(new BlueprintFile { BlueprintManager = new BlueprintManager() });

            // Set new project.
            this.SetProject(newProjectSettings);
        }

        public void Redo()
        {
            UndoService.Current[this.BlueprintManagerViewModel].Redo();
        }

        public void Save()
        {
            if (this.ProjectSettings == null)
            {
                return;
            }

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

        public void Undo()
        {
            UndoService.Current[this.BlueprintManagerViewModel].Undo();
        }

        #endregion

        #region Methods

        private static string GenerateBlueprintFilePath(string projectPath, int fileIndex)
        {
            return string.Format(
                "{0}{1}.{2}",
                Path.Combine(Path.GetDirectoryName(projectPath), Path.GetFileNameWithoutExtension(projectPath)),
                fileIndex == 0 ? string.Empty : string.Format("({0})", fileIndex),
                ProjectBlueprintExtension);
        }

        private void OnEntityComponentTypesChanged()
        {
            EntityComponentTypesChangedDelegate handler = this.EntityComponentTypesChanged;
            if (handler != null)
            {
                handler();
            }
        }

        [NotifyPropertyChangedInvocator]
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = this.PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private void SetProject(ProjectSettings projectSettings, string serializationPath = null)
        {
            this.ProjectSettings = projectSettings;
            this.SerializationPath = serializationPath;

            this.ProjectSettings.EntityComponentTypesChanged += this.OnEntityComponentTypesChanged;

            // Set first blueprint file as active blueprint manager.
            BlueprintFile firstBlueprintFile = this.ProjectSettings.BlueprintFiles.FirstOrDefault();
            this.BlueprintManager = firstBlueprintFile != null ? firstBlueprintFile.BlueprintManager : null;

            // Raise events.
            this.OnPropertyChanged("ProjectSettings");
            this.OnEntityComponentTypesChanged();
        }

        #endregion
    }
}