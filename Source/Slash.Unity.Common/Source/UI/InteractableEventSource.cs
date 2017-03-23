namespace Slash.Unity.Common.UI
{
    using UnityEngine;
    using UnityEngine.Events;

    public class InteractableEventSource : MonoBehaviour
    {
        public UnityEvent HandleDisabled = new UnityEvent();

        public UnityEvent HandleEnabled = new UnityEvent();

        public Interactable Interactable;

        public void OnEvent()
        {
            if (this.Interactable != null)
            {
                if (this.Interactable.IsInteractable)
                {
                    this.HandleEnabled.Invoke();
                }
                else
                {
                    this.HandleDisabled.Invoke();
                }
            }
        }
    }
}