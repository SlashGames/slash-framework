using strange.extensions.command.impl;
using SuprStijl.Buddy.Unity.Modules.Video.Services;

namespace SuprStijl.Buddy.Unity.Modules.Video.Commands
{
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