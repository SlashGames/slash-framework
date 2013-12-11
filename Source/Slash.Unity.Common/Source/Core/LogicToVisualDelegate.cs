// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LogicToVisualDelegate.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Unity.Common.Core
{
    using System.Collections.Generic;

    /// <summary>
    ///   Connection between logical events and the visual entity game objects.
    /// </summary>
    public sealed class LogicToVisualDelegate
    {
        #region Public Properties

        /// <summary>
        ///   Method names to call on the entity game object when the event occurs.
        ///   A set is used to make sure a method isn't called twice when the event occurs.
        /// </summary>
        public HashSet<string> CallbackNames { get; set; }

        /// <summary>
        ///   Logical event the visual behaviour is interested in.
        /// </summary>
        public object Event { get; set; }

        #endregion

        #region Public Methods and Operators

        public static LogicToVisualDelegate create(LogicToVisualDelegateAttribute delegateAttribute)
        {
            return new LogicToVisualDelegate
                {
                    Event = delegateAttribute.Event,
                    CallbackNames = new HashSet<string> { delegateAttribute.CallbackName }
                };
        }

        #endregion
    }
}