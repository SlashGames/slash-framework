// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EntityComponentBehaviour.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Unity.Common.ECS
{
    using Slash.ECS.Components;

    using UnityEngine;

    public class EntityComponentBehaviour<T> : MonoBehaviour
        where T : class, IEntityComponent
    {
        #region Fields

        public EntityBehaviour Entity;

        #endregion

        #region Properties

        protected T Component { get; set; }

        #endregion

        #region Methods

        protected void Start()
        {
            this.Component = this.Entity != null ? this.Entity.GetLogicComponent<T>() : null;
        }

        #endregion
    }
}