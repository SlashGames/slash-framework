namespace Slash.Unity.StrangeIoC.Initialization
{
    public class ApplicationDomainContext : StrangeContext
    {
        public ApplicationDomainContext()
            : this("Default")
        {
        }

        protected ApplicationDomainContext(string domainName)
        {
            this.Domain = domainName;
        }
    }
}