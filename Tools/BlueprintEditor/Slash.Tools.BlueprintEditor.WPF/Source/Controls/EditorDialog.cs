﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EditorDialog.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace BlueprintEditor.Controls
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Windows;

    /// <summary>
    ///   Provides consistent error and warning handling.
    /// </summary>
    public static class EditorDialog
    {
        #region Public Methods and Operators

        /// <summary>
        ///   Shows an error message with the specified title and text.
        /// </summary>
        /// <param name="title">Title of the error message.</param>
        /// <param name="text">Text of the error message.</param>
        public static void Error(string title, string text)
        {
            MessageBox.Show(text, title, MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK);
        }

        /// <summary>
        ///   Shows an info dialog with the specified title and text.
        /// </summary>
        /// <param name="title">Title of the info.</param>
        /// <param name="text">Text of the info.</param>
        public static void Info(string title, string text)
        {
            MessageBox.Show(text, title, MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.OK);
        }

        /// <summary>
        ///   Shows a warning with the specified title and text.
        /// </summary>
        /// <param name="title">Title of the warning.</param>
        /// <param name="text">Text of the warning.</param>
        public static void Warning(string title, string text)
        {
            MessageBox.Show(text, title, MessageBoxButton.OK, MessageBoxImage.Warning, MessageBoxResult.OK);
        }

        /// <summary>
        ///   Shows the messages of the passed exceptions as aggregated warning with the specified title.
        /// </summary>
        /// <param name="title">Title of the warning to show.</param>
        /// <param name="exceptions">Exceptions to include in the warning.</param>
        public static void Warning(string title, IEnumerable<Exception> exceptions)
        {
            var stringBuilder = new StringBuilder();

            foreach (var exception in exceptions)
            {
                stringBuilder.AppendLine(exception.Message);
            }

            Warning(title, stringBuilder.ToString());
        }

        #endregion
    }
}