using strange.extensions.command.impl;
using SuprStijl.Buddy.Unity.Modules.Video.Services;
using SuprStijl.Buddy.Unity.Modules.Video.Signals;

namespace SuprStijl.Buddy.Unity.Modules.Video.Commands
{
    public class LoadVideoCommand : Command
    {
        [Inject]
        public IVideoPlayerService VideoPlayerService { get; set; }

        [Inject]
        public LoadVideoParams Params { get; set; }

        public override void Execute()
        {
            this.VideoPlayerService.Load(this.Params.Identifier, this.Params.Path, this.Params.AutoPlay);
        }
    }
}