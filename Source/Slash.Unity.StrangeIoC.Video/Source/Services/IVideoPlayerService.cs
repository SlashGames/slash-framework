using UnityEngine;

namespace SuprStijl.Buddy.Unity.Modules.Video.Services
{
    public interface IVideoPlayerService
    {
        bool IsReadyToPlay(string identifier);

        bool IsPlaying(string identifier);

        void Load(string identifier, string path, bool autoPlay);

        void Pause(string identifier);

        void Play(string identifier);

        void SetLoop(string identifier, bool loop);

        void SetTargets(string identifier, GameObject[] targets);

        void Stop(string identifier);

        void PrewarmVideo(string identifier);

        bool HasVideo(string identifier);
    }
}