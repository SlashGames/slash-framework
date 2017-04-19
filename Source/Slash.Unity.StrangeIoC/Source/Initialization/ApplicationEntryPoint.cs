namespace Slash.Unity.StrangeIoC.Initialization
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    using strange.extensions.context.impl;

    using Slash.Reflection.Utils;
    using Slash.Unity.Common.PropertyDrawers;
    using Slash.Unity.StrangeIoC.Configs;

    using UnityEngine;

    public class ApplicationEntryPoint : ApplicationEntryPoint<ApplicationDomainContext>
    {
    }

    public class ApplicationEntryPoint<TDomainContext> : ContextView
        where TDomainContext : ApplicationDomainContext, new()
    {
        [TypeProperty(BaseType = typeof(StrangeBridge))]
        public List<string> BridgeTypes;

        private void Awake()
        {
            var domainContext = new TDomainContext();
            
            // Add sub modules.
            var subModuleConfigs = this.gameObject.GetComponentsInChildren<StrangeConfig>().ToList();
            foreach (var subModuleConfig in subModuleConfigs)
            {
                domainContext.AddSubModule(subModuleConfig);
            }

            // Add bridges.
            foreach (var bridgeType in this.BridgeTypes)
            {
                if (bridgeType != null)
                {
                    domainContext.AddBridge(ReflectionUtils.FindType(bridgeType));
                }
            }

            domainContext.Init();
            domainContext.SetContextView(this);

            this.context = domainContext;
        }

        private void Start()
        {
            this.context.Start();

            // Launch when ready.
            this.StartCoroutine(this.LaunchContextWhenReady((TDomainContext) this.context));
        }

        private IEnumerator LaunchContextWhenReady(TDomainContext domainContext)
        {
            yield return new WaitUntil(() => domainContext.IsReadyToLaunch);

            domainContext.Launch();
        }
    }
}