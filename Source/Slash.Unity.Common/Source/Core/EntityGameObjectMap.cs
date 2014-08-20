// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EntityGameObjectMap.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Unity.Common.Core
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

        public static EntityGameObjectMap Instance { get; private set; }

        #endregion

        #region Public Indexers

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