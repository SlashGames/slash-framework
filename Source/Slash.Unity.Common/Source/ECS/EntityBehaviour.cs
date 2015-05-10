// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EntityBehaviour.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Unity.Common.ECS
{
    using Slash.ECS;
    using Slash.ECS.Components;

    using UnityEngine;

    public class EntityBehaviour : MonoBehaviour
    {
        #region Properties

        public int EntityId { get; set; }

        public Game Game { get; set; }

        #endregion

        #region Public Methods and Operators

        public T GetLogicComponent<T>() where T : class, IEntityComponent
        {
            return this.Game != null && this.Game.EntityManager.EntityIsAlive(this.EntityId)
                ? this.Game.EntityManager.GetComponent<T>(this.EntityId)
                : null;
        }

        #endregion
    }
}