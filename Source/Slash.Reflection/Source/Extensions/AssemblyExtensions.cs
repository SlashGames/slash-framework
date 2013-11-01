// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AssemblyExtensions.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Reflection
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;

    /// <summary>
    ///   Extension methods to execute via an instance of an Assembly.
    /// </summary>
    public static class AssemblyExtensions
    {
        #region Public Methods and Operators

        /// <summary>
        ///   Returns all available assemblies for the specified assembly which match the given <paramref name="assemblyFilter" />.
        ///   Searches through the assembly directory.
        /// </summary>
        /// <param name="assembly"> Assembly to get available assemblies for. </param>
        /// <param name="assemblyFilter"> The filter which should be satisfied to consider an Assembly for the returned set of Types, if applicable. </param>
        /// <returns>
        ///   Any available assemblies for the specified assembly which match the <paramref name="assemblyFilter" /> .
        /// </returns>
        public static IEnumerable<Assembly> GetAvailableAssemblies(
            this Assembly assembly, Func<Assembly, bool> assemblyFilter = null)
        {
            string assemblyDirectory = Path.GetDirectoryName(assembly.Location);

            IEnumerable<string> assemblyFiles = GetAvailableAssemblies(assembly, assemblyDirectory);
            List<Assembly> availableAssemblies = new List<Assembly>();
            foreach (string assemblyFile in assemblyFiles)
            {
                Assembly availableAssembly = Assembly.LoadFrom(assemblyFile);
                if (assemblyFilter == null || assemblyFilter(availableAssembly))
                {
                    availableAssemblies.Add(availableAssembly);
                }
            }

            return availableAssemblies;
        }

        /// <summary>
        ///   Returns all the Type objects with match the given <paramref name="typeFilter" />
        ///   from the executing assembly and any assemblies in its or lower directories.
        /// </summary>
        /// <param name="assembly"> The Assembly on which the method is called. </param>
        /// <param name="assemblyFilter"> The filter which should be satisfied to consider an Assembly for the returned set of Types, if applicable. </param>
        /// <param name="typeFilter"> The filter which should be satisfied to include the Type in the returned set of Types, if applicable. </param>
        /// <returns>
        ///   Any Types which match the <paramref name="typeFilter" /> .
        /// </returns>
        public static IEnumerable<Type> GetAvailableTypes(
            this Assembly assembly, Func<Assembly, bool> assemblyFilter = null, Func<Type, bool> typeFilter = null)
        {
            IEnumerable<Assembly> availableAssemblies = GetAvailableAssemblies(assembly, assemblyFilter);
            List<Type> matchingTypes = new List<Type>();
            foreach (Assembly availableAssembly in availableAssemblies)
            {
                IEnumerable<Type> matchingTypesFromThisAssembly = availableAssembly.GetTypes();

                if (typeFilter != null)
                {
                    matchingTypesFromThisAssembly = matchingTypesFromThisAssembly.Where(typeFilter).ToArray();
                }

                matchingTypes.AddRange(matchingTypesFromThisAssembly);
            }

            Type[] distinctMatchingTypes = matchingTypes.Distinct().OrderBy(t => t.Name).ToArray();
            return distinctMatchingTypes;
        }

        #endregion

        #region Methods

        private static IEnumerable<string> GetAssembliesWithinDirectory(string directory)
        {
            return Directory.GetFiles(directory, "*.dll", SearchOption.TopDirectoryOnly);
        }

        private static IEnumerable<string> GetAvailableAssemblies(Assembly assembly, string assemblyDirectory)
        {
            IEnumerable<string> availableAssemblies = GetAssembliesWithinDirectory(assemblyDirectory);

            IEnumerable<string> assemblies = availableAssemblies as List<string> ?? availableAssemblies.ToList();
            if (assemblies.Count() > 1)
            {
                return assemblies;
            }

            // The currently-executing assembly is the only one it its
            // directory; this happens in deployment scenarios where
            // each assembly is compiled into a separate folder at runtime.
            // We therefore go back to the original deployment directory
            // and load all the assemblies in there:
            Uri assemblyCodeBaseUri = new Uri(assembly.CodeBase);

            string assemblyCodeBaseDirectory = Path.GetDirectoryName(assemblyCodeBaseUri.LocalPath);

            return GetAssembliesWithinDirectory(assemblyCodeBaseDirectory);
        }

        #endregion
    }
}