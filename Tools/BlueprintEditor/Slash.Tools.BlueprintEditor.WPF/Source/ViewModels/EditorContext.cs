// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EditorContext.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace BlueprintEditor.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.IO;
    using System.Linq;
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    using BlueprintEditor.Controls;

    using MonitoredUndo;

    using Slash.GameBase.Blueprints;
    using Slash.Reflection.Utils;
    using Slash.Serialization.Binary;
    using Slash.Tools.BlueprintEditor.Logic.Annotations;
    using Slash.Tools.BlueprintEditor.Logic.Context;

    using AggregateException = Slash.SystemExt.Exceptions.AggregateException;

    public sealed class EditorContext : INotifyPropertyChanged
    {
        #region Constants

        private const string EditorSettingsSerializationPath = "BlueprintEditor.settings.xml";

        #endregion

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

        private readonly XmlSerializer editorSettingsSerializer;

        private readonly LocalizationContext localizationContext;

        private readonly XmlSerializer projectSettingsSerializer;

        private bool ReadBlueprintsAsBinary = false;

        private bool WriteBlueprintsAsBinary = false;

        private BlueprintManagerViewModel blueprintManagerViewModel;

        private EditorSettings editorSettings;

        private ProjectExplorerViewModel projectExplorerViewModel;

        private ProjectFileViewModel selectedProjectFile;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///   Constructor.
        /// </summary>
        public EditorContext()
        {
            this.blueprintManagerSerializer = new XmlSerializer(typeof(BlueprintManager));
            this.projectSettingsSerializer = new XmlSerializer(typeof(ProjectSettings));
            this.editorSettingsSerializer = new XmlSerializer(typeof(EditorSettings));

            this.localizationContext = new LocalizationContext(this);
            this.AvailableLanguages = new ObservableCollection<string>();
            this.editorSettings = new EditorSettings();

            this.SetAvailableLanguages(new List<string>());

            this.LoadEditorSettings();
        }

        #endregion

        #region Delegates

        public delegate void AvailableLanguagesChangedDelegate();

        public delegate void BlueprintManagerChangedDelegate(
            BlueprintManager newBlueprintManager, BlueprintManager oldBlueprintManager);

        #endregion

        #region Public Events

        public event AvailableLanguagesChangedDelegate AvailableLanguagesChanged;

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

        public ObservableCollection<string> AvailableLanguages { get; private set; }

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

        public LocalizationContext LocalizationContext
        {
            get
            {
                return this.localizationContext;
            }
        }

        public ProjectExplorerViewModel ProjectExplorerViewModel
        {
            get
            {
                return this.projectExplorerViewModel;
            }
            set
            {
                if (value == this.projectExplorerViewModel)
                {
                    return;
                }

                this.projectExplorerViewModel = value;

                this.OnPropertyChanged("ProjectExplorerViewModel");
            }
        }

        public string ProjectLanguage
        {
            get
            {
                return this.editorSettings.ProjectLanguage;
            }

            set
            {
                if (this.editorSettings.ProjectLanguage == value)
                {
                    return;
                }

                this.editorSettings.ProjectLanguage = value;

                this.OnPropertyChanged("ProjectLanguage");

                this.SaveEditorSettings();
            }
        }

        /// <summary>
        ///   Gets the current project path with trailing backslash.
        /// </summary>
        public string ProjectPath
        {
            get
            {
                return Path.GetDirectoryName(this.SerializationPath) + "\\";
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

        public BlueprintViewModel SelectedBlueprint { get; set; }

        public ProjectFileViewModel SelectedProjectFile
        {
            get
            {
                return this.selectedProjectFile;
            }
            set
            {
                if (this.selectedProjectFile == value)
                {
                    return;
                }

                this.selectedProjectFile = value;

                this.OnSelectedProjectFileChanged();
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

        /// <summary>
        ///   Gets the path to <paramref name="path" /> relative to <paramref name="relativeTo" />.
        /// </summary>
        /// <param name="path">Path to get the relative path of.</param>
        /// <param name="relativeTo">Path to get the relative path to.</param>
        /// <returns>
        ///   Path to <paramref name="path" /> relative to <paramref name="relativeTo" />.
        /// </returns>
        public static string GetRelativePath(string path, string relativeTo)
        {
            var relativeToUri = new Uri(relativeTo);
            var pathUri = new Uri(path);
            var relativePathUri = relativeToUri.MakeRelativeUri(pathUri);

            return Uri.UnescapeDataString(relativePathUri.ToString());
        }

        public bool CanExecuteCopyBlueprint()
        {
            return this.SelectedBlueprint != null;
        }

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

        public void Close()
        {
            if (this.ProjectSettings == null)
            {
                return;
            }

            // TODO(co): Check for changes and ask user if to save before closing.

            this.SetProject(null);
        }

        public void CopyBlueprint()
        {
            this.BlueprintManagerViewModel.CreateNewBlueprint(
                "Copy of " + this.SelectedBlueprint.BlueprintId, this.SelectedBlueprint);
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

            // Convert file paths.
            newProjectSettings.ProjectAssemblies =
                newProjectSettings.ProjectAssembliesSerialized.Select(
                    assemblyPath =>
                    ReflectionUtils.FindAssembly(string.Format("{0}\\{1}", Path.GetDirectoryName(path), assemblyPath)))
                                  .ToList();

            newProjectSettings.LanguageFiles =
                newProjectSettings.LanguageFilesSerialized.Select(
                    languageFilePath =>
                    new LanguageFile
                        {
                            Path =
                                new FileInfo(string.Format("{0}\\{1}", Path.GetDirectoryName(path), languageFilePath))
                        .FullName
                        }).ToList();

            // Load blueprint files.
            foreach (var blueprintFile in newProjectSettings.BlueprintFiles)
            {
                var blueprintFileName = this.ReadBlueprintsAsBinary
                                            ? Path.ChangeExtension(blueprintFile.Path, "bytes")
                                            : blueprintFile.Path;

                var absoluteBlueprintFilePath = string.Format(
                    "{0}\\{1}", Path.GetDirectoryName(path), blueprintFileName);

                var fileInfo = new FileInfo(absoluteBlueprintFilePath);

                if (!fileInfo.Exists)
                {
                    throw new FileNotFoundException(string.Format("Blueprint file not found: {0}.", path));
                }

                using (var blueprintFileStream = fileInfo.OpenRead())
                {
                    try
                    {
                        BlueprintManager newBlueprintManager;

                        if (this.ReadBlueprintsAsBinary)
                        {
                            var binaryDeserializer = new BinaryDeserializer(blueprintFileStream);
                            newBlueprintManager = binaryDeserializer.Deserialize<BlueprintManager>();
                        }
                        else
                        {
                            newBlueprintManager =
                                (BlueprintManager)this.blueprintManagerSerializer.Deserialize(blueprintFileStream);
                        }

                        if (newBlueprintManager == null)
                        {
                            throw new SerializationException(
                                string.Format("Couldn't deserialize blueprint manager from '{0}'.", path));
                        }

                        blueprintFile.BlueprintManager = newBlueprintManager;
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
                }
            }

            // Set new project.
            this.SetProject(newProjectSettings, path);
        }

        /// <summary>
        ///   Loads the blueprints of all files of the current project..
        /// </summary>
        public void LoadBlueprints()
        {
            var blueprintManager = new BlueprintManager();

            foreach (var blueprintFile in this.ProjectSettings.BlueprintFiles)
            {
                blueprintManager.AddBlueprints(blueprintFile.BlueprintManager);
            }

            // Load all blueprints.
            this.BlueprintManagerViewModel = new BlueprintManagerViewModel(blueprintManager)
                {
                    AssemblyComponents = this.AvailableComponentTypes
                };

            // Setup blueprint parent hierarchy.
            if (this.BlueprintManagerViewModel != null)
            {
                try
                {
                    this.BlueprintManagerViewModel.SetupBlueprintHierarchy();
                }
                catch (AggregateException exception)
                {
                    EditorDialog.Warning("Blueprint hierarchy not properly set up", exception.InnerExceptions);
                }
            }
        }

        public void New()
        {
            ProjectSettings newProjectSettings = new ProjectSettings();
            newProjectSettings.BlueprintFiles.Add(new BlueprintFile { BlueprintManager = new BlueprintManager() });

            // Set new project.
            this.SetProject(newProjectSettings);
            this.SetAvailableLanguages(new List<string>());
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

            this.BlueprintManagerViewModel.UpdateLocalizationKeys();

            // Save blueprint files.
            for (int index = 0; index < this.ProjectSettings.BlueprintFiles.Count; index++)
            {
                var blueprintFile = this.ProjectSettings.BlueprintFiles[index];

                // Set generic blueprint file path if not set.
                if (blueprintFile.Path == null)
                {
                    var absolutePath = GenerateBlueprintFilePath(this.SerializationPath, index);
                    blueprintFile.Path = GetRelativePath(absolutePath, this.ProjectPath);
                }

                // Write text.
                var absoluteBlueprintFilePath = string.Format("{0}\\{1}", this.ProjectPath, blueprintFile.Path);
                var blueprintFileStream = new FileStream(absoluteBlueprintFilePath, FileMode.Create);
                this.blueprintManagerSerializer.Serialize(blueprintFileStream, blueprintFile.BlueprintManager);
                blueprintFileStream.Close();

                if (this.WriteBlueprintsAsBinary)
                {
                    // Write binary.
                    var blueprintFilePath = string.Format(
                        "{0}\\{1}", this.ProjectPath, Path.ChangeExtension(blueprintFile.Path, "bytes"));
                    blueprintFileStream = new FileStream(blueprintFilePath, FileMode.Create);
                    var binarySerializer = new BinarySerializer(blueprintFileStream);
                    binarySerializer.Serialize(blueprintFile.BlueprintManager);
                    blueprintFileStream.Close();
                }
            }

            // Save project.
            var fileStream = new FileStream(this.SerializationPath, FileMode.Create);

            // Convert file paths.
            this.ProjectSettings.ProjectAssembliesSerialized =
                this.ProjectSettings.ProjectAssemblies.Select(
                    projectAssembly => GetRelativePath(projectAssembly.CodeBase, this.SerializationPath)).ToArray();

            this.ProjectSettings.LanguageFilesSerialized =
                this.ProjectSettings.LanguageFiles.Select(
                    languageFile => GetRelativePath(languageFile.Path, this.SerializationPath)).ToArray();

            this.projectSettingsSerializer.Serialize(fileStream, this.ProjectSettings);
            this.localizationContext.SaveLanguages();
            fileStream.Close();
        }

        public void SetAvailableLanguages(IEnumerable<string> languageTags)
        {
            this.AvailableLanguages.Clear();
            this.AvailableLanguages.Add(EditorSettings.LanguageTagRawLocalizationKeys);

            foreach (var language in languageTags)
            {
                this.AvailableLanguages.Add(language);
            }

            this.OnAvailableLanguagesChanged();
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

        private void LoadEditorSettings()
        {
            var fileInfo = new FileInfo(EditorSettingsSerializationPath);

            if (fileInfo.Exists)
            {
                using (var fileStream = fileInfo.OpenRead())
                {
                    this.editorSettings = (EditorSettings)this.editorSettingsSerializer.Deserialize(fileStream);
                }
            }
            else
            {
                this.editorSettings = new EditorSettings();
                this.SaveEditorSettings();
            }
        }

        private void OnAvailableLanguagesChanged()
        {
            var handler = this.AvailableLanguagesChanged;
            if (handler != null)
            {
                handler();
            }
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

        private void OnSelectedProjectFileChanged()
        {
        }

        private void OnUndoStackChanged(object sender, EventArgs eventArgs)
        {
            this.OnPropertyChanged("UndoDescription");
        }

        private void SaveEditorSettings()
        {
            var fileInfo = new FileInfo(EditorSettingsSerializationPath);

            using (var fileStream = fileInfo.Create())
            {
                this.editorSettingsSerializer.Serialize(fileStream, this.editorSettings);
            }
        }

        private void SetProject(ProjectSettings projectSettings, string serializationPath = null)
        {
            this.ProjectSettings = projectSettings;
            this.SerializationPath = serializationPath;

            if (this.ProjectSettings != null)
            {
                this.ProjectSettings.EntityComponentTypesChanged += this.OnEntityComponentTypesChanged;

                // Setup project explorer
                this.ProjectExplorerViewModel = new ProjectExplorerViewModel();

                foreach (var blueprintFile in this.ProjectSettings.BlueprintFiles)
                {
                    // Add project file.
                    var projectFileViewModel = new ProjectFileViewModel(blueprintFile);
                    this.ProjectExplorerViewModel.ProjectFiles.Add(projectFileViewModel);
                }
            }
            else
            {
                this.BlueprintManagerViewModel = null;
            }

            // Load localization data.
            this.localizationContext.LoadLanguages();

            // Raise events.
            this.OnPropertyChanged("ProjectSettings");
            this.OnEntityComponentTypesChanged();
        }

        #endregion
    }
}