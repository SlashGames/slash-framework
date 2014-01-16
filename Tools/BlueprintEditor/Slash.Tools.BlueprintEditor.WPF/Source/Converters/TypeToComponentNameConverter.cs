// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TypeToComponentNameConverter.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace BlueprintEditor.Converters
{
    using System;
    using System.Globalization;
    using System.Windows.Data;

    using Slash.SystemExt.Utils;

    public class TypeToComponentNameConverter : IValueConverter
    {
        #region Public Methods and Operators

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var type = (Type)value;
            var typeName = type.Name;
            var componentName = typeName.Replace("Component", string.Empty);
            return componentName.SplitByCapitalLetters();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}