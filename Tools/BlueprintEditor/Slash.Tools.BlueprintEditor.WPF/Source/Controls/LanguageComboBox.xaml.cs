// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LanguageComboBox.xaml.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace BlueprintEditor.Controls
{
    using System.Collections.Specialized;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;
    using System.Windows;
    using System.Windows.Controls;

    using BlueprintEditor.Annotations;
    using BlueprintEditor.ViewModels;

    /// <summary>
    ///   Interaction logic for LanguageComboBox.xaml
    /// </summary>
    public partial class LanguageComboBox : UserControl
    {
        #region Static Fields

        public static readonly DependencyProperty SelectedLanguageProperty =
            DependencyProperty.Register(
                "SelectedLanguage",
                typeof(string),
                typeof(LanguageComboBox),
                new PropertyMetadata(null) { PropertyChangedCallback = OnSelectedLanguageChanged });

        #endregion

        #region Constructors and Destructors

        public LanguageComboBox()
        {
            this.InitializeComponent();

            this.DataContextChanged += this.OnDataContextChanged;
            this.ComboBox.SelectionChanged += this.OnSelectionChanged;
        }

        #endregion

        #region Public Events

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Public Properties

        public string SelectedLanguage
        {
            get
            {
                return (string)this.GetValue(SelectedLanguageProperty);
            }
            set
            {
                this.SetValue(SelectedLanguageProperty, value);
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

        private static void OnSelectedLanguageChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            LanguageComboBox comboBox = (LanguageComboBox)d;
            comboBox.OnPropertyChanged("SelectedLanguage");
        }

        private void OnAvailableLanguagesChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            this.ComboBox.SelectedIndex = 0;
        }

        private void OnDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            var editorContext = (EditorContext)e.NewValue;
            editorContext.AvailableLanguages.CollectionChanged += this.OnAvailableLanguagesChanged;
        }

        private void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string newSelectedLanguage = (string)this.ComboBox.SelectedItem;
            this.SelectedLanguage = newSelectedLanguage;
        }

        #endregion
    }
}