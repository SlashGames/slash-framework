// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EntityBehaviour.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Unity.Common.Core
{
    using Slash.ECS;
    using Slash.ECS.Components;

    using UnityEngine;

    public class EntityBehaviour : MonoBehaviour
    {
        #region Public Properties

        public int EntityId { get; set; }

        public Game Game { get; set; }

        #endregion

        #region Public Methods and Operators

        public T GetLogicComponent<T>() where T : IEntityComponent
        {
            return this.Game.EntityManager.GetComponent<T>(this.EntityId);
        }

        #endregion
    }
}