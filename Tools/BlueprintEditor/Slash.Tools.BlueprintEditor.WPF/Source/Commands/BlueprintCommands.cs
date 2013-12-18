// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BlueprintCommands.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace BlueprintEditor.Commands
{
    using System.Windows.Input;

    public static class BlueprintCommands
    {
        #region Static Fields

        /// <summary>
        ///   Command to add component to blueprint.
        /// </summary>
        public static ICommand AddComponentCommand = new RoutedCommand();

        /// <summary>
        ///   Command to remove component from blueprint.
        /// </summary>
        public static ICommand RemoveComponentCommand = new RoutedCommand();

        #endregion
    }
}