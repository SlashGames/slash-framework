// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GameProcess.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.ECS.Processes
{
    using Slash.ECS.Components;
    using Slash.ECS.Events;

    /// <summary>
    ///   Timed process that can be queued with other processes. Good examples
    ///   are animations, tweens, or "Go to that point, and open the door after."
    /// </summary>
    public abstract class GameProcess
    {
        #region Properties

        /// <summary>
        ///   Whether this process is currently being updated.
        /// </summary>
        public bool Active { get; internal set; }

        /// <summary>
        ///   Whether this process is about to be removed.
        /// </summary>
        public bool Dead { get; private set; }

        /// <summary>
        ///   Process to start after this one has finished.
        /// </summary>
        public GameProcess Next { get; set; }

        /// <summary>
        ///   Whether this process is waiting to be resumed for being updated again.
        /// </summary>
        public bool Paused { get; private set; }

        /// <summary>
        ///   Type of this process.
        /// </summary>
        public object ProcessType { get; set; }

        protected EntityManager EntityManager { get; private set; }

        protected EventManager EventManager { get; private set; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///   Initializes this process.
        /// </summary>
        public virtual void InitProcess(EntityManager entityManager, EventManager eventManager)
        {
            this.EntityManager = entityManager;
            this.EventManager = eventManager;
        }

        /// <summary>
        ///   Marks this process to be removed. Allows subclasses to handle being killed.
        /// </summary>
        public virtual void Kill()
        {
            this.Dead = true;
        }

        /// <summary>
        ///   Queues the specified process to be executed right after this one.
        /// </summary>
        /// <param name="next">Process to start after this one has finished.</param>
        /// <returns>
        ///   <paramref name="next" />
        /// </returns>
        public GameProcess Then(GameProcess next)
        {
            this.Next = next;
            return next;
        }

        /// <summary>
        ///   Pauses or resumes this process. Allows subclasses to handle pausing and resuming.
        /// </summary>
        public virtual void TogglePause()
        {
            this.Paused = !this.Paused;
        }

        /// <summary>
        ///   Updates this process.
        /// </summary>
        /// <param name="dt">Time since last update, in seconds.</param>
        public virtual void Update(float dt)
        {
        }

        #endregion
    }
}