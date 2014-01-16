// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ListToStringConverter.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace BlueprintEditor.Converters
{
    using System;
    using System.Globalization;
    using System.Windows.Data;

    public class ListToStringConverter : IValueConverter
    {
        #region Public Methods and Operators

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string str = (string)value;
            return string.IsNullOrWhiteSpace(str) ? "(none)" : str;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}