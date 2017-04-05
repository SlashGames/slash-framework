using strange.extensions.command.impl;
using SuprStijl.Buddy.Unity.Modules.Video.Services;
using SuprStijl.Buddy.Unity.Modules.Video.Signals;
using UnityEngine;

namespace SuprStijl.Buddy.Unity.Modules.Video.Commands
{
    public class PauseVideoCommand : Command
    {
        [Inject]
        public string Identifier { get; set; }

        [Inject]
        public VideoPausedSignal VideoPausedSignal { get; set; }

        [Inject]
        public IVideoPlayerService VideoPlayerService { get; set; }

        public override void Execute()
        {
            if (!this.VideoPlayerService.IsPlaying(this.Identifier))
            {
                Debug.Log("Video not playing, can't pause");
                return;
            }

            this.VideoPlayerService.Pause(this.Identifier);

            this.VideoPausedSignal.Dispatch();
        }
    }
}