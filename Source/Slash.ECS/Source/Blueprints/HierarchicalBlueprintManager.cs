// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HierarchicalBlueprintManager.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.ECS.Blueprints
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    using Slash.ECS.Inspector.Attributes;

    /// <summary>
    ///   Blueprint manager that consults children for looking up blueprints.
    /// </summary>
    public class HierarchicalBlueprintManager : IBlueprintManager
    {
        #region Fields

        /// <summary>
        ///   Child managers to consult if a key can't be found in this one.
        /// </summary>
        private readonly IList<IBlueprintManager> children;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///   Constructs a new blueprint manager without any children.
        /// </summary>
        public HierarchicalBlueprintManager()
        {
            this.children = new List<IBlueprintManager>();
        }

        /// <summary>
        ///   Constructs a new blueprint manager with the specified children.
        /// </summary>
        /// <param name="children">Child managers to consult if a key can't be found.</param>
        public HierarchicalBlueprintManager(params IBlueprintManager[] children)
            : this()
        {
            foreach (IBlueprintManager child in children.Where(child => child != null))
            {
                this.AddChild(child);
            }
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///   All registered blueprints of all child blueprint managers.
        /// </summary>
        public IEnumerable<KeyValuePair<string, Blueprint>> Blueprints
        {
            get
            {
                return this.children.SelectMany(child => child.Blueprints);
            }
        }

        /// <summary>
        ///   Child blueprint managers consulted when looking up blueprints.
        /// </summary>
        public IEnumerable<IBlueprintManager> Children
        {
            get
            {
                return this.children;
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///   Adds the blueprint with the specified id to the first child blueprint manager.
        /// </summary>
        /// <param name="blueprintId">Id of the new blueprint to add.</param>
        /// <param name="blueprint">New blueprint to add.</param>
        public void AddBlueprint(string blueprintId, Blueprint blueprint)
        {
            var child = this.children.FirstOrDefault();

            if (child == null)
            {
                throw new InvalidOperationException("No child blueprint manager to add blueprint to!");
            }

            // TODO(np): Specify which blueprint manager to add to.
            child.AddBlueprint(blueprintId, blueprint);
        }

        /// <summary>
        ///   Adds the passed blueprint manager to be consulted for future
        ///   blueprint lookups.
        /// </summary>
        /// <param name="child">New blueprint manager child to add.</param>
        public void AddChild(IBlueprintManager child)
        {
            this.children.Add(child);
        }

        /// <summary>
        ///   Changes the id under which a blueprint is stored.
        /// </summary>
        /// <param name="oldBlueprintId">Old blueprint id.</param>
        /// <param name="newBlueprintId">New blueprint id.</param>
        public void ChangeBlueprintId(string oldBlueprintId, string newBlueprintId)
        {
            foreach (var child in this.children.Where(child => child.ContainsBlueprint(oldBlueprintId)))
            {
                child.ChangeBlueprintId(oldBlueprintId, newBlueprintId);
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
            return this.children.Any(child => child.ContainsBlueprint(blueprintId));
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

            if (this.children.Any(child => child.TryGetBlueprint(blueprintId, out blueprint)))
            {
                return blueprint;
            }

            throw new KeyNotFoundException(blueprintId);
        }

        /// <summary>
        ///   Gets the list of components of the specified blueprint and all of its children.
        /// </summary>
        /// <param name="blueprint">Blueprint to get the inherited components of.</param>
        /// <returns>list of components of the specified blueprint and all of its children.</returns>
        public List<Type> GetDerivedBlueprintComponents(Blueprint blueprint)
        {
            return this.GetDerivedBlueprintComponentsRecursively(blueprint, new List<Type>());
        }

        /// <summary>
        ///   Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        ///   A <see cref="T:System.Collections.Generic.IEnumerator`1" /> that can be used to iterate through the collection.
        /// </returns>
        /// <filterpriority>1</filterpriority>
        public IEnumerator<Blueprint> GetEnumerator()
        {
            return this.children.SelectMany(child => child).GetEnumerator();
        }

        /// <summary>
        ///   Removes the blueprint with the specified id. Returns whether the blueprint was removed.
        /// </summary>
        /// <param name="blueprintId">Id of blueprint to search for.</param>
        /// <returns>True if the blueprint was removed; otherwise, false.</returns>
        public bool RemoveBlueprint(string blueprintId)
        {
            return this.children.Any(child => child.RemoveBlueprint(blueprintId));
        }

        /// <summary>
        ///   Searches for the blueprint with the specified id. Returns if the blueprint was found.
        /// </summary>
        /// <param name="blueprintId">Id of blueprint to search for.</param>
        /// <param name="blueprint">Parameter to write found blueprint to.</param>
        /// <returns>True if the blueprint was found; otherwise, false.</returns>
        public bool TryGetBlueprint(string blueprintId, out Blueprint blueprint)
        {
            foreach (IBlueprintManager child in this.children)
            {
                if (child.TryGetBlueprint(blueprintId, out blueprint))
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