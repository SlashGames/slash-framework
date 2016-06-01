// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GameBlueprintUtils.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Application.Games
{
    using System.Collections.Generic;
    using System.Linq;

    using Slash.Collections.AttributeTables;
    using Slash.ECS.Blueprints;
    using Slash.ECS.Blueprints.Extensions;
    using Slash.ECS.Components;

    /// <summary>
    ///   Utility methods for all issues related to blueprints of a game (e.g. entity creation,...).
    /// </summary>
    public static class GameBlueprintUtils
    {
        #region Public Methods and Operators

        /// <summary>
        ///   Creates entities for all blueprints with the specified ids.
        /// </summary>
        /// <param name="entityManager">Entity manager to create entities in.</param>
        /// <param name="blueprintManager">Blueprint manager to get blueprints from.</param>
        /// <param name="blueprintIds">Ids of blueprints to create entities for.</param>
        /// <returns>List of ids of created entities.</returns>
        public static List<int> CreateEntities(
            EntityManager entityManager,
            IBlueprintManager blueprintManager,
            IEnumerable<string> blueprintIds)
        {
            return
                blueprintIds.Select(
                    actionBlueprintId => CreateEntity(entityManager, blueprintManager, actionBlueprintId)).ToList();
        }

        /// <summary>
        ///   Searches for the blueprints with the specified ids and creates entities out of it.
        ///   If the blueprints are used several times, consider to fetch them from the game's
        ///   blueprint manager once and use them to create the entities.
        /// </summary>
        /// <param name="game">Game to get the blueprints from and create the entities in.</param>
        /// <param name="blueprintIds">Ids of blueprints to use.</param>
        /// <returns>Ids of created entities.</returns>
        public static List<int> CreateEntities(this Game game, IEnumerable<string> blueprintIds)
        {
            return blueprintIds.Select(actionBlueprintId => CreateEntity(game, actionBlueprintId)).ToList();
        }

        /// <summary>
        ///   Searches for the blueprint with the specified id and creates an entity out of it.
        ///   If the blueprint is used several times, consider to fetch the blueprint from the game's
        ///   blueprint manager once and use it to create the entities.
        /// </summary>
        /// <param name="game">Game to get the blueprint from and create the entity in.</param>
        /// <param name="blueprintId">Id of blueprint to use.</param>
        /// <returns>Id of created entity.</returns>
        public static int CreateEntity(this Game game, string blueprintId)
        {
            return CreateEntity(game, blueprintId, null);
        }

        /// <summary>
        ///   Searches for the blueprint with the specified id and creates an entity out of it.
        ///   If the blueprint is used several times, consider to fetch the blueprint from the game's
        ///   blueprint manager once and use it to create the entities.
        /// </summary>
        /// <param name="game">Game to get the blueprint from and create the entity in.</param>
        /// <param name="blueprintId">Id of blueprint to use.</param>
        /// <param name="configuration">Configuration to initialize the entity from.</param>
        /// <returns>Id of created entity.</returns>
        public static int CreateEntity(this Game game, string blueprintId, AttributeTable configuration)
        {
            return CreateEntity(game.EntityManager, game.BlueprintManager, blueprintId, configuration);
        }

        /// <summary>
        ///   Searches for the specified blueprint and creates an entity out of it.
        /// </summary>
        /// <param name="game">Game to create the entity in.</param>
        /// <param name="blueprint">Blueprint to use.</param>
        /// <param name="configuration">Configuration to initialize the entity from.</param>
        /// <returns>Id of created entity.</returns>
        public static int CreateEntity(this Game game, Blueprint blueprint, AttributeTable configuration)
        {
            return CreateEntity(game.EntityManager, blueprint, configuration);
        }

        /// <summary>
        ///   Creates an entity in the specified entity manager with the blueprint from the specified blueprint manager with the
        ///   specified id. Uses the specified configuration for initialization.
        /// </summary>
        /// <param name="entityManager">Entity manager to create entity at.</param>
        /// <param name="blueprintManager">Blueprint manager to use to find blueprint.</param>
        /// <param name="blueprintId">Id of blueprint to use.</param>
        /// <param name="configuration">Configuration to use for initialization of entity.</param>
        /// <returns>Id of created entity.</returns>
        public static int CreateEntity(
            EntityManager entityManager,
            IBlueprintManager blueprintManager,
            string blueprintId,
            AttributeTable configuration = null)
        {
            var blueprint = blueprintManager.GetBlueprint(blueprintId);
            return CreateEntity(entityManager, blueprint, configuration);
        }

        /// <summary>
        ///   Creates an entity in the specified entity manager with the specified blueprint.
        ///   Uses the specified configuration for initialization.
        /// </summary>
        /// <param name="entityManager">Entity manager to create entity at.</param>
        /// <param name="blueprint">Blueprint to use.</param>
        /// <param name="configuration">Configuration to use for initialization of entity.</param>
        /// <returns>Id of created entity.</returns>
        public static int CreateEntity(
            EntityManager entityManager,
            Blueprint blueprint,
            AttributeTable configuration = null)
        {
            return entityManager.CreateEntity(blueprint, configuration);
        }

        #endregion
    }
}