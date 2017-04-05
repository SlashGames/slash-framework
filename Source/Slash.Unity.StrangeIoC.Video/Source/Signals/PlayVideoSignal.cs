using strange.extensions.signal.impl;
using UnityEngine;

namespace SuprStijl.Buddy.Unity.Modules.Video.Signals
{
    public class PlayVideoData
    {
        public string Identifier { get; set; }

        public GameObject[] Targets { get; set; }

        public bool Loop { get; set; }
    }

    public class PlayVideoSignal : Signal<PlayVideoData>
    {
    }
}