// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SoundInterfaceBehaviour.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Unity.Common.Sound
{
    using System.Collections.Generic;
    using System.Linq;

    using UnityEngine;

    /// <summary>
    ///   Interface for playing music and sound effects.
    /// </summary>
    public class SoundInterfaceBehaviour : MonoBehaviour
    {
        #region Constants

        private const float DefaultVolume = 1.0f;

        #endregion

        #region Fields

        /// <summary>
        ///   Prefab for creating audio sources for sound effects.
        /// </summary>
        public GameObject SoundEffectSourcePrefab;

        /// <summary>
        ///   Sound effects that have finished playing.
        /// </summary>
        private readonly List<AudioSource> finishedSoundEffects = new List<AudioSource>();

        /// <summary>
        ///   Currently playing sound effects.
        /// </summary>
        private readonly List<AudioSource> soundEffectSources = new List<AudioSource>();

        /// <summary>
        ///   Audio sources for playing sounds on the respective channels.
        /// </summary>
        [SerializeField]
        [HideInInspector]
        // ReSharper disable FieldCanBeMadeReadOnly.Local - Unity won't serialize this when readonly.
        private List<AudioChannel> audioChannels = new List<AudioChannel>();

        #endregion

        // ReSharper restore FieldCanBeMadeReadOnly.Local

        #region Public Indexers

        public AudioChannel this[AudioChannelType channelType]
        {
            get
            {
                var audioChannel = this.audioChannels.FirstOrDefault(channel => channel.Type == channelType);

                if (audioChannel == null)
                {
                    audioChannel = new AudioChannel { Type = channelType };
                    this.audioChannels.Add(audioChannel);
                }

                return audioChannel;
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///   Stops the clip being played at the specified channel, if any.
        /// </summary>
        /// <param name="channelType">Channel to stop the current audio clip of.</param>
        public void ClearAudioChannel(AudioChannelType channelType)
        {
            var audioChannel = this[channelType];
            if (audioChannel.Source == null)
            {
                Debug.LogError(string.Format("No audio source set for channel {0}.", channelType));
                return;
            }

            audioChannel.Source.FadeOut();
        }

        public void PlayAudioClip(AudioChannelType channelType, AudioClip clip, float volume)
        {
            this.PlayAudioClip(channelType, clip, volume, true);
        }

        public void PlayAudioClip(AudioChannelType channelType, AudioClip clip, float volume, bool loop)
        {
            var audioChannel = this[channelType];
            if (audioChannel.Source == null)
            {
                Debug.LogError(string.Format("No audio source set for channel {0}.", channelType));
                return;
            }

            if (!audioChannel.Source.IsPlaying || audioChannel.Source.Clip != clip)
            {
                PlayAudioClip(audioChannel.Source, clip, volume, loop);
            }
        }

        /// <summary>
        ///   Plays the specified clip, if it's not already playing.
        /// </summary>
        /// <param name="channelType">Channel to play the clip at.</param>
        /// <param name="clip">Music clip to play.</param>
        public void PlayAudioClip(AudioChannelType channelType, AudioClip clip)
        {
            this.PlayAudioClip(channelType, clip, DefaultVolume);
        }

        public void PlayAudioClip(
            AudioChannelType channelType, AudioClip clip, float volume, bool loop, float fadeSpeed)
        {
            var audioChannel = this[channelType];
            if (audioChannel.Source == null)
            {
                Debug.LogError(string.Format("No audio source set for channel {0}.", channelType));
                return;
            }

            if (!audioChannel.Source.IsPlaying || audioChannel.Source.Clip != clip)
            {
                audioChannel.Source.Fade(clip, volume, loop, fadeSpeed);
            }
        }

        public void PlayAudioClipWithIntroAndLoop(
            AudioChannelType channelType, AudioClip introClip, AudioClip loopClip, float volume)
        {
            var audioChannel = this[channelType];
            if (audioChannel.Source == null)
            {
                Debug.LogError(string.Format("No audio source set for channel {0}.", channelType));
                return;
            }

            if (!audioChannel.Source.IsPlaying || audioChannel.Source.Clip != introClip)
            {
                PlayAudioIntroAndLoopClip(audioChannel.Source, introClip, loopClip, volume);
            }
        }

        /// <summary>
        ///   Plays the specified sound effect.
        /// </summary>
        /// <param name="clip">Sound effect clip to play.</param>
        public AudioSource PlaySoundEffect(AudioClip clip)
        {
            return this.PlaySoundEffect(clip, DefaultVolume);
        }

        /// <summary>
        ///   Plays the specified sound effect with the passed volume.
        /// </summary>
        /// <param name="clip">Sound effect clip to play.</param>
        /// <param name="volume">Volume to play the sound with.</param>
        public AudioSource PlaySoundEffect(AudioClip clip, float volume)
        {
            return this.PlaySoundEffect(clip, volume, false);
        }

        public AudioSource PlaySoundEffect(AudioClip clip, float volume, bool loop)
        {
            if (this.SoundEffectSourcePrefab == null)
            {
                Debug.LogError("No sound effect prefab set.");
                return null;
            }

            if (!AudioPlayerPrefs.PlaySound)
            {
                return null;
            }

            // Create new sound effect object.
            var soundEffectObject = (GameObject)Instantiate(this.SoundEffectSourcePrefab);
            soundEffectObject.transform.parent = this.transform;

            var audioSource = soundEffectObject.GetComponent<AudioSource>();
            this.soundEffectSources.Add(audioSource);

            // Play sound.
            audioSource.clip = clip;
            audioSource.volume = volume;
            audioSource.loop = loop;

            if (audioSource.enabled)
            {
                audioSource.Play();
            }

            return audioSource;
        }

        public void StopSoundEffect(AudioSource effectSource)
        {
            if (effectSource == null)
            {
                return;
            }

            effectSource.Stop();
        }

        #endregion

        #region Methods

        /// <summary>
        ///   Makes the passed audio source play the specified music clip with
        ///   with the passed parameters.
        /// </summary>
        /// <param name="audioSource">Audio source for playing the clip.</param>
        /// <param name="clip">Audio clip to play.</param>
        /// <param name="volume">Volume to play the clip with.</param>
        /// <param name="loop">Whether to loop the clip, or not.</param>
        private static void PlayAudioClip(FadingAudioSource audioSource, AudioClip clip, float volume, bool loop)
        {
            audioSource.Fade(clip, volume, loop);
        }

        private static void PlayAudioIntroAndLoopClip(
            FadingAudioSource audioSource, AudioClip introClip, AudioClip loopClip, float volume)
        {
            audioSource.Fade(introClip, loopClip, volume);
        }

        /// <summary>
        ///   Removes all audio sources for finished sound effects.
        /// </summary>
        private void CleanUpSoundEffects()
        {
            foreach (var source in this.soundEffectSources.Where(source => !source.isPlaying))
            {
                this.finishedSoundEffects.Add(source);
            }

            foreach (var finishedSound in this.finishedSoundEffects)
            {
                this.soundEffectSources.Remove(finishedSound);
                Destroy(finishedSound.gameObject);
            }

            this.finishedSoundEffects.Clear();
        }

        private void OnDisable()
        {
            AudioPlayerPrefs.PlayMusicChanged -= this.OnPlayMusicChanged;
            AudioPlayerPrefs.PlaySoundChanged -= this.OnPlaySoundChanged;
        }

        private void OnEnable()
        {
            AudioPlayerPrefs.PlayMusicChanged += this.OnPlayMusicChanged;
            AudioPlayerPrefs.PlaySoundChanged += this.OnPlaySoundChanged;

            this.OnPlayMusicChanged(AudioPlayerPrefs.PlayMusic);
            this.OnPlaySoundChanged(AudioPlayerPrefs.PlaySound);
        }

        private void OnLevelWasLoaded()
        {
            foreach (var soundEffect in this.soundEffectSources)
            {
                this.StopSoundEffect(soundEffect);
            }
        }

        private void OnPlayMusicChanged(bool playMusic)
        {
            var audioChannel = this[AudioChannelType.Music];
            audioChannel.Source.enabled = playMusic;
        }

        private void OnPlaySoundChanged(bool playSound)
        {
            var audioChannel = this[AudioChannelType.Ambient];
            audioChannel.Source.enabled = playSound;
        }

        private void Update()
        {
            this.CleanUpSoundEffects();
        }

        #endregion
    }
}