// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ProcessManager.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.ECS.Processes
{
    using System.Collections.Generic;

    using Slash.ECS.Components;
    using Slash.ECS.Events;

    /// <summary>
    ///   Allows ticking and queueing timed processes. Good examples are
    ///   animations, tweens, or "Go to that point, and open the door after."
    /// </summary>
    public class ProcessManager
    {
        #region Fields

        /// <summary>
        ///   Processes being updated in each tick.
        /// </summary>
        private readonly List<GameProcess> activeProcesses = new List<GameProcess>();

        /// <summary>
        ///   Processes about to be removed.
        /// </summary>
        private readonly List<GameProcess> deadProcesses = new List<GameProcess>();

        private readonly EntityManager entityManager;

        private readonly EventManager eventManager;

        /// <summary>
        ///   Processes about to become active.
        /// </summary>
        private readonly List<GameProcess> newProcesses = new List<GameProcess>();

        #endregion

        #region Constructors and Destructors

        public ProcessManager(EntityManager entityManager, EventManager eventManager)
        {
            this.entityManager = entityManager;
            this.eventManager = eventManager;
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///   Adds a new process to be updated in each tick.
        /// </summary>
        /// <param name="process">Process to add.</param>
        public void AddProcess(GameProcess process)
        {
            process.InitProcess(this.entityManager, this.eventManager);
            this.activeProcesses.Add(process);
            process.Active = true;
        }

        /// <summary>
        ///   Updates all active processes, removing dead ones and starting
        ///   next linked processes where necessary.
        /// </summary>
        /// <param name="dt">Time since last update, in seconds.</param>
        public void UpdateProcesses(float dt)
        {
            if (this.activeProcesses.Count > 0)
            {
                foreach (GameProcess process in this.activeProcesses)
                {
                    if (process.Dead)
                    {
                        // Check for following process.
                        if (process.Next != null)
                        {
                            this.newProcesses.Add(process.Next);
                            process.Next = null;
                        }

                        this.deadProcesses.Add(process);
                    }
                    else if (!process.Paused)
                    {
                        process.Update(dt);
                    }
                }

                // Remove dead processes.
                foreach (GameProcess deadProcess in this.deadProcesses)
                {
                    this.activeProcesses.Remove(deadProcess);
                }

                this.deadProcesses.Clear();
            }

            if (this.newProcesses.Count > 0)
            {
                // Add new processes.
                foreach (GameProcess newProcess in this.newProcesses)
                {
                    this.AddProcess(newProcess);
                }

                this.newProcesses.Clear();
            }
        }

        #endregion
    }
}