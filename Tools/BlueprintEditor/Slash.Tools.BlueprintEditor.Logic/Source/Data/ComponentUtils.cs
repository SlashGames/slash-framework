// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ComponentUtils.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Tools.BlueprintEditor.Logic.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    using Slash.GameBase.Inspector.Attributes;

    public static class ComponentUtils
    {
        #region Public Methods and Operators

        /// <summary>
        ///   Searches all loaded assemblies and returns the component types which have the LandscapeDesignerComponent attribute.
        /// </summary>
        /// <returns>List of found component types.</returns>
        public static IEnumerable<Type> FindComponentTypes(IEnumerable<Assembly> assemblies)
        {
            List<Type> componentTypes = new List<Type>();
            foreach (Assembly assembly in assemblies)
            {
                componentTypes.AddRange(
                    assembly.GetTypes()
                            .Where(type => Attribute.IsDefined(type, typeof(InspectorComponentAttribute))));
            }

            return componentTypes;
        }

        #endregion
    }
}