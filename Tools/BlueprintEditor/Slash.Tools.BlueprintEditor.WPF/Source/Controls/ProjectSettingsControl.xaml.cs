// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ProjectSettingsControl.xaml.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace BlueprintEditor.Controls
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Windows;

    using BlueprintEditor.Windows;

    using Microsoft.Win32;

    using Slash.Tools.BlueprintEditor.Logic.Context;

    /// <summary>
    ///   Interaction logic for ProjectSettingsControl.xaml
    /// </summary>
    public partial class ProjectSettingsControl
    {
        #region Constructors and Destructors

        public ProjectSettingsControl()
        {
            this.InitializeComponent();
        }

        #endregion

        #region Methods

        private void AddAssembly_OnClick(object sender, RoutedEventArgs e)
        {
            // Configure open file dialog box.
            OpenFileDialog dlg = new OpenFileDialog { DefaultExt = ".dll", Filter = "Assemblies (.dll)|*.dll" };

            // Show open file dialog box.
            bool? result = dlg.ShowDialog();

            // Process open file dialog box results 
            if (result != true)
            {
                return;
            }

            // Add assembly to project.
            ProjectSettings projectSettings = (ProjectSettings)this.DataContext;
            Assembly assembly = Assembly.LoadFile(dlg.FileName);

            try
            {
                projectSettings.AddAssembly(assembly);

                // Refresh list.
                this.AssembliesList.Items.Refresh();
            }
            catch (ReflectionTypeLoadException ex)
            {
                EditorDialog.Error(
                    "Error adding assembly",
                    string.Format(
                        "An error has occurred adding assembly {0}: {1}", dlg.FileName, ex.LoaderExceptions[0]));

                // TODO(np): Beautifully handle the error and prevent the crash.
                projectSettings.RemoveAssembly(assembly);
            }
        }

        private void AddLanguageFile_OnClick(object sender, RoutedEventArgs e)
        {
            // Configure open file dialog box.
            OpenFileDialog dlg = new OpenFileDialog { DefaultExt = ".txt", Filter = "Language Files (.txt)|*.txt" };

            // Show open file dialog box.
            bool? result = dlg.ShowDialog();

            // Process open file dialog box results 
            if (result != true)
            {
                return;
            }

            // Show language name dialog box.
            TextBoxWindow textboxDlg = new TextBoxWindow();
            result = textboxDlg.ShowDialog("Add Language", "Please enter the name of the language!");

            if (result != true)
            {
                return;
            }

            // Add language file to project.
            ProjectSettings projectSettings = (ProjectSettings)this.DataContext;
            LanguageFile languageFile = new LanguageFile { LanguageTag = textboxDlg.Text, Path = dlg.FileName };
            projectSettings.AddLanguageFile(languageFile);

            // Refresh list.
            this.LanguageFileList.Items.Refresh();
        }

        private void RemoveAssemblies_OnClick(object sender, RoutedEventArgs e)
        {
            // Get selected assemblies.
            IEnumerable<Assembly> selectedAssemblies = this.AssembliesList.SelectedItems.Cast<Assembly>().ToList();
            if (!selectedAssemblies.Any())
            {
                return;
            }

            ProjectSettings projectSettings = (ProjectSettings)this.DataContext;

            foreach (var selectedAssembly in selectedAssemblies)
            {
                // Check if still used.
                IEnumerable<Type> usedTypes = projectSettings.FindUsedTypes(selectedAssembly);
                IEnumerable<Type> usedTypesList = usedTypes as IList<Type> ?? usedTypes.ToList();
                if (usedTypesList.Any())
                {
                    string message =
                        string.Format(
                            "Can't remove assembly '{0}', {1} types are still used by the project:\n",
                            selectedAssembly.GetName().Name,
                            usedTypesList.Count());
                    const int MaxShownTypes = 5;
                    foreach (Type usedType in usedTypesList.Take(MaxShownTypes))
                    {
                        message += "- " + usedType.FullName + "\n";
                    }
                    if (usedTypesList.Count() > MaxShownTypes)
                    {
                        message += "- ...\n";
                    }
                    MessageBox.Show(message);
                    continue;
                }

                // Remove selected assembly.
                projectSettings.RemoveAssembly(selectedAssembly);
            }

            // Refresh list.
            this.AssembliesList.Items.Refresh();
        }

        private void RemoveLanguageFiles_OnClick(object sender, RoutedEventArgs e)
        {
            // Get selected language files.
            IEnumerable<LanguageFile> selectedLanguageFiles =
                this.LanguageFileList.SelectedItems.Cast<LanguageFile>().ToList();
            if (!selectedLanguageFiles.Any())
            {
                return;
            }

            ProjectSettings projectSettings = (ProjectSettings)this.DataContext;

            foreach (var selectedLanguageFile in selectedLanguageFiles)
            {
                // Remove selected assembly.
                projectSettings.RemoveLanguageFile(selectedLanguageFile);
            }

            // Refresh list.
            this.LanguageFileList.Items.Refresh();
        }

        #endregion
    }
}