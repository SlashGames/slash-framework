// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ProcessManager.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.ECS.Processes
{
    using System.Collections.Generic;

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

        /// <summary>
        ///   Processes about to become active.
        /// </summary>
        private readonly List<GameProcess> newProcesses = new List<GameProcess>();

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///   Adds a new process to be updated in each tick.
        /// </summary>
        /// <param name="process">Process to add.</param>
        public void AddProcess(GameProcess process)
        {
            process.InitProcess();
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

            // Add new processes.
            foreach (GameProcess newProcess in this.newProcesses)
            {
                this.AddProcess(newProcess);
            }

            this.deadProcesses.Clear();
            this.newProcesses.Clear();
        }

        #endregion
    }
}