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

    using Slash.Unity.StrangeIoC.Configs;

    using UnityEngine;

    public class StrangeContext : MVCSContext
    {
        /// <summary>
        ///   Registered bridges.
        /// </summary>
        private readonly List<Type> bridgeTypes;

        /// <summary>
        ///   Registered modules.
        /// </summary>
        private readonly List<StrangeConfig> modules;

        /// <summary>
        ///   Default constructor.
        /// </summary>
        public StrangeContext()
        {
            this.bridgeTypes = new List<Type>();
            this.modules = new List<StrangeConfig>();
        }

        /// <summary>
        ///   Constructor.
        /// </summary>
        /// <param name="view">View that fired up the context.</param>
        /// <param name="autoMapping">Indicates if auto mapping should be used.</param>
        public StrangeContext(MonoBehaviour view, bool autoMapping)
            : base(view, autoMapping)
        {
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
            base.mapBindings();

            // Map bindings for modules.
            foreach (var module in this.modules)
            {
                module.MapBindings(this.injectionBinder);
                module.MapBindings(this.commandBinder);
                module.MapBindings(this.mediationBinder);
            }
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

        protected void UseBridge(Type bridgeType)
        {
            this.injectionBinder.Bind(bridgeType).ToSingleton();
            this.bridgeTypes.Add(bridgeType);
        }

        protected void UseConfig(StrangeConfig config)
        {
            this.modules.Add(config);
        }
    }
}