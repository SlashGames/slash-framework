namespace Slash.Unity.StrangeIoC.Video.Views
{
    using strange.extensions.mediation.impl;
    using Slash.Unity.StrangeIoC.Video.Signals;

    public class PlayVideoMediator : Mediator
    {
        [Inject]
        public PlayVideoSignal PlayVideoSignal { get; set; }

        [Inject]
        public StopVideoSignal StopVideoSignal { get; set; }

        [Inject]
        public PlayVideoView View { get; set; }

        public override void OnRegister()
        {
            base.OnRegister();

            this.View.Enabled += this.OnEnabled;
            this.View.Disabled += this.OnDisabled;

            if (this.View.isActiveAndEnabled)
            {
                this.PlayVideo();
            }
        }

        public override void OnRemove()
        {
            base.OnRemove();

            this.View.Enabled -= this.OnEnabled;
            this.View.Disabled -= this.OnDisabled;
        }

        private void OnDisabled()
        {
            this.StopVideo();
        }

        private void OnEnabled()
        {
            this.PlayVideo();
        }

        private void PlayVideo()
        {
            if (!string.IsNullOrEmpty(this.View.Identifier))
            {
                this.PlayVideoSignal.Dispatch(new PlayVideoData
                {
                    Identifier = this.View.Identifier,
                    Targets = this.View.Targets,
                    Loop = this.View.Loop
                });
            }
        }

        private void StopVideo()
        {
            if (string.IsNullOrEmpty(this.View.Identifier))
            {
                return;
            }

            this.StopVideoSignal.Dispatch(this.View.Identifier);
        }
    }
}