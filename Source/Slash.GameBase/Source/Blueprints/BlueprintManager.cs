// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BlueprintManager.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.GameBase.Blueprints
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    using Slash.Collections.Utils;

    /// <summary>
    ///   Manager that maps blueprint ids to blueprints.
    /// </summary>
    public class BlueprintManager : IBlueprintManager
    {
        #region Fields

        /// <summary>
        ///   All registered blueprints.
        /// </summary>
        private readonly Dictionary<string, Blueprint> blueprints = new Dictionary<string, Blueprint>();

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///   Default constructor.
        /// </summary>
        public BlueprintManager()
        {
        }

        /// <summary>
        ///   Copy constructor.
        /// </summary>
        /// <param name="blueprintManager">Object to copy.</param>
        public BlueprintManager(BlueprintManager blueprintManager)
        {
            if (blueprintManager.blueprints != null)
            {
                this.blueprints = new Dictionary<string, Blueprint>(blueprintManager.blueprints.Count);
                this.addBlueprints(blueprintManager);
            }
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///   All registered blueprints.
        /// </summary>
        public IEnumerable<KeyValuePair<string, Blueprint>> Blueprints
        {
            get
            {
                return this.blueprints;
            }
        }

        #endregion

        #region Public Methods and Operators

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }
            if (ReferenceEquals(this, obj))
            {
                return true;
            }
            if (obj.GetType() != this.GetType())
            {
                return false;
            }
            return this.Equals((BlueprintManager)obj);
        }

        /// <summary>
        ///   Gets an enumerator over all registered blueprints.
        /// </summary>
        /// <returns>All registered blueprints.</returns>
        public IEnumerator GetEnumerator()
        {
            return this.blueprints.GetEnumerator();
        }

        public override int GetHashCode()
        {
            return (this.blueprints != null ? this.blueprints.GetHashCode() : 0);
        }

        public override string ToString()
        {
            string s = this.blueprints.Aggregate(
                string.Empty,
                (current, keyValuePair) => current + string.Format("{0}: {1}\n", keyValuePair.Key, keyValuePair.Value));
            return string.Format("Blueprints: {0}", s);
        }

        /// <summary>
        ///   Adds the blueprint with the specified id to the manager.
        /// </summary>
        /// <param name="blueprintId">Blueprint id of new blueprint.</param>
        /// <param name="blueprint">Blueprint to add.</param>
        public void addBlueprint(string blueprintId, Blueprint blueprint)
        {
            if (blueprintId == null)
            {
                throw new ArgumentNullException("blueprintId", "No blueprint id provided.");
            }

            this.blueprints.Add(blueprintId, blueprint);
        }

        /// <summary>
        ///   Adds all blueprints of the passed manager to this one.
        /// </summary>
        /// <param name="blueprintManager">Manager to add all blueprints of.</param>
        public void addBlueprints(BlueprintManager blueprintManager)
        {
            foreach (KeyValuePair<string, Blueprint> blueprintPair in blueprintManager.blueprints)
            {
                this.addBlueprint(blueprintPair.Key, new Blueprint(blueprintPair.Value));
            }
        }

        public void clearBlueprints()
        {
            this.blueprints.Clear();
        }

        /// <summary>
        ///   Checks if the blueprint manager contains the blueprint with the specified id.
        /// </summary>
        /// <param name="blueprintId">Id of blueprint to search for.</param>
        /// <returns>True if the blueprint was found; otherwise, false.</returns>
        public bool containsBlueprint(string blueprintId)
        {
            return this.blueprints.ContainsKey(blueprintId);
        }

        /// <summary>
        ///   Searches for the blueprint with the specified id. Throws a KeyNotFoundException if not found.
        /// </summary>
        /// <param name="blueprintId">Id of blueprint to search for.</param>
        /// <returns>Blueprint with the specified id.</returns>
        /// <exception cref="KeyNotFoundException">Thrown if no blueprint with the specified id exists.</exception>
        public Blueprint getBlueprint(string blueprintId)
        {
            if (blueprintId == null)
            {
                throw new ArgumentNullException("blueprintId", "No blueprint id provided.");
            }

            try
            {
                return this.blueprints[blueprintId];
            }
            catch (KeyNotFoundException)
            {
                throw new KeyNotFoundException(string.Format("Blueprint with id '{0}' wasn't found.", blueprintId));
            }
        }

        /// <summary>
        ///   Removes the blueprint with the specified id. Returns if the blueprint was removed.
        /// </summary>
        /// <param name="blueprintId">Id of blueprint to search for.</param>
        /// <returns>True if the blueprint was removed; otherwise, false.</returns>
        public bool removeBlueprint(string blueprintId)
        {
            return this.blueprints.Remove(blueprintId);
        }

        /// <summary>
        ///   Searches for the blueprint with the specified id. Returns if the blueprint was found.
        /// </summary>
        /// <param name="blueprintId">Id of blueprint to search for.</param>
        /// <param name="blueprint">Parameter to write found blueprint to.</param>
        /// <returns>True if the blueprint was found; otherwise, false.</returns>
        public bool tryGetBlueprint(string blueprintId, out Blueprint blueprint)
        {
            if (blueprintId == null)
            {
                throw new ArgumentNullException("blueprintId", "No blueprint id provided.");
            }

            return this.blueprints.TryGetValue(blueprintId, out blueprint);
        }

        #endregion

        #region Explicit Interface Methods

        IEnumerator<Blueprint> IEnumerable<Blueprint>.GetEnumerator()
        {
            return this.blueprints.Values.GetEnumerator();
        }

        #endregion

        #region Methods

        protected bool Equals(BlueprintManager other)
        {
            return CollectionUtils.SequenceEqual(this.blueprints, other.blueprints);
        }

        #endregion
    }
}