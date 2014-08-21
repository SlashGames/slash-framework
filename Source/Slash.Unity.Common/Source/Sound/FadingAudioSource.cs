// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FadingAudioSource.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Unity.Common.Sound
{
    using System.Collections;

    using UnityEngine;

    /// <summary>
    ///   Audio source that fades between clips instead of playing them immediately.
    /// </summary>
    [RequireComponent(typeof(AudioSource))]
    public class FadingAudioSource : MonoBehaviour
    {
        #region Fields

        /// <summary>
        ///   Volume to end the previous clip at.
        /// </summary>
        public float FadeOutThreshold = 0.05f;

        /// <summary>
        ///   Volume change per second when fading.
        /// </summary>
        public float FadeSpeed = 0.05f;

        /// <summary>
        ///   Actual audio source.
        /// </summary>
        private AudioSource audioSource;

        /// <summary>
        ///   Whether the audio source is currently fading, in or out.
        /// </summary>
        private FadeState fadeState = FadeState.None;

        /// <summary>
        ///   Whether to loop the next clip.
        /// </summary>
        private bool loopNextClip;

        /// <summary>
        ///   Next clip to fade to.
        /// </summary>
        private AudioClip nextClip;

        /// <summary>
        ///   Target volume to fade the next clip to.
        /// </summary>
        private float nextClipVolume;

        /// <summary>
        ///   Loop to play after intro clip.
        /// </summary>
        private AudioClip nextLoopedClip;

        private float originalFadeSpeed;

        #endregion

        #region Enums

        public enum FadeState
        {
            None,

            FadingOut,

            FadingIn
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///   Current clip of the audio source.
        /// </summary>
        public AudioClip Clip
        {
            get
            {
                return this.audioSource.clip;
            }
        }

        /// <summary>
        ///   Whether the audio source is currently playing a clip.
        /// </summary>
        public bool IsPlaying
        {
            get
            {
                return this.audioSource.isPlaying;
            }
        }

        /// <summary>
        ///   Whether the audio source is looping the current clip.
        /// </summary>
        public bool Loop
        {
            get
            {
                return this.audioSource.loop;
            }
        }

        /// <summary>
        ///   Current volume of the audio source.
        /// </summary>
        public float Volume
        {
            get
            {
                return this.audioSource.volume;
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///   If the audio source is enabled and playing, fades out the current clip and fades in the specified one, after.
        ///   If the audio source is enabled and not playing, fades in the specified clip immediately.
        ///   If the audio source is not enalbed, fades in the specified clip as soon as it gets enabled.
        /// </summary>
        /// <param name="clip">Clip to fade in.</param>
        /// <param name="volume">Volume to fade to.</param>
        /// <param name="loop">Whether to loop the new clip, or not.</param>
        public void Fade(AudioClip clip, float volume, bool loop)
        {
            this.Fade(clip, volume, loop, this.FadeSpeed);
        }

        /// <summary>
        ///   If the audio source is enabled and playing, fades out the current clip and fades in the specified intro clip, after.
        ///   If the audio source is enabled and not playing, fades in the specified intro clip immediately.
        ///   If the audio source is not enalbed, fades in the specified intro clip as soon as it gets enabled.
        ///   After the intro clip has finished, starts looping the loop clip immediately, without fading.
        /// </summary>
        /// <param name="clip">Clip to fade in.</param>
        /// <param name="loopClip">
        ///   Clip to loop after <paramref name="loopClip" /> has finished playing.
        /// </param>
        /// <param name="volume">Volume to fade to and play both clips with.</param>
        public void Fade(AudioClip clip, AudioClip loopClip, float volume)
        {
            if (clip == null || clip == this.audioSource.clip)
            {
                return;
            }

            this.nextClip = clip;
            this.nextLoopedClip = loopClip;
            this.nextClipVolume = volume;
            this.loopNextClip = false;

            if (this.audioSource.enabled)
            {
                if (this.IsPlaying)
                {
                    this.fadeState = FadeState.FadingOut;
                }
                else
                {
                    this.FadeToNextClip();
                }
            }
            else
            {
                this.FadeToNextClip();
            }
        }

        public void Fade(AudioClip clip, float volume, bool loop, float fadeSpeed)
        {
            if (clip == null || clip == this.audioSource.clip)
            {
                return;
            }

            this.nextClip = clip;
            this.nextLoopedClip = null;
            this.nextClipVolume = volume;
            this.loopNextClip = loop;
            this.FadeSpeed = fadeSpeed;

            if (this.audioSource.enabled)
            {
                if (this.IsPlaying)
                {
                    this.fadeState = FadeState.FadingOut;
                }
                else
                {
                    this.FadeToNextClip();
                }
            }
            else
            {
                this.FadeToNextClip();
            }
        }

        /// <summary>
        ///   Fades out the current audio clip, stopping the clip after.
        /// </summary>
        public void FadeOut()
        {
            this.nextClip = null;
            this.nextLoopedClip = null;
            this.fadeState = FadeState.FadingOut;
        }

        /// <summary>
        ///   Continues fading in the current audio clip.
        /// </summary>
        public void Play()
        {
            this.fadeState = FadeState.FadingIn;
            this.audioSource.Play();
        }

        /// <summary>
        ///   Stop playing the current audio clip immediately.
        /// </summary>
        public void Stop()
        {
            this.audioSource.Stop();
            this.fadeState = FadeState.None;
            this.FadeSpeed = this.originalFadeSpeed;
        }

        #endregion

        #region Methods

        private void Awake()
        {
            this.audioSource = this.audio;
            this.audioSource.volume = 0f;

            this.originalFadeSpeed = this.FadeSpeed;
        }

        private void FadeToNextClip()
        {
            if (this.nextClip == null)
            {
                this.Stop();
            }

            this.audioSource.clip = this.nextClip;
            this.audioSource.loop = this.nextLoopedClip == null && this.loopNextClip;

            this.fadeState = FadeState.FadingIn;

            if (this.audioSource.enabled)
            {
                this.audioSource.Play();

                if (this.nextLoopedClip != null)
                {
                    this.StartCoroutine(this.LoopClipAfterDelay(this.nextClip.length, this.nextLoopedClip));
                }
            }
        }

        /// <summary>
        ///   Waits for the specified amount of seconds, and loops the passed clip immediately after.
        /// </summary>
        /// <param name="delayInSeconds">Seconds to wait before starting to play the clip.</param>
        /// <param name="loopedClip">Clip to loop.</param>
        private IEnumerator LoopClipAfterDelay(float delayInSeconds, AudioClip loopedClip)
        {
            yield return new WaitForSeconds(delayInSeconds);

            this.audioSource.clip = loopedClip;
            this.audioSource.loop = true;

            if (this.audioSource.enabled)
            {
                this.audioSource.Play();
            }
        }

        private void OnDisable()
        {
            this.audioSource.enabled = false;
            this.Stop();
        }

        private void OnEnable()
        {
            this.audioSource.enabled = true;
            this.Play();

            if (this.nextLoopedClip != null)
            {
                this.StartCoroutine(this.LoopClipAfterDelay(this.nextClip.length, this.nextLoopedClip));
            }
        }

        private void Update()
        {
            if (!this.audioSource.enabled)
            {
                return;
            }

            if (this.fadeState == FadeState.FadingOut)
            {
                if (this.audioSource.volume > this.FadeOutThreshold)
                {
                    // Fade out current clip.
                    this.audioSource.volume -= this.FadeSpeed * Time.deltaTime;
                }
                else
                {
                    // Start fading in next clip.
                    this.FadeToNextClip();
                }
            }
            else if (this.fadeState == FadeState.FadingIn)
            {
                if (this.audioSource.volume < this.nextClipVolume)
                {
                    // Fade in next clip.
                    this.audioSource.volume += this.FadeSpeed * Time.deltaTime;
                }
                else
                {
                    // Stop fading in.
                    this.fadeState = FadeState.None;
                    this.FadeSpeed = this.originalFadeSpeed;
                }
            }
        }

        #endregion
    }
}