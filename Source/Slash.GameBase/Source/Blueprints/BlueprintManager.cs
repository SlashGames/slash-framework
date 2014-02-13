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
    using System.Xml;
    using System.Xml.Schema;
    using System.Xml.Serialization;

    using Slash.Collections.Utils;
    using Slash.Diagnostics.Contracts;
    using Slash.Serialization.Xml;

    /// <summary>
    ///   Manager that maps blueprint ids to blueprints.
    /// </summary>
    [Serializable]
    public sealed class BlueprintManager : IBlueprintManager, IXmlSerializable
    {
        #region Fields

        /// <summary>
        ///   All registered blueprints.
        /// </summary>
        private readonly SerializableDictionary<string, Blueprint> blueprints =
            new SerializableDictionary<string, Blueprint>
                {
                    ItemElementName = "Entry",
                    KeyElementName = "Id",
                    ValueElementName = "Blueprint"
                };

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
                this.AddBlueprints(blueprintManager);
            }
        }

        #endregion

        #region Delegates

        /// <summary>
        ///   Delegate for BlueprintsChanged event.
        /// </summary>
        public delegate void BlueprintsChangedDelegate();

        #endregion

        #region Public Events

        /// <summary>
        ///   Raised when blueprints of this manager changed.
        /// </summary>
        public event BlueprintsChangedDelegate BlueprintsChanged;

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

        /// <summary>
        ///   Adds the blueprint with the specified id to the manager.
        /// </summary>
        /// <param name="blueprintId">Blueprint id of new blueprint.</param>
        /// <param name="blueprint">Blueprint to add.</param>
        /// <exception cref="ArgumentNullException">Thrown if no blueprint id or blueprint was provided.</exception>
        /// <exception cref="ArgumentException">Thrown if a blueprint with the specified id already exists.</exception>
        public void AddBlueprint(string blueprintId, Blueprint blueprint)
        {
            Contract.RequiresNotNull(new { blueprintId }, "No blueprint id provided.");
            Contract.Requires<ArgumentException>(blueprintId != string.Empty, "No blueprint id provided.");
            Contract.RequiresNotNull(new { blueprint }, "No blueprint provided.");
            Contract.Requires<ArgumentException>(
                !this.blueprints.ContainsKey(blueprintId), string.Format("A blueprint with this id already exists: {0}", blueprintId), "blueprintId");

            this.blueprints.Add(blueprintId, blueprint);
            this.OnBlueprintsChanged();
        }

        /// <summary>
        ///   Adds all blueprints of the passed manager to this one.
        /// </summary>
        /// <param name="blueprintManager">Manager to add all blueprints of.</param>
        public void AddBlueprints(BlueprintManager blueprintManager)
        {
            foreach (KeyValuePair<string, Blueprint> blueprintPair in blueprintManager.blueprints)
            {
                this.AddBlueprint(blueprintPair.Key, new Blueprint(blueprintPair.Value));
            }
            this.OnBlueprintsChanged();
        }

        /// <summary>
        ///   Changes the id under which a blueprint is stored.
        /// </summary>
        /// <param name="oldBlueprintId">Old blueprint id.</param>
        /// <param name="newBlueprintId">New blueprint id.</param>
        /// <exception cref="ArgumentException">Thrown if the old blueprint id doesn't exist in the manager.</exception>
        public void ChangeBlueprintId(string oldBlueprintId, string newBlueprintId)
        {
            // Check that old blueprint id exists.
            Blueprint blueprint;
            if (!this.blueprints.TryGetValue(oldBlueprintId, out blueprint))
            {
                throw new ArgumentException(
                    string.Format("Blueprint id '{0}' not found.", oldBlueprintId), "oldBlueprintId");
            }

            this.blueprints.Remove(oldBlueprintId);
            this.blueprints.Add(newBlueprintId, blueprint);

            this.OnBlueprintsChanged();
        }

        /// <summary>
        ///   Removes all blueprints from the manager.
        /// </summary>
        public void ClearBlueprints()
        {
            this.blueprints.Clear();
            this.OnBlueprintsChanged();
        }

        /// <summary>
        ///   Checks if the blueprint manager contains the blueprint with the specified id.
        /// </summary>
        /// <param name="blueprintId">Id of blueprint to search for.</param>
        /// <returns>True if the blueprint was found; otherwise, false.</returns>
        public bool ContainsBlueprint(string blueprintId)
        {
            return this.blueprints.ContainsKey(blueprintId);
        }

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
        ///   Searches for the blueprint with the specified id. Throws a KeyNotFoundException if not found.
        /// </summary>
        /// <param name="blueprintId">Id of blueprint to search for.</param>
        /// <returns>Blueprint with the specified id.</returns>
        /// <exception cref="KeyNotFoundException">Thrown if no blueprint with the specified id exists.</exception>
        public Blueprint GetBlueprint(string blueprintId)
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

        public XmlSchema GetSchema()
        {
            return null;
        }

        public void ReadXml(XmlReader reader)
        {
            this.blueprints.ReadXml(reader);
        }

        /// <summary>
        ///   Removes the blueprint with the specified id. Returns if the blueprint was removed.
        /// </summary>
        /// <param name="blueprintId">Id of blueprint to search for.</param>
        /// <returns>True if the blueprint was removed; otherwise, false.</returns>
        public bool RemoveBlueprint(string blueprintId)
        {
            if (!this.blueprints.Remove(blueprintId))
            {
                return false;
            }

            this.OnBlueprintsChanged();

            return true;
        }

        public override string ToString()
        {
            string s = this.blueprints.Aggregate(
                string.Empty,
                (current, keyValuePair) => current + string.Format("{0}: {1}\n", keyValuePair.Key, keyValuePair.Value));
            return string.Format("Blueprints: {0}", s);
        }

        /// <summary>
        ///   Searches for the blueprint with the specified id. Returns if the blueprint was found.
        /// </summary>
        /// <param name="blueprintId">Id of blueprint to search for.</param>
        /// <param name="blueprint">Parameter to write found blueprint to.</param>
        /// <returns>True if the blueprint was found; otherwise, false.</returns>
        public bool TryGetBlueprint(string blueprintId, out Blueprint blueprint)
        {
            if (blueprintId == null)
            {
                throw new ArgumentNullException("blueprintId", "No blueprint id provided.");
            }

            return this.blueprints.TryGetValue(blueprintId, out blueprint);
        }

        public void WriteXml(XmlWriter writer)
        {
            this.blueprints.WriteXml(writer);
        }

        #endregion

        #region Explicit Interface Methods

        IEnumerator<Blueprint> IEnumerable<Blueprint>.GetEnumerator()
        {
            return this.blueprints.Values.GetEnumerator();
        }

        #endregion

        #region Methods

        private bool Equals(BlueprintManager other)
        {
            return CollectionUtils.SequenceEqual(this.blueprints, other.blueprints);
        }

        private void OnBlueprintsChanged()
        {
            BlueprintsChangedDelegate handler = this.BlueprintsChanged;
            if (handler != null)
            {
                handler();
            }
        }

        #endregion
    }
}