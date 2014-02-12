// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ValueMappingViewModel.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace BlueprintEditor.ViewModels
{
    using System.Collections.ObjectModel;

    /// <summary>
    ///   Model for mapping a given value to an element of a specified set.
    /// </summary>
    public class ValueMappingViewModel
    {
        #region Public Properties

        public ObservableCollection<string> AvailableMappingTargets { get; set; }

        public string MappingSource { get; set; }

        public string MappingTarget { get; set; }

        #endregion
    }
}