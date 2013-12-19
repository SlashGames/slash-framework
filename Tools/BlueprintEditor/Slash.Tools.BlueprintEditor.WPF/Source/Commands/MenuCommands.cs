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
        ///   Command to exit the application.
        /// </summary>
        public static ICommand Exit = new RoutedCommand("Exit", typeof(MenuCommands));

        #endregion
    }
}