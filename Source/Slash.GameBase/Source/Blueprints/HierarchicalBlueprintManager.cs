// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HierarchicalBlueprintManager.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.GameBase.Blueprints
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

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
                this.parents.Add(parent);
            }
        }

        #endregion

        #region Public Methods and Operators

        public IEnumerator<Blueprint> GetEnumerator()
        {
            return this.parents.SelectMany(parent => parent).GetEnumerator();
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

        #endregion

        #region Explicit Interface Methods

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        #endregion
    }
}