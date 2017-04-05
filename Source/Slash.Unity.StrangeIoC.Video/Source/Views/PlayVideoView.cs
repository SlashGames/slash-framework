using System;
using strange.extensions.mediation.impl;
using UnityEngine;

namespace SuprStijl.Buddy.Unity.Modules.Video.Views
{
    public class PlayVideoView : View
    {
        public string Identifier;

        public bool Loop;

        public GameObject[] Targets;

        public event Action Disabled;

        public event Action Enabled;

        protected virtual void OnDisabled()
        {
            var handler = this.Disabled;
            if (handler != null)
            {
                handler();
            }
        }

        protected virtual void OnEnabled()
        {
            var handler = this.Enabled;
            if (handler != null)
            {
                handler();
            }
        }

        private void OnDisable()
        {
            this.OnDisabled();
        }

        private void OnEnable()
        {
            this.OnEnabled();
        }
    }
}