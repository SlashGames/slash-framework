namespace Slash.Unity.Common.UI
{
    using System;

    using UnityEngine;
    using UnityEngine.Events;

    public class Interactable : MonoBehaviour
    {
        public InteractivityChangedEvent InteractivityChanged = new InteractivityChangedEvent();

        [SerializeField]
        private bool isInteractable;

        public bool IsInteractable
        {
            get
            {
                return this.isInteractable;
            }
            set
            {
                if (value == this.isInteractable)
                {
                    return;
                }

                this.isInteractable = value;

                this.InteractivityChanged.Invoke(this.isInteractable);
            }
        }

        private void OnDisable()
        {
            if (this.isInteractable)
            {
                this.InteractivityChanged.Invoke(false);
            }
        }

        private void OnEnable()
        {
            if (this.isInteractable)
            {
                this.InteractivityChanged.Invoke(true);
            }
        }

        [Serializable]
        public class InteractivityChangedEvent : UnityEvent<bool>
        {
        }
    }
}