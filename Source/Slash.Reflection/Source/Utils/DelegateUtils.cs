// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DelegateUtils.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Reflection.Utils
{
    using System;

    public static class DelegateUtils
    {
        #region Public Methods and Operators

        public static Delegate CreateDelegate(Type type, object target, string method)
        {
            return Delegate.CreateDelegate(type, target, method);
        }

        #endregion
    }
}