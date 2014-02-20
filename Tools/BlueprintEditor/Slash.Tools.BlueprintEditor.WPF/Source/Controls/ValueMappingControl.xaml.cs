// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ValueMappingControl.xaml.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace BlueprintEditor.Controls
{
    using System.ComponentModel;
    using System.Runtime.CompilerServices;
    using System.Windows.Controls;

    using BlueprintEditor.Annotations;
    using BlueprintEditor.ViewModels;

    /// <summary>
    ///   Control for mapping a given value to an element of a specified set.
    /// </summary>
    public partial class ValueMappingControl : INotifyPropertyChanged
    {
        #region Fields

        private readonly ValueMappingViewModel valueMapping;

        #endregion

        #region Constructors and Destructors

        public ValueMappingControl(ValueMappingViewModel valueMapping)
        {
            this.InitializeComponent();

            this.valueMapping = valueMapping;
            this.DataContext = valueMapping;

            this.CbMappingTarget.SelectionChanged += this.OnSelectionChanged;
        }

        #endregion

        #region Public Events

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Public Properties

        public ValueMappingViewModel ValueMapping
        {
            get
            {
                return this.valueMapping;
            }
        }

        #endregion

        #region Methods

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler = this.PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.valueMapping.MappingTarget = (string)this.CbMappingTarget.SelectedItem;
            this.OnPropertyChanged("ValueMapping");
        }

        #endregion
    }
}