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
    using System.Linq;
    using System.Windows;

    using BlueprintEditor.Controls;
    using BlueprintEditor.ViewModels;

    using Slash.Tools.BlueprintEditor.Logic.Data;

    public partial class ImportDataCSVWindow
    {
        #region Fields

        private readonly List<ValueMappingViewModel> valueMappings = new List<ValueMappingViewModel>();

        #endregion

        #region Constructors and Destructors

        public ImportDataCSVWindow(EditorContext context, IEnumerable<string> csvColumnHeaders)
        {
            this.InitializeComponent();

            this.CSVColumnHeaders = csvColumnHeaders;

            this.CbParentBlueprint.DataContext = context;
            this.CbParentBlueprint.PropertyChanged += this.OnSelectedParentBlueprintChanged;
            this.CbParentBlueprint.Filter = blueprint => blueprint.Parent == null;

            this.CbBlueprintIdMapping.DataContext = this;
            this.CbBlueprintIdMapping.SelectedIndex = 0;
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///   CSV column that contains the blueprint ids for the blueprints to create.
        /// </summary>
        public string BlueprintIdColumn
        {
            get
            {
                return (string)this.CbBlueprintIdMapping.SelectedItem;
            }
        }

        /// <summary>
        ///   Parent blueprint of the blueprints to create.
        /// </summary>
        public BlueprintViewModel BlueprintParent
        {
            get
            {
                return this.CbParentBlueprint.SelectedItem;
            }
        }

        /// <summary>
        ///   Headers of the columns of the imported CSV file.
        /// </summary>
        public IEnumerable<string> CSVColumnHeaders { get; private set; }

        /// <summary>
        ///   Maps attribute table keys to CSV columns.
        /// </summary>
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
                            MappingSource = inspectorProperty.Name,
                            AvailableMappingTargets = new ObservableCollection<string>(this.CSVColumnHeaders)
                        };

                    ValueMappingControl valueMappingControl = new ValueMappingControl(valueMapping);

                    // Add to panel.
                    this.valueMappings.Add(valueMapping);
                    this.SpAttributeMapping.Children.Add(valueMappingControl);
                }
            }
        }

        private void Cancel_OnClick(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }

        /// <summary>
        ///   Checks whether any CSV column is used for multiple attribute table keys.
        /// </summary>
        /// <returns>
        ///   <c>true</c>, if no CSV column is used for multiple attribute table keys, and
        ///   <c>false</c> otherwise.
        /// </returns>
        private bool CheckDuplicateMappings()
        {
            var mappedColumns = new HashSet<string>();
            mappedColumns.Add(this.BlueprintIdColumn);

            foreach (var mapping in this.ValueMappings.Where(mapping => mapping.MappingTarget != null))
            {
                if (mappedColumns.Contains(mapping.MappingTarget))
                {
                    return false;
                }

                mappedColumns.Add(mapping.MappingTarget);
            }

            return true;
        }

        private void ImportData_OnClick(object sender, RoutedEventArgs e)
        {
            if (!this.CheckDuplicateMappings())
            {
                var result =
                    MessageBox.Show(
                        "Some attribute table keys are mapped to the same CSV column. Continue anyway?",
                        "Duplicate Mappings",
                        MessageBoxButton.YesNo,
                        MessageBoxImage.Question,
                        MessageBoxResult.No);

                if (result != MessageBoxResult.Yes)
                {
                    return;
                }
            }

            this.DialogResult = true;
            this.Close();
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
            this.AddAttributeMappingsRecursively(this.CbParentBlueprint.SelectedItem);
        }

        #endregion
    }
}