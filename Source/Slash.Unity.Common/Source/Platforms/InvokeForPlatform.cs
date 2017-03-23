namespace Slash.Unity.Common.Platforms
{
    using UnityEngine;
    using UnityEngine.Events;

    public class InvokeForPlatform : MonoBehaviour
    {
        public RuntimePlatform Platform;

        public UnityEvent PlatformStarted;

        private void Start()
        {
            if (Application.platform == this.Platform)
            {
                this.PlatformStarted.Invoke();
            }
        }
    }
}