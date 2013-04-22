// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ConsoleAppender.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

#if LOG4NET

namespace SlashGames.Unity.Common.Logging
{
    using UnityEngine;

    using log4net.Appender;
    using log4net.Core;

    public class ConsoleAppender : AppenderSkeleton
    {
        #region Methods

        protected override void Append(LoggingEvent loggingEvent)
        {
            string message = this.RenderLoggingEvent(loggingEvent);

            if (loggingEvent.Level == Level.Error)
            {
                Debug.LogError(message);
            }
            else if (loggingEvent.Level == Level.Warn)
            {
                Debug.LogWarning(message);
            }
            else
            {
                Debug.Log(message);
            }
        }

        #endregion
    }
}

#endif