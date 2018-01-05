// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EntityGameObjectMap.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Unity.Common.ECS
{
    using System.Collections.Generic;

    using UnityEngine;

    /// <summary>
    ///   Maps entity ids to Unity game objects.
    /// </summary>
    public class EntityGameObjectMap : MonoBehaviour
    {
        #region Fields

        private readonly Dictionary<int, GameObject> entityToGameObject = new Dictionary<int, GameObject>();

        #endregion

        #region Public Properties

        /// <summary>
        ///   Current entity game object map.
        /// </summary>
        public static EntityGameObjectMap Instance { get; private set; }

        #endregion

        #region Public Indexers

        /// <summary>
        ///   Gets or sets the Unity game object mapped by the specified entity id.
        /// </summary>
        /// <param name="entityId">Entity id mapped to the game object.</param>
        /// <returns>Unity game object mapped by the specified entity id.</returns>
        public GameObject this[int entityId]
        {
            get
            {
                return this.entityToGameObject[entityId];
            }
            set
            {
                this.entityToGameObject[entityId] = value;
            }
        }

        #endregion

        #region Methods

        private void Awake()
        {
            if (Instance != null)
            {
                Debug.LogError("Multiple instances of EntityGameObjectMap detected!");
            }

            Instance = this;
        }

        #endregion
    }
}