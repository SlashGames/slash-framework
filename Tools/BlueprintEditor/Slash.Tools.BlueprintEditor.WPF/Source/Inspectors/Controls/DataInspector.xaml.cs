// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DataInspector.xaml.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace BlueprintEditor.Inspectors.Controls
{
    using System.Windows;

    using Slash.Collections.AttributeTables;
    using Slash.GameBase.Inspector.Attributes;
    using Slash.GameBase.Inspector.Data;

    /// <summary>
    ///   Inspector to edit a value in a combo box.
    /// </summary>
    public partial class DataInspector
    {
        #region Fields

        private readonly InspectorFactory inspectorFactory = new InspectorFactory(null);

        private IAttributeTable value;

        #endregion

        #region Constructors and Destructors

        public DataInspector()
        {
            this.InitializeComponent();
            this.DataContextChanged += this.OnDataContextChanged;
        }

        #endregion

        #region Methods

        private void OnDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            InspectorPropertyData dataContext = (InspectorPropertyData)this.DataContext;
            InspectorDataAttribute inspectorDataAttribute = (InspectorDataAttribute)dataContext.InspectorProperty;

            this.value = (IAttributeTable)dataContext.Value;

            InspectorType typeInfo = InspectorType.GetInspectorType(inspectorDataAttribute.PropertyType);
            this.inspectorFactory.AddInspectorControls(typeInfo, this.Controls, this.GetPropertyValue, this.OnPropertyValueChanged, false);
        }

        private object GetPropertyValue(InspectorPropertyAttribute inspectorProperty, out bool inherited)
        {
            object propertyValue;
            if (this.value != null &&
                this.value.TryGetValue(inspectorProperty.Name, out propertyValue))
            {
                inherited = false;
                return propertyValue;
            }

            inherited = true;
            return inspectorProperty.Default;
        }

        private void OnPropertyValueChanged(InspectorPropertyAttribute inspectorProperty, object newValue)
        {
            if (this.value == null)
            {
                this.value = new AttributeTable();
            }

            this.value.SetValue(inspectorProperty.Name, newValue);
            
            InspectorPropertyData dataContext = (InspectorPropertyData)this.DataContext;
            dataContext.Value = this.value;
        }

        #endregion
    }
}