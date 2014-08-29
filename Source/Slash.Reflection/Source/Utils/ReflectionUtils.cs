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
            List<Type> types = new List<Type>();
            Type baseType = typeof(T);
            foreach (Assembly assembly in AssemblyUtils.GetLoadedAssemblies())
            {
#if WINDOWS_STORE
                types.AddRange(
                    assembly.DefinedTypes.Where(baseType.GetTypeInfo().IsAssignableFrom)
                            .Select(typeInfo => typeInfo.AsType()));
#else
                types.AddRange(assembly.GetTypes().Where(baseType.IsAssignableFrom));
#endif
            }

            return types;
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

        #endregion
    }
}