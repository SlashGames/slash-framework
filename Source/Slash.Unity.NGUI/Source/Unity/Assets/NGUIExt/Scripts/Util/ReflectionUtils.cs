// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ReflectionUtils.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Unity.NGUIExt.Util
{
    using System;
    using System.Reflection;

#if !UNITY_EDITOR && UNITY_WINRT
    using System.Collections.Generic;
    using System.Linq;
#endif

    public static class ReflectionUtils
    {
#if !UNITY_EDITOR && UNITY_WINRT
        public static bool IsValueType(Type type)
        {
            return type.GetTypeInfo().IsValueType;
        }

        private static IEnumerable<Type> GetBaseTypes(Type type)
        {
            yield return type;

            var baseType = type.GetTypeInfo().BaseType;

            if (baseType != null)
            {
                foreach (var t in GetBaseTypes(baseType))
                {
                    yield return t;
                }
            }
        }

        public static PropertyInfo GetProperty(Type type, string name)
        {
            return
                GetBaseTypes(type)
                    .Select(baseType => baseType.GetTypeInfo().GetDeclaredProperty(name))
                    .FirstOrDefault(property => property != null);
        }

        public static MethodInfo GetMethod(Type type, string name)
        {
            return
                GetBaseTypes(type)
                    .Select(baseType => baseType.GetTypeInfo().GetDeclaredMethod(name))
                    .FirstOrDefault(method => method != null);
        }

        public static FieldInfo GetField(Type type, string name)
        {
            return
                GetBaseTypes(type)
                    .Select(baseType => baseType.GetTypeInfo().GetDeclaredField(name))
                    .FirstOrDefault(field => field != null);
        }

        public static bool IsEnum(Type type)
        {
            return type.GetTypeInfo().IsEnum;
        }

        public static Delegate CreateDelegate(Type type, object target, MethodInfo method)
        {
            return method.CreateDelegate(type, target);
        }

        public static bool IsAssignableFrom(Type first, Type second)
        {
            return first.GetTypeInfo().IsAssignableFrom(second.GetTypeInfo());
        }
#else
        public static bool IsValueType(Type type)
        {
            return type.IsValueType;
        }

        public static PropertyInfo GetProperty(Type type, string name)
        {
            return type.GetProperty(name);
        }

        public static MethodInfo GetMethod(Type type, string name)
        {
            return type.GetMethod(name);
        }

        public static bool IsEnum(Type type)
        {
            return type.IsEnum;
        }

        public static FieldInfo GetField(Type type, string name)
        {
            return type.GetField(name);
        }

        public static Delegate CreateDelegate(Type type, object target, MethodInfo method)
        {
            return Delegate.CreateDelegate(type, target, method);
        }

        public static bool IsAssignableFrom(Type first, Type second)
        {
            return first.IsAssignableFrom(second);
        }
#endif
    }
}