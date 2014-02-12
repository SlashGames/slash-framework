// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InspectorWithLabel.xaml.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace BlueprintEditor.Inspectors
{
    using System.Windows;
    using System.Windows.Controls;

    /// <summary>
    ///   Interaction logic for InspectorWithLabel.xaml
    /// </summary>
    public partial class InspectorWithLabel : UserControl
    {
        #region Static Fields

        public static readonly DependencyProperty ControlProperty = DependencyProperty.Register(
            "Control", typeof(object), typeof(InspectorWithLabel), new UIPropertyMetadata(null));

        #endregion

        #region Constructors and Destructors

        public InspectorWithLabel()
        {
            this.InitializeComponent();
        }

        #endregion

        #region Public Properties

        public object Control
        {
            get
            {
                return this.GetValue(ControlProperty);
            }
            set
            {
                this.SetValue(ControlProperty, value);
            }
        }
        
        #endregion
    }
}