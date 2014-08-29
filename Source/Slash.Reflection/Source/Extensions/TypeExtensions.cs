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
            return
                GetBaseTypes(type)
                    .Select(baseType => baseType.GetTypeInfo().GetDeclaredProperty(name))
                    .FirstOrDefault(property => property != null);
        }

        public static ConstructorInfo[] GetConstructors(this Type type)
        {
            return type.GetTypeInfo().DeclaredConstructors.ToArray();
        }

        public static bool IsValueType(this Type type)
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
#else
        public static bool IsValueType(this Type type)
        {
            return type.IsValueType;
        }
#endif
    }
}