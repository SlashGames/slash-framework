// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PropertyExtensions.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Reflection.Extensions
{
    using System.Reflection;

#if !WINDOWS_STORE
    using System;
#endif

    public static class PropertyExtensions
    {
#if WINDOWS_STORE
        public static MethodInfo GetGetMethod(this PropertyInfo propertyInfo)
        {
            return propertyInfo.GetMethod;
        }
#else
        public static T GetCustomAttribute<T>(this PropertyInfo property) where T : Attribute
        {
            return (T)Attribute.GetCustomAttribute(property, typeof(T));
        }
#endif
    }
}