// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PlayAudioBehaviour.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Unity.Common.Sound
{
    using UnityEngine;

    /// <summary>
    ///   Plays an audio clip until stopped.
    /// </summary>
    public class PlayAudioBehaviour : MonoBehaviour
    {
        #region Fields

        /// <summary>
        ///   Channel to play to clip at.
        /// </summary>
        public AudioChannelType AudioChannel;

        /// <summary>
        ///   Audio clip to play.
        /// </summary>
        public AudioClip AudioClip;

        #endregion

        #region Methods

        private void Awake()
        {
            var soundInterface = FindObjectOfType<SoundInterfaceBehaviour>();

            if (soundInterface == null)
            {
                Debug.LogWarning("Sound interface missing.");
                return;
            }

            if (this.AudioClip == null)
            {
                soundInterface.ClearAudioChannel(this.AudioChannel);
            }
            else
            {
                soundInterface.PlayAudioClip(this.AudioChannel, this.AudioClip, 1.0f);
            }
        }

        #endregion
    }
}