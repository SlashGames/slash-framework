// -----------------------------------------------------------------------
// <copyright file="EventManager.cs" company="Rainy Games">
// Copyright (c) Rainy Games. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace RainyGames.GameBase
{
    /// <summary>
    /// Allows listeners to register for game-related events and notifies them
    /// whenever one of these events is fired.
    /// </summary>
    public class EventManager
    {
        #region Delegates

        /// <summary>
        /// Delegate for all entity-related events.
        /// </summary>
        /// <param name="entityId">
        /// Id of the entity that caused an event.
        /// </param>
        public delegate void EntityDelegate(long entityId);

        /// <summary>
        /// Delegate for all player-related events.
        /// </summary>
        /// <param name="player">
        /// Player that caused an event.
        /// </param>
        public delegate void PlayerDelegate(Player player);

        /// <summary>
        /// Delegate for general game events.
        /// </summary>
        public delegate void GameDelegate();

        /// <summary>
        /// Delegate for system-related events.
        /// </summary>
        /// <param name="system">
        /// System that caused an event.
        /// </param>
        public delegate void SystemDelegate(ISystem system);

        /// <summary>
        /// Delegate for component-related events.
        /// </summary>
        /// <param name="entityId">
        /// Id of the entity whose component caused an event.
        /// </param>
        /// <param name="component">
        /// Component that caused an event.
        /// </param>
        public delegate void ComponentDelegate(long entityId, IComponent component);

        #endregion

        #region Events

        /// <summary>
        /// Called when a new entity has been created.
        /// </summary>
        public event EntityDelegate EntityCreated;

        /// <summary>
        /// Called when an entity has been removed.
        /// </summary>
        public event EntityDelegate EntityRemoved;

        /// <summary>
        /// Called when a new player has been added.
        /// </summary>
        public event PlayerDelegate PlayerAdded;

        /// <summary>
        /// Called when a player has been removed.
        /// </summary>
        public event PlayerDelegate PlayerRemoved;

        /// <summary>
        /// Called when the game starts.
        /// </summary>
        public event GameDelegate GameStarted;

        /// <summary>
        /// Called when the game has been paused.
        /// </summary>
        public event GameDelegate GamePaused;

        /// <summary>
        /// Called when the game has been resumed.
        /// </summary>
        public event GameDelegate GameResumed;

        /// <summary>
        /// Called when a new system has been added.
        /// </summary>
        public event SystemDelegate SystemAdded;

        /// <summary>
        /// Called when a new component has been added.
        /// </summary>
        public event ComponentDelegate ComponentAdded;

        /// <summary>
        /// Called when a component has been removed.
        /// </summary>
        public event ComponentDelegate ComponentRemoved;

        #endregion

        #region Methods

        /// <summary>
        /// Called when a new entity has been created.
        /// </summary>
        /// <param name="entityId">
        /// Id of the new entity.
        /// </param>
        internal void InvokeEntityCreated(long entityId)
        {
            EntityDelegate handler = this.EntityCreated;

            if (handler != null)
            {
                handler(entityId);
            }
        }

        /// <summary>
        /// Called when an entity has been removed.
        /// </summary>
        /// <param name="entityId">
        /// Id of the removed entity.
        /// </param>
        internal void InvokeEntityRemoved(long entityId)
        {
            EntityDelegate handler = this.EntityRemoved;

            if (handler != null)
            {
                handler(entityId);
            }
        }

        /// <summary>
        /// Called when a new player has been added.
        /// </summary>
        /// <param name="player">
        /// Player that has been added.
        /// </param>
        internal void InvokePlayerAdded(Player player)
        {
            PlayerDelegate handler = this.PlayerAdded;

            if (handler != null)
            {
                handler(player);
            }
        }

        /// <summary>
        /// Called when a player has been removed.
        /// </summary>
        /// <param name="player">
        /// Player that has been removed.
        /// </param>
        internal void InvokePlayerRemoved(Player player)
        {
            PlayerDelegate handler = this.PlayerRemoved;

            if (handler != null)
            {
                handler(player);
            }
        }

        /// <summary>
        /// Called when the game starts.
        /// </summary>
        internal void InvokeGameStarted()
        {
            GameDelegate handler = this.GameStarted;

            if (handler != null)
            {
                handler();
            }
        }

        /// <summary>
        /// Called when the game has been paused.
        /// </summary>
        internal void InvokeGamePaused()
        {
            GameDelegate handler = this.GamePaused;

            if (handler != null)
            {
                handler();
            }
        }

        /// <summary>
        /// Called when the game has been resumed.
        /// </summary>
        internal void InvokeGameResumed()
        {
            GameDelegate handler = this.GameResumed;

            if (handler != null)
            {
                handler();
            }
        }

        /// <summary>
        /// Called when a new system has been added.
        /// </summary>
        /// <param name="system">
        /// System that has been added.
        /// </param>
        internal void InvokeSystemAdded(ISystem system)
        {
            SystemDelegate handler = this.SystemAdded;

            if (handler != null)
            {
                handler(system);
            }
        }

        /// <summary>
        /// Called when a new component has been added.
        /// </summary>
        /// <param name="entityId">
        /// Id of the entity the component has been added to.
        /// </param>
        /// <param name="component">
        /// Component that has been added.
        /// </param>
        internal void InvokeComponentAdded(long entityId, IComponent component)
        {
            ComponentDelegate handler = this.ComponentAdded;

            if (handler != null)
            {
                handler(entityId, component);
            }
        }

        /// <summary>
        /// Called when a component has been removed.
        /// </summary>
        /// <param name="entityId">
        /// Id of the entity the component has been removed from.
        /// </param>
        /// <param name="component">
        /// Component that has been removed.
        /// </param>
        internal void InvokeComponentRemoved(long entityId, IComponent component)
        {
            ComponentDelegate handler = this.ComponentRemoved;

            if (handler != null)
            {
                handler(entityId, component);
            }
        }

        #endregion
    }
}
