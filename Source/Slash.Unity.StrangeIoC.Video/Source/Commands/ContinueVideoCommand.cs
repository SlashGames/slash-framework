using strange.extensions.command.impl;
using SuprStijl.Buddy.Unity.Modules.Video.Services;
using SuprStijl.Buddy.Unity.Modules.Video.Signals;

namespace SuprStijl.Buddy.Unity.Modules.Video.Commands
{
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