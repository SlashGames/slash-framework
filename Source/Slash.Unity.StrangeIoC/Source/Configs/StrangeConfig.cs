// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StrangeConfig.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Unity.StrangeIoC.Configs
{
    using System;
    using System.Collections.Generic;
    using strange.extensions.command.api;
    using strange.extensions.injector.api;
    using strange.extensions.mediation.api;
    using Slash.Reflection.Utils;
    using Slash.Unity.InspectorExt.PropertyDrawers;
    using Slash.Unity.StrangeIoC.Modules;
    using UnityEngine;
    using UnityEngine.Serialization;

    public abstract class StrangeConfig : MonoBehaviour, IModuleInstaller
    {
        [TypeProperty(BaseType = typeof(StrangeBridge))]
        public List<string> BridgeTypes;

        /// <summary>
        ///     Used features of this module.
        /// </summary>
        public List<StrangeConfig> Features;

        /// <summary>
        ///     Root view for module.
        /// </summary>
        public ModuleView ModuleView;

        /// <summary>
        ///     Scene to load for this module.
        /// </summary>
        [SerializeField]
        [FormerlySerializedAs("SceneName")]
        private string sceneName;

        /// <inheritdoc />
        public object SetupSettings
        {
            get { return null; }
        }

        /// <inheritdoc />
        public IEnumerable<IModuleInstaller> SubModules
        {
            get
            {
                if (this.Features != null)
                {
                    foreach (var feature in this.Features)
                    {
                        yield return feature;
                    }
                }
            }
        }

        /// <inheritdoc />
        public IEnumerable<Type> SubModuleTypes
        {
            get { yield break; }
        }

        /// <inheritdoc />
        public virtual void MapBindings(IInjectionBinder injectionBinder)
        {
        }

        /// <inheritdoc />
        public virtual void MapBindings(ICommandBinder commandBinder)
        {
        }

        /// <inheritdoc />
        public virtual void MapBindings(IMediationBinder mediationBinder)
        {
        }

        /// <inheritdoc />
        public virtual void UnmapCrossContextBindings(IInjectionBinder injectionBinder)
        {
        }

        /// <inheritdoc />
        public IEnumerable<Type> Bridges
        {
            get
            {
                if (this.BridgeTypes != null)
                {
                    foreach (var bridgeType in this.BridgeTypes)
                    {
                        yield return ReflectionUtils.FindType(bridgeType);
                    }
                }
            }
        }

        /// <inheritdoc />
        public string SceneName
        {
            get { return this.sceneName; }
            set { this.sceneName = value; }
        }
    }
}