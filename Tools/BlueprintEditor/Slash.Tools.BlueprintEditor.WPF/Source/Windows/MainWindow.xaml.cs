// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MainWindow.xaml.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace BlueprintEditor
{
    using System;
    using System.Windows;

    using BlueprintEditor.Windows.Controls;

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
        private BlueprintTreeViewController treeViewController;

        #endregion

        #region Constructors and Destructors

        public MainWindow()
        {
            this.InitializeComponent();

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
                System.Media.SystemSounds.Hand.Play();
                this.TbMessage.Text = ex.Message;
            }
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

            // Open file dialog.

            // If file was chosen, try to load.
        }

        private void MenuFileNewClick(object sender, RoutedEventArgs e)
        {
            // Check if context changed and should be saved before continuing.
            if (!this.CheckContextChange())
            {
                return;
            }

            // Create new blueprint manager.
            this.context.BlueprintManager = new BlueprintManager();
        }

        private void MenuFileSave_OnClick(object sender, RoutedEventArgs e)
        {
        }

        #endregion
    }
}