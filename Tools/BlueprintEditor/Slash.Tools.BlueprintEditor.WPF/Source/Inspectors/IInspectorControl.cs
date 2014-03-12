// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IInspectorControl.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace BlueprintEditor.Inspectors
{
    using BlueprintEditor.ViewModels;

    using Slash.GameBase.Inspector.Attributes;

    public delegate void InspectorControlValueChangedDelegate(
        InspectorPropertyAttribute inspectorProperty, object newValue);

    public interface IInspectorControl
    {
        #region Public Events

        /// <summary>
        ///   Raised when value of the inspector control changed.
        /// </summary>
        event InspectorControlValueChangedDelegate ValueChanged;

        #endregion

        #region Public Properties

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///   Initializes the control with the inspector property it is for and the current value.
        /// </summary>
        /// <param name="inspectorProperty">Inspector property the control is for.</param>
        /// <param name="editorContext">Editor context this control lives in.</param>
        /// <param name="localizationContext">Context used for showing and changing localized attribute values.</param>
        /// <param name="currentValue">Current value.</param>
        /// <param name="valueInherited">Indicates if the current value was inherited.</param>
        void Init(InspectorPropertyAttribute inspectorProperty, EditorContext editorContext, LocalizationContext localizationContext, object currentValue, bool valueInherited);

        #endregion
    }
}