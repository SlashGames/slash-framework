// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EntityConfigurationBehaviour.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Unity.Common.ECS
{
    using System;
    using System.Collections.Generic;

    using Slash.Application.Games;
    using Slash.Collections.AttributeTables;
    using Slash.ECS;
    using Slash.ECS.Blueprints.Extensions;
    using Slash.ECS.Events;

    using UnityEngine;

    /// <summary>
    ///   Creates a game entity from the specified configuration.
    /// </summary>
    /// <remarks>
    ///   Note that this behaviour currently does not support prefabs.
    ///   If you attach this behaviour to a prefab, all instances will always
    ///   have their configurations reset to the prefab values.
    /// </remarks>
    public class EntityConfigurationBehaviour : MonoBehaviour, ISerializationCallbackReceiver
    {
        #region Fields

        /// <summary>
        ///   Blueprint id of the entity to create.
        /// </summary>
        [SerializeField]
        [HideInInspector]
        public string BlueprintId;

        /// <summary>
        ///   Configuration of the entity to create.
        /// </summary>
        [HideInInspector]
        public AttributeTable Configuration;

        /// <summary>
        ///   Configuration keys as serialized by Unity.
        /// </summary>
        [SerializeField]
        [HideInInspector]
        public List<string> SerializedConfigurationKeys;

        /// <summary>
        ///   Configuration value types as serialized by Unity.
        /// </summary>
        [SerializeField]
        [HideInInspector]
        public List<string> SerializedConfigurationTypes;

        /// <summary>
        ///   Configuration values as serialized by Unity.
        /// </summary>
        [SerializeField]
        [HideInInspector]
        public List<string> SerializedConfigurationValues;

        /// <summary>
        ///   Game behaviour holding the reference to the game the entity belongs to.
        /// </summary>
        private GameBehaviour gameBehaviour;

        #endregion

        #region Public Properties

        /// <summary>
        ///   Id of the entity created with this configuration.
        /// </summary>
        public int EntityId { get; private set; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///   Deserializes this entity configuration.
        /// </summary>
        public void OnAfterDeserialize()
        {
            this.Configuration = new AttributeTable();

            for (int i = 0; i < this.SerializedConfigurationKeys.Count; i++)
            {
                var valueType = Type.GetType(this.SerializedConfigurationTypes[i]);
                var attributeKey = this.SerializedConfigurationKeys[i];
                var attributeValue = Convert.ChangeType(this.SerializedConfigurationValues[i], valueType);

                this.Configuration.Add(attributeKey, attributeValue);
            }
        }

        /// <summary>
        ///   Prepares this entity configuration for serialization by Unity.
        /// </summary>
        public void OnBeforeSerialize()
        {
            this.SerializedConfigurationKeys = new List<string>();
            this.SerializedConfigurationValues = new List<string>();
            this.SerializedConfigurationTypes = new List<string>();

            IEnumerable<KeyValuePair<object, object>> attributes = this.Configuration;

            foreach (var attribute in attributes)
            {
                this.SerializedConfigurationKeys.Add(attribute.Key.ToString());
                this.SerializedConfigurationValues.Add(attribute.Value.ToString());
                this.SerializedConfigurationTypes.Add(attribute.Value.GetType().ToString());
            }
        }

        #endregion

        #region Methods

        private void Awake()
        {
            this.gameBehaviour = GameBehaviour.Instance;

            this.gameBehaviour.GameChanged += this.OnGameChanged;
        }

        private void OnEntityRemoved(GameEvent e)
        {
            var entityId = (int)e.EventData;
            if (entityId == this.EntityId)
            {
                Destroy(this.gameObject);
            }
        }

        private void OnGameChanged(Game newGame, Game oldGame)
        {
            if (oldGame != null)
            {
                oldGame.EventManager.RemoveListener(FrameworkEvent.EntityRemoved, this.OnEntityRemoved);
            }

            if (newGame != null)
            {
                // Create entity.
                var blueprint = newGame.BlueprintManager.GetBlueprint(this.BlueprintId);
                this.EntityId = newGame.EntityManager.CreateEntity(blueprint, this.Configuration);

                if (EntityGameObjectMap.Instance != null)
                {
                    // Register entity object.
                    EntityGameObjectMap.Instance[this.EntityId] = this.gameObject;
                }

                newGame.EventManager.RegisterListener(FrameworkEvent.EntityRemoved, this.OnEntityRemoved);
            }
        }

        #endregion
    }
}