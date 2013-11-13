// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ILogger.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Diagnostics.Logging
{
    using System;

    public interface ILogger
    {
        #region Public Properties

        bool IsDebugEnabled { get; }

        bool IsErrorEnabled { get; }

        bool IsFatalEnabled { get; }

        bool IsInfoEnabled { get; }

        bool IsWarnEnabled { get; }

        #endregion

        #region Public Methods and Operators

        void Debug(object message);

        void Debug(object message, Exception t);

        void DebugFormat(string format, params object[] args);

        void DebugFormat(IFormatProvider provider, string format, params object[] args);

        void Error(object message);

        void Error(object message, Exception t);

        void ErrorFormat(string format, params object[] args);

        void ErrorFormat(IFormatProvider provider, string format, params object[] args);

        void Fatal(object message);

        void Fatal(object message, Exception t);

        void FatalFormat(string format, params object[] args);

        void FatalFormat(IFormatProvider provider, string format, params object[] args);

        void Info(object message);

        void Info(object message, Exception t);

        void InfoFormat(string format, params object[] args);

        void InfoFormat(IFormatProvider provider, string format, params object[] args);

        void Warn(object message);

        void Warn(object message, Exception t);

        void WarnFormat(string format, params object[] args);

        void WarnFormat(IFormatProvider provider, string format, params object[] args);

        #endregion
    }
}