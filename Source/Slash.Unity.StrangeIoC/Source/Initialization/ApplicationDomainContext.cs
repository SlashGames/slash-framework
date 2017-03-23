using UnityEngine;

namespace Slash.Unity.StrangeIoC.Initialization
{
    public class ApplicationDomainContext : StrangeContext
    {
        protected ApplicationDomainContext(string domainName, MonoBehaviour view, bool autoMapping)
            : base(view, autoMapping)
        {
            this.Domain = domainName;
        }
    }
}