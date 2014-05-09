// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ImportDataCSVWindow.xaml.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace BlueprintEditor.Windows
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Linq;
    using System.Text;
    using System.Windows;

    using BlueprintEditor.Controls;
    using BlueprintEditor.ViewModels;

    using CsvHelper;

    using Slash.GameBase.Inspector.Attributes;
    using Slash.Tools.BlueprintEditor.Logic.Data;

    /// <summary>
    ///   Window that allows importing CSV data into the current project.
    /// </summary>
    public partial class ImportCsvDataWindow
    {
        #region Constants

        /// <summary>
        ///   Default id of blueprint records to ignore.
        /// </summary>
        private const string DefaultIgnoredBlueprintId = "_NOTE";

        #endregion

        #region Fields

        /// <summary>
        ///   Editor context holding the current model and project settings.
        /// </summary>
        private readonly EditorContext context;

        /// <summary>
        ///   CSV reader to use for the import.
        /// </summary>
        private readonly ICsvReader csvReader;

        /// <summary>
        ///   Mappings between CSV columns and blueprint attribute table keys.
        /// </summary>
        private readonly List<PropertyValueMappingViewModel> valueMappings = new List<PropertyValueMappingViewModel>();

        /// <summary>
        ///   Custom CSV import data for storing CSV import settings for future imports.
        /// </summary>
        private CsvImportData importData;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///   Creates and shows a new window that allows importing CSV data into the current project.
        /// </summary>
        /// <param name="context">Editor context holding the current model and project settings.</param>
        /// <param name="csvReader">CSV reader to use for the import.</param>
        /// <param name="importData">Custom CSV import settings for initializing the window.</param>
        /// <exception cref="ArgumentNullException">
        ///   <paramref name="context" /> or <paramref name="csvReader" /> is null.
        /// </exception>
        public ImportCsvDataWindow(EditorContext context, ICsvReader csvReader, CsvImportData importData)
        {
            this.InitializeComponent();

            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            if (csvReader == null)
            {
                throw new ArgumentNullException("csvReader");
            }

            this.context = context;
            this.csvReader = csvReader;
            this.importData = importData;

            this.InitializeWindow();
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
                return this.CbParentBlueprint.SelectedBlueprint;
            }
        }

        /// <summary>
        ///   Headers of the columns of the imported CSV file.
        /// </summary>
        public IEnumerable<string> CSVColumnHeaders { get; private set; }

        /// <summary>
        ///   Id of blueprint records to ignore.
        /// </summary>
        public string IgnoredBlueprintId
        {
            get
            {
                return this.TbIgnoredBlueprintId.Text;
            }
        }

        /// <summary>
        ///   Whether to store CSV import settings for future imports into the same project.
        /// </summary>
        public bool SaveSettingsForFutureImports
        {
            get
            {
                return this.CbSaveSettings.IsChecked.GetValueOrDefault();
            }
        }

        /// <summary>
        ///   Maps attribute table keys to CSV columns.
        /// </summary>
        public ReadOnlyCollection<PropertyValueMappingViewModel> ValueMappings
        {
            get
            {
                return new ReadOnlyCollection<PropertyValueMappingViewModel>(this.valueMappings);
            }
        }

        #endregion

        #region Methods

        /// <summary>
        ///   Adds mapping controls for mapping CSV columns to blueprint attribute table keys for the specified parent blueprint.
        /// </summary>
        /// <param name="viewModel">Blueprint to add mapping controls for.</param>
        private void AddAttributeMappingsRecursively(BlueprintViewModel viewModel)
        {
            if (viewModel == null)
            {
                return;
            }

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
                    PropertyValueMappingViewModel valueMapping = new PropertyValueMappingViewModel
                        {
                            MappingSource = inspectorProperty.Name,
                            AvailableMappingTargets = new ObservableCollection<string>(this.CSVColumnHeaders),
                            InspectorProperty = inspectorProperty
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

            EditorDialog.Info("CSV Import Cancelled", "No data imported.");
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
            var mappedColumns = new HashSet<string> { this.BlueprintIdColumn };

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

        /// <summary>
        ///   Imports CSV data with the current settings into the current project.
        /// </summary>
        private void ImportData()
        {
            // Create a blueprint for each CSV row.
            var blueprintManagerViewModel = this.context.BlueprintManagerViewModel;
            var processedBlueprints = new HashSet<string>();
            var errors = new List<Exception>();

            var newBlueprints = 0;
            var updatedBlueprints = 0;
            var skippedBlueprints = 0;

            while (this.csvReader.CurrentRecord != null)
            {
                try
                {
                    // Get id of the blueprint to create or update.
                    var blueprintId = this.csvReader[this.BlueprintIdColumn];

                    // Skip ignored records, such as notes.
                    if (blueprintId == this.IgnoredBlueprintId)
                    {
                        this.csvReader.Read();
                        continue;
                    }

                    // Check for duplicate blueprints in the CSV file.
                    if (processedBlueprints.Contains(blueprintId))
                    {
                        throw new InvalidOperationException(string.Format("Duplicate blueprint id: {0}", blueprintId));
                    }

                    processedBlueprints.Add(blueprintId);

                    // Check whether blueprint already exists.
                    var dataBlueprint =
                        blueprintManagerViewModel.Blueprints.FirstOrDefault(
                            blueprint => blueprint.BlueprintId == blueprintId);
                    var newBlueprint = dataBlueprint == null;

                    if (newBlueprint)
                    {
                        // Create new blueprint.
                        blueprintManagerViewModel.NewBlueprintId = blueprintId;
                        dataBlueprint = blueprintManagerViewModel.CreateNewBlueprint();

                        // Reparent new blueprint.
                        blueprintManagerViewModel.ReparentBlueprint(
                            dataBlueprint.BlueprintId, this.BlueprintParent.BlueprintId, false);
                    }
                    else
                    {
                        // Check parent of existing blueprint.
                        if (dataBlueprint.Parent != this.BlueprintParent)
                        {
                            throw new InvalidOperationException(
                                string.Format(
                                    "Blueprint {0} is child of {1} but should be child of {2}.",
                                    dataBlueprint.BlueprintId,
                                    dataBlueprint.Parent.BlueprintId,
                                    this.BlueprintParent.BlueprintId));
                        }
                    }

                    // Map attribute table keys to CSV values.
                    foreach (var valueMapping in
                        this.ValueMappings.Where(mapping => !string.IsNullOrWhiteSpace(mapping.MappingTarget)))
                    {
                        var rawValue = this.csvReader[valueMapping.MappingTarget];
                        object convertedValue;

                        if (!valueMapping.InspectorProperty.TryConvertStringToListOrValue(rawValue, out convertedValue)
                            || convertedValue == null)
                        {
                            throw new CsvParserException(
                                string.Format(
                                    "{0}: Unable to convert '{1}' to '{2}' ({3}).",
                                    blueprintId,
                                    rawValue,
                                    valueMapping.MappingSource,
                                    valueMapping.InspectorProperty.PropertyType));
                        }

                        // Check for localized values.
                        var stringProperty = valueMapping.InspectorProperty as InspectorStringAttribute;

                        if (stringProperty != null && stringProperty.Localized)
                        {
                            this.context.LocalizationContext.SetLocalizedStringForBlueprint(
                                blueprintId, valueMapping.MappingSource, (string)convertedValue);
                        }
                        else
                        {
                            dataBlueprint.Blueprint.AttributeTable[valueMapping.MappingSource] = convertedValue;
                        }
                    }

                    // Increase counter.
                    if (newBlueprint)
                    {
                        newBlueprints++;
                    }
                    else
                    {
                        updatedBlueprints++;
                    }
                }
                catch (Exception exception)
                {
                    errors.Add(exception);
                    skippedBlueprints++;
                }

                // Read next record.
                this.csvReader.Read();
            }

            // Show import results.
            if (errors.Count > 0)
            {
                EditorDialog.Warning("Some data could not be imported", errors);
            }

            var importInfoBuilder = new StringBuilder();
            importInfoBuilder.AppendLine(string.Format("{0} blueprint(s) imported.", newBlueprints));
            importInfoBuilder.AppendLine(string.Format("{0} blueprint(s) updated.", updatedBlueprints));
            importInfoBuilder.AppendLine(string.Format("{0} blueprint(s) skipped.", skippedBlueprints));
            var importInfo = importInfoBuilder.ToString();

            EditorDialog.Info("CSV Import Complete", importInfo);

            if (this.SaveSettingsForFutureImports)
            {
                this.SaveImportSettings();
            }

            blueprintManagerViewModel.RefreshBlueprintView();
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

            this.ImportData();
        }
        
        /// <summary>
        ///   Initializes all controls of this window.
        /// </summary>
        private void InitializeWindow()
        {
            // Read column headers and first row.
            this.csvReader.Read();

            // Store column headers for access via combo boxes.
            this.CSVColumnHeaders = this.csvReader.FieldHeaders;

            // Fill Parent Blueprint combo box.
            this.CbParentBlueprint.DataContext = this.context.BlueprintManagerViewModel;
            this.CbParentBlueprint.PropertyChanged += this.OnSelectedParentBlueprintChanged;
            this.CbParentBlueprint.Filter = blueprint => blueprint.Parent == null;
            this.CbParentBlueprint.Refresh();

            // Fill Blueprint Id combo box.
            this.CbBlueprintIdMapping.DataContext = this;
            this.CbBlueprintIdMapping.SelectedIndex = 0;

            // Fill Ignored Blueprint Id text box.
            this.TbIgnoredBlueprintId.DataContext = this;
            this.TbIgnoredBlueprintId.Text = DefaultIgnoredBlueprintId;

            // Fill language combo box.
            this.CbLanguage.DataContext = this.context;

            if (this.importData == null)
            {
                return;
            }

            // Load custom CSV import.
            this.CbParentBlueprint.SelectedBlueprintId = this.importData.BlueprintParentId;
            this.CbBlueprintIdMapping.SelectedValue = this.importData.BlueprintIdColumn;
            this.TbIgnoredBlueprintId.Text = this.importData.IgnoredBlueprintId;

            this.UpdateAttributeMapping();

            foreach (var importMapping in this.importData.Mappings)
            {
                var existingMapping =
                    this.ValueMappings.FirstOrDefault(mapping => mapping.MappingSource == importMapping.MappingSource);

                if (existingMapping != null)
                {
                    existingMapping.MappingTarget = importMapping.MappingTarget;
                }
            }
        }
        
        private void OnSelectedParentBlueprintChanged(object sender, PropertyChangedEventArgs e)
        {
            this.UpdateAttributeMapping();
        }

        /// <summary>
        ///   Saves the current import settings for future imports into the current project.
        /// </summary>
        private void SaveImportSettings()
        {
            if (this.importData == null)
            {
                this.importData = new CsvImportData();
                this.context.ProjectSettings.CustomImports.Add(this.importData);
            }
            
            this.importData.BlueprintIdColumn = this.BlueprintIdColumn;
            this.importData.BlueprintParentId = this.BlueprintParent.BlueprintId;
            this.importData.IgnoredBlueprintId = this.IgnoredBlueprintId;
            this.importData.Mappings = new List<ValueMapping>();

            foreach (var mapping in this.ValueMappings)
            {
                this.importData.Mappings.Add(
                    new ValueMapping { MappingSource = mapping.MappingSource, MappingTarget = mapping.MappingTarget });
            }
        }

        /// <summary>
        ///   Clears all mapping controls and creates new ones for the selected parent blueprint.
        /// </summary>
        private void UpdateAttributeMapping()
        {
            // Clear attribute mapping controls.
            this.valueMappings.Clear();
            this.SpAttributeMapping.Children.Clear();

            // Add new attribute mapping controls.
            this.AddAttributeMappingsRecursively(this.CbParentBlueprint.SelectedBlueprint);
        }

        #endregion
    }
}