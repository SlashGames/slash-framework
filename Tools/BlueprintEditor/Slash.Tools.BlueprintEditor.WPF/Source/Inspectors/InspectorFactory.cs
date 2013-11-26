// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InspectorFactory.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace BlueprintEditor.Inspectors
{
    using Slash.GameBase.Attributes;

    public class InspectorFactory
    {
        #region Public Methods and Operators

        /// <summary>
        ///   Creates and positions a new inspector control for the passed property.
        /// </summary>
        /// <param name="inspectorProperty">Property to create an inspector control for.</param>
        /// <param name="currentValue">Current value of the observed property.</param>
        public IInspectorControl CreateInspectorControlFor(
            InspectorPropertyAttribute inspectorProperty, object currentValue)
        {
            // Create inspector control.
            IInspectorControl inspectorControl;
            if (inspectorProperty is InspectorBoolAttribute)
            {
                CheckBoxInspector checkboxInspector = new CheckBoxInspector();
                inspectorControl = checkboxInspector;
            }
            else if (inspectorProperty is InspectorStringAttribute || inspectorProperty is InspectorFloatAttribute
                     || inspectorProperty is InspectorIntAttribute)
            {
                TextBoxInspector stringInspector = new TextBoxInspector();
                inspectorControl = stringInspector;
            }
            else if (inspectorProperty is InspectorEnumAttribute)
            {
                inspectorControl = null;
            }
            else if (inspectorProperty is InspectorBlueprintAttribute)
            {
                inspectorControl = null;
            }
            else
            {
                inspectorControl = null;
            }

            // Setup control.
            if (inspectorControl != null)
            {
                inspectorControl.Init(inspectorProperty, currentValue);
            }

            return inspectorControl;
        }

        #endregion
    }
}