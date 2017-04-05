using System;
using System.Collections.Generic;
using strange.extensions.mediation.impl;

namespace SuprStijl.Buddy.Unity.Modules.Video.Views
{
    using Slash.Unity.Common.Streaming;

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