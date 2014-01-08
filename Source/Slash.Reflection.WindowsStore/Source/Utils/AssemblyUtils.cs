// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AssemblyUtils.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Reflection.Utils
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;

    using Windows.ApplicationModel;
    using Windows.Foundation;
    using Windows.Storage;

    public class AssemblyUtils
    {
        #region Public Methods and Operators

        /// <summary>
        /// Gets all assemblies that are loaded in the current application domain.
        /// </summary>
        /// <returns>All loaded assemblies.</returns>
        public static IEnumerable<Assembly> GetLoadedAssemblies()
        {
            StorageFolder folder = Package.Current.InstalledLocation;

            List<Assembly> assemblies = new List<Assembly>();

            IAsyncOperation<IReadOnlyList<StorageFile>> folderFilesAsync = folder.GetFilesAsync();
            folderFilesAsync.AsTask().Wait();

            foreach (StorageFile file in folderFilesAsync.GetResults())
            {
                if (file.FileType == ".dll" || file.FileType == ".exe")
                {
                    AssemblyName name = new AssemblyName { Name = file.Name };
                    Assembly asm = Assembly.Load(name);
                    assemblies.Add(asm);
                }
            }

            return assemblies;
        }

        #endregion
    }
}