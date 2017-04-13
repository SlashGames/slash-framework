namespace Slash.Unity.StrangeIoC.Modules
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    using strange.extensions.context.impl;

    using Slash.Reflection.Utils;
    using Slash.Unity.Common.PropertyDrawers;
    using Slash.Unity.StrangeIoC.Configs;

    using UnityEngine;

    public class ModuleEntryPoint : ContextView
    {
        [TypeProperty(BaseType = typeof(StrangeBridge))]
        public List<string> BridgeTypes;

        private void Awake()
        {
            var moduleContext = new ModuleContext();

            // Add sub modules.
            var subModuleConfigs = this.gameObject.GetComponentsInChildren<StrangeConfig>().ToList();
            foreach (var subModuleConfig in subModuleConfigs)
            {
                moduleContext.AddSubModule(subModuleConfig);
            }

            // Add bridges.
            foreach (var bridgeType in this.BridgeTypes)
            {
                if (bridgeType != null)
                {
                    moduleContext.AddBridge(ReflectionUtils.FindType(bridgeType));
                }
            }

            this.context = moduleContext;

            moduleContext.Init();
            moduleContext.SetContextView(this);
            moduleContext.Start();

            // Launch when ready.
            this.StartCoroutine(this.LaunchContextWhenReady(moduleContext));
        }

        private IEnumerator LaunchContextWhenReady(ModuleContext domainContext)
        {
            yield return new WaitUntil(() => domainContext.IsReadyToLaunch);

            domainContext.Launch();
        }
    }
}