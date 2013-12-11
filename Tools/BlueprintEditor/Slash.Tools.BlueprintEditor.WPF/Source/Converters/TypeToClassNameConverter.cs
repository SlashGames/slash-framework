// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TypeToClassNameConverter.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace BlueprintEditor.Converters
{
    using System;
    using System.Globalization;
    using System.Windows.Data;

    public class TypeToClassNameConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ((Type)value).Name;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}