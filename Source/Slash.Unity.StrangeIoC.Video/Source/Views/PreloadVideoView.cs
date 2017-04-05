using System;
using System.Collections.Generic;
using strange.extensions.mediation.impl;
using Slash.Unity.VR.Streaming;

namespace SuprStijl.Buddy.Unity.Modules.Video.Views
{
    [Serializable]
    public class PreloadVideo
    {
        public string Identifier;

        public List<PlatformPath> VideoResource;
    }

    public class PreloadVideoView : View
    {
        public List<PreloadVideo> Videos;
    }
}