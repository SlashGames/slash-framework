// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BlueprintControlContext.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace BlueprintEditor.Controls
{
    using System.Windows;

    using Slash.GameBase.Blueprints;

    public class BlueprintControlContext : DependencyObject
    {
        #region Static Fields

        public static readonly DependencyProperty BlueprintIdProperty = DependencyProperty.Register(
            "BlueprintId",
            typeof(string),
            typeof(BlueprintControlContext),
            new FrameworkPropertyMetadata(null, OnBlueprintIdChanged));

        public static readonly DependencyProperty BlueprintManagerProperty =
            DependencyProperty.Register(
                "BlueprintManager",
                typeof(BlueprintManager),
                typeof(BlueprintControlContext),
                new PropertyMetadata(null));

        public static readonly DependencyProperty BlueprintProperty = DependencyProperty.Register(
            "Blueprint", typeof(Blueprint), typeof(BlueprintControlContext), new PropertyMetadata(null));

        #endregion

        #region Public Properties

        public Blueprint Blueprint
        {
            get
            {
                return (Blueprint)this.GetValue(BlueprintProperty);
            }

            set
            {
                this.SetValue(BlueprintProperty, value);
            }
        }

        public string BlueprintId
        {
            get
            {
                return (string)this.GetValue(BlueprintIdProperty);
            }
            set
            {
                this.SetValue(BlueprintIdProperty, value);
            }
        }

        public BlueprintManager BlueprintManager
        {
            get
            {
                return (BlueprintManager)this.GetValue(BlueprintManagerProperty);
            }

            set
            {
                this.SetValue(BlueprintManagerProperty, value);
            }
        }

        #endregion

        #region Methods

        private static void OnBlueprintIdChanged(DependencyObject source, DependencyPropertyChangedEventArgs e)
        {
            BlueprintControlContext context = (BlueprintControlContext)source;

            string oldBlueprintId = (string)e.OldValue;
            string newBlueprintId = (string)e.NewValue;

            // Move in blueprint manager.
            if (context.BlueprintManager != null)
            {
                if (oldBlueprintId != null && newBlueprintId != null)
                {
                    context.BlueprintManager.ChangeBlueprintId(oldBlueprintId, newBlueprintId);
                }
            }
        }

        #endregion
    }
}