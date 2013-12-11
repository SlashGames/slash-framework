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
            projectSettings.AddAssembly(assembly);

            // Refresh list.
            this.AssembliesList.Items.Refresh();
        }

        private void RemoveAssembly_OnClick(object sender, RoutedEventArgs e)
        {
            // Get selected assembly.
            Assembly selectedAssembly = (Assembly)this.AssembliesList.SelectedItem;
            if (selectedAssembly == null)
            {
                return;
            }

            ProjectSettings projectSettings = (ProjectSettings)this.DataContext;

            // Check if still used.
            IEnumerable<Type> usedTypes = projectSettings.FindUsedTypes(selectedAssembly);
            IEnumerable<Type> usedTypesList = usedTypes as IList<Type> ?? usedTypes.ToList();
            if (usedTypesList.Any())
            {
                string message = string.Format(
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
                return;
            }

            // Remove selected assembly.
            projectSettings.RemoveAssembly(selectedAssembly);

            // Refresh list.
            this.AssembliesList.Items.Refresh();
        }

        #endregion
    }
}