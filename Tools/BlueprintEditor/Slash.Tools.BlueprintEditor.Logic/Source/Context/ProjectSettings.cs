// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ProjectSettings.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Tools.BlueprintEditor.Logic.Context
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Reflection;
    using System.Xml.Serialization;

    using Slash.GameBase.Blueprints;
    using Slash.Reflection.Utils;
    using Slash.Tools.BlueprintEditor.Logic.Data;

    /// <summary>
    ///   Blueprint file which belongs to a project.
    /// </summary>
    public sealed class BlueprintFile
    {
        #region Public Properties

        /// <summary>
        ///   Blueprint manager which contains the data.
        /// </summary>
        public BlueprintManager BlueprintManager { get; set; }

        /// <summary>
        ///   Path where the blueprint file is stored.
        /// </summary>
        public string Path { get; set; }

        #endregion
    }

    /// <summary>
    ///   Settings for the project the blueprints are edited for.
    /// </summary>
    [Serializable]
    public sealed class ProjectSettings
    {
        #region Fields

        /// <summary>
        ///   Available entity component types in the project.
        /// </summary>
        private IEnumerable<Type> entityComponentTypes;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///   Constructor.
        /// </summary>
        public ProjectSettings()
        {
            this.ProjectAssemblies = new List<Assembly>();
            this.BlueprintFiles = new List<BlueprintFile>();
        }

        #endregion

        #region Delegates

        public delegate void EntityComponentTypesChangedDelegate();

        #endregion

        #region Public Events

        public event EntityComponentTypesChangedDelegate EntityComponentTypesChanged;

        #endregion

        #region Public Properties

        [XmlIgnore]
        public IList<BlueprintFile> BlueprintFiles { get; set; }

        /// <summary>
        ///   Wrapper for ProjectAssemblies property for xml serialization.
        /// </summary>
        [XmlArray("BlueprintFiles", Order = 3)]
        [XmlArrayItem("BlueprintFile")]
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public string[] BlueprintFilesSerialized
        {
            get
            {
                return this.BlueprintFiles.Select(projectAssembly => projectAssembly.Path).ToArray();
            }
            set
            {
                this.BlueprintFiles = value.Select(path => new BlueprintFile { Path = path }).ToList();
            }
        }

        /// <summary>
        ///   Description of project.
        /// </summary>
        [XmlElement(Order = 1)]
        public string Description { get; set; }

        /// <summary>
        ///   Available entity component types in the project.
        /// </summary>
        public IEnumerable<Type> EntityComponentTypes
        {
            get
            {
                return this.entityComponentTypes ?? (this.entityComponentTypes = this.CollectEntityComponentTypes());
            }
        }

        /// <summary>
        ///   Project name.
        /// </summary>
        [XmlElement(Order = 0)]
        public string Name { get; set; }

        /// <summary>
        ///   Assemblies which belong to the project.
        /// </summary>
        [XmlIgnore]
        public IList<Assembly> ProjectAssemblies { get; private set; }

        /// <summary>
        ///   Wrapper for ProjectAssemblies property for xml serialization.
        /// </summary>
        [XmlArray("ProjectAssemblies", Order = 2)]
        [XmlArrayItem("ProjectAssembly")]
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public string[] ProjectAssembliesSerialized
        {
            get
            {
                return this.ProjectAssemblies.Select(projectAssembly => projectAssembly.CodeBase).ToArray();
            }
            set
            {
                this.ProjectAssemblies = value.Select(ReflectionUtils.FindAssembly).ToList();
                this.entityComponentTypes = null;
                this.OnEntityComponentTypesChanged();
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///   Adds an assembly to use to find available types.
        /// </summary>
        /// <param name="assembly">Assembly to add to the project.</param>
        public void AddAssembly(Assembly assembly)
        {
            // Check if assembly already exists in project.
            if (this.ProjectAssemblies.Contains(assembly))
            {
                return;
            }

            this.ProjectAssemblies.Add(assembly);
            this.entityComponentTypes = null;
            this.OnEntityComponentTypesChanged();
        }

        /// <summary>
        ///   Determines the used types from the specified assembly in the project.
        /// </summary>
        /// <param name="assembly">Assembly to check.</param>
        /// <returns>Enumeration of used types of the specified assembly.</returns>
        public IEnumerable<Type> FindUsedTypes(Assembly assembly)
        {
            // Check all blueprint managers.
            HashSet<Type> usedTypes = new HashSet<Type>();
            if (!this.ProjectAssemblies.Contains(assembly))
            {
                return usedTypes;
            }

            foreach (var blueprintFile in this.BlueprintFiles)
            {
                if (blueprintFile.BlueprintManager == null)
                {
                    continue;
                }

                foreach (KeyValuePair<string, Blueprint> blueprintPair in blueprintFile.BlueprintManager)
                {
                    Blueprint blueprint = blueprintPair.Value;

                    // Check component types.
                    foreach (Type componentType in
                        blueprint.ComponentTypes.Where(componentType => Equals(componentType.Assembly, assembly)))
                    {
                        usedTypes.Add(componentType);
                    }
                }
            }
            return usedTypes;
        }

        /// <summary>
        ///   Indicates if specified assembly is used in the project, i.e. any types of the assembly are
        ///   in use.
        /// </summary>
        /// <param name="assembly">Assembly to check.</param>
        /// <returns>True if the assembly is still used in the project; otherwise, false.</returns>
        public bool IsAssemblyUsed(Assembly assembly)
        {
            // Get component types used from this assembly.
            IEnumerable<Type> usedTypes = this.FindUsedTypes(assembly);
            return usedTypes.Any();
        }

        /// <summary>
        ///   Removes an assembly from the project.
        /// </summary>
        /// <param name="assembly">Assembly to remove.</param>
        public bool RemoveAssembly(Assembly assembly)
        {
            // Check if project contains assembly.
            if (!this.ProjectAssemblies.Contains(assembly))
            {
                return false;
            }

            // Check if still used.
            if (this.IsAssemblyUsed(assembly))
            {
                return false;
            }

            // Remove from project.
            this.ProjectAssemblies.Remove(assembly);

            this.entityComponentTypes = null;
            this.OnEntityComponentTypesChanged();

            return true;
        }

        #endregion

        #region Methods

        /// <summary>
        ///   Collects all entity component types from the project assemblies.
        /// </summary>
        /// <returns>All types which are inherited from IEntityComponent in the project assemblies.</returns>
        private IEnumerable<Type> CollectEntityComponentTypes()
        {
            return ComponentUtils.FindComponentTypes(this.ProjectAssemblies);
        }

        private void OnEntityComponentTypesChanged()
        {
            EntityComponentTypesChangedDelegate handler = this.EntityComponentTypesChanged;
            if (handler != null)
            {
                handler();
            }
        }

        #endregion
    }
}