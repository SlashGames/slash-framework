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

    /// <summary>
    ///   Utility methods for operating on assemblies.
    /// </summary>
    public class AssemblyUtils
    {
        #region Public Methods and Operators

        /// <summary>
        ///   Gets all assemblies that are loaded in the current application domain.
        /// </summary>
        /// <returns>All loaded assemblies.</returns>
        public static IEnumerable<Assembly> GetLoadedAssemblies()
        {
            return AppDomain.CurrentDomain.GetAssemblies();
        }

        #endregion
    }
}