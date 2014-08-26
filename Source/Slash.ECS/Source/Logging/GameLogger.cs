// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GameLogger.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.ECS.Logging
{
    /// <summary>
    ///   Logger interface that allows interested listeners to register for log messages.
    /// </summary>
    public class GameLogger
    {
        #region Delegates

        /// <summary>
        ///   Logs the specified message.
        /// </summary>
        /// <param name="message">Message to log.</param>
        public delegate void LogDelegate(string message);

        #endregion

        #region Public Events

        /// <summary>
        ///   Error message should be logged.
        /// </summary>
        public event LogDelegate ErrorLogged;

        /// <summary>
        ///   Info message should be logged.
        /// </summary>
        public event LogDelegate InfoLogged;

        /// <summary>
        ///   Warning message should be logged.
        /// </summary>
        public event LogDelegate WarningLogged;

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///   Logs the specified error message.
        /// </summary>
        /// <param name="message">Message to log.</param>
        public virtual void Error(string message)
        {
            this.OnError(message);
        }

        /// <summary>
        ///   Logs the specified info message.
        /// </summary>
        /// <param name="message">Message to log.</param>
        public virtual void Info(string message)
        {
            this.OnInfo(message);
        }

        /// <summary>
        ///   Logs the specified warning message.
        /// </summary>
        /// <param name="message">Message to log.</param>
        public virtual void Warning(string message)
        {
            this.OnWarning(message);
        }

        #endregion

        #region Methods

        private void OnError(string message)
        {
            var handler = this.ErrorLogged;
            if (handler != null)
            {
                handler(message);
            }
        }

        private void OnInfo(string message)
        {
            var handler = this.InfoLogged;
            if (handler != null)
            {
                handler(message);
            }
        }

        private void OnWarning(string message)
        {
            var handler = this.WarningLogged;
            if (handler != null)
            {
                handler(message);
            }
        }

        #endregion
    }
}