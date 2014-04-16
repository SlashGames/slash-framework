// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InspectorFactory.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace BlueprintEditor.Inspectors
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Windows;
    using System.Windows.Controls;

    using BlueprintEditor.Inspectors.Controls;
    using BlueprintEditor.ViewModels;

    using Slash.GameBase.Inspector.Attributes;
    using Slash.GameBase.Inspector.Data;
    using Slash.SystemExt.Utils;
    using Slash.Tools.BlueprintEditor.Logic.Data;

    public class InspectorFactory
    {
        #region Fields

        private readonly LocalizationContext localizationContext;

        private EditorContext editorContext;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///   Constructs a new factory for creating inspector controls.
        /// </summary>
        /// <param name="editorContext">Editor context the controls live in.</param>
        /// <param name="localizationContext">Context for localizing inspector values. <c>null</c> renders created controls unable to localize values.</param>
        public InspectorFactory(EditorContext editorContext, LocalizationContext localizationContext)
        {
            this.editorContext = editorContext;
            this.localizationContext = localizationContext;
        }

        #endregion

        #region Delegates

        public delegate object GetPropertyValueDelegate(InspectorPropertyAttribute inspectorProperty, out bool inherited
            );

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///   Adds inspectors for the components of the specified blueprint and its parents.
        /// </summary>
        /// <param name="viewModel">Blueprint to add component inspectors for.</param>
        /// <param name="panel">Panel to add inspectors to.</param>
        /// <param name="getPropertyValue">Callback to get current value for a property.</param>
        /// <param name="onValueChanged">Callback to invoke when a property value changed.</param>
        public void AddComponentInspectorsRecursively(
            BlueprintViewModel viewModel,
            Panel panel,
            GetPropertyValueDelegate getPropertyValue,
            InspectorControlValueChangedDelegate onValueChanged)
        {
            // Add inspectors for parent blueprints.
            if (viewModel.Parent != null)
            {
                this.AddComponentInspectorsRecursively(viewModel.Parent, panel, getPropertyValue, onValueChanged);
            }

            // Add inspectors for specified blueprint.
            foreach (var componentType in viewModel.AddedComponents)
            {
                // Get attributes.
                InspectorType componentInfo = InspectorComponentTable.Instance.GetInspectorType(componentType);
                if (componentInfo == null)
                {
                    continue;
                }

                this.AddInspectorControls(componentInfo, panel, getPropertyValue, onValueChanged);
            }
        }

        public void AddInspectorControls(
            InspectorType typeInfo,
            Panel panel,
            GetPropertyValueDelegate getPropertyValue,
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
                bool inherited = true;
                var propertyValue = getPropertyValue != null
                                        ? getPropertyValue(inspectorProperty, out inherited)
                                        : inspectorProperty.Default;

                // Create control for inspector property.
                IInspectorControl propertyControl = this.CreateInspectorControlFor(
                    inspectorProperty, propertyValue, inherited);
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
        /// <param name="valueInherited">Indicates if the current value was inherited.</param>
        public IInspectorControl CreateInspectorControlFor(
            InspectorPropertyAttribute inspectorProperty, object currentValue, bool valueInherited)
        {
            // Create inspector control.
            IInspectorControl inspectorControl;
            if (inspectorProperty.IsList)
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
            else if (inspectorProperty is InspectorEntityAttribute)
            {
                inspectorControl = new EntityInspector();
            }
            else
            {
                inspectorControl = null;
            }

            // Setup control.
            if (inspectorControl != null)
            {
                inspectorControl.Init(inspectorProperty, this.editorContext, this.localizationContext, currentValue, valueInherited);
            }

            return inspectorControl;
        }

        #endregion
    }
}