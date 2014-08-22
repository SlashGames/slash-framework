// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IBlueprintManager.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.ECS.Blueprints
{
    using System.Collections.Generic;

    public interface IBlueprintManager : IEnumerable<Blueprint>
    {
        #region Public Properties

        /// <summary>
        ///   All registered blueprints.
        /// </summary>
        IEnumerable<KeyValuePair<string, Blueprint>> Blueprints { get; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///   Adds the blueprint with the specified id to the manager.
        /// </summary>
        /// <param name="blueprintId">Blueprint id of new blueprint.</param>
        /// <param name="blueprint">Blueprint to add.</param>
        void AddBlueprint(string blueprintId, Blueprint blueprint);


        /// <summary>
        ///   Changes the id under which a blueprint is stored.
        /// </summary>
        /// <param name="oldBlueprintId">Old blueprint id.</param>
        /// <param name="newBlueprintId">New blueprint id.</param>
        void ChangeBlueprintId(string oldBlueprintId, string newBlueprintId);

        /// <summary>
        ///   Checks if the blueprint manager contains the blueprint with the specified id.
        /// </summary>
        /// <param name="blueprintId">Id of blueprint to search for.</param>
        /// <returns>True if the blueprint was found; otherwise, false.</returns>
        bool ContainsBlueprint(string blueprintId);

        /// <summary>
        ///   Searches for the blueprint with the specified id. Throws a KeyNotFoundException if not found.
        /// </summary>
        /// <param name="blueprintId">Id of blueprint to search for.</param>
        /// <returns>Blueprint with the specified id.</returns>
        /// <exception cref="KeyNotFoundException">Thrown if no blueprint with the specified id exists.</exception>
        Blueprint GetBlueprint(string blueprintId);

        /// <summary>
        ///   Removes the blueprint with the specified id. Returns if the blueprint was removed.
        /// </summary>
        /// <param name="blueprintId">Id of blueprint to search for.</param>
        /// <returns>True if the blueprint was removed; otherwise, false.</returns>
        bool RemoveBlueprint(string blueprintId);

        /// <summary>
        ///   Searches for the blueprint with the specified id. Returns if the blueprint was found.
        /// </summary>
        /// <param name="blueprintId">Id of blueprint to search for.</param>
        /// <param name="blueprint">Parameter to write found blueprint to.</param>
        /// <returns>True if the blueprint was found; otherwise, false.</returns>
        bool TryGetBlueprint(string blueprintId, out Blueprint blueprint);

        #endregion
    }
}