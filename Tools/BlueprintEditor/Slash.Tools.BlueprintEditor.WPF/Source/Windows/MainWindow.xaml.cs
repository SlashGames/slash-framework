// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MainWindow.xaml.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace BlueprintEditor.Windows
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.Serialization;
    using System.Text;
    using System.Windows;
    using System.Windows.Input;

    using BlueprintEditor.Controls;
    using BlueprintEditor.ViewModels;

    using CsvHelper;

    using Microsoft.Win32;

    using AggregateException = Slash.SystemExt.Exceptions.AggregateException;

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

            AppDomain.CurrentDomain.AssemblyResolve += this.DynamicAssemblyResolve;
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

        /// <summary>
        ///   Shows the messages of the passed exceptions as aggregated warning with the specified title.
        /// </summary>
        /// <param name="title">Title of the warning to show.</param>
        /// <param name="exceptions">Exceptions to include in the warning.</param>
        private static void ShowExceptionsAsWarning(string title, IEnumerable<Exception> exceptions)
        {
            var stringBuilder = new StringBuilder();

            foreach (var exception in exceptions)
            {
                stringBuilder.AppendLine(exception.Message);
            }

            EditorDialog.Warning(title, stringBuilder.ToString());
        }

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
            }
            catch (FileNotFoundException exception)
            {
                EditorDialog.Error("Unable to load project", exception.Message);
            }
        }

        private void BackgroundLoadContextCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            // Setup correct blueprint parent hierarchy.
            if (this.Context.BlueprintManagerViewModel != null)
            {
                try
                {
                    this.Context.BlueprintManagerViewModel.SetupBlueprintHierarchy();
                }
                catch (AggregateException exception)
                {
                    ShowExceptionsAsWarning("Blueprint hierarchy not properly set up", exception.InnerExceptions);
                }
            }

            // Hide progress bar.
            this.progressWindow.Close();

            this.UpdateWindowTitle();
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

        private void CanExecuteFileImportData(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = this.Context.BlueprintManagerViewModel != null;
        }

        private void CanExecuteFileOpen(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
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

        private void ExecutedFileImportData(object sender, ExecutedRoutedEventArgs e)
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
            using (var stream = openFileDialog.OpenFile())
            {
                var streamReader = new StreamReader(stream);
                var csvReader = new CsvReader(streamReader);

                // Read column headers and first row.
                csvReader.Read();

                // Allow user to specify which attribute table keys are mapped to which CSV columns.
                var importDataCsvWindow = new ImportDataCSVWindow(this.Context, csvReader.FieldHeaders) { Owner = this };
                result = importDataCsvWindow.ShowDialog();

                if (result != true)
                {
                    EditorDialog.Info("CSV Import Cancelled", "No data imported.");
                    return;
                }

                // Create a blueprint for each CSV row.
                var blueprintManagerViewModel = this.Context.BlueprintManagerViewModel;
                var processedBlueprints = new HashSet<string>();
                var errors = new List<Exception>();

                var newBlueprints = 0;
                var updatedBlueprints = 0;
                var skippedBlueprints = 0;

                while (csvReader.CurrentRecord != null)
                {
                    try
                    {
                        // Get id of the blueprint to create or update.
                        var blueprintId = csvReader[importDataCsvWindow.BlueprintIdColumn];

                        // Check for duplicate blueprints in the CSV file.
                        if (processedBlueprints.Contains(blueprintId))
                        {
                            throw new InvalidOperationException(
                                string.Format("Duplicate blueprint id: {0}", blueprintId));
                        }

                        processedBlueprints.Add(blueprintId);

                        // Check whether blueprint already exists.
                        var dataBlueprint =
                            blueprintManagerViewModel.Blueprints.FirstOrDefault(
                                blueprint => blueprint.BlueprintId == blueprintId);
                        var newBlueprint = dataBlueprint == null;

                        if (newBlueprint)
                        {
                            // Create new blueprint.
                            blueprintManagerViewModel.NewBlueprintId = blueprintId;
                            dataBlueprint = blueprintManagerViewModel.CreateNewBlueprint();

                            // Reparent new blueprint.
                            blueprintManagerViewModel.ReparentBlueprint(
                                dataBlueprint.BlueprintId, importDataCsvWindow.BlueprintParent.BlueprintId);
                        }
                        else
                        {
                            // Check parent of existing blueprint.
                            if (dataBlueprint.Parent != importDataCsvWindow.BlueprintParent)
                            {
                                throw new InvalidOperationException(
                                    string.Format(
                                        "Blueprint {0} is child of {1} but should be child of {2}.",
                                        dataBlueprint.BlueprintId,
                                        dataBlueprint.Parent.BlueprintId,
                                        importDataCsvWindow.BlueprintParent.BlueprintId));
                            }
                        }

                        // Map attribute table keys to CSV values.
                        foreach (var valueMapping in
                            importDataCsvWindow.ValueMappings.Where(
                                mapping => !string.IsNullOrWhiteSpace(mapping.MappingTarget)))
                        {
                            object convertedValue;
                            valueMapping.InspectorProperty.TryConvertStringToListOrValue(
                                csvReader[valueMapping.MappingTarget], out convertedValue);
                            dataBlueprint.Blueprint.AttributeTable[valueMapping.MappingSource] = convertedValue;
                        }

                        // Increase counter.
                        if (newBlueprint)
                        {
                            newBlueprints++;
                        }
                        else
                        {
                            updatedBlueprints++;
                        }
                    }
                    catch (Exception exception)
                    {
                        errors.Add(exception);
                        skippedBlueprints++;
                    }

                    // Read next record.
                    csvReader.Read();
                }

                // Show import results.
                if (errors.Count > 0)
                {
                    ShowExceptionsAsWarning("Some data could not be imported", errors);
                }

                var importInfoBuilder = new StringBuilder();
                importInfoBuilder.AppendLine(string.Format("{0} blueprint(s) imported.", newBlueprints));
                importInfoBuilder.AppendLine(string.Format("{0} blueprint(s) updated.", updatedBlueprints));
                importInfoBuilder.AppendLine(string.Format("{0} blueprint(s) skipped.", skippedBlueprints));
                var importInfo = importInfoBuilder.ToString();

                EditorDialog.Info("CSV Import Complete", importInfo);
            }
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

            // Show progress bar.
            this.progressWindow = new ProgressWindow();
            this.progressWindow.Show("Loading project...");

            // Load data.
            BackgroundWorker worker = new BackgroundWorker();
            worker.DoWork += this.BackgroundLoadContext;
            worker.RunWorkerCompleted += this.BackgroundLoadContextCompleted;
            worker.RunWorkerAsync(new BackgroundLoadContextData { Context = this.Context, Filename = filename });
        }

        private void ExecutedFileSave(object sender, RoutedEventArgs e)
        {
            this.SaveContext(this.Context.SerializationPath);
        }

        private void ExecutedSaveAs(object sender, RoutedEventArgs e)
        {
            this.SaveContext(null);
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

        /// <summary>
        ///   Updates the title of the main window, showing the current project name if available.
        /// </summary>
        private void UpdateWindowTitle()
        {
            if (this.Context != null && this.Context.ProjectSettings != null
                && !string.IsNullOrEmpty(this.Context.ProjectSettings.Name))
            {
                this.Title = string.Format("{0} - {1}", MainWindowTitle, this.Context.ProjectSettings.Name);
            }
            else
            {
                this.Title = MainWindowTitle;
            }
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