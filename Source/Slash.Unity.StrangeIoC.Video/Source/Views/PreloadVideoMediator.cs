namespace Slash.Unity.StrangeIoC.Video.Views
{
    using System.Linq;
    using strange.extensions.mediation.impl;
    using Slash.Unity.Common.Streaming;
    using Slash.Unity.StrangeIoC.Video.Signals;
    using UnityEngine;

    public class PreloadVideoMediator : Mediator
    {
        [Inject]
        public LoadVideoSignal LoadVideoSignal { get; set; }

        [Inject]
        public PreloadVideoView View { get; set; }

        public override void OnRegister()
        {
            base.OnRegister();

            if (this.View.isActiveAndEnabled)
            {
                // Preload videos.
                foreach (var preloadVideo in this.View.Videos)
                {
                    var platformVideoResource =
                        preloadVideo.VideoResource.FirstOrDefault(
                            platformPath => platformPath.Platform == Application.platform);
                    if (platformVideoResource != null)
                    {
                        // Loading a video from streaming assets expects the relative instead of the full path on Android.
                        var videoPath = platformVideoResource.Path.Location !=
                                        ExternalResource.ResourceLocation.StreamingAssets
                            ? platformVideoResource.Path.FullPath
                            : platformVideoResource.Path.Path;
                        this.LoadVideoSignal.Dispatch(new LoadVideoParams
                        {
                            Identifier = preloadVideo.Identifier,
                            Path = videoPath,
                            AutoPlay = false
                        });
                    }
                }
            }
        }
    }
}