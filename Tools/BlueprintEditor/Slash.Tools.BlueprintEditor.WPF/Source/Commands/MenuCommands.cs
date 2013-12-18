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

        public static ICommand EditRedoCommand = new RoutedCommand();

        public static ICommand EditUndoCommand = new RoutedCommand();

        #endregion
    }
}