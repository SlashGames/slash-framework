// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MenuCommands.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace BlueprintEditor.Commands
{
    using System.Windows.Input;

    public static class MenuCommands
    {
        #region Static Fields

        /// <summary>
        ///   Command to copy selected blueprint.
        /// </summary>
        public static ICommand CopyBlueprint = new RoutedCommand("Copy blueprint", typeof(MenuCommands));

        /// <summary>
        ///   Command to run a custom CSV import.
        /// </summary>
        public static ICommand CustomImport = new RoutedCommand("Custom Import", typeof(MenuCommands));

        /// <summary>
        ///   Command to exit the application.
        /// </summary>
        public static ICommand Exit = new RoutedCommand("Exit", typeof(MenuCommands));

        /// <summary>
        ///   Command to export localization data.
        /// </summary>
        public static ICommand ExportLocalization = new RoutedCommand("Export Localization", typeof(MenuCommands));

        /// <summary>
        ///   Command to import blueprints from an CSV file.
        /// </summary>
        public static ICommand ImportData = new RoutedCommand("Import Data", typeof(MenuCommands));

        /// <summary>
        ///   Command to import localization data.
        /// </summary>
        public static ICommand ImportLocalization = new RoutedCommand("Export Localization", typeof(MenuCommands));

        #endregion
    }
}