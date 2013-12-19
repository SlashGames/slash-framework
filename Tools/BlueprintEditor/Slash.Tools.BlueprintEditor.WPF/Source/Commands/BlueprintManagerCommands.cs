// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BlueprintManagerCommands.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace BlueprintEditor.Commands
{
    using System.Windows.Input;

    public static class BlueprintManagerCommands
    {
        #region Static Fields

        public static ICommand NewBlueprintCommand = new RoutedCommand();

        public static ICommand RemoveBlueprintCommand = new RoutedCommand();

        #endregion
    }
}