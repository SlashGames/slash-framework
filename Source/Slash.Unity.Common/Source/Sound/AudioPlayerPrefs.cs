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

        public const string PlayerPrefsKeyPlayMusic = "PlayMusic";

        public const string PlayerPrefsKeyPlaySound = "PlaySound";

        #endregion

        #region Delegates

        public delegate void PlayMusicChangedDelegate(bool playMusic);

        public delegate void PlaySoundChangedDelegate(bool playSound);

        #endregion

        #region Public Events

        public static event PlayMusicChangedDelegate PlayMusicChanged;

        public static event PlaySoundChangedDelegate PlaySoundChanged;

        #endregion

        #region Public Properties

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