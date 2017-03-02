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

        private List<Type> bridgeTypes;

        public StrangeContext(MonoBehaviour view, bool autoMapping)
            : base(view, autoMapping)
        {
            this.view = view;
        }

        protected string Domain { get; set; }

        protected override void addCoreComponents()
        {
            base.addCoreComponents();

            // Enable signals.
            this.injectionBinder.Unbind<ICommandBinder>();
            this.injectionBinder.Bind<ICommandBinder>().To<SignalCommandBinder>().ToSingleton();
        }

        protected override void mapBindings()
        {
            this.bridgeTypes = new List<Type>();

            // Get and use feature configs.
            ReflectionUtils.HandleTypesWithAttribute<UseStrangeConfigAttribute>(this.UseConfig);

            base.mapBindings();
        }

        protected override void postBindings()
        {
            base.postBindings();

            // Fire up bridges.
            foreach (var bridgeType in this.bridgeTypes)
            {
                this.injectionBinder.GetInstance(bridgeType);
            }
        }

        protected void UseConfig<T>() where T : StrangeConfig, new()
        {
            this.UseConfig(typeof(T));
        }

        protected void UseConfig(Type configType)
        {
            var config = Activator.CreateInstance(configType) as StrangeConfig;
            if (config == null)
            {
                Debug.LogErrorFormat(
                    "Config doesn't derive from StrangeConfig, so bindings can't be mapped: " + configType);
                return;
            }

            config.MapBindings(this.injectionBinder);
            config.MapBindings(this.commandBinder);
            config.MapBindings(this.mediationBinder);
        }

        private void UseConfig(Type featureConfigType, UseStrangeConfigAttribute configAttribute)
        {
            if (string.IsNullOrEmpty(configAttribute.Domain) || configAttribute.Domain == this.Domain)
            {
                this.UseConfig(featureConfigType);

                // Instantiate bridges.
                if (configAttribute.Bridges != null)
                {
                    foreach (var bridgeType in configAttribute.Bridges)
                    {
                        this.injectionBinder.Bind(bridgeType).ToSingleton();
                        this.bridgeTypes.Add(bridgeType);
                    }
                }
            }
        }
    }
}