// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AggregateException.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.SystemExt.Exceptions
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Diagnostics;
    using System.Linq;
    using System.Text;

    /// <summary>
    ///   Wraps multiple inner exceptions.
    ///   http://msdn.microsoft.com/en-us/magazine/ee321571.aspx
    /// </summary>
    [DebuggerDisplay("Count = {InnerExceptions.Count}")]
    public class AggregateException : Exception
    {
        #region Constants

        private const string DefaultMessage = "Multiple exceptions have occurred.";

        #endregion

        #region Fields

        private readonly IList<Exception> innerExceptions;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///   Constructs a new, empty aggregate exception.
        /// </summary>
        public AggregateException()
            : this(new List<Exception>())
        {
        }

        /// <summary>
        ///   Constructs a new aggreate exception wrapping the passed inner exceptions.
        /// </summary>
        /// <param name="innerExceptions">Inner exceptions to wrap.</param>
        public AggregateException(params Exception[] innerExceptions)
            : this(new List<Exception>(innerExceptions))
        {
        }

        /// <summary>
        ///   Constructs a new aggreate exception wrapping the passed inner exceptions.
        /// </summary>
        /// <param name="innerExceptions">Inner exceptions to wrap.</param>
        public AggregateException(IEnumerable<Exception> innerExceptions)
            : this(DefaultMessage, innerExceptions)
        {
        }

        /// <summary>
        ///   Constructs a new, empty aggregate exception with the specified detail message.
        /// </summary>
        /// <param name="message">Error message that explains the reason for the exception.</param>
        public AggregateException(string message)
            : this(message, new List<Exception>())
        {
        }

        /// <summary>
        ///   Constructs a new aggregate exception to wrap the passed inner exception with the specified detail message.
        /// </summary>
        /// <param name="message">Error message that explains the reason for the exception.</param>
        /// <param name="innerException">Exception to wrap.</param>
        public AggregateException(string message, Exception innerException)
            : this(message, new List<Exception> { innerException })
        {
        }

        /// <summary>
        ///   Constructs a new aggreate exception wrapping the passed inner exceptions with the specified detail message.
        /// </summary>
        /// <param name="message">Error message that explains the reason for the exception.</param>
        /// <param name="innerExceptions">Inner exceptions to wrap.</param>
        public AggregateException(string message, params Exception[] innerExceptions)
            : this(message, new List<Exception>(innerExceptions))
        {
        }

        /// <summary>
        ///   Constructs a new aggreate exception wrapping the passed inner exceptions with the specified detail message.
        /// </summary>
        /// <param name="message">Error message that explains the reason for the exception.</param>
        /// <param name="innerExceptions">Inner exceptions to wrap.</param>
        public AggregateException(string message, IEnumerable<Exception> innerExceptions)
            : base(message, innerExceptions.FirstOrDefault())
        {
            this.innerExceptions = innerExceptions.ToList();
            this.Messages = this.GetMessages();
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///   Wrapped exceptions.
        /// </summary>
        public ReadOnlyCollection<Exception> InnerExceptions
        {
            get
            {
                return new ReadOnlyCollection<Exception>(this.innerExceptions);
            }
        }

        /// <summary>
        ///   Get the messages describing the inner exceptions, one per line.
        /// </summary>
        public string Messages { get; private set; }

        #endregion

        #region Methods

        private string GetMessages()
        {
            StringBuilder stringBuilder = new StringBuilder();

            foreach (var exception in this.innerExceptions)
            {
                stringBuilder.AppendLine(exception.Message);
            }

            return stringBuilder.ToString();
        }

        #endregion
    }
}