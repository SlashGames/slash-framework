namespace Slash.Unity.StrangeIoC.Video.Commands
{
    using strange.extensions.command.impl;
    using Slash.Unity.StrangeIoC.Video.Services;

    public class StopVideoCommand : Command
    {
        [Inject]
        public string Identifier { get; set; }

        [Inject]
        public IVideoPlayerService VideoPlayerService { get; set; }

        public override void Execute()
        {
            this.VideoPlayerService.Stop(this.Identifier);
        }
    }
}