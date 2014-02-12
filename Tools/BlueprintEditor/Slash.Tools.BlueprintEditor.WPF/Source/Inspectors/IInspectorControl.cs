// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IInspectorControl.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace BlueprintEditor.Inspectors
{
    using Slash.GameBase.Inspector.Attributes;

    public delegate void InspectorControlValueChangedDelegate(
        InspectorPropertyAttribute inspectorProperty, object newValue, object oldValue);

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
        /// <param name="currentValue">Current value.</param>
        void Init(InspectorPropertyAttribute inspectorProperty, object currentValue);

        #endregion
    }
}