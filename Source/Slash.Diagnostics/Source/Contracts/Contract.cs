// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Contract.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Diagnostics.Contracts
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;

    using Slash.Diagnostics.ReSharper.Annotations;

    /// <summary>
    ///   Own version of C# Code Contracts to use before .NET 4.0
    ///   Contains static methods for representing program contracts such as preconditions, postconditions, and object invariants.
    ///   Just the basic functionalities are implemented.
    /// </summary>
    public static class Contract
    {
        #region Public Methods and Operators

        /// <summary>
        ///   Specifies a precondition contract for the enclosing method or property, and throws an exception with the provided message if the condition for the contract fails.
        /// </summary>
        /// <typeparam name="TException">The exception to throw if the condition is false.</typeparam>
        /// <param name="condition">The conditional expression to test.</param>
        /// <param name="userMessage">The message to display if the condition is false.</param>
        [ContractAnnotation("condition:false => halt")]
        public static void Requires<TException>(bool condition, string userMessage) where TException : Exception
        {
            if (condition)
            {
                return;
            }

            Exception exception = (Exception)Activator.CreateInstance(typeof(TException), userMessage);
            throw exception;
        }

        /// <summary>
        ///   Specifies a precondition contract for the enclosing method or property, and throws an exception with the provided parameters if the condition for the contract fails.
        /// </summary>
        /// <typeparam name="TException">The exception to throw if the condition is false.</typeparam>
        /// <param name="condition">The conditional expression to test.</param>
        /// <param name="args">Arguments to create exception from.</param>
        [ContractAnnotation("condition:false => halt")]
        public static void Requires<TException>(bool condition, params object[] args) where TException : Exception
        {
            if (condition)
            {
                return;
            }

            Exception exception = (Exception)Activator.CreateInstance(typeof(TException), args);
            throw exception;
        }

        /// <summary>
        ///   Checks if the specified container of variables are not null.
        /// </summary>
        /// <example>
        ///   Contract.RequiresNotNull(new { variable }, "Variable is null.");
        /// </example>
        /// <typeparam name="T">Type of variables.</typeparam>
        /// <param name="container">Variable(s) container.</param>
        /// <param name="userMessage">The message to display if one of the variables is null.</param>
        [ContractAnnotation("container:null => halt")]
        public static void RequiresNotNull<T>(T container, string userMessage) where T : class
        {
            if (container == null)
            {
                throw new ArgumentNullException("container", userMessage);
            }
            NullChecker<T>.Check(container, userMessage);
        }

        #endregion

        private static class NullChecker<T>
            where T : class
        {
            #region Static Fields

            private static readonly List<Func<T, bool>> Checkers;

            private static readonly List<string> names;

            #endregion

            #region Constructors and Destructors

            static NullChecker()
            {
                Checkers = new List<Func<T, bool>>();
                names = new List<string>();
                // We can't rely on the order of the properties, but we
                // can rely on the order of the constructor parameters
                // in an anonymous type - and that there'll only be
                // one constructor.
                foreach (string name in typeof(T).GetConstructors()[0].GetParameters().Select(p => p.Name))
                {
                    names.Add(name);
                    PropertyInfo property = typeof(T).GetProperty(name);
                    // I've omitted a lot of error checking, but here's
                    // at least one bit...
                    if (property.PropertyType.IsValueType)
                    {
                        throw new ArgumentException("Property " + property + " is a value type");
                    }
                    ParameterExpression param = Expression.Parameter(typeof(T), "container");
                    Expression propertyAccess = Expression.Property(param, property);
                    Expression nullValue = Expression.Constant(null, property.PropertyType);
                    Expression equality = Expression.Equal(propertyAccess, nullValue);
                    var lambda = Expression.Lambda<Func<T, bool>>(equality, param);
                    Checkers.Add(lambda.Compile());
                }
            }

            #endregion

            #region Methods

            /// <summary>
            ///   Checks if the specified item is null.
            /// </summary>
            /// <param name="item">Item to check.</param>
            /// <param name="userMessage">The message to display if the item is null.</param>
            internal static void Check(T item, string userMessage)
            {
                for (int i = 0; i < Checkers.Count; i++)
                {
                    if (Checkers[i](item))
                    {
                        throw new ArgumentNullException(names[i], userMessage);
                    }
                }
            }

            #endregion
        }
    }
}