using strange.extensions.command.api;
using strange.extensions.injector.api;
using strange.extensions.mediation.api;
using Slash.Unity.StrangeIoC.Configs;
using SuprStijl.Buddy.Unity.Modules.Video.Commands;
using SuprStijl.Buddy.Unity.Modules.Video.Services;
using SuprStijl.Buddy.Unity.Modules.Video.Signals;
using SuprStijl.Buddy.Unity.Modules.Video.Views;

namespace SuprStijl.Buddy.Unity.Modules.Video.Configs
{
    [UseStrangeConfig]
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