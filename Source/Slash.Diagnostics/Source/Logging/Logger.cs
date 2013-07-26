// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Logger.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Diagnostics.Logging
{
    using System;
    using System.IO;

#if LOG4NET
    using log4net;
    using log4net.Config;
#endif

    /// <summary>
    ///   This is a very simply class to abstract the use of log4net.
    ///   If log4net is included in the project, you simply use this
    ///   class as it is. If you remove the log4net.dll from your
    ///   project, all you have to do is uncomment
    ///   #define LOG4NET (say //#define LOG4NET)
    ///   at the top of this file.
    ///   You should definitely remove log4net when building Web players
    ///   as it increases the size significantly (around 1 to 2 MB!)
    /// </summary>
    /// <author>Jashan Chittesh - jc (you know it) ramtiga (dot) com</author>
    public class Logger
    {
        private const string StartMessage =
            "Logger configured\nlog4net configuration file:\n{0}\n\n" + "    =======================================\n"
            + "    === Logging configured successfully ===\n" + "    =======================================\n";

#if LOG4NET
        private readonly ILog log;
#endif

        /// <summary>
        ///   A logger to be used for logging statements in the code.
        ///   It is recommended to follow a pattern for instantiating this:
        ///   <code>private static readonly JCsLogger log = new JCsLogger(typeof(YourClassName));
        ///     ...
        ///     log.*(yourLoggingStuff); // Debug/Info/Warn/Error/Fatal[Format]</code>
        /// </summary>
        /// <param name="type"> the type that is using this logger </param>
        public Logger(Type type)
        {
#if LOG4NET
            this.log = LogManager.GetLogger(type);
#endif
        }

        public static void Configure(string configFile)
        {
#if LOG4NET
            FileInfo fileInfo = new FileInfo(configFile);
            XmlConfigurator.Configure(fileInfo);
            LogManager.GetLogger(typeof(Logger)).InfoFormat(StartMessage, configFile);
#endif
        }

        #region Test if a level is enabled for logging - Only works when log4net is active!

        public bool IsDebugEnabled
        {
            get
            {
#if LOG4NET
                bool result = false;
                result = this.log.IsDebugEnabled;
                return result;
#else
                return false;
#endif
            }
        }

        public bool IsInfoEnabled
        {
            get
            {
#if LOG4NET
                bool result = false;
                result = this.log.IsInfoEnabled;
                return result;
#else
                return false;
#endif
            }
        }

        public bool IsWarnEnabled
        {
            get
            {
#if LOG4NET
                bool result = false;
                result = this.log.IsWarnEnabled;
                return result;
#else
                return false;
#endif
            }
        }

        public bool IsErrorEnabled
        {
            get
            {
#if LOG4NET
                bool result = false;
                result = this.log.IsErrorEnabled;
                return result;
#else
                return false;
#endif
            }
        }

        public bool IsFatalEnabled
        {
            get
            {
#if LOG4NET
                bool result = false;
                result = this.log.IsFatalEnabled;
                return result;
#else
                return false;
#endif
            }
        }

        #endregion

        public void Debug(object message)
        {
#if LOG4NET
            this.log.Debug(message);
#endif
        }

        public void Info(object message)
        {
#if LOG4NET
            this.log.Info(message);
#endif
        }

        public void Warn(object message)
        {
#if LOG4NET
            this.log.Warn(message);
#endif
        }

        public void Error(object message)
        {
#if LOG4NET
            this.log.Error(message);
#endif
        }

        public void Fatal(object message)
        {
#if LOG4NET
            this.log.Fatal(message);
#endif
        }

        public void Debug(object message, Exception t)
        {
#if LOG4NET
            this.log.Debug(message, t);
#endif
        }

        public void Info(object message, Exception t)
        {
#if LOG4NET
            this.log.Info(message, t);
#endif
        }

        public void Warn(object message, Exception t)
        {
#if LOG4NET
            this.log.Warn(message, t);
#endif
        }

        public void Error(object message, Exception t)
        {
#if LOG4NET
            this.log.Error(message, t);
#endif
        }

        public void Fatal(object message, Exception t)
        {
#if LOG4NET
            this.log.Fatal(message, t);
#endif
        }

        public void DebugFormat(string format, params object[] args)
        {
#if LOG4NET
            this.log.DebugFormat(format, args);
#endif
        }

        public void InfoFormat(string format, params object[] args)
        {
#if LOG4NET
            this.log.InfoFormat(format, args);
#endif
        }

        public void WarnFormat(string format, params object[] args)
        {
#if LOG4NET
            this.log.WarnFormat(format, args);
#endif
        }

        public void ErrorFormat(string format, params object[] args)
        {
#if LOG4NET
            this.log.ErrorFormat(format, args);
#endif
        }

        public void FatalFormat(string format, params object[] args)
        {
#if LOG4NET
            this.log.FatalFormat(format, args);
#endif
        }

        public void DebugFormat(IFormatProvider provider, string format, params object[] args)
        {
#if LOG4NET
            this.log.DebugFormat(provider, format, args);
#endif
        }

        public void InfoFormat(IFormatProvider provider, string format, params object[] args)
        {
#if LOG4NET
            this.log.InfoFormat(provider, format, args);
#endif
        }

        public void WarnFormat(IFormatProvider provider, string format, params object[] args)
        {
#if LOG4NET
            this.log.WarnFormat(provider, format, args);
#endif
        }

        public void ErrorFormat(IFormatProvider provider, string format, params object[] args)
        {
#if LOG4NET
            this.log.ErrorFormat(provider, format, args);
#endif
        }

        public void FatalFormat(IFormatProvider provider, string format, params object[] args)
        {
#if LOG4NET
            this.log.FatalFormat(provider, format, args);
#endif
        }
    }
}