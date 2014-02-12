// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InspectorControl.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace BlueprintEditor.Inspectors
{
    using System.Windows.Controls;

    using Slash.GameBase.Inspector.Attributes;

    public class InspectorControl : UserControl, IInspectorControl
    {
        #region Fields

        /// <summary>
        ///   Data context of inspector.
        /// </summary>
        private InspectorPropertyData dataContext;

        #endregion

        #region Public Events

        /// <summary>
        ///   Raised when value of the inspector control changed.
        /// </summary>
        public event InspectorControlValueChangedDelegate ValueChanged;

        #endregion

        #region Public Properties

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///   Initializes the control with the inspector property it is for and the current value.
        /// </summary>
        /// <param name="inspectorProperty">Inspector property the control is for.</param>
        /// <param name="currentValue">Current value.</param>
        public void Init(InspectorPropertyAttribute inspectorProperty, object currentValue)
        {
            // Setup data context of control.
            this.dataContext = new InspectorPropertyData { InspectorProperty = inspectorProperty, Value = currentValue };
            this.dataContext.ValueChanged += this.OnValueChanged;
            this.DataContext = this.dataContext;
        }

        #endregion

        #region Methods

        private void OnValueChanged(InspectorPropertyAttribute inspectorProperty, object newValue, object oldValue)
        {
            InspectorControlValueChangedDelegate handler = this.ValueChanged;
            if (handler != null)
            {
                handler(inspectorProperty, newValue, oldValue);
            }
        }

        #endregion
    }
}