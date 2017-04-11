namespace Slash.Unity.StrangeIoC.Video.Signals
{
    using strange.extensions.signal.impl;
    using UnityEngine;

    public class PlayVideoData
    {
        public string Identifier { get; set; }

        public bool Loop { get; set; }

        public GameObject[] Targets { get; set; }
    }

    public class PlayVideoSignal : Signal<PlayVideoData>
    {
    }
}