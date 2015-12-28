// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ConversionUtils.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.SystemExt.Utils
{
    using System;

    public class ConversionUtils
    {
        #region Public Methods and Operators

        public static T Convert<T>(object value)
        {
            return (T)Convert(value, typeof(T));
        }

        public static object Convert(object value, Type type)
        {
            if (value == null)
            {
                return type.IsValueType ? Activator.CreateInstance(type) : null;
            }

            if (type.IsEnum)
            {
                return Enum.Parse(type, value.ToString());
            }

            return System.Convert.ChangeType(value, type);
        }

        #endregion
    }
}