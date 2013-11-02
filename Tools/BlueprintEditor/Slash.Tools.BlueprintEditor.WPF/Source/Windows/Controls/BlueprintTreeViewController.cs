// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BlueprintTreeViewController.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace BlueprintEditor.Windows.Controls
{
    using System.Windows.Controls;

    using Slash.GameBase.Blueprints;

    public class BlueprintTreeViewController
    {
        #region Fields

        private readonly BlueprintManager blueprintManager;

        private readonly TreeView treeView;

        #endregion

        #region Constructors and Destructors

        public BlueprintTreeViewController(TreeView treeView, BlueprintManager blueprintManager)
        {
            this.treeView = treeView;
            this.blueprintManager = blueprintManager;
            this.blueprintManager.BlueprintsChanged += this.OnBlueprintsChanged;

            this.UpdateTreeView();
        }

        #endregion

        #region Methods

        private void OnBlueprintsChanged(BlueprintManager changedBlueprintManager)
        {
            this.UpdateTreeView();
        }

        private void UpdateTreeView()
        {
            if (this.treeView == null)
            {
                return;
            }

            this.treeView.Items.Clear();
            if (this.blueprintManager == null)
            {
                return;
            }

            foreach (var blueprintPair in this.blueprintManager.Blueprints)
            {
                this.treeView.Items.Add(new BlueprintTreeViewItem(blueprintPair.Key, blueprintPair.Value));
            }
        }

        #endregion
    }
}