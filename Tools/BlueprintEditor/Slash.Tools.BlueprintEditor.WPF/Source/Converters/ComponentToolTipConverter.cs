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

    using Slash.ECS.Inspector.Data;
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
            InspectorType componentInfo = InspectorComponentTable.Instance.GetInspectorType(componentType);
            return string.Format("{0}\n{1}", componentType.FullName, componentInfo.Description);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}