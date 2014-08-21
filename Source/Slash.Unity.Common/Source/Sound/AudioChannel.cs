// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AudioChannel.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Unity.Common.Sound
{
    using System;

    /// <summary>
    ///   Channel to play an audio clip on.
    /// </summary>
    [Serializable]
    public class AudioChannel
    {
        #region Fields

        /// <summary>
        ///   Audio source for playing clips.
        /// </summary>
        public FadingAudioSource Source;

        /// <summary>
        ///   Type of this channel.
        /// </summary>
        public AudioChannelType Type;

        #endregion
    }
}