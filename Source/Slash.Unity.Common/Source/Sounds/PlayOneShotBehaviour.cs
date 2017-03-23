namespace Slash.Unity.Common.Sounds
{
    using UnityEngine;

    public class PlayOneShotBehaviour : MonoBehaviour
    {
        public AudioClip AudioClip;

        public AudioSource AudioSource;

        public bool PlayOnAwake;

        [Range(0, 2)]
        public float VolumeScale = 1.0f;

        [ContextMenu("Play")]
        public void Play()
        {
            if (this.AudioSource == null)
            {
                Debug.Log("No audio source set", this);
                return;
            }
            if (this.AudioClip == null)
            {
                Debug.Log("No audio clip set", this);
                return;
            }
            this.AudioSource.PlayOneShot(this.AudioClip, this.VolumeScale);
        }

        private void Awake()
        {
            if (this.PlayOnAwake)
            {
                this.Play();
            }
        }
    }
}