// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GameContext.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Unity.DataBind.Ext.ECS
{
    using System.Collections.Generic;

    using Slash.ECS.Components;
    using Slash.ECS.Events;
    using Slash.Unity.DataBind.Core.Data;

    /// <summary>
    ///   Special context for Slash.ECS framework to register events on EventManager
    ///   and get data from EntityManager.
    /// </summary>
    public abstract class GameContext : Context
    {
        #region Fields

        /// <summary>
        ///   Events to register for at event manager instances.
        /// </summary>
        private readonly Dictionary<object, EventManager.EventDelegate> events =
            new Dictionary<object, EventManager.EventDelegate>();

        #endregion

        #region Properties

        /// <summary>
        ///   Entity manager to access data from client logic.
        /// </summary>
        protected EntityManager EntityManager { get; set; }

        /// <summary>
        ///   Event manager to send actions into logic and register for events from logic.
        /// </summary>
        protected EventManager EventManager { get; set; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///   Deinitialize this context, i.e. removing from all registered events.
        /// </summary>
        public virtual void Deinit()
        {
            // Clear event handlers.
            if (this.EventManager != null)
            {
                foreach (var gameEvent in this.events)
                {
                    this.EventManager.RemoveListener(gameEvent.Key, gameEvent.Value);
                }
                this.EventManager = null;
                this.EntityManager = null;
            }
        }

        /// <summary>
        ///   Sets up the connection to the client logic.
        /// </summary>
        /// <param name="eventManager">Event manager to communicate with the client logic.</param>
        /// <param name="entityManager">Entity manager to access data from client logic.</param>
        public virtual void Init(EventManager eventManager, EntityManager entityManager)
        {
            // Setup event callbacks.
            this.EventManager = eventManager;
            this.EntityManager = entityManager;

            if (this.EventManager != null)
            {
                this.SetEventListeners();
            }

            if (this.EventManager != null)
            {
                foreach (var gameEvent in this.events)
                {
                    this.EventManager.RegisterListener(gameEvent.Key, gameEvent.Value);
                }
            }
        }

        /// <summary>
        ///   Ticks this context.
        /// </summary>
        /// <param name="dt">Time since the last update (in s).</param>
        public virtual void Update(float dt)
        {
        }

        #endregion

        #region Methods

        /// <summary>
        ///   Registers the passed callback for events with the specified key.
        /// </summary>
        /// <param name="eventKey">Key of the events to register to callback for.</param>
        /// <param name="callback">Callback to register.</param>
        protected void SetEventListener(object eventKey, EventManager.EventDelegate callback)
        {
            this.events.Add(eventKey, callback);
        }

        /// <summary>
        ///   Registers callbacks for interesting game events.
        /// </summary>
        protected virtual void SetEventListeners()
        {
        }

        #endregion
    }
}