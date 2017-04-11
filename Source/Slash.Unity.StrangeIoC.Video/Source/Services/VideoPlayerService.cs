namespace Slash.Unity.StrangeIoC.Video.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using Slash.Unity.StrangeIoC.Video.Signals;
    using UnityEngine;
    using UnityEngine.Video;

    public class VideoPlayerService : IVideoPlayerService
    {
        private readonly List<Video> videos = new List<Video>();

        private Video activeVideo;

        private VideoPlayer player;

        private AudioSource playerAudio;

        private Video prewarmedVideo;

        [Inject]
        public VideoLoadedSignal VideoLoadedSignal { get; set; }

        private VideoPlayer Player
        {
            get
            {
                if (this.player == null)
                {
                    var playerGameObject = new GameObject("Video Player");
                    this.player = playerGameObject.AddComponent<VideoPlayer>();
                    this.playerAudio = playerGameObject.AddComponent<AudioSource>();
                    this.player.prepareCompleted += preparedPlayer =>
                    {
                        Debug.Log("Video is ready");
                        if (this.VideoLoadedSignal != null)
                        {
                            this.VideoLoadedSignal.Dispatch();
                        }
                    };
                    this.player.frameReady += (readyPlayer, index) =>
                    {
                        if (index <= 1)
                        {
                            Debug.Log("First frame is ready");
                            if (this.VideoLoadedSignal != null)
                            {
                                this.VideoLoadedSignal.Dispatch();
                            }
                        }
                    };
                }
                return this.player;
            }
        }

        public bool IsReadyToPlay(string identifier)
        {
            var video = this.GetVideo(identifier);
            return video != null && (video == this.prewarmedVideo || video == this.activeVideo);
        }

        public bool IsPlaying(string identifier)
        {
            return this.activeVideo != null && this.activeVideo.Identifier == identifier;
        }

        public void Load(string identifier, string path, bool autoPlay)
        {
            // Check if video player exists.
            var video = this.GetVideo(identifier, true);
            video.Path = path;
        }

        public void Play(string identifier)
        {
            var video = this.GetVideo(identifier);
            if (video == null)
            {
                return;
            }

            if (this.activeVideo == video)
            {
                this.Player.Stop();
                this.SetupPlayer(video);
                this.Player.Play();
                return;
            }

            // Stop active video player.
            if (this.activeVideo != null)
            {
                this.Player.Stop();
                this.activeVideo = null;
            }

            this.SetupPlayer(video);
            if (video == this.prewarmedVideo)
            {
                this.Player.frame = 0;
                this.Player.Play();
            }
            else
            {
                this.prewarmedVideo = null;
                this.Player.url = video.Path;
                this.Player.Prepare();
            }

            this.activeVideo = video;
        }

        public void Pause(string identifier)
        {
            if (this.activeVideo == null || this.activeVideo.Identifier != identifier)
            {
                return;
            }

            this.Player.Pause();
        }

        public void Stop(string identifier)
        {
            if (this.activeVideo == null || this.activeVideo.Identifier != identifier)
            {
                return;
            }

            this.Player.Pause();
            this.Player.Stop();

            foreach (var target in this.activeVideo.Targets)
            {
                target.SetActive(false);
            }

            this.activeVideo = null;
        }

        public void PrewarmVideo(string identifier)
        {
            var video = this.GetVideo(identifier);
            if (video == null)
            {
                return;
            }

            this.PrewarmVideo(video);
        }

        public bool HasVideo(string identifier)
        {
            return this.GetVideo(identifier) != null;
        }

        public void SetTargets(string identifier, GameObject[] targets)
        {
            var videoPlayer = this.GetVideo(identifier);
            if (videoPlayer == null)
            {
                Debug.LogWarningFormat("No video player loaded with identifier '{0}'", identifier);
                return;
            }

            // Use targets.
            videoPlayer.Targets = targets;
        }

        public void SetLoop(string identifier, bool loop)
        {
            var videoPlayer = this.GetVideo(identifier);
            if (videoPlayer == null)
            {
                return;
            }

            videoPlayer.Loop = loop;
        }

        private Video GetVideo(string identifier, bool createIfNecessary = false)
        {
            var videoPlayer = this.videos.FirstOrDefault(existingVideo => existingVideo.Identifier == identifier);
            if (videoPlayer == null && createIfNecessary)
            {
                videoPlayer = new Video
                {
                    Identifier = identifier
                };
                this.videos.Add(videoPlayer);
            }
            return videoPlayer;
        }

        private void PrewarmVideo(Video video)
        {
            this.SetupPlayer(video);

            this.Player.Stop();

            this.Player.url = video.Path;
            this.Player.Prepare();
        }

        private void SetupPlayer(Video video)
        {
            List<GameObject> targets = null;
            if (video.Targets != null)
            {
                targets = new List<GameObject>();
                foreach (var target in video.Targets)
                {
                    if (target == null)
                    {
                        continue;
                    }

                    targets.Add(target);
                    target.SetActive(true);
                }
            }

            //this.Player.m_bLoop = video.Loop;
            //this.Player.m_TargetMaterial = targets != null ? targets.ToArray() : null;
            //this.Player.targetMaterialRenderer = video.Target;
            this.playerAudio.spatialBlend = 1;
        }

        private class Video
        {
            public string Identifier;

            public bool Loop;

            public string Path;

            public GameObject[] Targets;
        }
    }
}