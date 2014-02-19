// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PropertyValueMappingViewModel.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace BlueprintEditor.ViewModels
{
    using Slash.GameBase.Inspector.Attributes;

    public class PropertyValueMappingViewModel : ValueMappingViewModel
    {
        #region Public Properties

        public InspectorPropertyAttribute InspectorProperty { get; set; }

        #endregion
    }
}