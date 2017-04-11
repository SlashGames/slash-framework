namespace Slash.Unity.StrangeIoC.Initialization
{
    using System.Collections.Generic;
    using System.Linq;

    using strange.extensions.context.impl;

    using Slash.Reflection.Utils;
    using Slash.Unity.Common.PropertyDrawers;
    using Slash.Unity.StrangeIoC.Configs;

    public class ApplicationEntryPoint<TDomainContext> : ContextView
        where TDomainContext : ApplicationDomainContext, new()
    {
        [TypeProperty(BaseType = typeof(StrangeBridge))]
        public List<string> BridgeTypes;

        private void Awake()
        {
            var domainContext = new TDomainContext
            {
                BridgeTypes = this.BridgeTypes.Select(ReflectionUtils.FindType).ToList(),
                Modules = this.gameObject.GetComponentsInChildren<StrangeConfig>().ToList()
            };
            domainContext.Init(this);

            this.context = domainContext;
            this.context.Start();
        }
    }
}