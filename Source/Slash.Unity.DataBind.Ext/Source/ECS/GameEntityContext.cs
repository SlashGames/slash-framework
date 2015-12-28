// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GameEntityContext.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Unity.DataBind.Ext.ECS
{
    using Slash.ECS.Components;
    using Slash.ECS.Events;

    /// <summary>
    ///   Special context which represents a single entity from the logic.
    /// </summary>
    public class GameEntityContext : GameContext
    {
        #region Properties

        /// <summary>
        ///   Id of the entity this context represents.
        /// </summary>
        public int EntityId { get; set; }

        #endregion

        #region Public Methods and Operators

        public virtual void Init(EventManager eventManager, EntityManager entityManager, int entityId)
        {
            this.Init(eventManager, entityManager);

            this.EntityId = entityId;
        }

        #endregion

        #region Methods

        protected T GetComponent<T>() where T : IEntityComponent
        {
            return this.EntityManager.GetComponent<T>(this.EntityId);
        }

        private new void Init(EventManager eventManager, EntityManager entityManager)
        {
            base.Init(eventManager, entityManager);
        }

        #endregion
    }
}