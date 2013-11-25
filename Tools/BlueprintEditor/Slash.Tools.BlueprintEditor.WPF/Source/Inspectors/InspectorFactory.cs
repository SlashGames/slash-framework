// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InspectorFactory.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace BlueprintEditor.Inspectors
{
    using System;
    using System.Diagnostics;

    using Slash.GameBase.Attributes;

    public class InspectorFactory
    {

        /// <summary>
        ///   Creates and positions a new inspector control for the passed property.
        /// </summary>
        /// <param name="inspectorProperty">Property to create an inspector control for.</param>
        /// <param name="currentValue">Current value of the observed property.</param>
        public IInspectorControl CreateInspectorControlFor(
            InspectorPropertyAttribute inspectorProperty,
            object currentValue)
        {
            // Create inspector control.
            IInspectorControl inspectorControl;
            if (inspectorProperty is InspectorBoolAttribute)
            {
                inspectorControl = null;
            }
            else if (inspectorProperty is InspectorFloatAttribute)
            {
                inspectorControl = null;
            }
            else if (inspectorProperty is InspectorIntAttribute)
            {
                inspectorControl = null;
            }
            else if (inspectorProperty is InspectorStringAttribute)
            {
                StringInspector stringInspector = new StringInspector
                    {
                        LbName = { Content = inspectorProperty.Name },
                        TbValue = { Text = (string)currentValue }
                    };
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
            else if (inspectorProperty is InspectorPrototypeAttribute)
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
                inspectorControl.InspectorProperty = inspectorProperty;
            }

            return inspectorControl;
        } 
    }
}