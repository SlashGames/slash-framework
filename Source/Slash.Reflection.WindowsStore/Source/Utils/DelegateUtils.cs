// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DelegateUtils.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Reflection.Utils
{
    using System;
    using System.Reflection;

    /// <summary>
    ///   Utility methods for operating on delegates.
    /// </summary>
    public static class DelegateUtils
    {
        #region Public Methods and Operators

        /// <summary>
        ///   Creates a delegate of the specified type that represents the
        ///   specified instance method to invoke on the passed target object.
        /// </summary>
        /// <param name="type">Type of the delegate to create.</param>
        /// <param name="target">Object to invoke the method on.</param>
        /// <param name="method">Instance method to invoke.</param>
        /// <returns>
        ///   delegate of the specified type that represents the
        ///   specified instance method to invoke on the passed target object.
        /// </returns>
        public static Delegate CreateDelegate(Type type, object target, string method)
        {
            var methodInfo = target.GetType().GetTypeInfo().GetDeclaredMethod(method);
            return methodInfo.CreateDelegate(type);
        }

        #endregion
    }
}