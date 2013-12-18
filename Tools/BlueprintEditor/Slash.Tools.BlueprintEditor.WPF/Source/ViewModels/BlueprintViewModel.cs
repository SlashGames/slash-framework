// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BlueprintViewModel.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace BlueprintEditor.ViewModels
{
    using System.ComponentModel;
    using System.Runtime.CompilerServices;

    using BlueprintEditor.Annotations;

    using MonitoredUndo;

    using Slash.GameBase.Blueprints;

    public class BlueprintViewModel : INotifyPropertyChanged, ISupportsUndo
    {
        #region Fields

        /// <summary>
        ///   Id of the blueprint.
        /// </summary>
        private string blueprintId;

        #endregion

        #region Public Events

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Public Properties

        /// <summary>
        ///   Blueprint this item represents.
        /// </summary>
        public Blueprint Blueprint { get; set; }

        /// <summary>
        ///   Id of the blueprint.
        /// </summary>
        public string BlueprintId
        {
            get
            {
                return this.blueprintId;
            }
            set
            {
                if (value == this.blueprintId)
                {
                    return;
                }

                string oldBlueprintId = this.blueprintId;
                this.blueprintId = value;

                // Move in blueprint manager.
                if (this.BlueprintManager != null)
                {
                    if (oldBlueprintId != null && value != null)
                    {
                        this.BlueprintManager.ChangeBlueprintId(oldBlueprintId, value);
                    }
                }

                this.OnPropertyChanged();
            }
        }

        /// <summary>
        ///   Blueprint manager the blueprint belongs to.
        /// </summary>
        public BlueprintManager BlueprintManager { get; set; }

        public object Root { get; set; }

        #endregion

        #region Public Methods and Operators

        public object GetUndoRoot()
        {
            return this.Root;
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