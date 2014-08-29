// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TypeExtensions.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Reflection.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    public static class TypeExtensions
    {
#if WINDOWS_STORE
        public static PropertyInfo GetProperty(this Type type, string name)
        {
            return type.GetRuntimeProperty(name);
        }

        public static ConstructorInfo[] GetConstructors(this Type type)
        {
            return type.GetTypeInfo().DeclaredConstructors.ToArray();
        }

        public static bool IsGenericType(this Type type)
        {
            return type.GetTypeInfo().IsGenericType;
        }

        public static bool IsPrimitive(this Type type)
        {
            return type.GetTypeInfo().IsPrimitive;
        }

        public static bool IsEnum(this Type type)
        {
            return type.GetTypeInfo().IsEnum;
        }

        public static bool IsValueType(this Type type)
        {
            return type.GetTypeInfo().IsValueType;
        }

        public static IEnumerable<FieldInfo> GetInstanceFields(this Type type)
        {
            return type.GetRuntimeFields().Where(fieldInfo => !fieldInfo.IsStatic);
        }

        public static IEnumerable<PropertyInfo> GetInstanceProperties(this Type type)
        {
            return type.GetRuntimeProperties().Where(propertyInfo => !propertyInfo.GetMethod.IsStatic);
        }

        public static bool IsAttributeDefined<T>(this Type type) where T : Attribute
        {
            return type.GetTypeInfo().GetCustomAttribute<T>() != null;
        }

        public static MethodInfo GetGetMethod(this PropertyInfo propertyInfo)
        {
            return propertyInfo.GetMethod;
        }

        public static bool IsSealed(this Type type)
        {
            return type.GetTypeInfo().IsSealed;
        }

        public static Type[] GetGenericArguments(this Type type)
        {
            return type.GetTypeInfo().GenericTypeArguments;
        }

        public static MethodInfo GetMethod(this Type type, string methodName)
        {
            return type.GetTypeInfo().GetDeclaredMethod(methodName);
        }

        public static bool IsAssignableFrom(this Type type, Type other)
        {
            return type.GetTypeInfo().IsAssignableFrom(other.GetTypeInfo());
        }

#else
        public static bool IsValueType(this Type type)
        {
            return type.IsValueType;
        }

        public static bool IsGenericType(this Type type)
        {
            return type.IsValueType;
        }

        public static bool IsEnum(this Type type)
        {
            return type.IsEnum;
        }

         public static bool IsPrimitive(this Type type)
        {
            return type.IsPrimitive;
        }

                public static bool IsSealed(this Type type)
        {
            return type.IsSealed;
        }

        public static IEnumerable<FieldInfo> GetInstanceFields(this Type type)
        {
            return type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
        }

        public static IEnumerable<PropertyInfo> GetInstanceProperties(this Type type)
        {
            return type.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
        }

        public static bool IsAttributeDefined<T>(this Type type) where T : Attribute
        {
            return Attribute.IsDefined(type, typeof(T));
        }
#endif
    }
}