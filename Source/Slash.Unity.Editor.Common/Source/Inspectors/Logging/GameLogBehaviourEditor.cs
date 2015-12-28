// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GameLogBehaviourEditor.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Unity.Editor.Common.Inspectors.Logging
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Slash.ECS.Events;
    using Slash.Reflection.Utils;
    using Slash.Unity.Common.Logging;

    using UnityEditor;

    [CustomEditor(typeof(GameLogBehaviour))]
    public class GameLogBehaviourEditor : EventManagerLogBehaviourEditor
    {
        #region Fields

        /// <summary>
        ///   Cached event types.
        /// </summary>
        private List<Type> eventTypes;

        #endregion

        #region Properties

        /// <summary>
        ///   Event types which can be configured.
        /// </summary>
        protected override IEnumerable<Type> EventTypes
        {
            get
            {
                return this.eventTypes
                       ?? (this.eventTypes = ReflectionUtils.FindTypesWithAttribute<GameEventTypeAttribute>().ToList());
            }
        }

        #endregion
    }
}