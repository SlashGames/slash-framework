namespace Slash.Unity.StrangeIoC.Video.Services
{
    using UnityEngine;

    public interface IVideoPlayerService
    {
        bool HasVideo(string identifier);

        bool IsPlaying(string identifier);

        bool IsReadyToPlay(string identifier);

        void Load(string identifier, string path, bool autoPlay);

        void Pause(string identifier);

        void Play(string identifier);

        void PrewarmVideo(string identifier);

        void SetLoop(string identifier, bool loop);

        void SetTargets(string identifier, GameObject[] targets);

        void Stop(string identifier);
    }
}