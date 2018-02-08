﻿namespace Slash.Unity.StrangeIoC.Configs
{
    using System;
    using System.Collections.Generic;
    using strange.extensions.command.api;
    using strange.extensions.injector.api;
    using strange.extensions.mediation.api;

    public class StrangeModule : IModuleInstaller
    {
        private readonly List<Type> bridges;

        private readonly List<IModuleInstaller> subModules;

        private readonly List<Type> subModuleTypes;

        /// <summary>
        ///     Constructor.
        /// </summary>
        public StrangeModule()
        {
            this.bridges = new List<Type>();
            this.subModuleTypes = new List<Type>();
            this.subModules = new List<IModuleInstaller>();
        }

        /// <inheritdoc />
        public string SceneName { get; set; }

        /// <inheritdoc />
        public object SetupSettings { get; set; }

        /// <inheritdoc />
        public IEnumerable<IModuleInstaller> SubModules
        {
            get { return this.subModules; }
        }

        /// <inheritdoc />
        public IEnumerable<Type> SubModuleTypes
        {
            get { return this.subModuleTypes; }
        }

        /// <inheritdoc />
        public IEnumerable<Type> Bridges
        {
            get { return this.bridges; }
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

        protected void AddBridge<TBridge>() where TBridge : StrangeBridge
        {
            this.bridges.Add(typeof(TBridge));
        }

        protected void AddSubModule<TSubModule>(TSubModule subModule) where TSubModule : IModuleInstaller
        {
            this.subModules.Add(subModule);
        }

        protected void AddSubModule<TSubModule>() where TSubModule : IModuleInstaller
        {
            this.subModuleTypes.Add(typeof(TSubModule));
        }
    }
}