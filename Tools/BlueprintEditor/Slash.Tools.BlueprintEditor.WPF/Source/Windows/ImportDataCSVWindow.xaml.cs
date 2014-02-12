// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ImportDataCSVWindow.xaml.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace BlueprintEditor.Windows
{
    using BlueprintEditor.ViewModels;

    public partial class ImportDataCSVWindow
    {
        #region Fields

        private EditorContext context;

        #endregion

        #region Constructors and Destructors

        public ImportDataCSVWindow()
        {
            this.InitializeComponent();
        }

        #endregion

        #region Public Methods and Operators

        public void Init(EditorContext context)
        {
            this.context = context;
            this.CbBlueprints.DataContext = context;
        }

        #endregion
    }
}