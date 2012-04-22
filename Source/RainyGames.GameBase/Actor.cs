// -----------------------------------------------------------------------
// <copyright file="Actor.cs" company="Rainy Games">
// Copyright (c) Rainy Games. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace RainyGames.GameBase
{
    using System;
    using System.Collections.Generic;

    using RainyGames.GameBase.ActorComponents;

    /// <summary>
    /// Actor that is part of a specific game. Each actor has a unique id and
    /// name and can have additional actor components attached for extending
    /// its functionality. Actors are ticked by the game, allowing them to
    /// update themselves and all attached components.
    /// </summary>
    public class Actor
    {
        #region Constants and Fields

        /// <summary>
        /// Unique id to be assigned to the actor created next.
        /// </summary>
        private static int nextActorId = 0;

        /// <summary>
        /// Game this actor is part of.
        /// </summary>
        private Game game;

        /// <summary>
        /// Unique id of this actor.
        /// </summary>
        private int id;

        /// <summary>
        /// Name of this actor.
        /// </summary>
        private string name;

        /// <summary>
        /// Components attached to this actor, extending its functionality.
        /// </summary>
        private List<ActorComponent> components;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Constructs a new actor to be part of the specified game, assigning
        /// a unique id and name.
        /// </summary>
        /// <param name="game">
        /// Game the new actor will be part of.
        /// </param>
        public Actor(Game game)
        {
            if (game == null)
            {
                throw new ArgumentNullException("game");
            }

            this.game = game;

            this.id = nextActorId++;
            this.name = GetType().Name + "_" + this.id;
            this.components = new List<ActorComponent>();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Game this actor is part of.
        /// </summary>
        public Game Game
        {
            get { return this.game; }
        }

        /// <summary>
        /// Unique id of this actor.
        /// </summary>
        public int Id
        {
            get { return this.id; }
        }

        /// <summary>
        /// Name of this actor.
        /// </summary>
        public string Name
        {
            get { return this.name; }
        }

        /// <summary>
        /// Player owning this actor, i.e. that has build this unit or fired
        /// this projectile.
        /// </summary>
        public Player Owner { get; set; }

        #endregion

        #region Public Methods

        /// <summary>
        /// Ticks this actor, allowing itself and all attached components to
        /// update itself.
        /// </summary>
        /// <param name="dt">
        /// Time passed since the last tick.
        /// </param>
        public virtual void Update(float dt)
        {
            foreach (ActorComponent component in this.components)
            {
                component.Update(dt);
            }
        }

        /// <summary>
        /// Attaches the passed component to this actor. The component will be
        /// updated by this actor during each tick.
        /// </summary>
        /// <param name="component">
        /// Component to add to this game.
        /// </param>
        public void AddComponent(ActorComponent component)
        {
            this.components.Add(component);
        }

        /// <summary>
        /// Gets the first component of the specified type attached to this
        /// actor.
        /// </summary>
        /// <typeparam name="T">
        /// Type of the component to get.
        /// </typeparam>
        /// <returns>
        /// First component of the specified type attached to this actor.
        /// </returns>
        public T GetActorComponent<T>()
            where T : ActorComponent
        {
            return (T)this.components.Find(c => c.GetType().Equals(typeof(T)));
        }

        /// <summary>
        /// Removes the specified component from this actor, if it's attached.
        /// </summary>
        /// <param name="component">
        /// Component to remove.
        /// </param>
        /// <returns>
        /// <c>true</c>, if the component has been removed, and <c>false</c>
        /// otherwise.
        /// </returns>
        public bool RemoveComponent(ActorComponent component)
        {
            return this.components.Remove(component);
        }

        /// <summary>
        /// Returns the name of this actor.
        /// </summary>
        /// <returns>
        /// Name of this actor.
        /// </returns>
        public override string ToString()
        {
            return this.name;
        }

        #endregion
    }
}
