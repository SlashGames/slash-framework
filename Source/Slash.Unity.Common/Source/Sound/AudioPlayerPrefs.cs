// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AudioPlayerPrefs.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Unity.Common.Sound
{
    using UnityEngine;

    /// <summary>
    ///   Uses Unity PlayerPrefs for toggling sound and music.
    /// </summary>
    public class AudioPlayerPrefs
    {
        #region Constants

        private const string PlayerPrefsKeyPlayMusic = "PlayMusic";

        private const string PlayerPrefsKeyPlaySound = "PlaySound";

        #endregion

        #region Delegates

        /// <summary>
        ///   Music has been turned on or off.
        /// </summary>
        /// <param name="playMusic">Whether to play music.</param>
        public delegate void PlayMusicChangedDelegate(bool playMusic);

        /// <summary>
        ///   Sound effects have been turned on or off.
        /// </summary>
        /// <param name="playSound">Whether to play sound effects.</param>
        public delegate void PlaySoundChangedDelegate(bool playSound);

        #endregion

        #region Public Events

        /// <summary>
        ///   Music has been turned on or off.
        /// </summary>
        public static event PlayMusicChangedDelegate PlayMusicChanged;

        /// <summary>
        ///   Sound effects have been turned on or off.
        /// </summary>
        public static event PlaySoundChangedDelegate PlaySoundChanged;

        #endregion

        #region Public Properties

        /// <summary>
        ///   Whether to play background music.
        /// </summary>
        public static bool PlayMusic
        {
            get
            {
                return PlayerPrefs.GetInt(PlayerPrefsKeyPlayMusic, 1) > 0;
            }
            set
            {
                if (value == PlayMusic)
                {
                    return;
                }

                PlayerPrefs.SetInt(PlayerPrefsKeyPlayMusic, value ? 1 : 0);
                PlayerPrefs.Save();

                OnPlayMusicChanged(value);
            }
        }

        /// <summary>
        ///   Whether to play sound effects.
        /// </summary>
        public static bool PlaySound
        {
            get
            {
                return PlayerPrefs.GetInt(PlayerPrefsKeyPlaySound, 1) > 0;
            }
            set
            {
                if (value == PlaySound)
                {
                    return;
                }

                PlayerPrefs.SetInt(PlayerPrefsKeyPlaySound, value ? 1 : 0);
                PlayerPrefs.Save();

                OnPlaySoundChanged(value);
            }
        }

        #endregion

        #region Methods

        private static void OnPlayMusicChanged(bool playMusic)
        {
            var handler = PlayMusicChanged;
            if (handler != null)
            {
                handler(playMusic);
            }
        }

        private static void OnPlaySoundChanged(bool playSound)
        {
            var handler = PlaySoundChanged;
            if (handler != null)
            {
                handler(playSound);
            }
        }

        #endregion
    }
}