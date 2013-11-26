// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TextBoxInspector.xaml.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace BlueprintEditor.Inspectors
{
    using System;
    using System.Windows.Controls;

    using Slash.GameBase.Attributes;

    /// <summary>
    ///   Inspector to edit a value in a textbox.
    /// </summary>
    public sealed partial class TextBoxInspector : IInspectorControl
    {
        #region Fields

        /// <summary>
        ///   Current string value.
        /// </summary>
        private string value;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///   Constructor.
        /// </summary>
        public TextBoxInspector()
        {
            this.InitializeComponent();

            this.TbValue.TextChanged += this.TbValueOnTextChanged;
            this.value = this.TbValue.Text;
        }

        #endregion

        #region Public Events

        public event InspectorControlValueChangedDelegate ValueChanged;

        #endregion

        #region Public Properties

        public InspectorPropertyAttribute InspectorProperty { get; set; }

        #endregion

        #region Methods

        private void OnValueChanged(InspectorPropertyAttribute inspectorproperty, object newValue, object oldValue)
        {
            InspectorControlValueChangedDelegate handler = this.ValueChanged;
            if (handler != null)
            {
                handler(inspectorproperty, newValue, oldValue);
            }
        }

        private void TbValueOnTextChanged(object sender, TextChangedEventArgs textChangedEventArgs)
        {
            // Property isn't set on initialization.
            if (this.InspectorProperty == null)
            {
                return;
            }

            // Convert string value to correct type.
            object oldValue = this.InspectorProperty.ConvertFromString(this.value);
            object newValue = this.InspectorProperty.ConvertFromString(this.TbValue.Text);
            if (Equals(oldValue, newValue))
            {
                return;
            }

            this.OnValueChanged(this.InspectorProperty, newValue, oldValue);
            this.value = this.TbValue.Text;
        }

        #endregion
    }
}