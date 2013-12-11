// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ProjectSettingsControl.xaml.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace BlueprintEditor.Controls
{
    using System.Reflection;
    using System.Windows;
    using System.Windows.Controls;

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
            AssembliesList.Items.Refresh();
        }

        #endregion

        private void RemoveAssembly_OnClick(object sender, RoutedEventArgs e)
        {
            // Get selected assembly.
            Assembly selectedAssembly = (Assembly)this.AssembliesList.SelectedItem;
            if (selectedAssembly == null)
            {
                return;
            }

            // Remove selected assembly.
            ProjectSettings projectSettings = (ProjectSettings)this.DataContext;
            projectSettings.RemoveAssembly(selectedAssembly);

            // Refresh list.
            AssembliesList.Items.Refresh();
        }
    }
}