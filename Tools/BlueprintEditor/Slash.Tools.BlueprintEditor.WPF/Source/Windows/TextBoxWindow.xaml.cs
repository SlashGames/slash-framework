// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TextBoxWindow.xaml.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace BlueprintEditor.Windows
{
    using System.Windows;

    /// <summary>
    ///   Interaction logic for TextBoxWindow.xaml
    /// </summary>
    public partial class TextBoxWindow
    {
        #region Constructors and Destructors

        public TextBoxWindow()
        {
            this.InitializeComponent();
        }

        #endregion

        #region Public Properties

        public string Text
        {
            get
            {
                return this.TbAnswer.Text;
            }
        }

        #endregion

        #region Public Methods and Operators

        public bool? ShowDialog(string title, string question)
        {
            this.Title = title;
            this.LbQuestion.Content = question;
            this.TbAnswer.Focus();

            return this.ShowDialog();
        }

        #endregion

        #region Methods

        private void BtnCancel_OnClick(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }

        private void BtnOk_OnClick(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
            this.Close();
        }

        #endregion
    }
}