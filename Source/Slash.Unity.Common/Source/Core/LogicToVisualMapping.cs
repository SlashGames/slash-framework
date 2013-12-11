// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LogicToVisualMapping.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Unity.Common.Core
{
    using System;

    using Slash.GameBase.Components;

    public sealed class LogicToVisualMapping
    {
        #region Public Properties

        /// <summary>
        ///   Type of logic component which gets visualized.
        /// </summary>
        public Type LogicType { get; private set; }

        /// <summary>
        ///   Type of mono behaviour to visualize logic.
        /// </summary>
        public Type VisualType { get; private set; }

        #endregion

        #region Public Methods and Operators

        public static LogicToVisualMapping create<TLogic, TVisual>() where TLogic : IEntityComponent
        {
            return new LogicToVisualMapping { LogicType = typeof(TLogic), VisualType = typeof(TVisual) };
        }

        public static LogicToVisualMapping create(Type logicType, Type visualType)
        {
            if (!typeof(IEntityComponent).IsAssignableFrom(logicType))
            {
                throw new ArgumentException(
                    String.Format("Logic type '{0}' doesn't implement IEntityComponent.", logicType), "logicType");
            }

            return new LogicToVisualMapping { LogicType = logicType, VisualType = visualType };
        }

        public override string ToString()
        {
            return String.Format("LogicType: {0}, VisualType: {1}", this.LogicType, this.VisualType);
        }

        #endregion
    }
}