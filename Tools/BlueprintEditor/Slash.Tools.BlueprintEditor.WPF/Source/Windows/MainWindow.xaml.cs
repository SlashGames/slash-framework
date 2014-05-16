// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MainWindow.xaml.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace BlueprintEditor.Windows
{
    using System;
    using System.ComponentModel;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.Serialization;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;

    using BlueprintEditor.Controls;
    using BlueprintEditor.ViewModels;

    using CsvHelper;

    using Microsoft.Win32;

    using Slash.Tools.BlueprintEditor.Logic.Data;

    /// <summary>
    ///   Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        #region Constants

        /// <summary>
        ///   Title of the main window to be shown in addition to the project name.
        /// </summary>
        private const string MainWindowTitle = "Blueprint Editor";

        #endregion

        #region Static Fields

        public static readonly DependencyProperty ContextProperty = DependencyProperty.Register(
            "Context",
            typeof(EditorContext),
            typeof(BlueprintControl),
            new FrameworkPropertyMetadata(new EditorContext()));

        #endregion

        #region Fields

        /// <summary>
        ///   Window showing a progress bar and status label.
        /// </summary>
        private ProgressWindow progressWindow;

        #endregion

        #region Constructors and Destructors

        public MainWindow()
        {
            this.InitializeComponent();

            this.DataContext = this.Context;

            this.ProjectExplorer.PropertyChanged += this.OnProjectExplorerPropertyChanged;

            this.BlueprintControl.EditorContext = this.Context;
            this.BlueprintControl.SelectedBlueprintChaged += this.OnSelectedBlueprintChanged;

            AppDomain.CurrentDomain.AssemblyResolve += this.DynamicAssemblyResolve;

            this.UpdateRecentProjects();
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///   Editor context which contains editing data.
        /// </summary>
        public EditorContext Context
        {
            get
            {
                return (EditorContext)this.GetValue(ContextProperty);
            }

            set
            {
                this.SetValue(ContextProperty, value);
            }
        }

        #endregion

        #region Properties

        /// <summary>
        ///   Gets whether there's an active project set at the context.
        /// </summary>
        private bool ProjectActive
        {
            get
            {
                return this.Context != null && this.Context.ProjectSettings != null;
            }
        }

        #endregion

        #region Methods

        private void BackgroundLoadContext(object sender, DoWorkEventArgs e)
        {
            try
            {
                var data = (BackgroundLoadContextData)e.Argument;
                data.Context.Load(data.Filename);
            }
            catch (SerializationException exception)
            {
                EditorDialog.Error("Unable to load project", exception.Message);
                e.Cancel = true;
            }
            catch (FileNotFoundException exception)
            {
                EditorDialog.Error("Unable to load project", exception.Message);
                e.Cancel = true;
            }
        }

        private void BackgroundLoadContextCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (!e.Cancelled)
            {
                // Update custom imports.
                this.MenuDataCustomImport.Items.Clear();

                foreach (var customImport in this.Context.ProjectSettings.CustomImports)
                {
                    var menuItem = new MenuItem();

                    menuItem.Header = string.Format("Import _{0}...", customImport.BlueprintParentId);
                    menuItem.Tag = customImport;
                    menuItem.Click += this.ExecutedCustomImport;

                    this.MenuDataCustomImport.Items.Add(menuItem);
                }

                // Update available languages.
                var languageTags =
                    this.Context.ProjectSettings.LanguageFiles.Select(
                        languageFile => Path.GetFileNameWithoutExtension(languageFile.Path));

                this.Context.SetAvailableLanguages(languageTags);

                // Load blueprints.
                this.Context.LoadBlueprints();

                this.UpdateWindowTitle();
            }

            // Hide progress bar.
            this.progressWindow.Close();

            // Update recent projects.
            this.UpdateRecentProjects();
        }

        private void BackgroundSaveContext(object sender, DoWorkEventArgs e)
        {
            var context = (EditorContext)e.Argument;
            context.Save();
        }

        private void BackgroundSaveContextCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            // Hide progress bar.
            this.progressWindow.Close();
        }

        private void CanExecuteDataCustomImport(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = this.ProjectActive && this.Context.ProjectSettings.CustomImports.Count > 0;
        }

        private void CanExecuteDataExportLocalization(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = this.ProjectActive;
        }

        private void CanExecuteDataImportData(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = this.ProjectActive;
        }

        private void CanExecuteDataImportLocalization(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = this.ProjectActive;
        }

        private void CanExecuteEditCopyBlueprint(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = this.Context.CanExecuteCopyBlueprint();
        }

        private void CanExecuteEditRedo(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = this.Context.CanExecuteRedo();
        }

        private void CanExecuteEditUndo(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = this.Context.CanExecuteUndo();
        }

        private void CanExecuteFileClose(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = this.ProjectActive;
        }

        private void CanExecuteFileExit(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void CanExecuteFileOpen(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void CanExecuteFileRecentProjects(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = this.Context.RecentProjects != null && this.Context.RecentProjects.Count > 0;
        }

        private void CanExecuteFileSave(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = this.ProjectActive;
        }

        private void CanExecuteFileSaveAs(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = this.ProjectActive;
        }

        private void CanExecuteHelpAbout(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        /// <summary>
        ///   Checks if the context changed and what to do about it (save or discard changes).
        ///   The user can also choose to cancel, so the method returns if to continue execution.
        /// </summary>
        /// <returns>True if the execution should be continued; otherwise, false.</returns>
        private bool CheckContextChange()
        {
            // TODO: Check if changed.
            return true;
        }

        private Assembly DynamicAssemblyResolve(object sender, ResolveEventArgs args)
        {
            // Search in context.
            if (this.Context == null)
            {
                return null;
            }

            return
                this.Context.ProjectSettings.ProjectAssemblies.FirstOrDefault(
                    assembly => assembly.FullName == args.Name);
        }

        private void ExecutedCustomImport(object sender, RoutedEventArgs e)
        {
            var menuItem = (MenuItem)sender;
            var customImport = (CsvImportData)menuItem.Tag;
            this.ImportCSVData(customImport);
        }

        private void ExecutedDataExportLocalization(object sender, ExecutedRoutedEventArgs e)
        {
            // Configure save file dialog box.
            SaveFileDialog dlg = new SaveFileDialog
                {
                    FileName = this.Context.ProjectSettings.Name,
                    DefaultExt = LocalizationContext.LocalizationExportExtension,
                    Filter = string.Format("Localization Files|*.{0}", LocalizationContext.LocalizationExportExtension)
                };

            // Show save file dialog box.
            var result = dlg.ShowDialog();

            // Process save file dialog box results.
            if (result == false)
            {
                return;
            }

            // Save document.
            using (var stream = dlg.OpenFile())
            {
                this.Context.LocalizationContext.ExportLocalizationData(stream);
            }
        }

        private void ExecutedDataImportData(object sender, ExecutedRoutedEventArgs e)
        {
            this.ImportCSVData(null);
        }

        private void ExecutedDataImportLocalization(object sender, ExecutedRoutedEventArgs e)
        {
            // Configure open file dialog box.
            OpenFileDialog dlg = new OpenFileDialog
                {
                    FileName = this.Context.ProjectSettings.Name,
                    DefaultExt = LocalizationContext.LocalizationExportExtension,
                    Filter = string.Format("Localization Files|*.{0}", LocalizationContext.LocalizationExportExtension)
                };

            // Show open file dialog box.
            var result = dlg.ShowDialog();

            // Process open file dialog box results.
            if (result == false)
            {
                return;
            }

            // Open document.
            using (var stream = dlg.OpenFile())
            {
                this.Context.LocalizationContext.ImportLocalizationData(stream);
            }
        }

        private void ExecutedEditCopyBlueprint(object sender, ExecutedRoutedEventArgs e)
        {
            this.Context.CopyBlueprint();
        }

        private void ExecutedEditRedo(object sender, ExecutedRoutedEventArgs e)
        {
            this.Context.Redo();
        }

        private void ExecutedEditUndo(object sender, ExecutedRoutedEventArgs e)
        {
            this.Context.Undo();
        }

        private void ExecutedFileAbout(object sender, RoutedEventArgs e)
        {
            AboutWindow dlg = new AboutWindow { Owner = this };
            dlg.ShowDialog();
        }

        private void ExecutedFileClose(object sender, ExecutedRoutedEventArgs e)
        {
            this.Context.Close();
        }

        private void ExecutedFileExit(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void ExecutedFileOpen(object sender, RoutedEventArgs e)
        {
            // Check if context changed and should be saved before continuing.
            if (!this.CheckContextChange())
            {
                return;
            }

            // Configure open file dialog box
            OpenFileDialog dlg = new OpenFileDialog
                {
                    DefaultExt = EditorContext.ProjectExtension,
                    Filter = string.Format("Blueprint Editor Projects|*.{0}", EditorContext.ProjectExtension)
                };

            // Show open file dialog box
            bool? result = dlg.ShowDialog();

            // Process open file dialog box results 
            if (result != true)
            {
                return;
            }

            // If file was chosen, try to load.
            string filename = dlg.FileName;

            this.LoadProject(filename);
        }

        private void ExecutedFileSave(object sender, RoutedEventArgs e)
        {
            this.SaveContext(this.Context.SerializationPath);
        }

        private void ExecutedRecentProject(object sender, RoutedEventArgs e)
        {
            var menuItem = (MenuItem)sender;
            this.LoadProject((string)menuItem.Header);
        }

        private void ExecutedSaveAs(object sender, RoutedEventArgs e)
        {
            this.SaveContext(null);
        }

        private void ImportCSVData(CsvImportData importData)
        {
            // Show open file dialog box.
            OpenFileDialog openFileDialog = new OpenFileDialog
                {
                    AddExtension = true,
                    CheckFileExists = true,
                    CheckPathExists = true,
                    DefaultExt = ".csv",
                    Filter = "Comma-separated values (.csv)|*.csv",
                    ValidateNames = true
                };

            var result = openFileDialog.ShowDialog();

            if (result != true)
            {
                return;
            }

            // Open CSV file.
            try
            {
                using (var stream = openFileDialog.OpenFile())
                {
                    var streamReader = new StreamReader(stream);
                    var csvReader = new CsvReader(streamReader);
                    csvReader.Configuration.Delimiter = importData != null
                                                            ? importData.Delimiter
                                                            : CsvImportData.DefaultDelimiter;
                    var importCsvDataWindow = new ImportCsvDataWindow(this.Context, csvReader, importData)
                        {
                            Owner = this
                        };
                    importCsvDataWindow.ShowDialog();
                }
            }
            catch (IOException e)
            {
                EditorDialog.Error("Unable to open CSV file", e.Message);
            }
        }

        private void LoadProject(string filename)
        {
            // Show progress bar.
            this.progressWindow = new ProgressWindow();
            this.progressWindow.Show("Loading project...");

            // Load data.
            BackgroundWorker worker = new BackgroundWorker();
            worker.DoWork += this.BackgroundLoadContext;
            worker.RunWorkerCompleted += this.BackgroundLoadContextCompleted;
            worker.RunWorkerAsync(new BackgroundLoadContextData { Context = this.Context, Filename = filename });
        }

        private void MainWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            this.UpdateWindowTitle();
        }

        private void MenuFileNew_OnClick(object sender, RoutedEventArgs e)
        {
            // Check if context changed and should be saved before continuing.
            if (!this.CheckContextChange())
            {
                return;
            }

            // Create new blueprint manager.
            this.Context.New();
            this.UpdateWindowTitle();
        }

        private void MenuProjectSettings_OnClick(object sender, RoutedEventArgs e)
        {
            // Show project settings window.
            ProjectSettingsWindow dlg = new ProjectSettingsWindow
                {
                    Owner = this,
                    DataContext = this.Context.ProjectSettings
                };
            dlg.ShowDialog();

            // Update window title as soon as settings window is closed by the user.
            this.UpdateWindowTitle();
        }

        private void OnProjectExplorerPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals("SelectedItem"))
            {
                this.Context.SelectedProjectFile = this.ProjectExplorer.SelectedItem;
                this.UpdateWindowTitle();
            }
        }

        private void OnSelectedBlueprintChanged(BlueprintViewModel newBlueprint, BlueprintViewModel oldBlueprint)
        {
            this.Context.SelectedBlueprint = newBlueprint;
        }

        private void SaveContext(string path)
        {
            // Check if already a path to save was set, otherwise request.
            if (string.IsNullOrEmpty(path))
            {
                // Configure save file dialog box
                SaveFileDialog dlg = new SaveFileDialog
                    {
                        FileName = this.Context.ProjectSettings.Name,
                        DefaultExt = EditorContext.ProjectExtension,
                        Filter = string.Format("Blueprint Editor Projects|*.{0}", EditorContext.ProjectExtension)
                    };

                // Show save file dialog box
                bool? result = dlg.ShowDialog();

                // Process save file dialog box results 
                if (result == false)
                {
                    return;
                }

                // Save document 
                path = dlg.FileName;
            }

            // Show progress bar.
            this.progressWindow = new ProgressWindow();
            this.progressWindow.Show("Saving project...");

            // Save context.
            this.Context.SerializationPath = path;

            BackgroundWorker worker = new BackgroundWorker();
            worker.DoWork += this.BackgroundSaveContext;
            worker.RunWorkerCompleted += this.BackgroundSaveContextCompleted;
            worker.RunWorkerAsync(this.Context);
        }

        private void UpdateRecentProjects()
        {
            this.MenuFileRecentProjects.Items.Clear();

            foreach (var recentProject in this.Context.RecentProjects)
            {
                var menuItem = new MenuItem();

                menuItem.Header = recentProject;
                menuItem.Click += this.ExecutedRecentProject;

                this.MenuFileRecentProjects.Items.Add(menuItem);
            }
        }

        /// <summary>
        ///   Updates the title of the main window, showing the current project name if available.
        /// </summary>
        private void UpdateWindowTitle()
        {
            var newTitle = MainWindowTitle;

            if (this.Context != null && this.Context.ProjectSettings != null
                && !string.IsNullOrEmpty(this.Context.ProjectSettings.Name))
            {
                newTitle += string.Format(" - {0}", this.Context.ProjectSettings.Name);
            }

            if (this.Context != null && this.Context.SelectedProjectFile != null
                && !string.IsNullOrEmpty(this.Context.SelectedProjectFile.ProjectFileName))
            {
                newTitle += string.Format(" - {0}", this.Context.SelectedProjectFile.ProjectFileName);
            }

            this.Title = newTitle;
        }

        #endregion

        private class BackgroundLoadContextData
        {
            #region Public Properties

            public EditorContext Context { get; set; }

            public string Filename { get; set; }

            #endregion
        }
    }
}