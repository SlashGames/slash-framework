// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AboutWindow.xaml.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace BlueprintEditor.Windows
{
    using System.Reflection;
    using System.Windows;

    /// <summary>
    ///   Interaction logic for AboutWindow.xaml
    /// </summary>
    public partial class AboutWindow
    {
        #region Constructors and Destructors

        public AboutWindow()
        {
            this.InitializeComponent();
            this.DataContext = this;
        }

        #endregion

        #region Public Properties

        public string Version
        {
            get
            {
                var assembly = Assembly.GetExecutingAssembly();
                return string.Format("Version {0}", assembly.GetName().Version);
            }
        }

        #endregion

        #region Methods

        private void ButtonClose_OnClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        #endregion
    }
}