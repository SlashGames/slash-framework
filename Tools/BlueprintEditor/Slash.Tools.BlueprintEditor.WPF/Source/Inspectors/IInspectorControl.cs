// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IInspectorControl.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace BlueprintEditor.Inspectors
{
    using Slash.GameBase.Attributes;

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

        /// <summary>
        ///   Inspector property this control is for.
        /// </summary>
        InspectorPropertyAttribute InspectorProperty { get; set; }

        #endregion
    }
}