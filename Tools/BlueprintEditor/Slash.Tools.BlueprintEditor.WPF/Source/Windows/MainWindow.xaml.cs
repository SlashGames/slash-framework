// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MainWindow.xaml.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace BlueprintEditor.Windows
{
    using System;
    using System.Runtime.Serialization;
    using System.Windows;

    using BlueprintEditor.Controls;

    using Microsoft.Win32;

    using Slash.GameBase.Blueprints;
    using Slash.Tools.BlueprintEditor.Logic.Context;

    /// <summary>
    ///   Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
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

            this.DataContext = Context;
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

        #region Methods

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

        private void MenuFileExit_OnClick(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
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
        }

        private void MenuFileOpen_OnClick(object sender, RoutedEventArgs e)
        {
            // Check if context changed and should be saved before continuing.
            if (!this.CheckContextChange())
            {
                return;
            }

            // Configure open file dialog box
            OpenFileDialog dlg = new OpenFileDialog
                {
                    FileName = "Blueprints",
                    DefaultExt = ".xml",
                    Filter = "Xml documents (.xml)|*.xml"
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
            }
            catch (SerializationException exception)
            {
                MessageBox.Show(exception.Message, "Loading failed");
            }
        }

        private void MenuFileSaveAs_OnClick(object sender, RoutedEventArgs e)
        {
            this.SaveContext(null);
        }

        private void MenuFileSave_OnClick(object sender, RoutedEventArgs e)
        {
            this.SaveContext(this.Context.SerializationPath);
        }

        private void MenuProjectSettings_OnClick(object sender, RoutedEventArgs e)
        {
            ProjectSettingsWindow dlg = new ProjectSettingsWindow
                {
                    Owner = this,
                    DataContext = this.Context.ProjectSettings
                };
            dlg.ShowDialog();
        }
        
        private void OnEntityComponentTypesChanged()
        {
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
                        FileName = "Blueprints",
                        DefaultExt = ".xml",
                        Filter = "Xml documents (.xml)|*.xml"
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
            this.BlueprintControl.BlueprintControlContext = new BlueprintControlContext
                {
                    Blueprint = eventArgs.Blueprint,
                    BlueprintId = eventArgs.BlueprintId,
                    BlueprintManager = this.Context.BlueprintManager
                };
        }

        #endregion
    }
}