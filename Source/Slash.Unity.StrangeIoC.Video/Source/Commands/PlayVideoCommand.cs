namespace Slash.Unity.StrangeIoC.Video.Commands
{
    using strange.extensions.command.impl;
    using Slash.Unity.StrangeIoC.Video.Services;
    using Slash.Unity.StrangeIoC.Video.Signals;

    public class PlayVideoCommand : Command
    {
        [Inject]
        public PlayVideoData Params { get; set; }

        [Inject]
        public IVideoPlayerService VideoPlayerService { get; set; }

        [Inject]
        public VideoPlayingSignal VideoPlayingSignal { get; set; }

        public override void Execute()
        {
            this.VideoPlayerService.SetLoop(this.Params.Identifier, this.Params.Loop);
            this.VideoPlayerService.SetTargets(this.Params.Identifier, this.Params.Targets);
            this.VideoPlayerService.Play(this.Params.Identifier);

            this.VideoPlayingSignal.Dispatch();
        }
    }
}