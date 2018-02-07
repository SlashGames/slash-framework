namespace Slash.Unity.StrangeIoC.Configs
{
    using System;
    using System.Collections.Generic;
    using strange.extensions.command.api;
    using strange.extensions.injector.api;
    using strange.extensions.mediation.api;

    public class StrangeModule : IModuleInstaller
    {
        private List<Type> bridges;

        /// <summary>
        ///     Constructor.
        /// </summary>
        public StrangeModule()
        {
            this.bridges = new List<Type>();
        }

        /// <inheritdoc />
        public string SceneName { get; set; }

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
            if (this.bridges == null)
            {
                this.bridges = new List<Type>();
            }

            this.bridges.Add(typeof(TBridge));
        }
    }
}