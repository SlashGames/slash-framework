// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BlueprintTreeViewController.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace BlueprintEditor.Windows.Controls
{
    using System.Windows.Controls;

    using Slash.GameBase.Blueprints;

    /// <summary>
    ///   Capsules a tree view to show the blueprints of a specified blueprint manager.
    /// </summary>
    public class BlueprintTreeViewController
    {
        #region Fields

        /// <summary>
        ///   Tree view to capsule.
        /// </summary>
        private readonly TreeView treeView;

        /// <summary>
        ///   Blueprint manager to visualize with tree view.
        /// </summary>
        private BlueprintManager blueprintManager;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///   Constructor.
        /// </summary>
        /// <param name="treeView">Tree view to capsule.</param>
        /// <param name="blueprintManager">Blueprint manager to visualize with tree view.</param>
        public BlueprintTreeViewController(TreeView treeView, BlueprintManager blueprintManager)
        {
            this.treeView = treeView;
            this.BlueprintManager = blueprintManager;
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///   Blueprint manager to visualize with tree view.
        /// </summary>
        public BlueprintManager BlueprintManager
        {
            get
            {
                return this.blueprintManager;
            }
            set
            {
                if (ReferenceEquals(value, this.blueprintManager))
                {
                    return;
                }

                if (this.blueprintManager != null)
                {
                    this.blueprintManager.BlueprintsChanged -= this.OnBlueprintsChanged;
                }

                this.blueprintManager = value;

                if (this.blueprintManager != null)
                {
                    this.blueprintManager.BlueprintsChanged += this.OnBlueprintsChanged;
                }

                this.UpdateTreeView();
            }
        }

        /// <summary>
        ///   Returns the selected item.
        /// </summary>
        public BlueprintTreeViewItem SelectedItem
        {
            get
            {
                if (this.treeView == null || this.treeView.SelectedItem == null)
                {
                    return null;
                }

                return (BlueprintTreeViewItem)this.treeView.SelectedItem;
            }
        }

        #endregion

        #region Methods

        private void OnBlueprintsChanged()
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