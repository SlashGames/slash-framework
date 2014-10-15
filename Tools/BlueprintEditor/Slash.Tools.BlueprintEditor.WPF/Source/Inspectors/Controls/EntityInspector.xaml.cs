// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EntityInspector.xaml.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace BlueprintEditor.Inspectors.Controls
{
    using System;
    using System.Collections;
    using System.ComponentModel;
    using System.Windows;

    using BlueprintEditor.Controls;
    using BlueprintEditor.ViewModels;

    using Slash.Collections.AttributeTables;
    using Slash.Collections.Utils;
    using Slash.ECS.Configurations;
    using Slash.ECS.Inspector.Attributes;

    /// <summary>
    ///   Interaction logic for EntityInspector.xaml
    /// </summary>
    public partial class EntityInspector
    {
        #region Fields

        private InspectorFactory inspectorFactory;

        private BlueprintViewModel selectedBlueprint;

        #endregion

        #region Constructors and Destructors

        public EntityInspector()
        {
            this.InitializeComponent();
            this.DataContextChanged += this.OnDataContextChanged;

            DependencyPropertyDescriptor dpd =
                DependencyPropertyDescriptor.FromProperty(
                    BlueprintComboBox.SelectedBlueprintProperty, typeof(BlueprintComboBox));
            dpd.AddValueChanged(this.CbBlueprint, this.OnSelectedBlueprintChanged);
        }

        #endregion

        #region Methods

        private object GetCurrentAttributeValue(InspectorPropertyAttribute inspectorProperty, out bool inherited)
        {
            object value;
            EntityConfiguration entityConfiguration = (EntityConfiguration)this.Value;
            if (entityConfiguration != null
                && entityConfiguration.Configuration.TryGetValue(inspectorProperty.Name, out value))
            {
                inherited = false;
            }
            else
            {
                inherited = true;
                value = this.GetDefaultValue(inspectorProperty);
            }
            return value;
        }

        private object GetDefaultValue(InspectorPropertyAttribute inspectorProperty)
        {
            return this.selectedBlueprint != null
                       ? this.selectedBlueprint.Blueprint.GetAttributeTable()
                             .GetValueOrDefault(inspectorProperty.Name, inspectorProperty.Default)
                       : null;
        }

        private void OnBlueprintChanged(BlueprintViewModel newBlueprint)
        {
            if (Equals(this.selectedBlueprint, newBlueprint))
            {
                return;
            }

            this.selectedBlueprint = newBlueprint;

            // Update attribute table.
            this.UpdateAttributeTable();

            // Set value.
            EntityConfiguration entityConfiguration = (EntityConfiguration)this.Value ?? new EntityConfiguration();
            entityConfiguration.BlueprintId = newBlueprint != null ? newBlueprint.BlueprintId : string.Empty;

            this.Value = entityConfiguration;

            this.OnValueChanged();
        }

        private void OnDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            InspectorPropertyData dataContext = (InspectorPropertyData)this.DataContext;

            this.inspectorFactory = new InspectorFactory(
                dataContext.EditorContext,
                dataContext.EditorContext != null ? dataContext.EditorContext.LocalizationContext : null);

            EntityConfiguration entityConfiguration = (EntityConfiguration)this.Value;
            if (entityConfiguration != null)
            {
                // Select correct blueprint view model.
                this.CbBlueprint.SelectedBlueprintId = entityConfiguration.BlueprintId;
            }

            this.UpdateAttributeTable();
        }

        private void OnPropertyControlValueChanged(InspectorPropertyAttribute inspectorProperty, object newValue)
        {
            EntityConfiguration entityConfiguration = (EntityConfiguration)this.Value ?? new EntityConfiguration();

            // Remove value if default value or inherited from blueprint. Otherwise set it.
            object defaultValue = this.GetDefaultValue(inspectorProperty);

            var defaultList = defaultValue as IList;
            var newList = newValue as IList;

            if (defaultList != null && newList != null && CollectionUtils.ListEqual(defaultList, newList))
            {
                entityConfiguration.Configuration.RemoveValue(inspectorProperty.Name);
            }
            else if (Equals(newValue, defaultValue))
            {
                entityConfiguration.Configuration.RemoveValue(inspectorProperty.Name);
            }
            else
            {
                entityConfiguration.Configuration.SetValue(inspectorProperty.Name, newValue);
            }

            this.OnValueChanged();
        }

        private void OnSelectedBlueprintChanged(object sender, EventArgs e)
        {
            this.OnBlueprintChanged(this.CbBlueprint.SelectedBlueprint);
        }

        private void UpdateAttributeTable()
        {
            // Clear old attributes.
            this.AttributesPanel.Children.Clear();

            if (this.selectedBlueprint == null)
            {
                return;
            }

            // Add inspectors for blueprint components.
            this.inspectorFactory.AddComponentInspectorsRecursively(
                this.selectedBlueprint,
                this.AttributesPanel,
                this.GetCurrentAttributeValue,
                this.OnPropertyControlValueChanged);
        }

        #endregion
    }
}