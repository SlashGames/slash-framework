// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ProgressWindow.xaml.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace BlueprintEditor.Windows
{
    /// <summary>
    ///   Shows a progress bar and a status label.
    /// </summary>
    public partial class ProgressWindow
    {
        #region Constructors and Destructors

        public ProgressWindow()
        {
            this.InitializeComponent();
        }

        #endregion

        #region Public Methods and Operators

        public void Show(string message)
        {
            this.Text.Content = message;
            this.Show();
        }

        #endregion
    }
}