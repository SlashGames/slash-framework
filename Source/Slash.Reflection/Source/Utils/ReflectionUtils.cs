// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ReflectionUtils.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Reflection.Utils
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    using Slash.SystemExt.Utils;

    /// <summary>
    ///   Provides utility methods for reflecting types and members.
    /// </summary>
    public class ReflectionUtils
    {
        #region Public Methods and Operators

#if !WINDOWS_STORE
        /// <summary>
        ///   Loads an assembly from the specified file.
        /// </summary>
        /// <param name="assemblyFile">Path to assembly file.</param>
        /// <returns>Loaded assembly from specified path.</returns>
        public static Assembly FindAssembly(string assemblyFile)
        {
            return Assembly.LoadFrom(assemblyFile);
        }
#endif

        /// <summary>
        ///   <para>
        ///     Looks up the specified full type name in all loaded assemblies,
        ///     ignoring assembly version.
        ///   </para>
        ///   <para>
        ///     In order to understand how to access generic types,
        ///     see http://msdn.microsoft.com/en-us/library/w3f99sx1.aspx.
        ///   </para>
        /// </summary>
        /// <param name="fullName">Full name of the type to find.</param>
        /// <returns>Type with the specified name.</returns>
        /// <exception cref="TypeLoadException">If the type couldn't be found.</exception>
        public static Type FindType(string fullName)
        {
            if (string.IsNullOrEmpty(fullName))
            {
                return null;
            }

            // Split type name from .dll version.
            fullName = SystemExtensions.RemoveAssemblyInfo(fullName);

            Type t = Type.GetType(fullName);

            if (t != null)
            {
                return t;
            }

            foreach (Assembly asm in AssemblyUtils.GetLoadedAssemblies())
            {
                t = asm.GetType(fullName);
                if (t != null)
                {
                    return t;
                }
            }

            throw new TypeLoadException(string.Format("Unable to find type {0}.", fullName));
        }

#if WINDOWS_STORE
        public static IEnumerable<Type> FindTypes(Func<TypeInfo, bool> condition)
        {
            List<Type> types = new List<Type>();
            foreach (Assembly assembly in AssemblyUtils.GetLoadedAssemblies())
            {

                types.AddRange(assembly.DefinedTypes.Where(condition).Select(typeInfo => typeInfo.AsType()));
            }

            return types;
        }
#else
        public static IEnumerable<Type> FindTypes(Func<Type, bool> condition)
        {
            List<Type> types = new List<Type>();
            foreach (Assembly assembly in AssemblyUtils.GetLoadedAssemblies())
            {
                try
                {
                    types.AddRange(assembly.GetTypes().Where(condition));
                }
                catch (Exception)
                {
                }
            }

            return types;
        }
#endif
        /// <summary>
        ///   Searches all loaded assemblies and returns the types which have the specified attribute.
        /// </summary>
        /// <returns>List of found types.</returns>
        /// <typeparam name="T">Type of the attribute to get the types of.</typeparam>
        public static IEnumerable<Type> FindTypesWithAttribute<T>() where T : Attribute
        {
            List<Type> types = new List<Type>();
            foreach (Assembly assembly in AssemblyUtils.GetLoadedAssemblies())
            {
#if WINDOWS_STORE
                types.AddRange(
                    assembly.DefinedTypes.Where(typeInfo => typeInfo.GetCustomAttribute<T>() != null)
                            .Select(typeInfo => typeInfo.AsType()));
#else
                types.AddRange(assembly.GetTypes().Where(type => Attribute.IsDefined(type, typeof(T))));
#endif
            }

            return types;
        }

        /// <summary>
        ///   Searches all loaded assemblies and returns the types which have the specified attribute.
        /// </summary>
        /// <returns>List of found types.</returns>
        /// <typeparam name="T">Type of the attribute to get the types of.</typeparam>
        public static IEnumerable<Type> FindTypesWithBase<T>() where T : class
        {
            return FindTypesWithBase(typeof(T));
        }

        /// <summary>
        ///   Searches all loaded assemblies and returns the types which have the specified attribute.
        /// </summary>
        /// <param name="baseType">Type of the attribute to get the types of.</param>
        /// <returns>List of found types.</returns>
        public static IEnumerable<Type> FindTypesWithBase(Type baseType)
        {
            List<Type> types = new List<Type>();
#if WINDOWS_STORE
            return FindTypes(baseType.GetTypeInfo().IsAssignableFrom);
#else
            return FindTypes(baseType.IsAssignableFrom);
#endif
            }

        /// <summary>
        ///   Searches all loaded assemblies and returns the types which have the specified attribute
        ///   and executes the specified action for those.
        /// </summary>
        /// <param name="action">Action to execute for found types and their attribute.</param>
        /// <typeparam name="T">Type of the attribute to get the types of.</typeparam>
        public static void HandleTypesWithAttribute<T>(Action<Type, T> action) where T : Attribute
        {
            foreach (Assembly assembly in AssemblyUtils.GetLoadedAssemblies())
            {
#if WINDOWS_STORE
                foreach (var typeInfo in assembly.DefinedTypes)
                {
                    T attribute = typeInfo.GetCustomAttribute<T>();
                    if (attribute != null)
                    {
                        action(typeInfo.AsType(), attribute);
                    }
                }
#else
                foreach (Type type in assembly.GetTypes())
                {
                    T attribute = (T)Attribute.GetCustomAttribute(type, typeof(T));
                    if (attribute != null)
                    {
                        action(type, attribute);
                    }
                }
#endif
            }
        }

#if WINDOWS_STORE
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

        public static object[] GetAttributes(Type type, Type attributeType, bool inherit)
        {
            return type.GetTypeInfo().GetCustomAttributes(type, inherit).ToArray();
        }

        public static T GetAttribute<T>(MemberInfo member) where T : Attribute
        {
            return member.GetCustomAttribute<T>();
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

        public static object[] GetAttributes(Type type, Type attributeType, bool inherit)
        {
            return type.GetCustomAttributes(type, inherit);
        }

        public static T GetAttribute<T>(MemberInfo member) where T : Attribute
        {
            return (T)Attribute.GetCustomAttribute(member, typeof(T));
        }
#endif

        #endregion
    }
}