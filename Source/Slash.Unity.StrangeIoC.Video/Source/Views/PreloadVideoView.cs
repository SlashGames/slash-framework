namespace Slash.Unity.StrangeIoC.Video.Views
{
    using System;
    using System.Collections.Generic;
    using strange.extensions.mediation.impl;
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