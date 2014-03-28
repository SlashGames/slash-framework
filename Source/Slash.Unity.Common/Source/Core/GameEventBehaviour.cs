// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GameEventBehaviour.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Unity.Common.Core
{
    using System.Collections.Generic;

    using Slash.GameBase;
    using Slash.GameBase.Events;

    using UnityEngine;

    public class GameEventBehaviour : MonoBehaviour
    {
        #region Fields

        /// <summary>
        ///   Events to register for at FreudBot games.
        /// </summary>
        private readonly Dictionary<object, EventManager.EventDelegate> events =
            new Dictionary<object, EventManager.EventDelegate>();

        /// <summary>
        ///   Behaviour which manages the game.
        /// </summary>
        private GameBehaviour gameBehaviour;

        #endregion

        #region Properties

        protected Game Game
        {
            get
            {
                return this.gameBehaviour != null ? this.gameBehaviour.Game : null;
            }
        }

        #endregion

        #region Methods

        protected virtual void Awake()
        {
            this.gameBehaviour = FindObjectOfType<GameBehaviour>();
            if (this.gameBehaviour != null)
            {
                this.gameBehaviour.GameChanged += this.OnGameChanged;
            }

            this.RegisterListeners();
        }

        protected void OnGameChanged(Game newGame, Game oldGame)
        {
            if (oldGame != null)
            {
                foreach (var gameEvent in this.events)
                {
                    oldGame.EventManager.RemoveListener(gameEvent.Key, gameEvent.Value);
                }
            }

            if (newGame != null)
            {
                foreach (var gameEvent in this.events)
                {
                    newGame.EventManager.RegisterListener(gameEvent.Key, gameEvent.Value);
                }
            }
        }

        /// <summary>
        ///   Registers for the specified event at any future FreudBot games that might start.
        /// </summary>
        /// <param name="eventType">Type of the event to register for.</param>
        /// <param name="callback">Method to call when the event is raised.</param>
        protected void RegisterListener(object eventType, EventManager.EventDelegate callback)
        {
            this.events.Add(eventType, callback);
        }

        /// <summary>
        ///   Method to register for the required events in derived behaviours.
        /// </summary>
        protected virtual void RegisterListeners()
        {
        }

        #endregion
    }
}