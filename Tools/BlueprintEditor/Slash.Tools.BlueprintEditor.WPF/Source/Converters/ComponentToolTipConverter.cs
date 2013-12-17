// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ComponentToolTipConverter.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace BlueprintEditor.Converters
{
    using System;
    using System.Globalization;
    using System.Windows.Data;

    using Slash.Tools.BlueprintEditor.Logic.Data;

    public class ComponentToolTipConverter : IValueConverter
    {
        #region Public Methods and Operators

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return null;
            }

            Type componentType = (Type)value;
            InspectorComponent componentInfo = InspectorComponentTable.GetComponent(componentType);
            return string.Format("{0}\n{1}", componentType.FullName, componentInfo.Component.Description);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}