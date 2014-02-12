// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ImportDataCSVWindow.xaml.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace BlueprintEditor.Windows
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Windows;

    using BlueprintEditor.Controls;
    using BlueprintEditor.ViewModels;

    using Slash.Tools.BlueprintEditor.Logic.Data;

    public partial class ImportDataCSVWindow
    {
        #region Fields

        private readonly IEnumerable<string> csvColumns;

        private readonly List<ValueMappingViewModel> valueMappings = new List<ValueMappingViewModel>();

        #endregion

        #region Constructors and Destructors

        public ImportDataCSVWindow(EditorContext context, IEnumerable<string> csvColumns)
        {
            this.InitializeComponent();

            this.CbBlueprints.DataContext = context;
            this.CbBlueprints.PropertyChanged += this.OnSelectedParentBlueprintChanged;

            this.csvColumns = csvColumns;
        }

        #endregion

        #region Public Properties

        public ReadOnlyCollection<ValueMappingViewModel> ValueMappings
        {
            get
            {
                return new ReadOnlyCollection<ValueMappingViewModel>(this.valueMappings);
            }
        }

        #endregion

        #region Methods

        private void AddAttributeMappingsRecursively(BlueprintViewModel viewModel)
        {
            // Add mapping controls for parent blueprints.
            if (viewModel.Parent != null)
            {
                this.AddAttributeMappingsRecursively(viewModel.Parent);
            }

            // Add mapping controls for specified blueprint.
            foreach (var componentType in viewModel.AddedComponents)
            {
                // Get attributes.
                var componentInfo = InspectorComponentTable.Instance.GetInspectorType(componentType);
                if (componentInfo == null)
                {
                    continue;
                }

                foreach (var inspectorProperty in componentInfo.Properties)
                {
                    // Create attribute mapping control.
                    ValueMappingViewModel valueMapping = new ValueMappingViewModel
                        {
                            MappingSource = inspectorProperty.PropertyName,
                            AvailableMappingTargets = new ObservableCollection<string>(this.csvColumns)
                        };

                    ValueMappingControl valueMappingControl = new ValueMappingControl(valueMapping);

                    // Add to panel.
                    this.valueMappings.Add(valueMapping);
                    this.SpAttributeMapping.Children.Add(valueMappingControl);
                }
            }
        }

        private void ImportData_OnClick(object sender, RoutedEventArgs e)
        {
            this.Close();

            // TODO(np): Provide Cancel button.
        }

        private void OnSelectedParentBlueprintChanged(object sender, PropertyChangedEventArgs e)
        {
            this.UpdateAttributeMapping();
        }

        private void UpdateAttributeMapping()
        {
            // Clear attribute mapping controls.
            this.valueMappings.Clear();
            this.SpAttributeMapping.Children.Clear();

            // Add new attribute mapping controls.
            this.AddAttributeMappingsRecursively(this.CbBlueprints.SelectedItem);
        }

        #endregion
    }
}