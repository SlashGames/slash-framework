// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MainWindow.xaml.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace BlueprintEditor.Windows
{
    using System.Runtime.Serialization;
    using System.Windows;
    using System.Windows.Input;

    using BlueprintEditor.Controls;
    using BlueprintEditor.ViewModels;

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

        #region Constructors and Destructors

        public MainWindow()
        {
            this.InitializeComponent();

            this.Context.EntityComponentTypesChanged += this.OnEntityComponentTypesChanged;

            this.OnEntityComponentTypesChanged();

            this.DataContext = this.Context;
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
            e.CanExecute = true;
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

            try
            {
                this.Context.Load(filename);
                this.UpdateWindowTitle();
            }
            catch (SerializationException exception)
            {
                EditorDialog.Error("Unable to load project", exception.Message);
            }
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

        private void OnEntityComponentTypesChanged()
        {
            // Update component table.
            InspectorComponentTable.LoadComponents();

            this.BlueprintControl.AvailableComponentTypes = this.Context.AvailableComponentTypes;
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

            // Save context.
            this.Context.SerializationPath = path;
            this.Context.Save();
        }

        private void TreeBlueprints_OnBlueprintSelectionChanged(object sender, RoutedEventArgs e)
        {
            BlueprintSelectionChangedEventArgs eventArgs = ((BlueprintSelectionChangedEventArgs)e);
            this.BlueprintControl.DataContext = eventArgs.Blueprint;
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
    }
}