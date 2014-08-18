// --------------------------------------------------------------------------------------------------------------------
// <copyright file="WaitProcess.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.GameBase.Processes
{
    /// <summary>
    ///   Waits a specified amount of time before starting the next process.
    /// </summary>
    public class WaitProcess : GameProcess
    {
        #region Constructors and Destructors

        /// <summary>
        ///   Creates a new process to wait before starting the next one.
        /// </summary>
        /// <param name="seconds">Time to wait, in seconds.</param>
        public WaitProcess(float seconds)
        {
            this.Lifetime = seconds;
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///   Time to wait before starting the next process, in seconds.
        /// </summary>
        public float Lifetime { get; private set; }

        /// <summary>
        ///   Time already waited, in seconds.
        /// </summary>
        public float TimeElapsed { get; private set; }

        #endregion

        #region Public Methods and Operators

        public override void Update(float dt)
        {
            base.Update(dt);

            this.TimeElapsed += dt;

            if (this.TimeElapsed >= this.Lifetime)
            {
                this.Kill();
            }
        }

        #endregion
    }
}