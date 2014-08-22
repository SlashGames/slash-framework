// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GameLogger.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.ECS.Logging
{
    public class GameLogger
    {
        #region Delegates

        public delegate void LogDelegate(string message);

        #endregion

        #region Events

        public event LogDelegate ErrorLogged;

        public event LogDelegate InfoLogged;

        public event LogDelegate WarningLogged;

        #endregion

        #region Public Methods and Operators

        public virtual void Error(string message)
        {
            this.OnError(message);
        }

        public virtual void Info(string message)
        {
            this.OnInfo(message);
        }

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