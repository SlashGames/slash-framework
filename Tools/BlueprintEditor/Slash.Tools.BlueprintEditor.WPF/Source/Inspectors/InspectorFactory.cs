// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InspectorFactory.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace BlueprintEditor.Inspectors
{
    using System;
    using System.Windows;
    using System.Windows.Controls;

    using BlueprintEditor.Inspectors.Controls;

    using Slash.GameBase.Inspector.Attributes;
    using Slash.GameBase.Inspector.Data;
    using Slash.SystemExt.Utils;

    public class InspectorFactory
    {
        #region Public Methods and Operators

        public void AddInspectorControls(
            InspectorType typeInfo,
            Panel panel,
            Func<InspectorPropertyAttribute, object> getPropertyValue,
            InspectorControlValueChangedDelegate onValueChanged,
            bool addNameLabel = true)
        {
            if (addNameLabel)
            {
                // Add label for component name.
                var componentName = typeInfo.Type.Name;
                componentName = componentName.Replace("Component", string.Empty);
                componentName = componentName.SplitByCapitalLetters();

                Label componentLabel = new Label
                    {
                        Content = componentName,
                        ToolTip = typeInfo.Description,
                        FontWeight = FontWeights.Bold
                    };
                panel.Children.Add(componentLabel);
            }

            // Add inspectors for component properties.
            foreach (var inspectorProperty in typeInfo.Properties)
            {
                // Get current value.
                var propertyValue = getPropertyValue != null
                                        ? getPropertyValue(inspectorProperty)
                                        : inspectorProperty.Default;

                // Create control for inspector property.
                IInspectorControl propertyControl = this.CreateInspectorControlFor(inspectorProperty, propertyValue);
                if (propertyControl == null)
                {
                    continue;
                }

                if (onValueChanged != null)
                {
                    // Subscribe for change of value.
                    propertyControl.ValueChanged += onValueChanged;
                }

                // Create wrapper.
                InspectorWithLabel inspectorWrapper = new InspectorWithLabel
                    {
                        DataContext = ((FrameworkElement)propertyControl).DataContext,
                        Control = propertyControl
                    };

                // Add to panel.
                panel.Children.Add(inspectorWrapper);
            }
        }

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
            if (inspectorProperty.List)
            {
                inspectorControl = new ListInspector();
            }
            else if (inspectorProperty is InspectorBoolAttribute)
            {
                inspectorControl = new CheckBoxInspector();
            }
            else if (inspectorProperty is InspectorStringAttribute)
            {
                inspectorControl = new TextBoxInspector();
            }
            else if (inspectorProperty is InspectorFloatAttribute)
            {
                inspectorControl = new SingleUpDownInspector();
            }
            else if (inspectorProperty is InspectorIntAttribute)
            {
                inspectorControl = new IntegerUpDownInspector();
            }
            else if (inspectorProperty is InspectorEnumAttribute)
            {
                inspectorControl = new ComboBoxInspector();
            }
            else if (inspectorProperty is InspectorBlueprintAttribute)
            {
                inspectorControl = null;
            }
            else if (inspectorProperty is InspectorDataAttribute)
            {
                inspectorControl = new DataInspector();
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