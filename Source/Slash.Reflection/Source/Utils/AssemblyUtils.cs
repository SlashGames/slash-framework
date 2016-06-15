// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AssemblyUtils.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Reflection.Utils
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;

#if WINDOWS_STORE
    using Windows.ApplicationModel;
    using Windows.Foundation;
    using Windows.Storage;
#endif

    /// <summary>
    ///   Utility methods for operating on assemblies.
    /// </summary>
    public class AssemblyUtils
    {
#if WINDOWS_STORE
        #region Static Fields

        /// <summary>
        ///   Cached list of loaded assemblies.
        /// </summary>
        private static List<Assembly> loadedAssemblies;

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///   Gets all assemblies that are loaded in the current application domain.
        /// </summary>
        /// <returns>All loaded assemblies.</returns>
        public static IEnumerable<Assembly> GetLoadedAssemblies()
        {
            // Check if cached.
            if (loadedAssemblies != null)
            {
                return loadedAssemblies;
            }

            // Find assemblies.
            StorageFolder folder = Package.Current.InstalledLocation;

            loadedAssemblies = new List<Assembly>();

            IAsyncOperation<IReadOnlyList<StorageFile>> folderFilesAsync = folder.GetFilesAsync();
            folderFilesAsync.AsTask().Wait();

            foreach (StorageFile file in folderFilesAsync.GetResults())
            {
                if (file.FileType == ".dll" || file.FileType == ".exe")
                {
                    try
                    {
                        var filename = file.Name.Substring(0, file.Name.Length - file.FileType.Length);
                        AssemblyName name = new AssemblyName { Name = filename };
                        Assembly asm = Assembly.Load(name);
                        loadedAssemblies.Add(asm);
                    }
                    catch (BadImageFormatException)
                    {
                        /*
                         * TODO(np): Thrown reflecting on C++ executable files for which the C++ compiler
                         * stripped the relocation addresses (such as Unity dlls):
                         * http://msdn.microsoft.com/en-us/library/x4cw969y(v=vs.110).aspx
                         */
                    }
                }
            }

            return loadedAssemblies;
        }

        #endregion
#endif

#if !WINDOWS_PHONE

        public static void CheckReferencedAssembliesAreLoaded()
        {
            var loadedAssemblies = GetLoadedAssemblies().ToList();
            foreach (var loadedAssembly in loadedAssemblies)
            {
                CheckReferencedAssembliesAreLoaded(loadedAssembly, new List<Assembly>(loadedAssemblies));
            }
        }

        private static void CheckReferencedAssembliesAreLoaded(Assembly assembly, IList<Assembly> loadedAssemblies)
        {
            var loadedAssemblyNames = loadedAssemblies.Select(a => a.FullName).ToArray();

            var referencedAssemblies = assembly.GetReferencedAssemblies();
            var assemblyNamesToLoad =
                referencedAssemblies.Where(
                    referencedAssembly => !loadedAssemblyNames.Contains(referencedAssembly.FullName));
            foreach (var assemblyName in assemblyNamesToLoad)
            {
                try
                {
                    var loadedAssembly = AppDomain.CurrentDomain.Load(assemblyName);

                    // Check if really not loaded already, might just be another version.
                    if (!loadedAssemblies.Contains(loadedAssembly))
                    {
                        loadedAssemblies.Add(loadedAssembly);

                        // Do recursive for loaded assembly.
                        CheckReferencedAssembliesAreLoaded(loadedAssembly, loadedAssemblies);
                    }
                }
                catch (FileNotFoundException)
                {
                    // NOTE(co): Okay or not?
                }
                catch (FileLoadException)
                {
                    // NOTE(co): Okay or not?
                }
            }
        }
#endif

        /// <summary>
        ///   Gets all assemblies that are loaded in the current application domain.
        /// </summary>
        /// <returns>All loaded assemblies.</returns>
        public static IEnumerable<Assembly> GetLoadedAssemblies()
        {
            return AppDomain.CurrentDomain.GetAssemblies();
        }
    }
}