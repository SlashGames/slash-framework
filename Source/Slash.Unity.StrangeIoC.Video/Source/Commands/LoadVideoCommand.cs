namespace Slash.Unity.StrangeIoC.Video.Commands
{
    using strange.extensions.command.impl;
    using Slash.Unity.StrangeIoC.Video.Services;
    using Slash.Unity.StrangeIoC.Video.Signals;

    public class LoadVideoCommand : Command
    {
        [Inject]
        public LoadVideoParams Params { get; set; }

        [Inject]
        public IVideoPlayerService VideoPlayerService { get; set; }

        public override void Execute()
        {
            this.VideoPlayerService.Load(this.Params.Identifier, this.Params.Path, this.Params.AutoPlay);
        }
    }
}