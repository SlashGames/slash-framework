namespace Slash.Unity.StrangeIoC.Initialization
{
    using Slash.Unity.StrangeIoC.Initialization.Commands;
    using Slash.Unity.StrangeIoC.Initialization.Signals;

    public class ApplicationContext : StrangeContext
    {
        public override void Launch()
        {
            base.Launch();

            var startSignal = this.injectionBinder.GetInstance<ApplicationStartSignal>();
            startSignal.Dispatch();
        }

        protected override void mapBindings()
        {
            // Setup application lifecycle.
            this.injectionBinder.Bind<ApplicationStartSignal>().ToSingleton();
            this.CommandBinder.Bind<ApplicationStartSignal>().To<ApplicationStartCommand>().InSequence();

            //// Create window manager.
            //var windowManager = new WindowManager((MonoBehaviour)this.GetContextView());
            //this.injectionBinder.Bind<WindowManager>().ToValue(windowManager).ToSingleton();

            //// Setup windows module.
            //var windowsModule = new WindowsModule();
            //windowsModule.Init(this.CommandBinder);

            //// Setup logging.
            //var configAsset = Resources.Load<TextAsset>("log4net");
            //if (configAsset != null)
            //{
            //    Logger.Configure(new MemoryStream(configAsset.bytes));
            //    var logger = new Logger(typeof(ILogger));
            //    this.injectionBinder.Bind<ILogger>().To(logger);
            //}

            base.mapBindings();
        }
    }
}