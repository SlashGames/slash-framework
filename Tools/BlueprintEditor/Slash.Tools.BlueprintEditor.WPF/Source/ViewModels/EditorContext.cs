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
        
        #endregion

        #region Public Events
        
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

                this.BlueprintManagerViewModel = new BlueprintManagerViewModel(this.blueprintManager)
                    {
                        AssemblyComponents = this.AvailableComponentTypes
                    };
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

                UndoRoot oldUndoRoot = this.UndoRoot;
                if (oldUndoRoot != null)
                {
                    oldUndoRoot.UndoStackChanged -= this.OnUndoStackChanged;
                    oldUndoRoot.RedoStackChanged -= this.OnRedoStackChanged;
                }

                this.blueprintManagerViewModel = value;

                // Monitor undo system.
                UndoRoot undoRoot = this.UndoRoot;
                if (undoRoot != null)
                {
                    undoRoot.UndoStackChanged += this.OnUndoStackChanged;
                    undoRoot.RedoStackChanged += this.OnRedoStackChanged;
                }

                this.OnPropertyChanged("BlueprintManagerViewModel");
            }
        }

        /// <summary>
        ///   Project to edit.
        /// </summary>
        public ProjectSettings ProjectSettings { get; set; }

        /// <summary>
        ///   Dynamic description for redo action.
        /// </summary>
        public string RedoDescription
        {
            get
            {
                UndoRoot undoRoot = this.UndoRoot;
                ChangeSet lastChange = undoRoot != null ? undoRoot.RedoStack.FirstOrDefault() : null;
                return lastChange != null ? string.Format("_Redo '{0}'", lastChange.Description) : "_Redo";
            }
        }

        /// <summary>
        ///   File path to store project xml at.
        /// </summary>
        public string SerializationPath { get; set; }

        /// <summary>
        ///   Dynamic description for undo action.
        /// </summary>
        public string UndoDescription
        {
            get
            {
                UndoRoot undoRoot = this.UndoRoot;
                ChangeSet lastChange = undoRoot != null ? undoRoot.UndoStack.FirstOrDefault() : null;
                return lastChange != null ? string.Format("_Undo '{0}'", lastChange.Description) : "_Undo";
            }
        }

        #endregion

        #region Properties

        /// <summary>
        ///   Returns the undo root for the current blueprint manager view model.
        /// </summary>
        private UndoRoot UndoRoot
        {
            get
            {
                return UndoService.Current[this.blueprintManagerViewModel];
            }
        }

        #endregion

        #region Public Methods and Operators

        public bool CanExecuteRedo()
        {
            var undoRoot = UndoService.Current[this.BlueprintManagerViewModel];
            return undoRoot != null && undoRoot.CanRedo;
        }

        public bool CanExecuteUndo()
        {
            var undoRoot = UndoService.Current[this.BlueprintManagerViewModel];
            return undoRoot != null && undoRoot.CanUndo;
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
            if (this.BlueprintManagerViewModel != null)
            {
                this.BlueprintManagerViewModel.AssemblyComponents = this.ProjectSettings.EntityComponentTypes;
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

        private void OnRedoStackChanged(object sender, EventArgs e)
        {
            this.OnPropertyChanged("RedoDescription");
        }

        private void OnUndoStackChanged(object sender, EventArgs eventArgs)
        {
            this.OnPropertyChanged("UndoDescription");
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