// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LanguageFileToNameConverter.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace BlueprintEditor.Converters
{
    using System;
    using System.Globalization;
    using System.Windows.Data;

    using Slash.Tools.BlueprintEditor.Logic.Context;

    public class LanguageFileToNameConverter : IValueConverter
    {
        #region Public Methods and Operators

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            LanguageFile languageFile = (LanguageFile)value;
            return languageFile.Path;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}