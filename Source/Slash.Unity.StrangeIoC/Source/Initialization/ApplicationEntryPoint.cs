namespace Slash.Unity.StrangeIoC.Initialization
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using strange.extensions.context.impl;
    using Slash.Reflection.Utils;
    using Slash.Unity.InspectorExt.PropertyDrawers;
    using Slash.Unity.StrangeIoC.Configs;
    using Slash.Unity.StrangeIoC.Modules;

    using UnityEngine;

    public class ApplicationEntryPoint : ApplicationEntryPoint<ApplicationDomainContext>
    {
    }

    public class ApplicationEntryPoint<TDomainContext> : ModuleView
        where TDomainContext : ApplicationDomainContext, new()
    {
        [TypeProperty(BaseType = typeof(StrangeBridge))]
        public List<string> BridgeTypes;

        /// <summary>
        ///   Main config for application.
        /// </summary>
        public StrangeConfig Config;

        private void Awake()
        {
            var domainContext = new TDomainContext {Config = this.Config};


            // Add bridges.
            foreach (var bridgeType in this.BridgeTypes)
            {
                if (bridgeType != null)
                {
                    domainContext.AddBridge(ReflectionUtils.FindType(bridgeType));
                }
            }

            domainContext.Init();
            domainContext.SetModuleView(this);

            Context.firstContext = this.context = domainContext;
        }

        private IEnumerator LaunchContextWhenReady(TDomainContext domainContext)
        {
            yield return new WaitUntil(() => domainContext.IsReadyToLaunch);

            domainContext.Launch();
        }

        private void Start()
        {
            this.context.Start();

            // Launch when ready.
            this.StartCoroutine(this.LaunchContextWhenReady((TDomainContext) this.context));
        }
    }
}