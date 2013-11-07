// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MainWindow.xaml.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace BlueprintEditor
{
    using System;
    using System.Collections.Generic;
    using System.Media;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media;

    using BlueprintEditor.Windows;
    using BlueprintEditor.Windows.Controls;

    using Microsoft.Win32;

    using Slash.GameBase.Blueprints;
    using Slash.Tools.BlueprintEditor.Logic.Context;
    
    /// <summary>
    ///   Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        #region Fields

        public static readonly DependencyProperty ContextProperty = DependencyProperty.Register(
            "Context",
            typeof(EditorContext),
            typeof(BlueprintControl),
            new FrameworkPropertyMetadata(new EditorContext()));

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

        #region Constructors and Destructors

        public MainWindow()
        {
            this.InitializeComponent();

            //this.DataContext = this.context;

            this.Context.BlueprintManagerChanged += this.OnBlueprintManagerChanged;

            this.TreeBlueprints.BlueprintManager = this.Context.BlueprintManager;
            //this.BlueprintControl.AvailableComponentTypes = this.context.EntityComponentTypes;
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

        private void MenuFileLoad_OnClick(object sender, RoutedEventArgs e)
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
            this.Context.Load(filename);
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

        private void MenuFileSaveAs_OnClick(object sender, RoutedEventArgs e)
        {
            this.SaveContext(null);
        }

        private void MenuFileSave_OnClick(object sender, RoutedEventArgs e)
        {
            this.SaveContext(this.Context.SerializationPath);
        }

        private void OnBlueprintManagerChanged(
            BlueprintManager newBlueprintManager, BlueprintManager oldBlueprintManager)
        {
            this.TreeBlueprints.BlueprintManager = newBlueprintManager;
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

        #endregion

        private void TreeBlueprints_OnBlueprintSelectionChanged(object sender, RoutedEventArgs e)
        {
            BlueprintSelectionChangedEventArgs eventArgs = ((BlueprintSelectionChangedEventArgs)e);
            this.BlueprintControl.BlueprintId = eventArgs.BlueprintId;
            this.BlueprintControl.Blueprint = eventArgs.Blueprint;
        }
    }
}