// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HierarchicalBlueprintManager.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.GameBase.Blueprints
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    using Slash.Diagnostics.Contracts;
    using Slash.GameBase.Inspector.Attributes;

    public class HierarchicalBlueprintManager : IBlueprintManager
    {
        #region Fields

        /// <summary>
        ///   Parent managers to consult if a key can't be found in this one.
        /// </summary>
        private readonly IList<IBlueprintManager> parents;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///   Constructs a new blueprint manager without any parents.
        /// </summary>
        public HierarchicalBlueprintManager()
        {
            this.parents = new List<IBlueprintManager>();
        }

        /// <summary>
        ///   Constructs a new blueprint manager with the specified parents.
        /// </summary>
        /// <param name="parents">Parent managers to consult if a key can't be found.</param>
        public HierarchicalBlueprintManager(params IBlueprintManager[] parents)
            : this()
        {
            foreach (IBlueprintManager parent in parents.Where(parent => parent != null))
            {
                this.AddParent(parent);
            }
        }

        #endregion

        #region Public Properties

        public IEnumerable<KeyValuePair<string, Blueprint>> Blueprints
        {
            get
            {
                return this.parents.SelectMany(parent => parent.Blueprints);
            }
        }

        public IEnumerable<IBlueprintManager> Parents
        {
            get
            {
                return this.parents;
            }
        }

        #endregion

        #region Public Methods and Operators

        public void AddBlueprint(string blueprintId, Blueprint blueprint)
        {
            Contract.RequiresNotNull(new { blueprintId }, "No blueprint id provided.");
            Contract.Requires<ArgumentException>(blueprintId != string.Empty, "No blueprint id provided.");
            Contract.RequiresNotNull(new { blueprint }, "No blueprint provided.");
            Contract.Requires<ArgumentException>(
                !this.ContainsBlueprint(blueprintId),
                string.Format("A blueprint with this id already exists: {0}", blueprintId),
                "blueprintId");

            // TODO(np): Specify which blueprint manager to add to.
            this.parents.FirstOrDefault().AddBlueprint(blueprintId, blueprint);
        }

        public void AddParent(IBlueprintManager parent)
        {
            this.parents.Add(parent);
        }

        public void ChangeBlueprintId(string oldBlueprintId, string newBlueprintId)
        {
            foreach (var parent in this.parents.Where(parent => parent.ContainsBlueprint(oldBlueprintId)))
            {
                parent.ChangeBlueprintId(oldBlueprintId, newBlueprintId);
                return;
            }

            throw new ArgumentException(
                string.Format("Blueprint id '{0}' not found.", oldBlueprintId), "oldBlueprintId");
        }

        /// <summary>
        ///   Checks if the blueprint manager contains the blueprint with the specified id.
        /// </summary>
        /// <param name="blueprintId">Id of blueprint to search for.</param>
        /// <returns>True if the blueprint was found; otherwise, false.</returns>
        public bool ContainsBlueprint(string blueprintId)
        {
            return this.parents.Any(parent => parent.ContainsBlueprint(blueprintId));
        }

        /// <summary>
        ///   Searches for the blueprint with the specified id. Throws a KeyNotFoundException if not found.
        /// </summary>
        /// <param name="blueprintId">Id of blueprint to search for.</param>
        /// <returns>Blueprint with the specified id.</returns>
        /// <exception cref="KeyNotFoundException">Thrown if no blueprint with the specified id exists.</exception>
        public Blueprint GetBlueprint(string blueprintId)
        {
            Blueprint blueprint = null;

            if (this.parents.Any(parent => parent.TryGetBlueprint(blueprintId, out blueprint)))
            {
                return blueprint;
            }

            throw new KeyNotFoundException(blueprintId);
        }

        /// <summary>
        ///   Gets the list of components of the specified blueprint and all of its parents.
        /// </summary>
        /// <param name="blueprint">Blueprint to get the inherited components of.</param>
        /// <returns>list of components of the specified blueprint and all of its parents.</returns>
        public List<Type> GetDerivedBlueprintComponents(Blueprint blueprint)
        {
            return this.GetDerivedBlueprintComponentsRecursively(blueprint, new List<Type>());
        }

        public IEnumerator<Blueprint> GetEnumerator()
        {
            return this.parents.SelectMany(parent => parent).GetEnumerator();
        }

        public bool RemoveBlueprint(string blueprintId)
        {
            return this.parents.Any(parent => parent.RemoveBlueprint(blueprintId));
        }

        /// <summary>
        ///   Searches for the blueprint with the specified id. Returns if the blueprint was found.
        /// </summary>
        /// <param name="blueprintId">Id of blueprint to search for.</param>
        /// <param name="blueprint">Parameter to write found blueprint to.</param>
        /// <returns>True if the blueprint was found; otherwise, false.</returns>
        public bool TryGetBlueprint(string blueprintId, out Blueprint blueprint)
        {
            foreach (IBlueprintManager parent in this.parents)
            {
                if (parent.TryGetBlueprint(blueprintId, out blueprint))
                {
                    return true;
                }
            }

            blueprint = null;
            return false;
        }

        /// <summary>
        ///   Overwrites the values of all localized string attributes of all blueprints with auto-generated values of the form BlueprintId.AttributeKey.
        /// </summary>
        public void UpdateLocalizationKeys()
        {
            foreach (var blueprintWithName in this.Blueprints)
            {
                var blueprintName = blueprintWithName.Key;
                var blueprint = blueprintWithName.Value;
                var blueprintComponents = this.GetDerivedBlueprintComponents(blueprint);

                foreach (var componentType in blueprintComponents)
                {
                    var properties = componentType.GetProperties();

                    foreach (var property in properties)
                    {
                        var stringAttribute =
                            (InspectorStringAttribute)
                            Attribute.GetCustomAttribute(property, typeof(InspectorStringAttribute));

                        if (stringAttribute != null && stringAttribute.Localized)
                        {
                            var attributeKey = stringAttribute.Name;
                            var attributeValue = string.Format("{0}.{1}", blueprintName, attributeKey);

                            blueprint.AttributeTable[attributeKey] = attributeValue;
                        }
                    }
                }
            }
        }

        #endregion

        #region Explicit Interface Methods

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        #endregion

        #region Methods

        private List<Type> GetDerivedBlueprintComponentsRecursively(Blueprint blueprint, List<Type> componentTypes)
        {
            componentTypes.AddRange(blueprint.ComponentTypes);

            if (!string.IsNullOrEmpty(blueprint.ParentId))
            {
                var parentBlueprint = this.GetBlueprint(blueprint.ParentId);
                this.GetDerivedBlueprintComponentsRecursively(parentBlueprint, componentTypes);
            }

            return componentTypes;
        }

        #endregion
    }
}