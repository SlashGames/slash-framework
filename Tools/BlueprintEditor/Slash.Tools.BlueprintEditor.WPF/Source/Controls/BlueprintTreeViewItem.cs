// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BlueprintTreeViewItem.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace BlueprintEditor.Windows.Controls
{
    using System;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media.Imaging;

    using Slash.GameBase.Blueprints;

    public class BlueprintTreeViewItem : TreeViewItem
    {
        /// <summary>
        ///   Blueprint this item represents.
        /// </summary>
        public Blueprint Blueprint
        {
            get
            {
                return this.blueprint;
            }
            set
            {
                this.blueprint = value;
            }
        }

        /// <summary>
        ///   Id of the blueprint.
        /// </summary>
        public string BlueprintId
        {
            get
            {
                return this.blueprintId;
            }
            set
            {
                this.blueprintId = value;
                this.textBlock.Text = this.blueprintId;
            }
        }
        
        #region Data Member

        Image image;
        TextBlock textBlock;

        private Blueprint blueprint;

        private string blueprintId;

        #endregion

        #region Properties
        
        #endregion

        #region Constructor

        /// <summary>
        ///   Constructor.
        /// </summary>
        /// <param name="blueprintId"></param>
        /// <param name="blueprint"></param>
        public BlueprintTreeViewItem(string blueprintId, Blueprint blueprint)
        {
            this.blueprintId = blueprintId;
            this.blueprint = blueprint;
            CreateTreeViewItemTemplate();
        }

        #endregion

        #region Private Methods

        private void CreateTreeViewItemTemplate()
        {
            StackPanel stack = new StackPanel { Orientation = Orientation.Horizontal };

            this.image = new Image
                {
                    HorizontalAlignment = HorizontalAlignment.Left,
                    VerticalAlignment = VerticalAlignment.Center,
                    Width = 16,
                    Height = 16,
                    Margin = new Thickness(2)
                };

            stack.Children.Add(this.image);

            this.textBlock = new TextBlock
                {
                    Margin = new Thickness(2),
                    VerticalAlignment = VerticalAlignment.Center,
                    Text = this.blueprintId
                };

            stack.Children.Add(this.textBlock);

            Header = stack;
        }

        #endregion
    }
}