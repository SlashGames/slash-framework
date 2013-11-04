// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MainWindow.xaml.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace BlueprintEditor
{
    using System;
    using System.Media;
    using System.Windows;

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

        /// <summary>
        ///   Editor context which contains editing data.
        /// </summary>
        private readonly EditorContext context = new EditorContext();

        /// <summary>
        ///   Controller which takes care about the blueprint tree view.
        /// </summary>
        private readonly BlueprintTreeViewController treeViewController;

        #endregion

        #region Constructors and Destructors

        public MainWindow()
        {
            this.InitializeComponent();

            this.context.BlueprintManagerChanged += this.OnBlueprintManagerChanged;

            this.treeViewController = new BlueprintTreeViewController(
                this.TreeBlueprints, this.context.BlueprintManager);
        }

        #endregion

        #region Methods

        private void BtAddBlueprint_OnClick(object sender, RoutedEventArgs e)
        {
            string newBlueprintId = this.TbNewBlueprintId.Text;
            try
            {
                this.context.BlueprintManager.AddBlueprint(newBlueprintId, new Blueprint());
            }
            catch (ArgumentException ex)
            {
                SystemSounds.Hand.Play();
                this.TbMessage.Text = ex.Message;
            }
        }

        private void BtDeleteBlueprint_OnClick(object sender, RoutedEventArgs e)
        {
            // Check if an item is selected.
            BlueprintTreeViewItem selectedItem = this.treeViewController.SelectedItem;
            if (selectedItem == null)
            {
                return;
            }

            // Delete current selected blueprint.
            this.context.BlueprintManager.RemoveBlueprint(selectedItem.BlueprintId);
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
            this.context.Load(filename);
        }

        private void MenuFileNew_OnClick(object sender, RoutedEventArgs e)
        {
            // Check if context changed and should be saved before continuing.
            if (!this.CheckContextChange())
            {
                return;
            }

            // Create new blueprint manager.
            this.context.New();
        }

        private void MenuFileSaveAs_OnClick(object sender, RoutedEventArgs e)
        {
            this.SaveContext(null);
        }

        private void MenuFileSave_OnClick(object sender, RoutedEventArgs e)
        {
            this.SaveContext(this.context.SerializationPath);
        }

        private void OnBlueprintManagerChanged(
            BlueprintManager newBlueprintManager, BlueprintManager oldBlueprintManager)
        {
            this.treeViewController.BlueprintManager = newBlueprintManager;
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
            this.context.SerializationPath = path;
            this.context.Save();
        }

        #endregion
    }
}