// -----------------------------------------------------------------------
// <copyright file="Game.cs" company="Rainy Games">
// Copyright (c) Rainy Games. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace RainyGames.GameBase
{
    using System.Collections.Generic;

    /// <summary>
    /// Base class of most Rainy Games games. Provides default functionality
    /// that is common across many games, such as actors that are ticked or
    /// players that are participating in the game.
    /// </summary>
    public class Game
    {
        #region Constants and Fields

        // TODO use faster data structures here

        /// <summary>
        /// Actors that are part of this game.
        /// </summary>
        private List<Actor> actors;

        /// <summary>
        /// Players participating in this game.
        /// </summary>
        private List<Player> players;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Constructs a new game without actors or players.
        /// </summary>
        public Game()
        {
            this.actors = new List<Actor>();
            this.players = new List<Player>();
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Ticks this game, allowing all actors to update themselves.
        /// </summary>
        /// <param name="dt">
        /// Time passed since the last tick.
        /// </param>
        public void Update(float dt)
        {
            foreach (Actor actor in this.actors)
            {
                actor.Update(dt);
            }
        }

        /// <summary>
        /// Adds the passed actor to this game. The actor will be updated
        /// during each tick.
        /// </summary>
        /// <param name="actor">
        /// Actor to add to this game.
        /// </param>
        public void AddActor(Actor actor)
        {
            this.actors.Add(actor);
        }

        /// <summary>
        /// Gets the actor with the specified id, if it's part of this game.
        /// </summary>
        /// <param name="actorId">
        /// Id of the actor to get.
        /// </param>
        /// <returns>
        /// Actor with the specified id, if it's part of this game, and
        /// <c>null</c> otherwise.
        /// </returns>
        public Actor GetActor(int actorId)
        {
            return this.actors.Find(a => a.Id == actorId);
        }

        /// <summary>
        /// Removes the specified actor from this game. The actor won't be
        /// updated any longer by this game.
        /// </summary>
        /// <param name="actor">
        /// Actor to remove.
        /// </param>
        /// <returns>
        /// <c>true</c>, if the actor has been removed, and <c>false</c>
        /// otherwise.
        /// </returns>
        public bool RemoveActor(Actor actor)
        {
            return this.actors.Remove(actor);
        }

        /// <summary>
        /// Adds the passed player to this game.
        /// </summary>
        /// <param name="player">
        /// Player to add to this game.
        /// </param>
        public void AddPlayer(Player player)
        {
            this.players.Add(player);
        }

        /// <summary>
        /// Gets the player with the specified index, if there's one
        /// participating in this game.
        /// </summary>
        /// <param name="playerIndex">
        /// Index of the player to get.
        /// </param>
        /// <returns>
        /// Player with the specified index, if there's one
        /// participating in this game, and <c>null</c> otherwise.
        /// </returns>
        public Player GetPlayer(int playerIndex)
        {
            return this.players.Find(p => p.PlayerIndex == playerIndex);
        }

        /// <summary>
        /// Removes the specified player from this game.
        /// </summary>
        /// <param name="player">
        /// Player to remove.
        /// </param>
        /// <returns>
        /// <c>true</c>, if the player has been removed, and <c>false</c>
        /// otherwise.
        /// </returns>
        public bool RemovePlayer(Player player)
        {
            return this.players.Remove(player);
        }

        #endregion
    }
}
