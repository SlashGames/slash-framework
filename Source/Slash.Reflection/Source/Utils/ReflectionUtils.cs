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
            return FindType(fullName, AppDomain.CurrentDomain);
        }

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
        /// <param name="domain">App domain to search for the type.</param>
        /// <returns>Type with the specified name.</returns>
        /// <exception cref="TypeLoadException">If the type couldn't be found.</exception>
        public static Type FindType(string fullName, AppDomain domain)
        {
            if (string.IsNullOrEmpty(fullName))
            {
                return null;
            }

            // Split type name from .dll version.
            if (!fullName.Contains("["))
            {
                fullName = fullName.Split(new[] { ',' })[0];
            }

            Type t = Type.GetType(fullName);

            if (t != null)
            {
                return t;
            }

            foreach (Assembly asm in domain.GetAssemblies())
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
        /// <param name="domain">App domain to search for the types.</param>
        /// <returns>List of found types.</returns>
        public static IEnumerable<Type> FindTypesWithAttribute<T>(AppDomain domain = null) where T : Attribute
        {
            // Use current domain if domain not specified.
            if (domain == null)
            {
                domain = AppDomain.CurrentDomain;
            }

            List<Type> types = new List<Type>();
            foreach (Assembly assembly in domain.GetAssemblies())
            {
                types.AddRange(assembly.GetTypes().Where(type => Attribute.IsDefined(type, typeof(T))));
            }

            return types;
        }

        /// <summary>
        ///   Searches all loaded assemblies and returns the types which have the specified attribute
        ///   and executes the specified action for those.
        /// </summary>
        /// <param name="action">Action to execute for found types and their attribute.</param>
        /// <param name="domain">App domain to search for the types.</param>
        public static void HandleTypesWithAttribute<T>(Action<Type, T> action, AppDomain domain = null)
            where T : Attribute
        {
            // Use current domain if domain not specified.
            if (domain == null)
            {
                domain = AppDomain.CurrentDomain;
            }

            foreach (Assembly assembly in domain.GetAssemblies())
            {
                foreach (Type type in assembly.GetTypes())
                {
                    T attribute = (T)Attribute.GetCustomAttribute(type, typeof(T));
                    if (attribute != null)
                    {
                        action(type, attribute);
                    }
                }
            }
        }

        #endregion
    }
}