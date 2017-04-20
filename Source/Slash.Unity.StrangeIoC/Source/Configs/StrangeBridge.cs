namespace Slash.Unity.StrangeIoC.Configs
{
    public abstract class StrangeBridge
    {
        /// <summary>
        ///   Called to initialize bridge.
        /// </summary>
        protected virtual void Init()
        {
        }

        /// <summary>
        ///   Called to deinitialize the bridge.
        /// </summary>
        protected virtual void Deinit()
        {
        }

        [Deconstruct]
        public void OnDeconstruct()
        {
            this.Deinit();
        }

        [PostConstruct]
        public void OnPostConstruct()
        {
            this.Init();
        }
    }
}