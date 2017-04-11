// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ApplicationContext.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Unity.StrangeIoC.Initialization
{
    using System;
    using System.Collections.Generic;

    using strange.extensions.command.api;
    using strange.extensions.command.impl;
    using strange.extensions.context.impl;

    using Slash.Reflection.Utils;
    using Slash.Unity.StrangeIoC.Configs;

    using UnityEngine;

    public class StrangeContext : MVCSContext
    {
        protected readonly MonoBehaviour view;

        private readonly List<Type> bridgeTypes;

        /// <summary>
        ///   Registered modules.
        /// </summary>
        private readonly List<StrangeConfig> modules;

        public StrangeContext(MonoBehaviour view, bool autoMapping)
            : base(view, autoMapping)
        {
            this.view = view;
            this.bridgeTypes = new List<Type>();
            this.modules = new List<StrangeConfig>();
        }

        protected string Domain { get; set; }

        /// <inheritdoc />
        public override void Launch()
        {
            base.Launch();

            // Setup views for modules.
            foreach (var module in this.modules)
            {
                module.SetupView();
            }
        }

        /// <inheritdoc />
        protected override void addCoreComponents()
        {
            base.addCoreComponents();

            // Enable signals.
            this.injectionBinder.Unbind<ICommandBinder>();
            this.injectionBinder.Bind<ICommandBinder>().To<SignalCommandBinder>().ToSingleton();
        }

        /// <inheritdoc />
        protected override void mapBindings()
        {
            // Get and use feature configs.
            ReflectionUtils.HandleTypesWithAttribute<UseStrangeConfigAttribute>(this.UseConfig);

            base.mapBindings();
        }

        /// <inheritdoc />
        protected override void postBindings()
        {
            base.postBindings();

            // Fire up bridges.
            foreach (var bridgeType in this.bridgeTypes)
            {
                this.injectionBinder.GetInstance(bridgeType);
            }
        }

        protected void UseConfig<T>(Type[] configBridges = null) where T : StrangeConfig, new()
        {
            this.UseConfig(typeof(T), configBridges);
        }

        protected void UseConfig(Type configType, Type[] configBridges)
        {
            var config = Activator.CreateInstance(configType) as StrangeConfig;
            if (config == null)
            {
                Debug.LogErrorFormat(
                    "Config doesn't derive from StrangeConfig, so bindings can't be mapped: " + configType);
                return;
            }

            this.UseConfig(config);

            // Instantiate bridges.
            if (configBridges != null)
            {
                foreach (var bridgeType in configBridges)
                {
                    this.injectionBinder.Bind(bridgeType).ToSingleton();
                    this.bridgeTypes.Add(bridgeType);
                }
            }
        }

        protected void UseConfig(StrangeConfig config)
        {
            config.MapBindings(this.injectionBinder);
            config.MapBindings(this.commandBinder);
            config.MapBindings(this.mediationBinder);

            this.modules.Add(config);
        }

        private void UseConfig(Type featureConfigType, UseStrangeConfigAttribute configAttribute)
        {
            if (string.IsNullOrEmpty(configAttribute.Domain) || configAttribute.Domain == this.Domain)
            {
                this.UseConfig(featureConfigType, configAttribute.Bridges);
            }
        }
    }
}