namespace Slash.Unity.StrangeIoC.Video.Configs
{
    using strange.extensions.command.api;
    using strange.extensions.injector.api;
    using strange.extensions.mediation.api;
    using Slash.Unity.StrangeIoC.Configs;
    using Slash.Unity.StrangeIoC.Video.Commands;
    using Slash.Unity.StrangeIoC.Video.Services;
    using Slash.Unity.StrangeIoC.Video.Signals;
    using Slash.Unity.StrangeIoC.Video.Views;

    public class VideoConfig : StrangeConfig
    {
        public override void MapBindings(IInjectionBinder injectionBinder)
        {
            injectionBinder.Bind<LoadVideoSignal>().ToSingleton().CrossContext();
            injectionBinder.Bind<PlayVideoSignal>().ToSingleton().CrossContext();
            injectionBinder.Bind<StopVideoSignal>().ToSingleton().CrossContext();
            injectionBinder.Bind<PauseVideoSignal>().ToSingleton().CrossContext();
            injectionBinder.Bind<ContinueVideoSignal>().ToSingleton().CrossContext();
            injectionBinder.Bind<VideoLoadedSignal>().ToSingleton().CrossContext();
            injectionBinder.Bind<VideoPlayingSignal>().ToSingleton().CrossContext();
            injectionBinder.Bind<VideoPausedSignal>().ToSingleton().CrossContext();

            injectionBinder.Bind<IVideoPlayerService>().To<VideoPlayerService>().ToSingleton().CrossContext();
        }

        public override void MapBindings(ICommandBinder commandBinder)
        {
            commandBinder.Bind<LoadVideoSignal>().To<LoadVideoCommand>();
            commandBinder.Bind<PlayVideoSignal>().To<PlayVideoCommand>();
            commandBinder.Bind<PauseVideoSignal>().To<PauseVideoCommand>();
            commandBinder.Bind<ContinueVideoSignal>().To<ContinueVideoCommand>();
            commandBinder.Bind<StopVideoSignal>().To<StopVideoCommand>();
        }

        public override void MapBindings(IMediationBinder mediationBinder)
        {
            mediationBinder.BindView<PreloadVideoView>().ToMediator<PreloadVideoMediator>();
            mediationBinder.BindView<PlayVideoView>().ToMediator<PlayVideoMediator>();
        }
    }
}