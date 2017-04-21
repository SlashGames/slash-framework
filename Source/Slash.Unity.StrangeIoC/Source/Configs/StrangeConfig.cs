// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StrangeConfig.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Unity.StrangeIoC.Configs
{
    using System.Collections.Generic;
    using strange.extensions.command.api;
    using strange.extensions.context.impl;
    using strange.extensions.injector.api;
    using strange.extensions.mediation.api;
    using Slash.Unity.Common.PropertyDrawers;
    using Slash.Unity.StrangeIoC.Modules;
    using Slash.Unity.StrangeIoC.Modules.Commands;
    using Slash.Unity.StrangeIoC.Modules.Signals;
    using UnityEngine;

    public abstract class StrangeConfig : MonoBehaviour
    {
        [TypeProperty(BaseType = typeof(StrangeBridge))]
        public List<string> BridgeTypes;

        /// <summary>
        ///     Dependencies to other modules.
        /// </summary>
        public List<StrangeConfig> Dependencies;

        /// <summary>
        ///     Root view for module.
        /// </summary>
        public ModuleView ModuleView;

        /// <summary>
        ///     Scene to load for this module.
        /// </summary>
        public string SceneName { get; set; }

        /// <summary>
        ///     Maps bindings to the injection binder.
        /// </summary>
        /// <param name="injectionBinder">Binder to map to.</param>
        public virtual void MapBindings(IInjectionBinder injectionBinder)
        {
        }

        /// <summary>
        ///     Maps bindings to the command binder.
        /// </summary>
        /// <param name="commandBinder">Binder to map to.</param>
        public virtual void MapBindings(ICommandBinder commandBinder)
        {
        }

        /// <summary>
        ///     Maps bindings to the mediation binder.
        /// </summary>
        /// <param name="mediationBinder">Binder to map to.</param>
        public virtual void MapBindings(IMediationBinder mediationBinder)
        {
        }
    }
}