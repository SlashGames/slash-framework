// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EventManagerOperator.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Unity.Common.ECS
{
    using System.Collections.Generic;

    using Slash.ECS;
    using Slash.ECS.Events;

    using UnityEngine;

    public class EventManagerOperator : MonoBehaviour
    {
        #region Fields

        public bool AddListenersOnStart = true;

        /// <summary>
        ///   Event listeners that are added on enable, and removed on disable.
        /// </summary>
        private Dictionary<object, EventManager.EventDelegate> listeners;

        /// <summary>
        ///   Indicates if the behaviour was already started.
        /// </summary>
        private bool started;

        #endregion

        #region Properties

        /// <summary>
        ///   Event manager.
        /// </summary>
        protected EventManager EventManager { get; private set; }

        #endregion

        #region Methods

        protected virtual void Awake()
        {
            GameBehaviour gameBehaviour = FindObjectOfType<GameBehaviour>();
            if (gameBehaviour != null)
            {
                gameBehaviour.GameChanged += this.OnGameChanged;
                if (gameBehaviour.Game != null)
                {
                    this.OnGameChanged(gameBehaviour.Game, null);
                }
            }

            this.listeners = new Dictionary<object, EventManager.EventDelegate>();
        }

        /// <summary>
        ///   Initializes this system, setting it up for being added and removed as event listener.
        /// </summary>
        /// <see cref="SetListener" />
        protected virtual void Init()
        {
        }

        protected virtual void OnDisable()
        {
            this.RemoveListeners();
        }

        protected virtual void OnEnable()
        {
            if (this.started)
            {
                this.AddListeners();
            }
        }

        protected virtual void OnGameChanged(Game newGame, Game oldGame)
        {
            // Remove listeners from old event manager.
            this.RemoveListeners();

            // Get new event manager.
            this.EventManager = newGame.EventManager;

            // Add listeners to new event manager.
            this.AddListeners();
        }

        /// <summary>
        ///   Sets the passed callback for events with the specified key.
        ///   The callback will automatically be added on enable, and removed on disable.
        /// </summary>
        /// <param name="eventKey">Key of events to set the callback for.</param>
        /// <param name="callback">Callback to automatically add on enable, and remove on disable.</param>
        protected virtual void SetListener(object eventKey, EventManager.EventDelegate callback)
        {
            this.listeners.Add(eventKey, callback);
        }

        protected virtual void Start()
        {
            this.Init();

            if (this.AddListenersOnStart)
            {
                this.AddListeners();
            }

            this.started = true;
        }

        private void AddListeners()
        {
            if (this.EventManager == null)
            {
                Debug.LogWarning("No event manager available.", this);
                return;
            }

            if (this.listeners == null)
            {
                return;
            }

            foreach (var listener in this.listeners)
            {
                this.EventManager.RegisterListener(listener.Key, listener.Value);
            }
        }

        private void RemoveListeners()
        {
            if (this.EventManager == null)
            {
                return;
            }

            if (this.listeners == null)
            {
                return;
            }

            foreach (var listener in this.listeners)
            {
                this.EventManager.RemoveListener(listener.Key, listener.Value);
            }
        }

        #endregion
    }
}