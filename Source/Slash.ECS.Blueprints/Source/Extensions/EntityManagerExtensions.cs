// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EntityManagerExtensions.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.ECS.Blueprints.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Slash.Collections.AttributeTables;
    using Slash.ECS.Components;

    /// <summary>
    ///   Extension methods for the entity manager which use a blueprint for easier creation and initialization of new
    ///   entities.
    /// </summary>
    public static class EntityManagerExtensions
    {
        #region Public Methods and Operators

        /// <summary>
        ///   Creates a new entity, adding components matching the passed
        ///   blueprint and initializing these with the data stored in the
        ///   blueprint and the specified configuration. Configuration data
        ///   is preferred over blueprint data.
        /// </summary>
        /// <param name="entityManager">Entity Manager to add entity to.</param>
        /// <param name="blueprint"> Blueprint describing the entity to create. </param>
        /// <param name="configuration"> Data for initializing the entity. </param>
        /// <param name="additionalComponents">Components to add to the entity, in addition to the ones specified by the blueprint.</param>
        /// <returns> Unique id of the new entity. </returns>
        public static int CreateEntity(
            this EntityManager entityManager,
            Blueprint blueprint,
            IAttributeTable configuration = null,
            List<Type> additionalComponents = null)
        {
            var entityId = entityManager.CreateEntity();
            InitEntity(entityManager, entityId, blueprint, configuration, additionalComponents);
            return entityId;
        }

        /// <summary>
        ///   Initializes the specified entity, adding components matching the
        ///   passed blueprint and initializing these with the data stored in
        ///   the blueprint and the specified configuration. Configuration
        ///   data is preferred over blueprint data.
        /// </summary>
        /// <param name="entityManager">Entity Manager to add entity to.</param>
        /// <param name="entityId">Id of the entity to initialize.</param>
        /// <param name="blueprint"> Blueprint describing the entity to create. </param>
        /// <param name="configuration"> Data for initializing the entity. </param>
        /// <param name="additionalComponents">Components to add to the entity, in addition to the ones specified by the blueprint.</param>
        public static void InitEntity(
            this EntityManager entityManager,
            int entityId,
            Blueprint blueprint,
            IAttributeTable configuration = null,
            IEnumerable<Type> additionalComponents = null)
        {
            if (blueprint == null)
            {
                throw new ArgumentNullException("blueprint", "Blueprint is null.");
            }

            // Setup attribute table.
            var attributeTable = new HierarchicalAttributeTable();
            if (configuration != null)
            {
                attributeTable.AddParent(configuration);
            }

            // Add attribute tables of all ancestors.
            var blueprintAttributeTable = blueprint.GetAttributeTable();
            if (blueprintAttributeTable != null)
            {
                attributeTable.AddParent(blueprintAttributeTable);
            }

            // Build list of components to add.
            var blueprintComponentTypes = blueprint.GetAllComponentTypes();
            var componentTypes = additionalComponents != null
                ? blueprintComponentTypes.Union(additionalComponents)
                : blueprintComponentTypes;

            // Add components.
            foreach (var type in componentTypes)
            {
                entityManager.AddComponent(type, entityId, attributeTable);
            }

            // Raise event.
            entityManager.OnEntityInitialized(entityId);
        }

        #endregion
    }
}