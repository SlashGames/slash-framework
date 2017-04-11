namespace Slash.Unity.StrangeIoC.Video.Commands
{
    using strange.extensions.command.impl;
    using Slash.Unity.StrangeIoC.Video.Services;
    using Slash.Unity.StrangeIoC.Video.Signals;

    public class ContinueVideoCommand : Command
    {
        [Inject]
        public string Identifier { get; set; }

        [Inject]
        public IVideoPlayerService VideoPlayerService { get; set; }

        [Inject]
        public VideoPlayingSignal VideoPlayingSignal { get; set; }

        public override void Execute()
        {
            this.VideoPlayerService.Play(this.Identifier);

            this.VideoPlayingSignal.Dispatch();
        }
    }
}