// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DelegateUtils.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Reflection.Utils
{
    using System;
    using System.Reflection;

    public static class DelegateUtils
    {
        #region Public Methods and Operators

        public static Delegate CreateDelegate(Type type, object target, string method)
        {
            var methodInfo = target.GetType().GetTypeInfo().GetDeclaredMethod(method);
            return methodInfo.CreateDelegate(type);
        }

        #endregion
    }
}