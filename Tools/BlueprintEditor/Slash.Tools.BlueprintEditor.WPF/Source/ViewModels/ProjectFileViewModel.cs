// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ProjectFileViewModel.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace BlueprintEditor.ViewModels
{
    using System.ComponentModel;
    using System.Runtime.CompilerServices;

    using BlueprintEditor.Annotations;

    using Slash.ECS.Blueprints;
    using Slash.Tools.BlueprintEditor.Logic.Context;

    public class ProjectFileViewModel : INotifyPropertyChanged
    {
        #region Fields

        private string projectFileName;

        #endregion

        #region Constructors and Destructors

        public ProjectFileViewModel(BlueprintFile blueprintFile)
        {
            this.ProjectFileName = blueprintFile.Path;
            this.BlueprintManager = blueprintFile.BlueprintManager;
        }

        #endregion

        #region Public Events

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Public Properties

        public BlueprintManager BlueprintManager { get; private set; }

        public string ProjectFileName
        {
            get
            {
                return this.projectFileName;
            }
            set
            {
                if (value == this.projectFileName)
                {
                    return;
                }

                this.projectFileName = value;

                this.OnPropertyChanged();
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

        #endregion
    }
}