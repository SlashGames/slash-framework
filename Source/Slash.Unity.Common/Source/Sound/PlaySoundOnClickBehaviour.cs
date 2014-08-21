// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PlaySoundOnClickBehaviour.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Unity.Common.Sound
{
    using UnityEngine;

    public class PlaySoundOnClickBehaviour : MonoBehaviour
    {
        #region Fields

        /// <summary>
        ///   Sound effect to play.
        /// </summary>
        public AudioClip Clip;

        /// <summary>
        ///   Volume to play the sound effect with.
        /// </summary>
        [Range(0f, 1f)]
        public float Volume = 1f;

        private SoundInterfaceBehaviour soundInterface;

        #endregion

        #region Methods

        private void Awake()
        {
            this.soundInterface = FindObjectOfType<SoundInterfaceBehaviour>();
        }

        private void OnClick()
        {
            if (this.soundInterface != null)
            {
                this.soundInterface.PlaySoundEffect(this.Clip, this.Volume);
            }
        }

        #endregion
    }
}