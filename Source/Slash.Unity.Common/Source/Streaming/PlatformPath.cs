namespace Slash.Unity.Common.Streaming
{
    using System;
    using UnityEngine;

    [Serializable]
    public class PlatformPath
    {
        /// <summary>
        ///     Path to use on this platform.
        /// </summary>
        public ExternalResource Path;

        /// <summary>
        ///     Platform the slides are used on.
        /// </summary>
        public RuntimePlatform Platform;
    }
}