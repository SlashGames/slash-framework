namespace Slash.Unity.StrangeIoC.Initialization
{
    using System;
    using System.Collections.Generic;

    using Slash.Unity.StrangeIoC.Configs;

    public class ApplicationDomainContext : StrangeContext
    {
        protected ApplicationDomainContext(string domainName)
        {
            this.Domain = domainName;
        }

        /// <summary>
        ///   Bridge types to use.
        /// </summary>
        public List<Type> BridgeTypes { get; set; }

        /// <summary>
        ///   Application modules.
        /// </summary>
        public List<StrangeConfig> Modules { get; set; }

        /// <summary>
        ///   Initializes the domain context.
        /// </summary>
        /// <param name="domainView">View of domain.</param>
        public void Init(object domainView)
        {
            //If firstContext was unloaded, the contextView will be null. Assign the new context as firstContext.
            if (firstContext == null || firstContext.GetContextView() == null)
            {
                firstContext = this;
            }
            else
            {
                firstContext.AddContext(this);
            }
            this.SetContextView(domainView);
            this.addCoreComponents();
            this.autoStartup = true;
        }

        /// <inheritdoc />
        protected override void instantiateCoreComponents()
        {
            base.instantiateCoreComponents();

            if (this.Modules != null)
            {
                // Add modules.
                foreach (var module in this.Modules)
                {
                    this.UseConfig(module);
                }
            }

            if (this.BridgeTypes != null)
            {
                // Add bridges.
                foreach (var bridgeType in this.BridgeTypes)
                {
                    if (bridgeType != null)
                    {
                        this.UseBridge(bridgeType);
                    }
                }
            }
        }
    }
}