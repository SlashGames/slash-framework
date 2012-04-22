// -----------------------------------------------------------------------
// <copyright file="ActorComponent.cs" company="Rainy Games">
// Copyright (c) Rainy Games. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace RainyGames.GameBase.ActorComponents
{
    using System;

    /// <summary>
    /// Abstract base class of all actor components. Actor components can be
    /// attached to actors in order to add further functionality, i.e. health
    /// or attacks. They are ticked by their owning actors.
    /// </summary>
    public abstract class ActorComponent
    {
        #region Constants and Fields

        /// <summary>
        /// Actor this component is attached to.
        /// </summary>
        private Actor actor;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Constructs a new actor component.
        /// </summary>
        /// <param name="actor">
        /// Actor the new component will be attached to.
        /// </param>
        public ActorComponent(Actor actor)
        {
            if (actor == null)
            {
                throw new ArgumentNullException("actor");
            }

            this.actor = actor;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Actor this component is attached to.
        /// </summary>
        public Actor Actor
        {
            get { return this.actor; }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Allows this actor component to update itself.
        /// </summary>
        /// <param name="dt">
        /// Time passed since the last tick.
        /// </param>
        public abstract void Update(float dt);

        #endregion
    }
}
