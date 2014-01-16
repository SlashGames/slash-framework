// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ListInspector.xaml.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace BlueprintEditor.Inspectors
{
    using System.Windows;

    using BlueprintEditor.Windows;

    /// <summary>
    ///   Inspector to show and edit a list of values.
    /// </summary>
    public partial class ListInspector
    {
        #region Constructors and Destructors

        public ListInspector()
        {
            this.InitializeComponent();
        }

        #endregion

        #region Methods

        private void BtEdit_OnClick(object sender, RoutedEventArgs e)
        {
            // Show window for editing the list.
            ListInspectorWindow dlg = new ListInspectorWindow();
            var propertyData = (InspectorPropertyData)this.DataContext;
            dlg.Init(propertyData);
            dlg.ShowDialog();

            // Store edited list.
            propertyData.Value = dlg.GetPropertyValue();
        }

        #endregion
    }
}