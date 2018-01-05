// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LogicToVisualDelegateAttribute.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Unity.Common.ECS
{
    using System;

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class LogicToVisualDelegateAttribute : Attribute
    {
        #region Public Properties

        public string CallbackName { get; set; }

        public object Event { get; set; }

        #endregion
    }
}